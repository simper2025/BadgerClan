using BadgerClan.Logic.Bot;

namespace BadgerClan.Logic;

public record Team
{
    private static int NextId = 1;
    public int Id;
    public string Color;
    public string Name;
    public IBot Bot;
    public int Medpacs = 0;

    public Team(string name, string color, IBot bot)
    {
        Id = NextId++;
        Name = name;
        Color = color;
        Bot = bot;
    }
    public Team(int teamId)
    {
        Id = teamId;
        Name = $"Team {teamId}";
        Color = "black";
        Bot = new NothingBot();
    }

    public async Task<List<Move>> PlanMovesAsync(GameState state) => await Bot.PlanMovesAsync(state);
}
