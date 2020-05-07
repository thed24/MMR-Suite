using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

namespace Cacher.Database
{
    internal class DynamoDbRepository : IRepository<Document, Primitive>
    {
        private static readonly RegionEndpoint Region = RegionEndpoint.GetBySystemName("ap-southeast-2");
        private readonly Table _table;
        private readonly string _tableName = "SummonerLpHistory";

        public DynamoDbRepository()
        {
            var client = new AmazonDynamoDBClient(Region);
            _table = Table.LoadTable(client, _tableName);
        }

        public Document Get(Primitive summoner)
        {
            return _table.GetItemAsync(summoner).Result;
        }

        public void Delete(Primitive summoner)
        {
            _table.DeleteItemAsync(summoner);
        }

        public void Update(Document summoner)
        {
            _table.UpdateItemAsync(summoner);
        }

        public void Add(Document summoner)
        {
            _table.PutItemAsync(summoner);
        }
    }
}