using System;
using System.Threading;

namespace Cacher.Service
{
    internal class SummonerController
    {
        private readonly RiotService _connector;
        private readonly DatabaseService _database;

        public SummonerController(RiotService connector, DatabaseService database)
        {
            _connector = connector;
            _database = database;
        }

        public void ParseInput(string input)
        {
            var entries = _connector.GetSummonersInSummonerLeague(input);

            //
            foreach (var entry in entries)
                try
                {
                    var summonersStats = _connector.GetSummonerStats(entry.SummonerId, entry.SummonerName);
                    _database.Add(summonersStats);
                    Thread.Sleep(3000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
        }
    }
}