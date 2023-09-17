using System.Runtime.InteropServices;
using _01_MSG4.Headers;
using Common;

namespace _01_MSG4;

/*
 * Проблема Big-endian / Little-endian: https://en.wikipedia.org/wiki/Endianness#Overview
 * Проблема возникает, если оперировать в памяти с последовательностью больше, чем один БАЙТ.
 * В памяти байты могут переверрнуться в обратный порядок.
 * Направление зависит от операционной системы.
 * Проверяется через: BitConverter.IsLittleEndian
 *
 * Группа The Co-ordination Group for Meteorological Satellites - CGMS, разработала спецификацию:
 * https://www.cgms-info.org/wp-content/uploads/2021/10/cgms-lrit-hrit-global-specification-(v2-8-of-30-oct-2013).pdf
 * Нас интересует Table 4-1 Primary Header Record, страница 14.
 */
public class Program
{
    public static void Main(string[] args)
    {
        Headers_FromBytes();
        // PrimaryHdr_ThroughStructure<PrimeHeaderAsSequentialPack1>();
    }

    static void Headers_FromBytes()
    {
        using var fs = new FileStream(Paths.Get_MSG4_FilePath(), FileMode.Open);

        var primeHdr = new _0_PrimeHeader();
        var imgStructHdr = new _1_ImageStructure();
        var imgNavHdr = new _2_ImageNavigation();
        var imgDataFunc = new _3_ImageDataFunction();
        var annotation = new _4_Annotation();
        var timeStamp = new _5_TimeStamp();
        var ancillaryText = new _6_AncillaryText();

//region Table 4-1 Primary Header Record
        var buf = new byte[primeHdr.Length];
        fs.Read(buf, 0, primeHdr.Length);

        primeHdr.Type = buf[0];
        primeHdr.Length = BigEndian.GetUInt16(buf, 1);
        primeHdr.FileTypeCode = buf[3];
        primeHdr.TotalHeaderLength = BigEndian.GetUInt32(buf, 4);
        primeHdr.DataLength = BigEndian.GetUInt64(buf, 8);
//endregion Table 4-1 Primary Header Record


        // Table 4-4 Image Structure Record
        if (CheckNextHeaderType(fs) == 1)
        {
            buf = new byte[imgStructHdr.Length];
            fs.Read(buf, 0, imgStructHdr.Length);

            imgStructHdr.Type = buf[0];
            imgStructHdr.Length = BigEndian.GetUInt16(buf, 1);
            imgStructHdr.NB_NumberOfBitsPerPixel = buf[3];
            imgStructHdr.NC_NumberOfColumns = BigEndian.GetUInt16(buf, 4);
            imgStructHdr.NL_NumberOfLines = BigEndian.GetUInt16(buf, 6);
            imgStructHdr.CFLG_CompressionFlag = buf[8];
        }

        // Table 4-5 Image Navigation Record
        if (CheckNextHeaderType(fs) == 2)
        {
            imgNavHdr.ProjName = BigEndian.GetString(fs, 3, 32);

            buf = new byte[imgNavHdr.Length];
            fs.Read(buf, 0, imgNavHdr.Length);

            imgNavHdr.Type = buf[0];
            imgNavHdr.Length = BigEndian.GetUInt16(buf, 1);
            imgNavHdr.CFAC_ColumnScalingFactor = BigEndian.GetInt32(buf, 35);
            imgNavHdr.LFAC_LineScalingFactor = BigEndian.GetInt32(buf, 39);
            imgNavHdr.COFF_ColumnOffset = BigEndian.GetInt32(buf, 43);
            imgNavHdr.LOFF_LineOffset = BigEndian.GetInt32(buf, 47);
        }

        // Table 4-6 Image Data Function Record
        if (CheckNextHeaderType(fs) == 3)
        {
            buf = new byte[3];
            fs.Read(buf, 0, 3);

            imgDataFunc.Type = buf[0];
            imgDataFunc.Length = BigEndian.GetUInt16(buf, 1);

            var textSize = (byte)(annotation.Length - 3);
            imgDataFunc.DataDefinitionBlock = BigEndian.GetString(fs, 0, textSize);
            fs.Seek(textSize, SeekOrigin.Current);
        }

        // Table 4-7 Annotation Record
        if (CheckNextHeaderType(fs) == 4)
        {
            buf = new byte[3];
            fs.Read(buf, 0, 3);

            annotation.Type = buf[0];
            annotation.Length = BigEndian.GetUInt16(buf, 1);

            var textSize = (byte)(annotation.Length - 3);
            annotation.Text = BigEndian.GetString(fs, 0, textSize);
            fs.Seek(textSize, SeekOrigin.Current);
        }

        // Table 4-8 Time Stamp Record
        if (CheckNextHeaderType(fs) == 5)
        {
            buf = new byte[timeStamp.Length];
            fs.Read(buf, 0, timeStamp.Length);

            timeStamp.Type = buf[0];
            timeStamp.Length = BigEndian.GetUInt16(buf, 1);
            timeStamp.CDS_P_Field = buf[3];
            timeStamp.CounterOfDaysFrom1Jan1958 = BigEndian.GetUInt16(buf, 4);
            timeStamp.MillisecondsOfDay = BigEndian.GetUInt32(buf, 6);
            timeStamp.WriteDateTime = new DateTime(1958, 1, 1)
                .AddDays(timeStamp.CounterOfDaysFrom1Jan1958)
                .AddMilliseconds(timeStamp.MillisecondsOfDay);
        }

        // 4.2.2.6 Ancillary Text Record
        if (CheckNextHeaderType(fs) == 6)
        {
            buf = new byte[3];
            fs.Read(buf, 0, 3);

            ancillaryText.Type = buf[0];
            ancillaryText.Length = BigEndian.GetUInt16(buf, 1);

            var textSize = (byte)(ancillaryText.Length - 3);
            ancillaryText.Text = BigEndian.GetString(fs, 0, textSize);
            fs.Seek(textSize, SeekOrigin.Current);
        }

        Console.WriteLine();
    }

