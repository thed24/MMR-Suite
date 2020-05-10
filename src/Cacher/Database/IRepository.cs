namespace Cacher.Database
{
    using System.Collections.Generic;

    internal interface IRepository<T, in TO>
    {
        T Get(TO summonerId);

        IEnumerable<T> GetAll();

        void Delete(TO summonerId);

        void Update(T summonerId);

        void Add(T summonerId);
    }
}