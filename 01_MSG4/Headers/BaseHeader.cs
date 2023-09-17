namespace _01_MSG4.Headers;

/*
 * Figure 4-2 General LRIT/HRIT Header Record Structure
 * https://www.cgms-info.org/wp-content/uploads/2021/10/cgms-lrit-hrit-global-specification-(v2-8-of-30-oct-2013).pdf
 */
public class BaseHeader
{
    private readonly byte _type;
    private readonly UInt16 _length;

    public BaseHeader(byte type, byte length)
    {
        _type = type;
        _length = length;
    }

    public byte Type // Table 4-3 Header Record Type
    {
        get => _type;
        set
        {
            if (value != _type)
                throw new Exception($"Тип заголовка = {value}, а должен быть {_type}");
        }
    }

    public virtual UInt16 Length // header record length
    {
        get => _length;
        set
        {
            if (value != _length)
                throw new Exception($"Длина заголовка = {value}, а должна быть {_length}");
        }
    }
}