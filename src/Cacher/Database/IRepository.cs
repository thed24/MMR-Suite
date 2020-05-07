namespace Cacher.Database
{
    internal interface IRepository<I, O>
    {
        I Get(O summonerId);

        void Delete(O summonerId);

        void Update(I summonerId);

        void Add(I summonerId);
    }
}