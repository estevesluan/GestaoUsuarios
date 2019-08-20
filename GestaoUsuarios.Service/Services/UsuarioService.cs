using FluentValidation;
using GestaoUsuarios.Domain.Entities;
using GestaoUsuarios.Domain.Interfaces;
using System;
using System.Linq;

namespace GestaoUsuarios.Service.Services
{
    public class UsuarioService : IService<Usuario>
    {
        private readonly IRepository<Usuario> _repositoryUsuario;

        public UsuarioService(IRepository<Usuario> repositoryUsuario)
        {
            _repositoryUsuario = repositoryUsuario;
        }

        public Usuario Post<V>(Usuario obj) where V : AbstractValidator<Usuario>
        {
            Validate(obj, Activator.CreateInstance<V>());

            if(_repositoryUsuario.SelectAll().Where(x => x.Cpf == obj.Cpf).Any()){
                throw new Exception("CPF já está cadastrado!");
            }

            _repositoryUsuario.Insert(obj);
            return obj;
        }

        public Usuario Put<V>(Usuario obj) where V : AbstractValidator<Usuario>
        {
            Validate(obj, Activator.CreateInstance<V>());

            if (_repositoryUsuario.SelectAll().Where(x => x.Cpf == obj.Cpf && x.Id != obj.Id).Any())
            {
                throw new Exception($"CPF já está cadastrado para outro usuário!");
            }

            _repositoryUsuario.Update(obj);
            return obj;
        }

        public void Delete(int id)
        {
            if(_repositoryUsuario.SelectAll().Count() == 1)
            {
                throw new Exception("Não é possível excluir o último usuário!");
            }

            _repositoryUsuario.Delete(id);
        }

        public Usuario Get(int id)
        {
            return _repositoryUsuario.Select(id);
        }

        public Usuario Get(string cpf)
        {
            return _repositoryUsuario.SelectAll().Where(x => x.Cpf == cpf).FirstOrDefault();
        }

        public IQueryable<Usuario> Get()
        {
            return _repositoryUsuario.SelectAll();
        }

        private void Validate<V>(Usuario obj, V v) where V : AbstractValidator<Usuario>
        {
            if (obj == null)
                throw new Exception("Registros não detectados!");

            v.ValidateAndThrow(obj);
        }
    }
}
