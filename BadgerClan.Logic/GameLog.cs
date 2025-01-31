public record GameLog(
  int TurnNumber,
  LogType Type
);

public enum LogType
{
  Placed,
  Moved,
  Attacked,
  Healed,
  Died,
  
}