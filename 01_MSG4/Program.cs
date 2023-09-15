using System.Runtime.InteropServices;
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
        FromBytes();
        // ThroughStructure<PrimeHeaderAsSequentialPack1>();
    }

    static void FromBytes()
    {
        using var fs = new FileStream(Paths.Get_MSG4_FilePath(), FileMode.Open);
        var primaryHdr = new byte[8];
        fs.Read(primaryHdr, 0, 8);

        // 1 байт
        var hdr_type = primaryHdr[0];

        // следующие 2 байта
        var hdr_rec_len = (UInt16)((primaryHdr[1] << 8) + primaryHdr[2]);

        // следующий 1 байт
        var file_type = primaryHdr[3];

        // следующие 4 байта
        var total_hdr_len = (UInt32)((primaryHdr[4] << 24) + (primaryHdr[5] << 16) + (primaryHdr[6] << 8) + primaryHdr[7]);

        Console.WriteLine();
    }

    static void ThroughStructure<T>()
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