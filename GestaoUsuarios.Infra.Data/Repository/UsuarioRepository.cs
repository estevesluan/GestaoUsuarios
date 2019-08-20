using System.Linq;
using GestaoUsuarios.Domain.Entities;
using GestaoUsuarios.Domain.Interfaces;
using GestaoUsuarios.Infra.Data.Context;

namespace GestaoUsuarios.Infra.Data.Repository
{
    public class UsuarioRepository : IRepository<Usuario>
    {
        private readonly GestaoUsuariosContext _context;

        public UsuarioRepository(GestaoUsuariosContext context)
        {
            _context = context;
        }

        public void Insert(Usuario obj)
        {
            _context.Set<Usuario>().Add(obj);
            _context.SaveChanges();
        }

        public void Update(Usuario obj)
        {
            _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            _context.Set<Usuario>().Remove(Select(id));
            _context.SaveChanges();
        }

        public IQueryable<Usuario> SelectAll()
        {
            return _context.Set<Usuario>();
        }

        public Usuario Select(int id)
        {
            return _context.Set<Usuario>().Find(id);
        }
    }
}
