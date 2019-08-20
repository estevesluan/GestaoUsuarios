using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace GestaoUsuarios.WebApp.Hubs
{
    public class UsuarioHub : Hub
    {
        public async Task AtualizarListaDeUsuarios()
        {
            await Clients.All.SendAsync("AtualizarLista");
        }
    }
}
