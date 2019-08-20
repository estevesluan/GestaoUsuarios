using System;
using System.Threading.Tasks;
using GestaoUsuarios.Domain.Entities;
using GestaoUsuarios.WebApp.HttpClients;
using GestaoUsuarios.WebApp.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ViaCEP;

namespace GestaoUsuarios.WebApp.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IHubContext<UsuarioHub> _hubUsuario;
        private readonly UsuarioApiClient _api;

        public UsuarioController(UsuarioApiClient api, IHubContext<UsuarioHub> hubUsuario)
        {
            _api = api;
            _hubUsuario = hubUsuario;
        }

        [HttpGet]
        public IActionResult Cadastro(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Dados(int id)
        {
            var model = await _api.GetUsuarioAsync(id);
            return Json(model);
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Cadastro(UsuarioUpload model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _api.PostOrPutUsuarioAsync(model);
                    ModelState.Clear();
                    await _hubUsuario.Clients.All.SendAsync("AtualizarLista");
                    return View();
                }
                catch (Exception e)
                {
                    return Json(new { erro = e.Message });
                }
                
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult Detalhes(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Remover(int id)
        {
            var model = await _api.GetUsuarioAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            try
            {
                await _api.DeleteUsuarioAsync(id);
                await _hubUsuario.Clients.All.SendAsync("AtualizarLista");
            }
            catch (Exception e)
            {
                return Json(new { erro = e.Message });
            }
            
            return Ok();
        }

        [HttpGet]
        public IActionResult Lista(string pesquisa)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListaUsuarios(string pesquisa)
        {
            var lista = await _api.GetUsuarioAsync(pesquisa);

            return Json(lista);
        }

        [HttpGet]
        public async Task<IActionResult> ListaUsuariosTotalPaginas(string pesquisa)
        {
            var lista = await _api.GetUsuarioAsync(pesquisa);

            return Json(lista);
        }

        [HttpGet]
        public async Task<IActionResult> ListaUsuariosPagina(string pesquisa, int numeroPagina, int numeroItensPorPagina)
        {
            UsuarioPaginacao usuarioPaginacao = await _api.GetUsuarioPaginaAsync(pesquisa, numeroPagina, numeroItensPorPagina);

            return Json(usuarioPaginacao);
        }

        [HttpGet]
        public IActionResult CarregarEndereco(string cep)
        {
            try
            {
                ViaCEPResult result = ViaCEPClient.Search(cep);
                return Json(result);
            }
            catch (Exception)
            {
                return Json(new { erro = "Cep não encontrado" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Foto(int id)
        {
            byte[] img = await _api.GetFotoAsync(id);
            if (img != null)
            {
                return File(img, "image/png");
            }
            return null;
        }
    }
}
