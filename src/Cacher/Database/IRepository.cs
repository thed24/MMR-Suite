namespace Cacher.Database
{
    using System.Collections.Generic;

    internal interface IRepository<I, O>
    {
        I Get(O summonerId);

        IEnumerable<I> GetAll();

        void Delete(O summonerId);

        void Update(I summonerId);

        void Add(I summonerId);
    }
}