    static byte CheckNextHeaderType(FileStream fs)
    {
        var buf = new byte[1];
        fs.Read(buf, 0, 1);
        fs.Seek(-1, SeekOrigin.Current);

        return buf[0];
    }


    static void PrimaryHdr_ThroughStructure<T>()
    {
        using var fs = new FileStream(Paths.Get_MSG4_FilePath(), FileMode.Open);

        var structLen = Marshal.SizeOf(typeof(T));
        var primaryHdrBuf = new byte[structLen];
        var len = fs.Read(primaryHdrBuf, 0, structLen);

        var bufPtr = GCHandle.Alloc(primaryHdrBuf, GCHandleType.Pinned);
        var primaryHdr = Marshal.PtrToStructure(bufPtr.AddrOfPinnedObject(), typeof(T));
        bufPtr.Free();

        Console.WriteLine();
    }
}

struct PrimeHeaderAsIs
{
    public byte hdr_type; // 1 байт
    public UInt16 hdr_rec_len; // следующие 2 байта
    public byte file_type; // следующий 1 байт
    public UInt32 total_hdr_len; // следующие 4 байта
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct PrimeHeaderAsSequentialPack1
{
    public byte hdr_type; // 1 байт
    public UInt16 hdr_rec_len; // следующие 2 байта
    public byte file_type; // следующий 1 байт
    public UInt32 total_hdr_len; // следующие 4 байта
}

[StructLayout(LayoutKind.Explicit)]
struct PrimeHeaderAsExplicit
{
    [FieldOffset(0)] public byte hdr_type; // 1 байт

    [FieldOffset(1)] public UInt16 hdr_rec_len; // следующие 2 байта

    [FieldOffset(3)] public byte file_type; // следующий 1 байт

    [FieldOffset(4)] public UInt32 total_hdr_len; // следующие 4 байта
}