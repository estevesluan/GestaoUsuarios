using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GestaoUsuarios.WebApp.HttpClients;
using GestaoUsuarios.WebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoUsuarios.WebApp.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        private readonly UsuarioApiClient _api;

        public LoginController(UsuarioApiClient api)
        {
            _api = api;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _api.PostLoginAsync(model);
            if (result.Succeeded)
            {
                //Adicionar itens claim
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Cpf),
                    new Claim("Token", result.Token)
                };

                //Add direitos na identidade principal
                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                var authProp = new AuthenticationProperties
                {
                    IssuedUtc = DateTime.UtcNow,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(25),
                    IsPersistent = true
                };

                //Autenticar com cookie
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProp);

                return Json(new { sucesso = "Login realizado com sucesso" });
            }

            return Json(new { erro = "Não autorizado! Tente novamente." });
        }

        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}