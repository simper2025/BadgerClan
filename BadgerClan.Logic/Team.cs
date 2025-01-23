using BadgerClan.Logic.Bot;

namespace BadgerClan.Logic;

public record Team
{

    public int Id;
    public string Color;
    public IBot? Bot;
    public int Medpacs = 0;

    public Team(int id) : this(id, "red", null)
    {
    }

    public Team(int id, string color, IBot bot)
    {
        Id = id;
        Color = color;
        Bot = bot;        
    }

}
