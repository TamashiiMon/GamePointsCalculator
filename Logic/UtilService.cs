using Data;

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
            if (string.IsNullOrEmpty(winnerText)) winnerText = highestMember.PreName;
            else  winnerText += " & " + highestMember.PreName;
        }
        
        return "Winner is: " + winnerText;
    }
}