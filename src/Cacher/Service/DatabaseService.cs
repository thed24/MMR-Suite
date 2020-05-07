using System;
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

        private Document DocumentWrapper(string summonerName)
        {
            var summonerDocument = new Document();
            summonerDocument["SummonerName"] = summonerName;
            return summonerDocument;
        }

        private Document DocumentWrapper(SummonerStats summonerStats)
        {
            var document = DocumentWrapper(summonerStats.Name);
            document["Time"] = summonerStats.Time;
            document["Rank"] = summonerStats.Rank;
            document["LP"] = summonerStats.LpLog;
            document["Tier"] = summonerStats.Tier;
            return document;
        }

        internal void Add(SummonerStats summonerStats)
        {
            var summoner = Get(summonerStats.Name);
            if (summoner != null)
            {
                Console.Write(summonerStats.Name + " exists, updating.");
                summoner.LpLog.Add(summonerStats.LpLog[0]);
                var summonerToAdd = DocumentWrapper(summoner);
                _repository.Add(summonerToAdd);
            }
            else
            {
                Console.Write(summonerStats.Name + " does not exist, adding.");
                var summonerToAdd = DocumentWrapper(summonerStats);
                _repository.Add(summonerToAdd);
            }
        }

        private SummonerStats Get(string summonerName)
        {
            var summonerFromDb = _repository.Get(summonerName);
            return summonerFromDb is null ? null : DocumentUnwrapper(summonerFromDb);
        }
    }
}