using BadgerClan.Logic;
using BadgerClan.Logic.Bot;
using BadgerClan.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<Lobby>();
builder.Services.AddSingleton<RunAndGun>();
builder.Services.AddSingleton<Turtle>();
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


app.MapPost("/bots/turtle", async (MoveRequest request, ILogger<Program> logger, Turtle runAndGun) =>
{
    logger.LogInformation("turtle moved in game {gameId} Turn #{TurnNumber}", request.GameId, request.TurnNumber);
    // var first = request.Units.First();
    // logger.LogInformation("Total Units: {Count}; Unit: {Id} ({Q},{R})", 
    //     request.Units.Count(), first.Id, first.Location.Q, first.Location.R);
    var currentTeam = new Team(request.YourTeamId)
    {
        Medpacs = request.Medpacs
    };
    var gameState = new GameState(request.GameId, request.BoardSize, request.TurnNumber, request.Units.Select(FromDto), request.TeamIds, currentTeam);

    return new MoveResponse(await runAndGun.PlanMovesAsync(gameState));
});



app.MapPost("/bots/runandgun", async (MoveRequest request, ILogger<Program> logger, RunAndGun runAndGun) =>
{
    logger.LogInformation("runandgun moved in game {gameId} Turn #{TurnNumber}", request.GameId, request.TurnNumber);
    // var first = request.Units.First();
    // logger.LogInformation("Total Units: {Count}; Unit: {Id} ({Q},{R})", 
    //     request.Units.Count(), first.Id, first.Location.Q, first.Location.R);
    var currentTeam = new Team(request.YourTeamId)
    {
        Medpacs = request.Medpacs
    };
    var gameState = new GameState(request.GameId, request.BoardSize, request.TurnNumber, request.Units.Select(FromDto), request.TeamIds, currentTeam);

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

