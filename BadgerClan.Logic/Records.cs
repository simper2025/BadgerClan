namespace BadgerClan.Logic;


public record MoveRequest(IEnumerable<UnitDto> Units, IEnumerable<int> TeamIds, int YourTeamId, int TurnNumber, string GameId, int BoardSize, int Medpacs, int NextMedpacValue);
public record MoveResponse(List<Move> Moves);
public record UnitDto(string Type, int Id, int Attack, int AttackDistance, int Health, int MaxHealth, double Moves, double MaxMoves, Coordinate Location, int Team);
