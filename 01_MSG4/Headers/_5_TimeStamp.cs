namespace _01_MSG4.Headers;

/*
 * Подробнее смотри:
 *   Table 4-9 – Time Stamp
 * здесь:
 *   https://www.eumetsat.int/media/44518
 *
 * и
 *   4.2.2.5 Time Stamp Record
 * здесь:
 *   https://www.cgms-info.org/wp-content/uploads/2021/10/cgms-lrit-hrit-global-specification-(v2-8-of-30-oct-2013).pdf
 */
public class _5_TimeStamp : BaseHeader
{
    private readonly byte _CDS_P_Field = 64;

    public _5_TimeStamp() : base(5, 10)
    {
    }

    public byte CDS_P_Field
    {
        get => _CDS_P_Field;
        set
        {
            if (value != _CDS_P_Field)
                throw new Exception($"P-Field fixed value according to CCSDS = {value}, а должен быть {_CDS_P_Field}");
        }
    }

    public UInt16 CounterOfDaysFrom1Jan1958 { get; set; }
    public UInt32 MillisecondsOfDay { get; set; }
    public DateTime WriteDateTime { get; set; }
}