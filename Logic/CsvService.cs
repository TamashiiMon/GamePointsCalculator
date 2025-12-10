using Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

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
    
    public static async Task DownloadCSV(IJSRuntime JS)
    {
        var bytes = CsvService.ExportCSV();
        var base64 = Convert.ToBase64String(bytes);

        string js = $@"
            const link = document.createElement('a');
            link.download = 'players.csv';
            link.href = 'data:text/csv;base64,{base64}';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        ";

        await JS.InvokeVoidAsync("eval", js);
    }

    public static async Task UploadCSV(IBrowserFile file)
    {
        var buffer = new byte[file.Size];

        using (var stream = file.OpenReadStream(10 * 1024 * 1024))
        {
            await stream.ReadAsync(buffer, 0, (int)file.Size);
        }

        var content = System.Text.Encoding.UTF8.GetString(buffer);
        var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        CsvService.ImportCSV(lines);
    }
}