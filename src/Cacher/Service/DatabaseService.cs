using System;
using System.Linq;
using Amazon.DynamoDBv2.DocumentModel;
using Cacher.Database;
using Cacher.Model;

namespace Cacher.Service
{
    public class DatabaseService
    {
        private readonly DynamoDbRepository _repository = new DynamoDbRepository();

        private SummonerStats DocumentUnwrapper(Document document)
        {
            var name = document["SummonerName"].AsPrimitive().AsString();
            var rank = document["Rank"].AsPrimitive().AsString();
            var lp = document["LP"].AsPrimitiveList().AsListOfString();
            var tier = document["Tier"].AsPrimitive().AsString();
            var time = document["Time"].AsPrimitive().AsString();
            var timeAsDateTime = DateTime.Parse(time);

            return new SummonerStats(rank, lp, name, tier, timeAsDateTime);
        }
        private Document DocumentWrapper(SummonerStats summonerStats)
        {
            Document document = new Document
            {
                ["SummonerName"] = summonerStats.Name,
                ["Time"] = summonerStats.Time,
                ["Rank"] = summonerStats.Rank,
                ["LP"] = summonerStats.LpLog,
                ["Tier"] = summonerStats.Tier
            };
            return document;
        }
        
        private SummonerStats Get(string summonerName)
        {
            var summonerFromDb = _repository.Get(summonerName);
            return summonerFromDb is null ? null : DocumentUnwrapper(summonerFromDb);
        }
        
        internal void Add(SummonerStats summonerStats)
        {
            var summoner = Get(summonerStats.Name);
            Document summonerToAdd;
            if (summoner != null)
            {
                Console.Write(summonerStats.Name + " exists, updating.");
                var newLp = summonerStats.LpLog[0];
                if (!summoner.LpLog.Last().Equals(newLp)) return;
                summoner.LpLog.Add(newLp);
                summonerToAdd = DocumentWrapper(summoner);
                _repository.Update(summonerToAdd);
            }
            else
            {
                Console.Write(summonerStats.Name + " does not exist, adding.");
                summonerToAdd = DocumentWrapper(summonerStats);
                _repository.Add(summonerToAdd);
            }
        }
    }
}