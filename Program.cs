namespace CR3000Data_Merge;

class Program
{
    static void Main(string[] args)
    {
        const string filePath = @"C:\Users\liangch\OneDrive - University of Eastern Finland\Documents\MyDesktopFile\phd_things\Projects\JukkaPumpanen\CR3000\LoggerNet_Jukka_variio";
        FileChecker fileChecker = new FileChecker
        {
            FilePath = filePath
        };
       List<string> fileLists= fileChecker.FileLists();
       foreach (var file in fileLists)
       {
           Console.WriteLine(file);
       }
       
       DateManager dateManager = new DateManager()
       {
           PathList = fileLists,
       };
       var results = dateManager.DateGetter();
       Console.WriteLine($"The begin datetime is {results.Item1}, the end datetime is {results.Item2}");
    }
}