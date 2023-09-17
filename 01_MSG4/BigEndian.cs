namespace _01_MSG4;

public class BigEndian
{
    public static UInt16 GetUInt16(byte[] buf, byte from) =>
        (UInt16)(
            (buf[from] << 8) +
            buf[from + 1]
        );

    public static UInt32 GetUInt32(byte[] buf, byte from) =>
        (UInt32)(
            (buf[from] << 24) +
            (buf[from + 1] << 16) +
            (buf[from + 2] << 8) +
            buf[from + 3]
        );

    public static Int32 GetInt32(byte[] buf, byte from) =>
        (Int32)GetUInt32(buf, from);

    public static UInt64 GetUInt64(byte[] buf, byte from) =>
        (UInt64)(
            (buf[from] << 56) +
            (buf[from + 1] << 48) +
            (buf[from + 2] << 40) +
            (buf[from + 3] << 32) +
            (buf[from + 4] << 24) +
            (buf[from + 5] << 16) +
            (buf[from + 6] << 8) +
            buf[from + 7]
        );

    public static string GetString(FileStream fs, byte from, byte count)
    {
        var buf = new byte[count];

        fs.Seek(from, SeekOrigin.Current);
        fs.Read(buf, 0, count);
        fs.Seek(-(from + count), SeekOrigin.Current);

        return System.Text.Encoding.UTF8.GetString(buf).Trim();
    }
}