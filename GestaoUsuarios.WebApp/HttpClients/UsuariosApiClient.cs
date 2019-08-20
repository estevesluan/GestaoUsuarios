using GestaoUsuarios.Domain.Entities;
using GestaoUsuarios.Domain.Extensions;
using GestaoUsuarios.WebApp.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GestaoUsuarios.WebApp.HttpClients
{
    public class UsuarioApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _acessor;

        public UsuarioApiClient(HttpClient client, IHttpContextAccessor accessor)
        {
            _httpClient = client;
            _acessor = accessor;
        }

        private void AddBearerToken()
        {
            var token = _acessor.HttpContext.User.Claims.First(c => c.Type == "Token").Value;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<LoginResultViewModel> PostLoginAsync(LoginViewModel model)
        {
            var resposta = await _httpClient.PostAsJsonAsync("login", model);
            return new LoginResultViewModel(await resposta.Content.ReadAsStringAsync(), resposta.StatusCode);
        }

        public async Task<byte[]> GetFotoAsync(int id)
        {
            AddBearerToken();
            var resposta = await _httpClient.GetAsync($"usuarios/{id}/foto");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsByteArrayAsync();
        }

        public async Task<UsuarioApi> GetUsuarioAsync(int id)
        {
            AddBearerToken();
            var resposta = await _httpClient.GetAsync($"usuarios/{id}");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsAsync<UsuarioApi>();
        }

        public async Task<IEnumerable<UsuarioApi>> GetUsuarioAsync(string pesquisa)
        {
            AddBearerToken();
            var resposta = await _httpClient.GetAsync($"usuarios?pesquisa={pesquisa}");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsAsync<IEnumerable<UsuarioApi>>();
        }

        public async Task<IEnumerable<UsuarioApi>> GetUsuarioTotalPaginaAsync(string pesquisa)
        {
            AddBearerToken();
            var resposta = await _httpClient.GetAsync($"usuarios?pesquisa={pesquisa}");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsAsync<IEnumerable<UsuarioApi>>();
        }

        public async Task<UsuarioPaginacao> GetUsuarioPaginaAsync(string pesquisa, int numeroPagina, int numeroItensPorPagina)
        {
            AddBearerToken();
            var resposta = await _httpClient.GetAsync($"usuarios?pesquisa={pesquisa}&numeroPagina={numeroPagina}&numeroItensPorPagina={numeroItensPorPagina}");
            resposta.EnsureSuccessStatusCode();
            return await resposta.Content.ReadAsAsync<UsuarioPaginacao>();
        }

        public async Task DeleteUsuarioAsync(int id)
        {
            AddBearerToken();
            var resposta = await _httpClient.DeleteAsync($"usuarios/{id}");
            resposta.EnsureSuccessStatusCode();

            if (resposta.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                throw new InvalidOperationException(resposta.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task PostOrPutUsuarioAsync(UsuarioUpload usuario)
        {
            AddBearerToken();
            HttpContent content = CreateMultipartContent(usuario);
            var resposta = await (usuario.Id == 0 ? _httpClient.PostAsync("usuarios", content) : _httpClient.PutAsync("usuarios", content));
            resposta.EnsureSuccessStatusCode();
            if (resposta.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                throw new InvalidOperationException(resposta.Content.ReadAsStringAsync().Result);
            }
        }

        private string EnvolveComAspasDuplas(string valor)
        {
            return $"\u0022{valor}\u0022";
        }

        private HttpContent CreateMultipartContent(UsuarioUpload usuario)
        {
            var content = new MultipartFormDataContent();

            if (usuario.Id > 0)
            {
                content.Add(new StringContent(Convert.ToString(usuario.Id)), EnvolveComAspasDuplas("id"));
            }

            content.Add(new StringContent(usuario.Nome), EnvolveComAspasDuplas("nome"));
            content.Add(new StringContent(usuario.Sobrenome), EnvolveComAspasDuplas("Sobrenome"));
            content.Add(new StringContent(usuario.Nascimento.ToString()), EnvolveComAspasDuplas("nascimento"));
            content.Add(new StringContent(usuario.Cpf), EnvolveComAspasDuplas("cpf"));
            content.Add(new StringContent(usuario.Senha), EnvolveComAspasDuplas("senha"));
            content.Add(new StringContent(usuario.SenhaConfirma), EnvolveComAspasDuplas("senhaconfirma"));
            content.Add(new StringContent(usuario.Telefone), EnvolveComAspasDuplas("telefone"));
            content.Add(new StringContent(usuario.Cep.Replace("-", "")), EnvolveComAspasDuplas("cep"));
            content.Add(new StringContent(usuario.NomeMae), EnvolveComAspasDuplas("nomeMae"));

            if (!string.IsNullOrEmpty(usuario.Email))
            {
                content.Add(new StringContent(usuario.Email), EnvolveComAspasDuplas("email"));
            }

            if (!string.IsNullOrEmpty(usuario.NomePai))
            {
                content.Add(new StringContent(usuario.NomePai), EnvolveComAspasDuplas("nomePai"));
            }

            if (usuario.NumeroEndereco != null)
            {
                content.Add(new StringContent(Convert.ToString(usuario.NumeroEndereco)), EnvolveComAspasDuplas("numeroEndereco"));
            }

            if (usuario.Foto != null)
            {
                var imageContent = new ByteArrayContent(usuario.Foto.ConvertToBytes());
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
                content.Add(imageContent, EnvolveComAspasDuplas("foto"), EnvolveComAspasDuplas("foto.png"));
            }

            return content;
        }
    }
}
