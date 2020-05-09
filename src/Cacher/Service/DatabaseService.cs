namespace Cacher.Service
{
    using System;
    using System.Linq;
    using Amazon.DynamoDBv2.DocumentModel;
    using Cacher.Database;
    using Cacher.Model;

    public class DatabaseService
    {
        private readonly DynamoDbRepository _repository = new DynamoDbRepository();

        private SummonerStats DocumentUnwrapper(Document document)
        {
            var name = document["SummonerName"].AsString();
            var rank = document["Rank"].AsString();
            var lp = document["LP"].AsListOfString();
            var tier = document["Tier"].AsString();
            var time = document["Time"].AsString();
            var timeAsDateTime = DateTime.Parse(time);

            return new SummonerStats(rank, lp, name, tier, timeAsDateTime);
        }

        private Document DocumentWrapper(SummonerStats summonerStats)
        {
            var document = new Document
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
            if (summoner is null)
                _repository.Add(DocumentWrapper(summonerStats));
            else
                Update(summonerStats.LpLog.Last(), summoner);
        }

        private SummonerStats Get(string summonerName)
        {
            var summonerFromDb = _repository.Get(summonerName);
            return summonerFromDb != null && summonerFromDb.Any() ? DocumentUnwrapper(summonerFromDb) : null;
        }

        private void Update(string newLp, SummonerStats summoner)
        {
            if (summoner.LpLog.Last().Equals(newLp)) return;
            summoner.LpLog.Add(newLp);
            _repository.Update(DocumentWrapper(summoner));
        }
    }
}