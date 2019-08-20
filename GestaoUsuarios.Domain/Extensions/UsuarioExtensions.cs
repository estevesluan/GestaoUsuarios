using GestaoUsuarios.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace GestaoUsuarios.Domain.Extensions
{
    public static class UsuarioExtensions
    {
        public static byte[] ConvertToBytes(this IFormFile image)
        {
            if (image == null)
            {
                return null;
            }
            using (var inputStream = image.OpenReadStream())
            using (var stream = new MemoryStream())
            {
                inputStream.CopyTo(stream);
                return stream.ToArray();
            }
        }

        public static Usuario ToUsuario(this UsuarioUpload model)
        {
            return new Usuario
            {
                Id = model.Id,
                Nome = model.Nome,
                Sobrenome = model.Sobrenome,
                NomeMae = model.NomeMae,
                NomePai = model.NomePai,
                Nascimento = model.Nascimento,
                Cpf = model.Cpf,
                Senha = model.Senha,
                Telefone = model.Telefone,
                Email = model.Email,
                Foto = model.Foto.ConvertToBytes(),
                Cep = model.Cep,
                NumeroEndereco = model.NumeroEndereco,
                Criacao = DateTime.Now,
                Atualizacao = DateTime.Now
            };
        }

        public static UsuarioApi ToApi(this Usuario usuario)
        {
            return new UsuarioApi
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Sobrenome = usuario.Sobrenome,
                NomeMae = usuario.NomeMae,
                NomePai = usuario.NomePai,
                Nascimento = usuario.Nascimento,
                Cpf = usuario.Cpf,
                Senha = usuario.Senha,
                Telefone = usuario.Telefone,
                Email = usuario.Email,
                Foto = $"/api/usuarios/{usuario.Id}/foto",
                Cep = usuario.Cep,
                NumeroEndereco = usuario.NumeroEndereco
            };
        }

        public static UsuarioUpload ToModel(this Usuario usuario)
        {
            return new UsuarioUpload
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Sobrenome = usuario.Sobrenome,
                NomeMae = usuario.NomeMae,
                NomePai = usuario.NomePai,
                Nascimento = usuario.Nascimento,
                Cpf = usuario.Cpf,
                Senha = usuario.Senha,
                Telefone = usuario.Telefone,
                Email = usuario.Email,
                Cep = usuario.Cep,
                NumeroEndereco = usuario.NumeroEndereco,
            };
        }

        public static UsuarioUpload ToUpload(this UsuarioApi usuario)
        {
            return new UsuarioUpload
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Sobrenome = usuario.Sobrenome,
                NomeMae = usuario.NomeMae,
                NomePai = usuario.NomePai,
                Nascimento = usuario.Nascimento,
                Cpf = usuario.Cpf,
                Senha = usuario.Senha,
                Telefone = usuario.Telefone,
                Email = usuario.Email,
                Cep = usuario.Cep,
                NumeroEndereco = usuario.NumeroEndereco,
            };
        }
    }
}
