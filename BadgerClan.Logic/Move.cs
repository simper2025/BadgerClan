namespace BadgerClan.Logic;

public record Move(MoveType Type, int UnitId, Coordinate Target);

public enum MoveType{
    Walk,
    Attack,
    Medpac,
}