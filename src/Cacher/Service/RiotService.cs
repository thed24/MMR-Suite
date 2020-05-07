using System;
using System.Collections.Generic;
using System.Linq;
using Cacher.Model;
using RiotSharp;
using RiotSharp.Endpoints.LeagueEndpoint;
using RiotSharp.Misc;

namespace Cacher.Service
{
    internal class RiotService
    {
        private readonly RiotApi _api;
        private readonly Region _region = Region.Oce;

        public RiotService(RiotApi api)
        {
            _api = api;
        }

        internal SummonerStats GetSummonerStats(string summonerId, string summonerName)
        {
            var summonerLeagueEntries = _api.League.GetLeagueEntriesBySummonerAsync(Region.Oce, summonerId)
                .Result.First(x => x.QueueType.Equals(Queue.RankedSolo5x5));

            var leagueEntriesAsync = _api.League.GetLeagueByIdAsync(Region.Oce, summonerLeagueEntries.LeagueId).Result
                .Entries;

            var rank = leagueEntriesAsync.Find(x => x.SummonerName.Equals(summonerName)).Rank;
            var tier = summonerLeagueEntries.Tier;
            var lp = new List<string>();
            lp.Add(leagueEntriesAsync.Find(x => x.SummonerName.Equals(summonerName)).LeaguePoints.ToString());

            return new SummonerStats(rank, lp, summonerName, tier, DateTime.Now);
        }

        internal List<LeagueItem> GetSummonersInSummonerLeague(string summonerName)
        {
            var summoner = _api.Summoner.GetSummonerByNameAsync(_region, summonerName).Result;
            var summonerLeagues = _api.League.GetLeagueEntriesBySummonerAsync(_region, summoner.Id).Result;
            return _api.League.GetLeagueByIdAsync(_region, summonerLeagues[0].LeagueId).Result.Entries;
        }
    }
}