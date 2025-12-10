using Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Logic;

public class UtilService
{
    public static String getWinner()
    {
        if (DataContext.Players.Count == 0) return "No players found";
        
        var maxValue = DataContext.Players.Max(member => member.Points);
        var highestMembers = DataContext.Players.Where(m => m.Points == maxValue).ToList();
        var winnerText = string.Empty;
       
        foreach (var highestMember in highestMembers)
        {
            if (string.IsNullOrEmpty(winnerText)) winnerText = highestMember.ToString();
            else  winnerText += " & " + highestMember;
        }
        
        return "Winner is: " + winnerText;
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