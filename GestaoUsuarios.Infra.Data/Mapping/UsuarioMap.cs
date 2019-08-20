using GestaoUsuarios.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestaoUsuarios.Infra.Data.Mapping
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");

            builder.HasKey(c => c.Id);

            builder
                .Property(l => l.Nome)
                .HasColumnType("nvarchar(40)")
                .IsRequired();

            builder
                .Property(l => l.Sobrenome)
                .HasColumnType("nvarchar(80)")
                .IsRequired();

            builder
                .Property(l => l.NomeMae)
                .HasColumnType("nvarchar(120)")
                .IsRequired();

            builder
                .Property(l => l.NomePai)
                .HasColumnType("nvarchar(120)");

            builder
                .Property(l => l.Nascimento)
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(l => l.Cpf)
                .HasColumnType("nvarchar(11)")
                .IsRequired();

            builder
                .HasIndex(l => l.Cpf)
                .IsUnique();

            builder
                .Property(l => l.Senha)
                .HasColumnType("nvarchar(32)")
                .IsRequired();

            builder
                .Property(l => l.Telefone)
                .HasColumnType("nvarchar(11)")
                .IsRequired();

            builder
                .Property(l => l.Email)
                .HasColumnType("nvarchar(80)");

            builder
                .Property(l => l.Foto);

            builder
                .Property(l => l.Cep)
                .HasColumnType("nvarchar(8)")
                .IsRequired();

            builder
                .Property(l => l.NumeroEndereco)
                .HasColumnType("int");

            builder
                .Property(l => l.Criacao)
                .HasColumnType("datetime")
                .IsRequired();

            builder
                .Property(l => l.Atualizacao)
                .HasColumnType("datetime")
                .IsRequired();
        }
    }
}
