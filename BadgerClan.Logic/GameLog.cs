using BadgerClan.Logic;

public record GameLog(
  int TurnNumber,
  LogType Type,
  int? UnitId = null,
  Coordinate? SourceCoordinate = null,
  Coordinate? DestinationCoordinate = null
);

public enum LogType
{
  Placed,
  Moved,
  Attacked,
  Healed,
  Died,
  
}