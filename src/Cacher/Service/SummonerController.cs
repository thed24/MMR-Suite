namespace Cacher.Service
{
    using System;

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

            foreach (var entry in entries)
                try
                {
                    var summonersStats = _connector.CreateSummonerStats(entry.SummonerId, entry.SummonerName);
                    _database.Add(summonersStats);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
        }
    }
}