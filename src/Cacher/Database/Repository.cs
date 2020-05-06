using Amazon.DynamoDBv2.DocumentModel;

namespace Cacher.Database
{
    internal interface IRepository<I, O>
    {
        O Get(string summonerId);

        void Delete(I summonerId);

        void Update(I summonerId);

        void Add(I summonerId);
    }
}