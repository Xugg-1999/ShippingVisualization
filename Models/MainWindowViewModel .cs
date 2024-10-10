using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Threading;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private DateTime? _selectedDate;
    private DeliveryNotice _selectedDeliveryNotice;
    private DispatcherTimer _timer;
    public int SelectedRefreshFrequency { get; set; } = 5;
    public ObservableCollection<DeliveryNotice> ShippingRecords { get; set; }
    public ObservableCollection<DeliveryNoticeNextDay> ShippingRecordsNextDay { get; set; }
    public ObservableCollection<CompletedLoading> CompletedLoading { get; set; }
    public ObservableCollection<UnCompletedLoading> UnCompletedLoading { get; set; }
    public MainWindowViewModel()
    {
        // 确保初始化 ShippingRecords 避免 NullReferenceException
        ShippingRecords = new ObservableCollection<DeliveryNotice>();
        ShippingRecordsNextDay = new ObservableCollection<DeliveryNoticeNextDay>();
        CompletedLoading = new ObservableCollection<CompletedLoading>();
        UnCompletedLoading = new ObservableCollection<UnCompletedLoading>();

        SelectedDate = DateTime.Now;
        SelectedDeliveryNotice = null;
        //定时执行任务
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(SelectedRefreshFrequency);
        _timer.Tick += Timer_Tick;
        _timer.Start();

        GetDeliveryNoticeToDay();
        GetDeliveryNoticeToNextDay();
    }
    public void UpdateRefreshInterval(int intervalInSeconds)
    {
        SelectedRefreshFrequency = intervalInSeconds;
        _timer.Interval = TimeSpan.FromSeconds(intervalInSeconds);
        OnPropertyChanged(nameof(SelectedRefreshFrequency));
    }

    public DateTime? SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (_selectedDate != value)
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                GetDeliveryNoticeToDay();
                GetDeliveryNoticeToNextDay();
            }
        }
    }
    public DeliveryNotice SelectedDeliveryNotice
    {
        get => _selectedDeliveryNotice;
        set
        {
            if (_selectedDeliveryNotice != value)
            {
                _selectedDeliveryNotice = value;
                OnPropertyChanged(nameof(SelectedDeliveryNotice));
            }
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void Timer_Tick(object sender, EventArgs e)
    {
        GetDeliveryNoticeToDay();
        GetDeliveryNoticeToNextDay();
        if (SelectedDeliveryNotice != null)
        {
            GetCompletedLoading(SelectedDeliveryNotice.FBILLNO);
            GetUnCompletedLoading(SelectedDeliveryNotice.FBILLNO);
        }
    }
    /// <summary>
    /// 值改变事件
    /// </summary>
    /// <param name="selectedNotice"></param>
    public void HandleRowClick(DeliveryNotice selectedNotice)
    {
        if (selectedNotice != null)
        {
            SelectedDeliveryNotice = selectedNotice; // Set selected notice
            string deliveryNoticeNo = selectedNotice.FBILLNO; // 获取发货通知单号
            GetCompletedLoading(deliveryNoticeNo); // 调用已装柜方法
            GetUnCompletedLoading(deliveryNoticeNo); // 调用未装柜方法
        }
    }
    /// <summary>
    /// 查询当日发货计划
    /// </summary>
    public void GetDeliveryNoticeToDay()
    {
        ShippingRecords.Clear();
        string connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;
        DateTime? selectedDate = SelectedDate;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sqlQuery = @"WITH AAA
                                AS (SELECT A.FCARRIAGENO,
                                           MIN(A.FCREATEDATE) AS FSTARTTIME,
                                           MAX(A.FCREATEDATE) AS FENDTIME,
                                           DATEDIFF(MINUTE, MIN(A.FCREATEDATE), MAX(A.FCREATEDATE)) AS FTOTALDURATION
                                    FROM T_SAL_OUTSTOCK A
                                    WHERE A.FPUSH='1'
									AND A.FDOCUMENTSTATUS<>'D'
                                    GROUP BY A.FCARRIAGENO)
                                SELECT ROW_NUMBER() OVER (ORDER BY A.FLOADINGTIME) AS FNO,
                                       A.FBILLNO,
                                       A.FBLNO,
                                       A.FBOXTYPE,
                                       A.FLOADINGTIME,
                                       AAA.FSTARTTIME,
                                       AAA.FENDTIME,
                                       AAA.FTOTALDURATION,
                                       D.FNAME AS FSALER,
                                       CASE
                                           WHEN A.FCLOSESTATUS = 'B' THEN
                                               '已结束'
                                           WHEN A.FCLOSESTATUS = 'A' THEN
                                               '未开始'
                                           ELSE
                                               '其他状态'
                                       END AS FSTATUS
                                FROM T_SAL_DELIVERYNOTICE A
                                    JOIN T_BD_OPERATORENTRY B
                                        ON A.FSALESMANID = B.FENTRYID
                                    JOIN T_HR_EMPINFO C
                                        ON B.FSTAFFID = C.FSTAFFID
                                    JOIN dbo.T_HR_EMPINFO_L D
                                        ON C.FID = D.FID
                                    LEFT JOIN AAA
                                        ON AAA.FCARRIAGENO = A.FBILLNO
                                WHERE B.FOPERATORTYPE = 'XSY'
                                      AND D.FLOCALEID = 2052
                                      AND A.FCANCELSTATUS = 'A'
                                      AND A.FDOCUMENTSTATUS = 'C'
                                      AND A.FLOADINGTIME<>''
                                      AND A.FDATE = @FDATE
                                ORDER BY A.FLOADINGTIME ASC; ";
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@FDATE", selectedDate ?? DateTime.Now);
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ShippingRecords.Add(new DeliveryNotice
                    {
                        FNO = reader.IsDBNull(reader.GetOrdinal("FNO")) ? string.Empty : reader["FNO"].ToString(),
                        FBILLNO = reader.IsDBNull(reader.GetOrdinal("FBILLNO")) ? string.Empty : reader["FBILLNO"].ToString(),
                        FSALER = reader.IsDBNull(reader.GetOrdinal("FSALER")) ? string.Empty : reader["FSALER"].ToString(),
                        FBLNO = reader.IsDBNull(reader.GetOrdinal("FBLNO")) ? string.Empty : reader["FBLNO"].ToString(),
                        FBOXTYPE = reader.IsDBNull(reader.GetOrdinal("FBOXTYPE")) ? string.Empty : reader["FBOXTYPE"].ToString(),
                        FLOADINGTIME = reader.IsDBNull(reader.GetOrdinal("FLOADINGTIME")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FLOADINGTIME")),
                        FSTARTTIME = reader.IsDBNull(reader.GetOrdinal("FSTARTTIME")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FSTARTTIME")),
                        FENDTIME = reader.IsDBNull(reader.GetOrdinal("FENDTIME")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FENDTIME")),
                        FTOTALDURATION = reader["FTOTALDURATION"].ToString(),
                        FSTATUS = reader["FSTATUS"].ToString()
                    });
                }
            }
        }
    }
    /// <summary>
    /// 查询第二天发货计划
    /// </summary>
    public void GetDeliveryNoticeToNextDay()
    {
        ShippingRecordsNextDay.Clear();
        string connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;
        DateTime? selectedDate = SelectedDate;
        string selectedDateNextDay = selectedDate.Value.AddDays(1).ToString("yyyy-MM-dd");
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sqlQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY A.FLOADINGTIME) AS FNO,
                                    A.FBILLNO,
                                    A.FBLNO,
                                    A.FBOXTYPE,
                                    A.FLOADINGTIME
                              FROM T_SAL_DELIVERYNOTICE A
                              JOIN T_BD_OPERATORENTRY B ON A.FSALESMANID = B.FENTRYID
                              JOIN T_HR_EMPINFO C ON B.FSTAFFID = C.FSTAFFID
                              JOIN dbo.T_HR_EMPINFO_L D ON C.FID = D.FID
                              WHERE B.FOPERATORTYPE = 'XSY'
                                    AND D.FLOCALEID = 2052
                                    AND A.FCANCELSTATUS = 'A'
                                    AND A.FDOCUMENTSTATUS = 'C'
                                    AND A.FLOADINGTIME<>''
                                    AND A.FDATE = @FDATE
                                    ORDER BY A.FLOADINGTIME ASC ";
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@FDATE", selectedDateNextDay ?? DateTime.Now.ToString("yyyy-MM-dd"));
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ShippingRecordsNextDay.Add(new DeliveryNoticeNextDay
                    {
                        FNO = reader.IsDBNull(reader.GetOrdinal("FNO")) ? string.Empty : reader["FNO"].ToString(),
                        FBILLNO = reader["FBILLNO"].ToString(),
                        FBLNO = reader["FBLNO"].ToString(),
                        FBOXTYPE = reader["FBOXTYPE"].ToString(),
                        FLOADINGTIME = reader.IsDBNull(reader.GetOrdinal("FLOADINGTIME")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FLOADINGTIME"))
                    });
                }
            }
        }
    }

    /// <summary>
    /// 已装柜
    /// </summary>
    public void GetCompletedLoading(string deliveryNoticeNo)
    {
        CompletedLoading.Clear();
        string connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;
        DateTime? selectedDate = SelectedDate;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sqlQuery = @"SELECT A.FCARRIAGENO AS FBILLNO,
                                 D.FNUMBER AS FMATERIALID,
                                 E.FNAME AS FMATERIALNAME,
                                 E.FSPECIFICATION AS FSPEC,
                                 B.FREALQTY AS FQTY,
                                 C.FNUMBER AS FUNIT
                          FROM T_SAL_OUTSTOCK A
                              JOIN T_SAL_OUTSTOCKENTRY B
                                  ON A.FID = B.FID
                              JOIN dbo.T_BD_UNIT C
                                  ON B.FUNITID = C.FUNITID
                              JOIN T_BD_MATERIAL D
                                  ON B.FMATERIALID = D.FMATERIALID
                              JOIN dbo.T_BD_MATERIAL_L E
                                  ON B.FMATERIALID = E.FMATERIALID
                          WHERE  E.FLOCALEID = '2052' 
                                  AND A.FCARRIAGENO = @DeliveryNoticeNo;";
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@DeliveryNoticeNo", deliveryNoticeNo);
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    CompletedLoading.Add(new CompletedLoading
                    {
                        FBILLNO = reader["FBILLNO"].ToString(),
                        FMATERIALID = reader["FMATERIALID"].ToString(),
                        FMATERIALNAME = reader["FMATERIALNAME"].ToString(),
                        FSPEC = reader["FSPEC"].ToString(),
                        FQTY = Math.Round(Convert.ToDecimal(reader["FQTY"]), 1).ToString(),
                        FUNIT = reader["FUNIT"].ToString()
                    });
                }
            }
        }
    }

    /// <summary>
    /// 未装柜
    /// </summary>
    public void GetUnCompletedLoading(string deliveryNoticeNo)
    {
        UnCompletedLoading.Clear();
        string connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;
        DateTime? selectedDate = SelectedDate;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sqlQuery = @"SELECT  A.FBILLNO,
                               C.FNUMBER AS FMATERIALID,
                               D.FNAME AS FMATERIALNAME,
                               D.FSPECIFICATION AS FSPEC,
                               E.FNUMBER AS FUNIT,
                               B.FQTY AS FQTY
                        FROM T_SAL_DELIVERYNOTICE A
                            JOIN T_SAL_DELIVERYNOTICEENTRY B
                                ON A.FID = B.FID
                            JOIN dbo.T_BD_MATERIAL C
                                ON B.FMATERIALID = C.FMATERIALID
                            JOIN dbo.T_BD_MATERIAL_L D
                                ON C.FMATERIALID = D.FMATERIALID
                            JOIN dbo.T_BD_UNIT E
                                ON B.FUNITID = E.FUNITID
                        WHERE 1=1
                               AND B.FQTY <> B.FSUMOUTQTY
                               AND D.FLOCALEID = '2052'
                              AND A.FBILLNO =  @DeliveryNoticeNo";
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@DeliveryNoticeNo", deliveryNoticeNo);
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    UnCompletedLoading.Add(new UnCompletedLoading
                    {
                        FBILLNO = reader["FBILLNO"].ToString(),
                        FMATERIALID = reader["FMATERIALID"].ToString(),
                        FMATERIALNAME = reader["FMATERIALNAME"].ToString(),
                        FSPEC = reader["FSPEC"].ToString(),
                        FQTY = Math.Round(Convert.ToDecimal(reader["FQTY"]), 1).ToString(),
                        FUNIT = reader["FUNIT"].ToString()
                    });
                }
            }
        }
    }
}
