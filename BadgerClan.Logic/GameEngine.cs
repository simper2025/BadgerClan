namespace BadgerClab.Logic;

public class GameEngine
{

    public GameState ProcessTurn(GameState state, List<Move> moves){
        
        foreach(var move in moves){

            switch(move.Type) {
                case MoveType.Walk:
                    var unit = state.Units.FirstOrDefault(u => u.Id == move.unitId);
                    if (unit != null){
                        if(unit.Location.Distance(move.target) == 1){
                            unit.Location = new Coordinate(move.target.Col, move.target.Row);
                        }
                    }
                break;
            }


        }

        return state;
    }
}
