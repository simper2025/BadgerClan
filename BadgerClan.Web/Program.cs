// Ignore Spelling: Dto

using BadgerClan.Logic;
using BadgerClan.Logic.Bot;
using BadgerClan.Web.Components;
using BadgerClan.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<Lobby>();
builder.Services.AddSingleton<BotStore>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.MapPost("/bots/{botname}", 
    async (string botname, MoveRequest request, ILogger<Program> logger, BotStore botStore) =>
{
    logger.LogInformation("{botname} moved in game {gameId} Turn #{TurnNumber}", 
        botname, request.GameId, request.TurnNumber);

    var currentTeam = new Team(request.YourTeamId)
    {
        Medpacs = request.Medpacs
    };
    var gameState = new GameState(request.GameId, request.BoardSize, request.TurnNumber, request.Units.Select(FromDto), request.TeamIds, currentTeam);

    IBot ibot;
    switch (botname)
    {
        case "turtle":
            ibot = botStore.GetBot<Turtle>(gameState.Id, currentTeam.Id);
            break;
        case "runandgun":
            ibot = botStore.GetBot<RunAndGun>(gameState.Id, currentTeam.Id);
            break;
        default:
            ibot = botStore.GetBot<NothingBot>(gameState.Id, currentTeam.Id);
            break;
    }
    return new MoveResponse(await ibot.PlanMovesAsync(gameState));
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
