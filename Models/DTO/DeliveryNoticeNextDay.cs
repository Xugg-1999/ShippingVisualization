/// <summary>
/// 次日发货计划
/// </summary>
public class DeliveryNoticeNextDay
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
}
