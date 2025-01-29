using BadgerClan.Logic;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

string url = app.Configuration["ASPNETCORE_URLS"]?.Split(";").Last() ?? throw new Exception("Unable to find URL");
int port = new Uri(url).Port;

Console.Clear();
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("Welcome to the Sample BadgerClan Bot!");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("The first time you run this program, please run the following two commands:");
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\t winget install Microsoft.devtunnel");//DevTunnel explanation: https://learn.microsoft.com/en-us/azure/developer/dev-tunnels/overview
Console.WriteLine("\t devtunnel user login");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine();
Console.WriteLine("Change the code in Program.cs to add custom behavior.");
Console.WriteLine();
Console.WriteLine("Use the following URL to join your bot:");
Console.WriteLine();
Console.Write($"\tLocal:  ");
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"{url}");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine();
Console.WriteLine("\tCompetition: 1) Start a DevTunnel for this port with the following command:");
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"\t                devtunnel host -p {port} --allow-anonymous");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine($"\t             2) Copy the \"Connect via browser\" URL from the DevTunnel output");
Console.WriteLine($"\t                (that will be your bot's URL)");
Console.WriteLine();
//Console.WriteLine("In the output from the 'devtunnel host' command, look for the \"Connect via browser:\" URL.  Paste that in the browser as your bot's address");


app.MapGet("/", () => "Sample BadgerClan bot.  Modify the code in Program.cs to change how the bot performs.");

app.MapPost("/", (GameState request) =>
{
    // ***************************************************************************
    // ***************************************************************************
    // **
    // ** Your code goes right here.
    // ** Look in the request object to see the game state.
    // ** Then add your moves to the myMoves list.
    // **
    // ***************************************************************************
    // ***************************************************************************

    var myMoves = SuperSimpleExampleBot.MakeMoves(request);//Very simple bot example.  Delete this line when you write your own bot.

    return new MoveResponse(myMoves);
});

app.Run();

public record GameState(IEnumerable<Unit> Units, IEnumerable<int> TeamIds, int YourTeamId, int TurnNumber, string GameId, int BoardSize, int Medpacs);
public record Unit(string Type, int Id, int Attack, int AttackDistance, int Health, int MaxHealth, double Moves, double MaxMoves, Coordinate Location, int Team);