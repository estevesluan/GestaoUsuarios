using FluentValidation;
using GestaoUsuarios.Domain.Entities;
using System.Linq;

namespace GestaoUsuarios.Domain.Interfaces
{
    public interface IService<T> where T : BaseEntity
    {
        T Post<V>(T obj) where V : AbstractValidator<T>;

        T Put<V>(T obj) where V : AbstractValidator<T>;

        void Delete(int id);

        T Get(int id);

        IQueryable<T> Get();
    }
}
