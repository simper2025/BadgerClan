using BadgerClan.Logic;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

string url = app.Configuration["ASPNETCORE_URLS"]?.Split(";")[1] ?? throw new Exception("Unable to find URL");
int port = new Uri(url).Port;

Console.Clear();
Console.WriteLine("Welcome to the Sample BadgerClan Bot!");
Console.WriteLine("Change the code in Program.cs to add custom behavior.");
Console.WriteLine("If you're running this locally, use the following URL to join your bot:");
Console.WriteLine();
Console.WriteLine($"\t{url}");
Console.WriteLine();
Console.WriteLine("For the competition, start a DevTunnel for this port with the following command:");
Console.WriteLine("\t winget install Microsoft.devtunnel");
Console.WriteLine("\t [ restart your command line after installing devtunnel ]");
Console.WriteLine("\t devtunnel user login");
Console.WriteLine($"\t devtunnel host -p {port} --allow-anonymous");
Console.WriteLine();
Console.WriteLine("In the output from the 'devtunnel host' command, look for the \"Connect via browser:\" URL");


app.MapGet("/", () => "Sample BadgerClan bot.  Modify the code in Program.cs to change how the bot performs.");

app.MapPost("/", (MoveRequest request) =>
{
    app.Logger.LogInformation("Received move request for game {gameId} turn {turnNumber}", request.GameId, request.TurnNumber);
    var myMoves = new List<Move>();

    // ***************************************************************************
    // ***************************************************************************
    // **
    // ** Your code goes right here.
    // ** Look in the request object to see the game state.
    // ** Then add your moves to the myMoves list.
    // **
    // ***************************************************************************
    // ***************************************************************************

    return new MoveResponse(myMoves);
});

app.Run();
