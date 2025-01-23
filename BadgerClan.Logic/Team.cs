using BadgerClan.Logic.Bot;

namespace BadgerClan.Logic;

public record Team
{
    private static int NextId = 1;
    public int Id;
    public string Color;
    public string Name;
    public IBot? Bot;
    public string BotEndpoint;
    public int Medpacs = 0;

    public Team(int id) : this($"Team {id}", "red", "")
    {
        Id = id;
        if (Id > NextId)
        {
            NextId = Id + 1;
        }
    }

    public Team(string name, string color, string endpoint)
    {
        Id = NextId++;
        Name = name;
        Color = color;
        Bot = null;
        BotEndpoint = endpoint;
    }

    public Team(string name, string color, IBot bot) : this(name, color, "")
    {
        Bot = bot;
    }

}
