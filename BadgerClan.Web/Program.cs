// Ignore Spelling: Dto

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
