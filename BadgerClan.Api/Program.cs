using BadgerClan.Api.Moves;
using BadgerClan.Logic;
using BadgerClan.Logic.Bot;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

string url = app.Configuration["ASPNETCORE_URLS"]?.Split(";").Last() ?? throw new Exception("Unable to find URL");
int port = new Uri(url).Port;



app.MapGet("/RunAndGun", (HttpContext context) =>
{
    app.Logger.LogInformation("Received Run and Gun Request");
    Strategies.Strategy = "runandgun";
});


app.MapGet("/DoNothing", (HttpContext context) =>
{
    app.Logger.LogInformation("Received Do Nothing Request");
    Strategies.Strategy = "";
});

app.MapGet("/Turtle", (HttpContext context) =>
{
    app.Logger.LogInformation("Received Turtle Request");
    Strategies.Strategy = "turtle";
});


app.MapPost("/", (MoveRequest request) =>
{
    app.Logger.LogInformation("Received move request for game {gameId} turn {turnNumber}", request.GameId, request.TurnNumber);
    var moves = new List<Move>();

    switch (Strategies.Strategy)
    {
        case "runandgun":
            Strategies.RunAndGun(request, moves);
            break;
        case "turtle":
            Strategies.Turtle(request, moves);
            break;
        default:
            app.Logger.LogInformation("No strategie chose.");
            moves = new List<Move>();
            break;
    }
    return new MoveResponse(moves);

});

app.Run();

