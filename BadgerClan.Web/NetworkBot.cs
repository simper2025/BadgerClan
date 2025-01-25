using BadgerClan.Logic;
using BadgerClan.Logic.Bot;

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
        return moveResponse?.Moves ?? [];
    }
}
