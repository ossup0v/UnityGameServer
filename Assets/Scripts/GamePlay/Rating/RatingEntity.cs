using System;

public class RatingEntity
{
    public Guid PlayerId { get; set; }
    public string Username { get; set; }
    public int Killed { get; set; }
    public int KilledBots { get; set; }
    public int Died { get; set; }
    public int Team { get; set; }
}
