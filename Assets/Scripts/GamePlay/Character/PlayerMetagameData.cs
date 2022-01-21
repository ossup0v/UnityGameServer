using System;

public class PlayerMetagameData
{
    public Guid Id { get; set; }
    public int Team { get; set; }
    public string Username { get; set; }

    public override string ToString()
    {
        return $"Id:{Id} Team:{Team} Username:{Username}";
    }
}
