using BadgerClan.Logic;
using BadgerClan.Logic.Bot;
using BadgerClan.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<Lobby>();
builder.Services.AddSingleton<RunAndGun>();
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

app.MapGet("/bots/runandgun/join", (string gameId, ILogger<Program> logger, Lobby lobby, RunAndGun bot) =>
{
    logger.LogInformation("runandgun joined game {gameId}", gameId);
    var playerName = PlayerHelpers.GetRandomPlayerName();
    return new JoinGameResponse(playerName);
});
app.MapPost("/bots/runandgun/move", (MoveRequest request, ILogger<Program> logger, RunAndGun bot) =>
{
    logger.LogInformation("runandgun moved in game {gameId}", request.state.Name);
    return new MoveResponse(bot.PlanMoves(request.state));
});

app.Run();


public record JoinGameResponse(string playerName);
public record MoveRequest(GameState state);
public record MoveResponse(List<Move> moves);

