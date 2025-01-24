using BadgerClan.Logic;
using BadgerClan.Logic.Bot;
using BadgerClan.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<Lobby>();
builder.Services.AddSingleton<BotStore>();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.MapPost("/bots/turtle", async (MoveRequest request, ILogger<Program> logger, BotStore botStore) =>
{
    logger.LogInformation("turtle moved in game {gameId} Turn #{TurnNumber}", request.GameId, request.TurnNumber);
    var currentTeam = new Team(request.YourTeamId)
    {
        Medpacs = request.Medpacs
    };
    var gameState = new GameState(request.GameId, request.BoardSize, request.TurnNumber, request.Units.Select(FromDto), request.TeamIds, currentTeam);
    var turtle = botStore.GetBot<Turtle>(gameState.Id, currentTeam.Id);

    return new MoveResponse(await turtle.PlanMovesAsync(gameState));
});


app.MapPost("/bots/nothing", async (MoveRequest request, ILogger<Program> logger, BotStore botStore) =>
{
    logger.LogInformation("runandgun moved in game {gameId} Turn #{TurnNumber}", request.GameId, request.TurnNumber);
    var currentTeam = new Team(request.YourTeamId)
    {
        Medpacs = request.Medpacs
    };
    var gameState = new GameState(request.GameId, request.BoardSize, request.TurnNumber, request.Units.Select(FromDto), request.TeamIds, currentTeam);
    var donothing = botStore.GetBot<NothingBot>(gameState.Id, currentTeam.Id);

    return new MoveResponse(await donothing.PlanMovesAsync(gameState));
});


app.MapPost("/bots/runandgun", async (MoveRequest request, ILogger<Program> logger, BotStore botStore) =>
{
    logger.LogInformation("runandgun moved in game {gameId} Turn #{TurnNumber}", request.GameId, request.TurnNumber);
    var currentTeam = new Team(request.YourTeamId)
    {
        Medpacs = request.Medpacs
    };
    var gameState = new GameState(request.GameId, request.BoardSize, request.TurnNumber, request.Units.Select(FromDto), request.TeamIds, currentTeam);
    var runAndGun = botStore.GetBot<RunAndGun>(gameState.Id, currentTeam.Id);

    return new MoveResponse(await runAndGun.PlanMovesAsync(gameState));
});

app.Run();

Unit FromDto(UnitDto dto)
{
    return Unit.Factory(
        dto.Type,
        dto.Id,
        dto.Attack,
        dto.AttackDistance,
        dto.Health,
        dto.MaxHealth,
        dto.Moves,
        dto.MaxMoves,
        dto.Location,
        dto.Team
    );
}



public record JoinGameResponse(string playerName);
public record MoveRequest(IEnumerable<UnitDto> Units, IEnumerable<int> TeamIds, int YourTeamId, int TurnNumber, string GameId, int BoardSize, int Medpacs);
public record MoveResponse(List<Move> Moves);
public record UnitDto(string Type, int Id, int Attack, int AttackDistance, int Health, int MaxHealth, double Moves, double MaxMoves, Coordinate Location, int Team);

public class NetworkBot : IBot
{

    HttpClient client = new();
    public NetworkBot(Uri endpoint)
    {
        client.BaseAddress = endpoint;
    }
    UnitDto MakeDto(Unit unit)
    {
        return new UnitDto(
            unit.Type,
            unit.Id,
            unit.Attack,
            unit.AttackDistance,
            unit.Health,
            unit.MaxHealth,
            unit.Moves,
            unit.MaxMoves,
            unit.Location,
            unit.Team
        );
    }

    public async Task<List<Move>> PlanMovesAsync(GameState state)
    {
        var moveRequest = new MoveRequest(
            state.Units.Select(MakeDto),
            state.TeamList.Select(t => t.Id),
            state.CurrentTeamId,
            state.TurnNumber,
            state.Id.ToString(),
            state.Dimension,
            state.CurrentTeam.Medpacs
        );
        var response = await client.PostAsJsonAsync("", moveRequest);
        var moveResponse = await response.Content.ReadFromJsonAsync<MoveResponse>();
        return moveResponse.Moves;
    }
}

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
