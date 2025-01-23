using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadgerClan.Logic;

public class Lobby
{
    private List<GameState> games { get; } = new();
    public event Action<GameState> LobbyChanged;
    public void AddGame(string gameName)
    {
        var game = new GameState(gameName);
        games.Add(game);
        LobbyChanged?.Invoke(game);
    }
    public ReadOnlyCollection<GameState> Games => games.AsReadOnly();
}
