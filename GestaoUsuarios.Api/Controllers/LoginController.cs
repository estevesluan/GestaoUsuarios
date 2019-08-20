using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GestaoUsuarios.Domain.Entities;
using GestaoUsuarios.Domain.Interfaces;
using GestaoUsuarios.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GestaoUsuarios.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private IService<Usuario> _serviceUsuario;

        public LoginController(IService<Usuario> serviceUsuario)
        {
            _serviceUsuario = serviceUsuario;
        }

        [HttpPost]
        public IActionResult Token(LoginViewModel model)
        {
            if (_serviceUsuario.Get().Any(x => x.Cpf == model.Cpf && x.Senha == model.Senha))
            {
                //cria token (header + payload >> direitos + signature)
                var direitos = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, model.Cpf),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var chave = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("usuarios-authentication-valid"));
                var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "GestaoUsuarios.WebApp",
                    audience: "Postman",
                    claims: direitos,
                    signingCredentials: credenciais,
                    expires: DateTime.Now.AddMinutes(30)
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(tokenString);
            }
            return Unauthorized(); //401
        }
    }
}