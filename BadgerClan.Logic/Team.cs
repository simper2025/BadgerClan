using BadgerClan.Logic.Bot;

namespace BadgerClan.Logic;

public record Team
{

    public int Id;
    public string Color;
    public string Name;
    public IBot? Bot;
    public string BotEndpoint;
    public int Medpacs = 0;

    public Team(int id) : this(id, $"Team {id}", "red", "")
    {

    }

    public Team(int id, string name, string color, string endpoint)
    {
        Id = id;
        Name = name;
        Color = color;
        Bot = null;
        BotEndpoint = endpoint;
    }

    public Team(int id, string name, string color, IBot bot) : this(id, name, color, "")
    {
        Bot = bot;
    }

}
