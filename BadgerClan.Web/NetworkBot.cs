using BadgerClan.Logic;
using BadgerClan.Logic.Bot;

public class NetworkBot : IBot
{
    HttpClient client = new()
    { 
        Timeout = TimeSpan.FromSeconds(.5)
    };

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
            state.CurrentTeam.Medpacs,
            state.NextMedpac
        );
        var response = await client.PostAsJsonAsync("", moveRequest);
        MoveResponse moveResponse;
        try
        {
            moveResponse = await response.Content.ReadFromJsonAsync<MoveResponse>();
        }
        catch (Exception e)
        {
            //TODO: Inject ilogger
            Console.WriteLine(e.Message);
            moveResponse = new MoveResponse(new List<Move>());
        }
        return moveResponse?.Moves ?? [];
    }
}
