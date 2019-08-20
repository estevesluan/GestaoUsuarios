using GestaoUsuarios.Domain.Entities;
using GestaoUsuarios.Infra.Data.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace GestaoUsuarios.Infra.Data.Context
{
    public class GestaoUsuariosContext : DbContext
    {
        public DbSet<Usuario> Usuario { get; set; }

        public GestaoUsuariosContext(DbContextOptions<GestaoUsuariosContext> options)
            : base(options)
        {
            //irá criar o banco e a estrutura de tabelas necessárias
            this.Database.EnsureCreated();

            //Primeiro acesso cria o usuário padrão
            if (this.Set<Usuario>().Count() == 0)
            {
                Usuario usuarioPadrao = new Usuario();

                usuarioPadrao.Nome = "ADM";
                usuarioPadrao.Sobrenome = "Sistema";
                usuarioPadrao.NomeMae = "Nome mãe";
                usuarioPadrao.Nascimento = DateTime.Now;
                usuarioPadrao.Cpf = "11111111111";
                usuarioPadrao.Senha = "adm2019";
                usuarioPadrao.Telefone = "11111111111";
                usuarioPadrao.Cep = "01001000";
                usuarioPadrao.Criacao = DateTime.Now;
                usuarioPadrao.Atualizacao = DateTime.Now;

                this.Set<Usuario>().Add(usuarioPadrao);
                this.SaveChanges();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>(new UsuarioMap().Configure);
        }
    }
}
