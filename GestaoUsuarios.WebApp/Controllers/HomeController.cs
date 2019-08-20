using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GestaoUsuarios.WebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace GestaoUsuarios.WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cadastro()
        {
            return RedirectToAction("Cadastro", "Usuario");
        }

        public IActionResult Lista()
        {
            return RedirectToAction("Lista", "Usuario");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
