using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace APS_01_2021.Hubs
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            //Esse método se ativa sozinho ao se conectar
            //Pega todos os contatos e seus respectivos status e envia para o usuario
            //Envia a situação do usuario para os outro usurios
            //adiciona a conexão com o grupo de contatos
            //await Groups.AddToGroupAsync(Context.ConnectionId, "Signa")
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //envia para os contatos que desconectou
            //removea a conexão com o grupo de contatos
            //await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Signa")
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
