namespace _01_MSG4.Headers;

/*
 * Подробнее смотри:
 *   4.2.2.1 Image Structure Record
 *   4.2.3.1 Image Data Files
 * здесь:
 *   https://www.cgms-info.org/wp-content/uploads/2021/10/cgms-lrit-hrit-global-specification-(v2-8-of-30-oct-2013).pdf
 */
public class _1_ImageStructure : BaseHeader
{
    public _1_ImageStructure() : base(1, 9)
    {
    }

    public byte NB_NumberOfBitsPerPixel { get; set; }
    public UInt16 NC_NumberOfColumns { get; set; }
    public UInt16 NL_NumberOfLines { get; set; }

    /*
     * CFLG = 0  without compression
     * CFLG = 1  lossless compression
     * CFLG = 2  lossy compression method
     */
    public byte CFLG_CompressionFlag { get; set; }
}