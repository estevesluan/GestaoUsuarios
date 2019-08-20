using System;
using System.Linq;
using GestaoUsuarios.Domain.Entities;
using GestaoUsuarios.Domain.Extensions;
using GestaoUsuarios.Domain.Interfaces;
using GestaoUsuarios.Service.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoUsuarios.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private IService<Usuario> _serviceUsuario;

        public UsuariosController(IService<Usuario> serviceUsuario)
        {
            _serviceUsuario = serviceUsuario;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public IActionResult Post([FromForm] UsuarioUpload item)
        {
            try
            {
                _serviceUsuario.Post<UsuarioValidator>(item.ToUsuario());

                return new NoContentResult();
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPut]
        [DisableRequestSizeLimit]
        public IActionResult Put([FromForm] UsuarioUpload item)
        {
            try
            {
                Usuario usuario = item.ToUsuario();

                if (item.Foto == null)
                {
                    usuario.Foto = _serviceUsuario.Get().Where(x => x.Id == item.Id).Select(s => s.Foto).FirstOrDefault();
                }

                usuario.Atualizacao = DateTime.Now;

                _serviceUsuario.Put<UsuarioValidator>(usuario);

                return new NoContentResult();
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _serviceUsuario.Delete(id);

                return new NoContentResult();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet("{id}/foto")]
        public IActionResult Foto(int id)
        {
            byte[] img = _serviceUsuario.Get(id)?.Foto;

            if (img != null)
            {
                return File(img, "image/png");
            }
            return null;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return new ObjectResult(_serviceUsuario.Get(id));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public IActionResult Get(string pesquisa, int numeroPagina, int numeroItensPorPagina)
        {
            try
            {
                IQueryable<Usuario> lista = _serviceUsuario.Get();

                if (!String.IsNullOrEmpty(pesquisa))
                {
                    lista = lista.Where(x => (x.Nome + x.Sobrenome).ToUpper().Contains(pesquisa.ToUpper()));
                }

                UsuarioPaginacao usuarioPaginacao = new UsuarioPaginacao();
                usuarioPaginacao.NumeroPagina = numeroPagina;
                usuarioPaginacao.NumeroItensPorPagina = numeroItensPorPagina;
                usuarioPaginacao.Total = (int)Math.Ceiling((lista.Count() / (double) numeroItensPorPagina));

                lista = lista.OrderByDescending(x => x.Atualizacao).Skip((numeroPagina - 1) * numeroItensPorPagina).Take(numeroItensPorPagina);

                usuarioPaginacao.Usuarios = lista.ToList();

                return new ObjectResult(usuarioPaginacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}