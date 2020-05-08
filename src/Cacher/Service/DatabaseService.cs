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

        internal void Add(SummonerStats summonerStats)
        {
            var summoner = Get(summonerStats.Name);
            if (summoner != null)
            {
                Update(summonerStats.LpLog.Last(), summoner);
            }
            else
            {
                _repository.Add(DocumentWrapper(summonerStats));
            }
        }
        private SummonerStats Get(string summonerName)
        {
            var summonerFromDb = _repository.Get(summonerName);
            return summonerFromDb is null ? null : DocumentUnwrapper(summonerFromDb);
        }
        
        private void Update(string newLp, SummonerStats summoner)
        {
            if (summoner.LpLog.Last().Equals(newLp)) return;
            summoner.LpLog.Add(newLp);
            _repository.Update(DocumentWrapper(summoner));
        }
    }
}