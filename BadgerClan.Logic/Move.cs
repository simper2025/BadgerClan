namespace BadgerClan.Logic;

public record Move(MoveType Type, int unitId, Coordinate target);

public enum MoveType{
    Walk,
    Attack,
}