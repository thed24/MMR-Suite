namespace Cacher.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cacher.Model;
    using RiotSharp;
    using RiotSharp.Endpoints.LeagueEndpoint;
    using RiotSharp.Misc;

    internal class RiotService
    {
        private const Region Region = RiotSharp.Misc.Region.Oce;
        private readonly RiotApi _api;

        public RiotService(RiotApi api)
        {
            _api = api;
        }

        internal SummonerStats CreateSummonerStats(string summonerId, string summonerName)
        {
            var summonerLeague = _api.League.GetLeagueEntriesBySummonerAsync(Region.Oce, summonerId)
                .Result.First(x => x.QueueType.Equals(Queue.RankedSolo5x5));

            var leagueEntries = _api.League.GetLeagueByIdAsync(Region.Oce, summonerLeague.LeagueId).Result
                .Entries;

            var rank = leagueEntries.Find(x => x.SummonerName.Equals(summonerName)).Rank;
            var tier = summonerLeague.Tier;
            var lp = new List<string>
            {
                leagueEntries.Find(x => x.SummonerName.Equals(summonerName)).LeaguePoints.ToString()
            };

            return new SummonerStats(rank, lp, summonerName, tier, DateTime.Now);
        }

        internal IEnumerable<LeagueItem> GetSummonersInSummonerLeague(string summonerName)
        {
            var summoner = _api.Summoner.GetSummonerByNameAsync(Region, summonerName).Result;
            var summonerLeagues = _api.League.GetLeagueEntriesBySummonerAsync(Region, summoner.Id).Result;
            return _api.League.GetLeagueByIdAsync(Region, summonerLeagues[0].LeagueId).Result.Entries;
        }
    }
}