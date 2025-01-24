using BadgerClan.Logic.Bot;

namespace BadgerClan.Logic;

public record Team
{
    private static int NextId = 1;
    public int Id { get; }
    public string Color { get; }
    public string Name { get; }
    public IBot Bot { get; }
    public int Medpacs { get; set; } = 0;

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
        if (Id > NextId)
            NextId = Id + 1;
        Name = $"Team {teamId}";
        Color = "black";
        Bot = new NothingBot();
    }

    public async Task<List<Move>> PlanMovesAsync(GameState state) => await Bot.PlanMovesAsync(state);
}
