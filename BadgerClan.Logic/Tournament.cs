using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadgerClan.Logic
{
    public class Tournament
    {
        public bool Started = false;
        public string Name;
        public int TeamCount => Teams.Count;
        public Guid Id { get; } = Guid.NewGuid();

        public event Action<Tournament>? TournamentChanged;

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
            if(Started) return;
            Teams.Add(team);
            TournamentChanged?.Invoke(this);
        }
        public void RemoveTeam(Team team)
        {
            if(Started) return;
            if (Teams.Any(t => t.Name == team.Name))
            {
                Teams.RemoveAll(t => t.Name == team.Name);
            }
            TournamentChanged?.Invoke(this);
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
