using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avalentini.Expensi.Domain.Data.Repository
{
    public interface IRepository<T>
    {
        Task<IList<T>> GetAll();
        Task<T> Get(string id);
        void Add(T resource);
        void Edit(string id, T resource);
        void Remove(string id);
        bool Exists(T resource);
    }
}
