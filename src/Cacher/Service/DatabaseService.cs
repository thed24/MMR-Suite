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
            var summoner = Get(summonerStats);
            if (summoner is null)
                _repository.Add(DocumentWrapper(summonerStats));
            else
                Update(summonerStats.LpLog.Last(), summoner);
        }

        private SummonerStats Get(SummonerStats summonerStats)
        {
            var wrappedSummonerStats = DocumentWrapper(summonerStats);
            var summonerFromDb = _repository.Get(wrappedSummonerStats);
            return summonerFromDb != null && summonerFromDb.Any() ? DocumentUnwrapper(summonerFromDb) : null;
        }

        private void Update(string newLp, SummonerStats summonerStats)
        {
            if (summonerStats.LpLog.Last().Equals(newLp)) return;
            summonerStats.LpLog.Add(newLp);
            _repository.Update(DocumentWrapper(summonerStats));
        }
    }
}