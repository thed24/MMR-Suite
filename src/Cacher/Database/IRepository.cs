namespace Cacher.Database
{
    using System.Collections.Generic;

    internal interface IRepository<T>
    {
        T Get(T summonerId);

        IEnumerable<T> GetAll();

        void Update(T summonerId);

        void Add(T summonerId);
    }
}