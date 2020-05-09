namespace Calculator.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using Amazon.DynamoDBv2.DocumentModel;
    using Cacher.Database;
    using Cacher.Model;
    using static Cacher.Service.RiotDatabaseConnector;

    public class DatabaseService
    {
        private readonly DynamoDbRepository _repository = new DynamoDbRepository();

        public IEnumerable<SummonerStats> GetAll()
        {
            var summonerDocuments = _repository.GetAll();
            return summonerDocuments.Select(DocumentUnwrapper);
        }
    }
}