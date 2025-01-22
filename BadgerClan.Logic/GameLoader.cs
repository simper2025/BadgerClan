
using BadgerClan.Logic.Bot;

namespace BadgerClan.Logic;

public class GameLoader
{

    public static GameState CreateGame(int count, Func<IBot> robot){
        var state = new GameState();

        for (int i = 0; i < count; i++){
            
        }
        return state;
    }

}