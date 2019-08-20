using GestaoUsuarios.Domain.Entities;
using System.Linq;

namespace GestaoUsuarios.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Insert(T obj);

        void Update(T obj);

        void Delete(int id);

        T Select(int id);

        IQueryable<T> SelectAll();
    }
}
