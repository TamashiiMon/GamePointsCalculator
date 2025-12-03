namespace Data;

public class Player
{
    public int UserId { get; set; }
    
    public string PreName { get; set; }
    public string LastName { get; set; }
    public int Points { get; set; }

    public Player()
    {
        UserId = DataContext.Players.Count + 1;
    }

    public bool isVailed()
    {
        if (UserId != 0 && PreName != "" && LastName != "")
        {
            return true;
        } 
        return false;
    }
}