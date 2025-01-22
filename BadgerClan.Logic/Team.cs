using BadgerClan.Logic.Bot;

namespace BadgerClan.Logic;

public record Team
{

    public int Id;
    public string Color;
    public IBot? Bot;

    public Team(int id)
    {
        Id = id;
        Color = "red";
        Bot = null;
    }

}
