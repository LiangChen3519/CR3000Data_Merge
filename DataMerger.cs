using System.Globalization;

namespace CR3000Data_Merge;

public class DataMerger
{
    public required List<string> PathList { get; init; }
    public required string OutputFilePath {get; init; }

    public required List<DateTime> FullTimeSeries { get; init; }

    // for all files we skip the 1st line, 3rd line and 4th line
    private List<int> _skipedLine = [0, 2, 3];

    public Dictionary<string, int> ConsolidateHeaders()
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

    public void MergeData(Dictionary<string, int> consolidatedHeaders)
    {
        // Dictionary to store data with TIMESTAMP as the key
        Dictionary<DateTime, Dictionary<string, string>> dataCargo =
            new Dictionary<DateTime, Dictionary<string, string>>();
        // Read and map data from each file
        foreach (var filePath in PathList)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                int lineIndex = 0;
                List<string> fileHeaders = new List<string>();

                while ((line = reader.ReadLine()) != null)
                {
                    if (_skipedLine.Contains(lineIndex))
                    {
                        lineIndex++;
                        continue;
                    }

                    if (lineIndex == 1) // header is with index of 1
                    {
                        fileHeaders = line.Split(',').ToList();
                        lineIndex++;
                        continue;
                    }

                    var columns = line.Split(',');
                    if (DateTime.TryParseExact(columns[0].Trim('\'','"'), "yyyy-MM-dd HH:mm:ss", 
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out DateTime timestamp))
                    {
                        if (!dataCargo.ContainsKey(timestamp))
                        {
                            dataCargo[timestamp] = new Dictionary<string, string>();
                        }

                        for (int i = 1; i < columns.Length; i++)
                        {
                            string header = fileHeaders[i];
                            dataCargo[timestamp][header] = columns[i];
                        }
                    }

                    lineIndex++;
                }
            }
        }

        // Write merged data to output file
        using (StreamWriter writer = new StreamWriter(OutputFilePath))
        {
            List<string> headers = new List<string> { "TIMESTAMP" };
            headers.AddRange(consolidatedHeaders.OrderBy(h => h.Value).Select(h => h.Key));
            writer.WriteLine(string.Join(",", headers));

            foreach (var timestamp in FullTimeSeries)
            {
                List<string> lineData = new List<string> { timestamp.ToString("yyyy-MM-dd HH:mm:ss") };

                foreach (var header in headers.Skip(0)) // Skip TIMESTAMP in consolidated headers
                {
                    if (dataCargo.ContainsKey(timestamp) && dataCargo[timestamp].ContainsKey(header))
                    {
                        lineData.Add(dataCargo[timestamp][header]);
                    }
                    else
                    {
                        lineData.Add("NA"); // Fill missing data with N/A
                    }
                }

                writer.WriteLine(string.Join(",", lineData));
            }
        }
    }
}