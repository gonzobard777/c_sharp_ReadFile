namespace _01_MSG4.Headers;

/*
 * Подробнее смотри:
 *   4.2.2.4 Annotation Record
 * здесь:
 *   https://www.cgms-info.org/wp-content/uploads/2021/10/cgms-lrit-hrit-global-specification-(v2-8-of-30-oct-2013).pdf
 */
public class _4_Annotation : BaseHeader
{
    public _4_Annotation() : base(4, 0)
    {
    }

    public override UInt16 Length { get; set; }
    public string Text { get; set; }
}