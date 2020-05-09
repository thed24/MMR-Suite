namespace Cacher.Service
{
    using System.Linq;
    using Cacher.Database;
    using Cacher.Model;
    using static RiotDatabaseConnector;

    public class DatabaseService
    {
        private readonly DynamoDbRepository _repository = new DynamoDbRepository();

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