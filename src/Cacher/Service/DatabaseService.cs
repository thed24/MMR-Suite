using System;
using System.Globalization;
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
            var name = document["SummonerName"].AsPrimitive();
            var time = document["Time"].AsPrimitive();
            var rank = document["Rank"].AsPrimitive();
            var lp = document["LP"].AsPrimitiveList();
            var tier = document["Tier"].AsPrimitive();

            return new SummonerStats(rank, lp, name, tier, time);
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
            document["Time"] = summonerStats.Time.ToString(CultureInfo.InvariantCulture);
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

        internal SummonerStats Get(string summonerName)
        {
            var summonerFromDb = _repository.Get(summonerName);
            return summonerFromDb is null ? null : DocumentUnwrapper(_repository.Get(summonerFromDb));
        }
    }
}