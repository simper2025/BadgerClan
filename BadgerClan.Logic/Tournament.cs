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
        public bool Ended = false;
        public string Name;
        public int TeamCount => Teams.Count;
        public Guid Id { get; } = Guid.NewGuid();

        public const int MininumTeamCount = 4;

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
            if(Ended) return;
            Teams.Add(team);
            TournamentChanged?.Invoke(this);
        }
        public void RemoveTeam(Team team)
        {
            if(Ended) return;
            if (Teams.Any(t => t.Name == team.Name))
            {
                Teams.RemoveAll(t => t.Name == team.Name);
            }
            TournamentChanged?.Invoke(this);
        }

    }
}
