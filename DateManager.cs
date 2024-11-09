namespace CR3000Data_Merge;

public class DateManager
{
    // path a list with available file path with .dat data
    public required List<string> PathList { get; init; }

    // deifine a method get all first date and last date
    public (string, string) DateGetter()
    {
        var startDates = new List<string>
        {
            Capacity = 0
        };
        //var EndDates = new List<string>();
        if (PathList.Count > 0)
        {
            var l = PathList.Count;
            for (var i = 0; i < l; i++)
            {
                using (StreamReader sr = new StreamReader(PathList[i]))
                {
                    int lineNumber = 0;
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        if (lineNumber > 3)
                        {
                            var date = line.Split(',')[0];
                            startDates.Add(date);
                        }

                        lineNumber++;
                        line = sr.ReadLine();
                    }
                }
            }
        }

        // We convert these string to datetime and sort
        var sortDates = startDates
            .Select(date => date.Trim('\'','"'))
            .OrderBy(DateTime.Parse)
            .ToList();
        return (sortDates[0], sortDates[^1]);
    }


    public void DateSetter(string startDate, string endDate)
    {
        
    }
}