namespace Common;

public class Paths
{
    public static string Get_MSG4_FilePath()
    {
        return new FileInfo(Path.Combine(
                // solution dir
                new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName,
                "files",
                "H-000-MSG4__-MSG4________-VIS006___-000008___-201802281500-__"
            ))
            .FullName;
    }
}