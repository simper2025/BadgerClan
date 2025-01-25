using BadgerClan.Logic.Bot;

public class BotStore
{
    private readonly IBot nothingBot = new NothingBot();
    private readonly Dictionary<(Guid GameId, int TeamId), IBot> bots = new();
    public void AddBot(Guid GameId, int TeamId, IBot bot) => bots[(GameId, TeamId)] = bot;
    public IBot GetBot<T>(Guid GameId, int TeamId) where T : IBot, new()
    {
        if (!bots.ContainsKey((GameId, TeamId)))
        {
            T bot = new();
            bots[(GameId, TeamId)] = bot;
        }

        return bots[(GameId, TeamId)];
    }
}
