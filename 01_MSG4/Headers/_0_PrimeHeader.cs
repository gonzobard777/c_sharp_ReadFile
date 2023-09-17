namespace _01_MSG4.Headers;

public class _0_PrimeHeader : BaseHeader
{
    public _0_PrimeHeader() : base(0, 16)
    {
    }

    public byte FileTypeCode { get; set; } // Table 4-2 File Type
    public UInt32 TotalHeaderLength { get; set; } // total header length of all header records
    public UInt64 DataLength { get; set; }
}