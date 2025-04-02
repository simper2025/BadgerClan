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

        public Tournament(string name)
        {
            Name = name;
        }
    }
}
