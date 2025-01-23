using BadgerClan.Logic.Bot;

namespace BadgerClan.Logic;

public record Team
{

    public int Id;
    public string Color;
    public string Name;
    public IBot? Bot;
    public int Medpacs = 0;

    public Team(int id) : this(id, $"Team {id}", "red", new Nothing())
    {
        
    }

    public Team(int id, string name, string color, IBot bot)
    {
        Id = id;
        Name = name;
        Color = color;
        Bot = bot;        
    }

}
