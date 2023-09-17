namespace _01_MSG4.Headers;

/*
 * Подробнее смотри:
 *   4.2.2.2 Image Navigation Record
 *   4.4 Navigation of Image Data
 * здесь:
 *   https://www.cgms-info.org/wp-content/uploads/2021/10/cgms-lrit-hrit-global-specification-(v2-8-of-30-oct-2013).pdf
 */
public class _2_ImageNavigation : BaseHeader
{
    public _2_ImageNavigation() : base(2, 51)
    {
    }


    public string ProjName { get; set; }
    public Int32 CFAC_ColumnScalingFactor { get; set; }
    public Int32 LFAC_LineScalingFactor { get; set; }
    public Int32 COFF_ColumnOffset { get; set; }
    public Int32 LOFF_LineOffset { get; set; }
}