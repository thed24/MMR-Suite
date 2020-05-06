using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DocumentModel;

namespace Cacher.Model
{
    public class SummonerStats
    {
        public SummonerStats(string rank, int lp, string name, string tier, DateTime time)
        {
            LpLog = new List<string> {lp.ToString()};
            Time = time;
            Rank = rank;
            Name = name;
            Tier = tier;
        }

        public SummonerStats(string rank, int lp, string name, string tier)
        {
            LpLog = new List<string> {lp.ToString()};
            Time = DateTime.Now;
            Rank = rank;
            Name = name;
            Tier = tier;
        }

        public SummonerStats(Primitive rank, PrimitiveList lp, Primitive name, Primitive tier, Primitive time)
        {
            LpLog = lp.AsListOfString();
            Time = time.AsDateTime();
            Rank = rank.AsString();
            Name = name.AsString();
            Tier = tier.AsString();
        }

        public DateTime Time { get; }
        public string Name { get; }
        public string Rank { get; }
        public List<string> LpLog { get; }
        public string Tier { get; }
    }
}