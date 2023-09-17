namespace _01_MSG4.Headers;

/*
 * Подробнее смотри:
 *   4.2.2.3 Image Data Function Record
 * здесь:
 *   https://www.cgms-info.org/wp-content/uploads/2021/10/cgms-lrit-hrit-global-specification-(v2-8-of-30-oct-2013).pdf
 */
public class _3_ImageDataFunction : BaseHeader
{
    public _3_ImageDataFunction() : base(3, 0)
    {
    }

    public override UInt16 Length { get; set; }
    public string DataDefinitionBlock { get; set; }
}