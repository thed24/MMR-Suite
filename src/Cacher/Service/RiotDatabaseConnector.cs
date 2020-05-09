namespace Cacher.Service
{
    using System;
    using Amazon.DynamoDBv2.DocumentModel;
    using Cacher.Model;

    public static class RiotDatabaseConnector
    {
        public static SummonerStats DocumentUnwrapper(Document document)
        {
            var name = document["SummonerName"].AsString();
            var rank = document["Rank"].AsString();
            var lp = document["LP"].AsListOfString();
            var tier = document["Tier"].AsString();
            var time = document["Time"].AsString();
            var timeAsDateTime = DateTime.Parse(time);

            return new SummonerStats(rank, lp, name, tier, timeAsDateTime);
        }

        public static Document DocumentWrapper(SummonerStats summonerStats)
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
    }
}