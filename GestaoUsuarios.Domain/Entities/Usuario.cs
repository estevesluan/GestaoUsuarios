using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace GestaoUsuarios.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string NomeMae { get; set; }
        public string NomePai { get; set; }
        public DateTime Nascimento { get; set; }
        public string Cpf { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public byte[] Foto { get; set; }
        public string Cep { get; set; }
        public int? NumeroEndereco { get; set; }
        public DateTime Criacao { get; set; }
        public DateTime Atualizacao { get; set; }
    }

    [XmlType("Usuario")]
    public class UsuarioApi : BaseEntity
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string NomeMae { get; set; }
        public string NomePai { get; set; }
        public DateTime Nascimento { get; set; }
        public string Cpf { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Foto { get; set; }
        public string Cep { get; set; }
        public int? NumeroEndereco { get; set; }
    }

    public class UsuarioUpload: BaseEntity
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Sobrenome { get; set; }
        [Required]
        public string NomeMae { get; set; }
        public string NomePai { get; set; }
        [Required]
        public DateTime Nascimento { get; set; }
        [Required]
        public string Cpf { get; set; }
        [Required]
        public string Senha { get; set; }
        [Required]
        public string SenhaConfirma { get; set; }
        [Required]
        public string Telefone { get; set; }
        public string Email { get; set; }
        public IFormFile Foto { get; set; }
        [Required]
        public string Cep { get; set; }
        public int? NumeroEndereco { get; set; }
    }

    public class UsuarioPaginacao
    {
        public int Total { get; set; }
        public int NumeroItensPorPagina { get; set; }
        public int NumeroPagina { get; set; }
        public IEnumerable<Usuario> Usuarios { get; set; }
    }
}
