/// <summary>
/// 已装柜详情
/// </summary>
public class CompletedLoading
{
    /// <summary>
    /// 销售订单
    /// </summary>
    public string FBILLNO { get; set; }

    /// <summary>
    /// 物料编码
    /// </summary>
    public string FMATERIALID { get; set; }
    /// <summary>
    /// 物料名称
    /// </summary>
    public string FMATERIALNAME { get; set; }
    /// <summary>
    /// 规格型号
    /// </summary>
    public string FSPEC { get; set; }
    /// <summary>
    /// 数量
    /// </summary>
    public string FQTY { get; set; }
    /// <summary>
    /// 单位
    /// </summary>
    public string FUNIT { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime FCREATEDATE { get; set; }

}
