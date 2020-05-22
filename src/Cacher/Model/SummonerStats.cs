namespace Cacher.Model
{
    using System;
    using System.Collections.Generic;

    public class SummonerStats
    {
        public DateTime Time { get; }
        public string Name { get; }
        public string Rank { get; }
        public List<string> LpLog { get; }
        public string Tier { get; }
        public SummonerStats(string rank, List<string> lp, string name, string tier, DateTime time)
        {
            LpLog = lp;
            Time = time;
            Rank = rank;
            Name = name;
            Tier = tier;
        }
    }
}