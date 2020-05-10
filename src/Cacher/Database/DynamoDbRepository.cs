namespace Cacher.Database
{
    using System.Collections.Generic;
    using Amazon;
    using Amazon.DynamoDBv2;
    using Amazon.DynamoDBv2.DocumentModel;

    public class DynamoDbRepository : IRepository<Document, Primitive>
    {
        private const string TableName = "SummonerLpHistory";
        private static readonly RegionEndpoint Region = RegionEndpoint.GetBySystemName("ap-southeast-2");
        private readonly Table _table;

        public DynamoDbRepository()
        {
            var client = new AmazonDynamoDBClient(Region);
            _table = Table.LoadTable(client, TableName, DynamoDBEntryConversion.V2);
        }

        public Document Get(Primitive summoner)
        {
            return _table.GetItemAsync(summoner).Result;
        }

        public IEnumerable<Document> GetAll()
        {
            var conditions = new ScanFilter();
            return _table.Scan(conditions).GetRemainingAsync().Result;
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