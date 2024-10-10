/// <summary>
///当日发货计划
/// </summary>
public class DeliveryNotice
{

    /// <summary>
    /// 序号
    /// </summary>
    public string FNO { get; set; }
    /// <summary>
    /// 发货通知单号
    /// </summary>
    public string FBILLNO { get; set; }
    /// <summary>
    /// 销售员
    /// </summary>
    public string FSALER { get; set; }
    /// <summary>
    /// 提单号
    /// </summary>
    public string FBLNO { get; set; }
    /// <summary>
    /// 箱型
    /// </summary>
    public string FBOXTYPE { get; set; }
    /// <summary>
    /// 装车时间
    /// </summary>
    public DateTime? FLOADINGTIME { get; set; }
    /// <summary>
    /// 开始扫描时间
    /// </summary>
    public DateTime? FSTARTTIME { get; set; }
    /// <summary>
    /// 结束扫描时间
    /// </summary>
    public DateTime? FENDTIME { get; set; }
    /// <summary>
    /// 累计用时
    /// </summary>
    public string FTOTALDURATION { get; set; }
    /// <summary>
    /// 状态
    /// </summary>
    public string FSTATUS { get; set; }

}
