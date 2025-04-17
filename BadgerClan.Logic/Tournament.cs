using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadgerClan.Logic
{
    public class Tournament
    {
        public bool Started = false;
        public string Name;
        public int TeamCount;
        public DateTime Created { get; } = DateTime.Now;
        public List<Team> Teams { get; init; }

        public List<GameState> GameStates { get; }

        public Tournament(string name)
        {
            Name = name;
            Teams = new List<Team>();
            GameStates = new List<GameState>();
        }

        public void AddTeam(Team team)
        {
            Teams.Add(team);
        }

        public void Start()
        {
            Started = true;
            var gameCount = Teams.Count/4;
            for (int i = 0; i < gameCount; i++)
            {
                GameStates.Add(new GameState(Name + ": Game #" + (gameCount + 1)));
            }

            
        }
    }
}
