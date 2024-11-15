﻿namespace CR3000Data_Merge;

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
       
       // check the datetime produce
       var startDate = DateTime.Parse(results.Item1);
       var endDate = DateTime.Parse(results.Item2);
       var results2 = dateManager.DateSetter(startDate, endDate,30);
       // check the data merger module
       DataMerger md = new DataMerger()
       {
           PathList = fileLists,
           OutputFilePath = "finnal.dat",
           FullTimeSeries = results2,
       };
       var myHeader = md.ConsolidateHeaders();
       Console.WriteLine("we got the dictionary");
       //print the dictionary
       foreach (var pair in myHeader)
       {
           Console.WriteLine($"{pair.Key}: {pair.Value} \n");
       }
       
       // check output data
       md.MergeData(myHeader);
        Console.WriteLine("we got the final data");
    }
}