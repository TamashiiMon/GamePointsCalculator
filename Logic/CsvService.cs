using Data;

namespace Logic;

public class CsvService
{
    public static byte[] ExportCSV()
    {
        using var memoryStream = new MemoryStream();
        using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
        {
            writer.WriteLine($"{DataType.PRENAME},{DataType.LASTNAME},{DataType.POINTS},{DataType.USERID}");

            foreach (var player in DataContext.Players)
            {
                writer.WriteLine(player.ToCSV());
            }
            writer.Flush();
        }
        return memoryStream.ToArray();
    }

    
    public static void ImportCSV(string[] lines)
    {

        Dictionary<DataType, int> DataTypeMap = new();
        DataContext.Players.Clear();
        foreach (var line in lines)
        {
            if (line.EndsWith("USERID"))
            {
                string[] columns = line.Split(',');
                int columnsCount = 0;
                foreach (var column in columns)
                {
                    if (column == "USERID") DataTypeMap.TryAdd(DataType.USERID, columnsCount);
                    else if (column == "PRENAME") DataTypeMap.TryAdd(DataType.PRENAME, columnsCount);
                    else if (column == "LASTNAME") DataTypeMap.TryAdd(DataType.LASTNAME, columnsCount);
                    else if (column == "POINTS") DataTypeMap.TryAdd(DataType.POINTS, columnsCount);
                    columnsCount++;
                }
                continue;
            }
            DataContext.Players.Add(Player.FromCSV(line, DataTypeMap));
        }
    }
}