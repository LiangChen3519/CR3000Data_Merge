namespace CR3000Data_Merge;

public class DataMerger
{
    public required List<string> PathList { get; init; }
    //public required List<DateTime> FullTimeSeries { get; init; }
    // for all files we skip the 1st line, 3rd line and 4th line
    private List<int> skipedLine = [0, 2, 3];

    public Dictionary<string,int> ConsolidateHeaders()
    {
        var consolidatedHeader = new Dictionary<string, int>();
        int colIndex = 1;
        foreach (var file in PathList)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string header = null;
                int lineIndex = 0;
                while ((header = sr.ReadLine()) != null)
                {
                    if (lineIndex == 1)
                    {
                        var heads = header.Split(",");
                        foreach (var h in heads.Skip(0))
                        {
                            if (!consolidatedHeader.ContainsKey(h))
                            {
                                consolidatedHeader[h] = colIndex;
                                colIndex++;
                            }
                        }
                        
                    }
                    lineIndex++;
                }
            }
        }
        
        return consolidatedHeader;
    }
}