using BadgerClan.Logic;

public class SuperSimpleExampleBot
{
    public static List<Move> MakeMoves(GameState gameState)
    {
        var myTeamId = gameState.YourTeamId;
        var myUnits = findMyUnits(myTeamId, gameState.Units);
        var enemies = findEnemies(myTeamId, gameState.Units);
        var moves = new List<Move>();

        foreach (var unit in myUnits)
        {
            var closestEnemy = findClosest(unit, enemies);
            bool iCanAttack = closestEnemy.Location.Distance(unit.Location) <= unit.AttackDistance;
            bool iHaveHealthPacksAvailable = gameState.Medpacs > 0;
            bool iNeedHealth = unit.Health < unit.MaxHealth;

            if (iCanAttack)
            {
                //You are allowed two movements per turn.
                moves.Add(new Move(MoveType.Attack, unit.Id, closestEnemy.Location));
                moves.Add(new Move(MoveType.Attack, unit.Id, closestEnemy.Location));
            }
            else if (iNeedHealth && iHaveHealthPacksAvailable)
            {
                moves.Add(new Move(MoveType.Medpac, unit.Id, unit.Location));
            }
            else
            {
                moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(closestEnemy.Location)));
            }
        }

        return moves;
    }

    private static List<Unit> findMyUnits(int myTeamId, IEnumerable<Unit> units)
    {
        var myUnits = new List<Unit>();
        foreach (var unit in units)
        {
            if (unit.Team == myTeamId)
            {
                myUnits.Add(unit);
            }
        }

        return myUnits;
    }

    private static List<Unit> findEnemies(int myTeamId, IEnumerable<Unit> units)
    {
        var enemies = new List<Unit>();
        foreach (var unit in units)
        {
            if (unit.Team != myTeamId)
            {
                enemies.Add(unit);
            }
        }

        return enemies;
    }

    private static Unit findClosest(Unit unit, List<Unit> otherUnits)
    {
        var closest = otherUnits[0];
        foreach (var enemy in otherUnits)
        {
            if (enemy.Location.Distance(unit.Location) < closest.Location.Distance(unit.Location))
            {
                closest = enemy;
            }
        }

        return closest;
    }
}