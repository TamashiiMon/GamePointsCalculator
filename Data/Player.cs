using System.ComponentModel.DataAnnotations;
using Data;
using DataType = Data.DataType;

public class Player
{
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "First name is required")]
    public string PreName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; }

    [Range(0, 999, ErrorMessage = "Points must be between 0 and 999")]
    public int Points { get; set; }

    public Player()
    {
        UserId = DataContext.Players.Count + 1;
    }
    
    public string ToCSV()
    {
        return $"{this.PreName},{this.LastName},{this.Points},{this.UserId}";
    }


    public static Player FromCSV(string csv, Dictionary<DataType, int> types)
    {
        var values = csv.Split(",");
        
        int preNameIndex = types.TryGetValue(DataType.PRENAME, out preNameIndex) ? preNameIndex : 0;
        int lastNameIndex = types.TryGetValue(DataType.LASTNAME,  out lastNameIndex) ? lastNameIndex : 0;
        int pointsIndex = types.TryGetValue(DataType.POINTS, out pointsIndex) ? pointsIndex : 0;
        int userIdIndex = types.TryGetValue(DataType.USERID, out userIdIndex ) ? userIdIndex : 0;

        var player = new Player();
        player.PreName = values[preNameIndex];
        player.LastName = values[lastNameIndex];
        player.Points = int.Parse(values[pointsIndex]);
        player.UserId = int.Parse(values[userIdIndex]);
        return player;
    }
}