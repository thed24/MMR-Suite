using System;
using System.Collections.Generic;

namespace Cacher.Model
{
    public class SummonerStats
    {
        public SummonerStats(string rank, List<string> lp, string name, string tier, DateTime time)
        {
            LpLog = lp;
            Time = time;
            Rank = rank;
            Name = name;
            Tier = tier;
        }

        public DateTime Time { get; }
        public string Name { get; }
        public string Rank { get; }
        public List<string> LpLog { get; }
        public string Tier { get; }
    }
}