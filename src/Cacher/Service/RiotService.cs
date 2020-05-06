using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cacher.Model;
using RiotSharp;
using RiotSharp.Endpoints.LeagueEndpoint;
using RiotSharp.Endpoints.LeagueEndpoint.Enums;
using RiotSharp.Endpoints.SummonerEndpoint;
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
            const string queue = Queue.RankedSolo5x5;
            Thread.Sleep(3000);
            var summonerLeagueEntries = _api.League.GetLeagueEntriesBySummonerAsync(Region.Oce, summonerId)
                .Result.First(x => x.QueueType.Equals(queue));
            Thread.Sleep(3000);
            var leagueEntriesAsync = _api.League.GetLeagueByIdAsync(Region.Oce, summonerLeagueEntries.LeagueId).Result
                .Entries;
            var rank = leagueEntriesAsync.Find(x => x.SummonerName.Equals(summonerName)).Rank;
            var lp = leagueEntriesAsync.Find(x => x.SummonerName.Equals(summonerName)).LeaguePoints;
            var tier = summonerLeagueEntries.Tier;

            return new SummonerStats(rank, lp, summonerName, tier);
        }

        internal List<LeagueItem> GetSummonersInSummonerLeague(string summonerName)
        {
            Thread.Sleep(3000);
            var summoner = _api.Summoner.GetSummonerByNameAsync(_region, summonerName).Result;
            Thread.Sleep(3000);
            var summonerLeagues = _api.League.GetLeagueEntriesBySummonerAsync(_region, summoner.Id).Result;
            Thread.Sleep(3000);
            return _api.League.GetLeagueByIdAsync(_region, summonerLeagues[0].LeagueId).Result.Entries;
        }
    }
}