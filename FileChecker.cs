namespace CR3000Data_Merge;

public class FileChecker
{
    public required string FilePath { get; init; }
  
    private readonly string[] _excludeKeyWords = ["Constants", "Flags", "Daily", "Info"];

    public List<string> FileLists()
    {
        if (!Directory.Exists(FilePath))
        {
            throw new DirectoryNotFoundException($"{FilePath} is not a valid directory");
        }
        Console.WriteLine($"{FilePath} is a valid directory");
        var fileLists = Directory.GetFiles(FilePath, "*.dat", SearchOption.AllDirectories)
            .Where(file => !_excludeKeyWords.Any(file.Contains))
            .ToList();
        return fileLists;
    }
}