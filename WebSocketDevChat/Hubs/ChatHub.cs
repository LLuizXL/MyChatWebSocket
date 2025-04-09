using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace WebSocketDevChat.Hubs
{

    public class ChatHub : Hub
    {
        // Dicionário para mapear ID para nomes de usuário
        private static ConcurrentDictionary<string, string> _usernames = new();

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Conectado usuário com ID: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public Task SetUsername(string username)
        {
            _usernames[Context.ConnectionId] = username;
            Console.WriteLine($"Usuário conectado: {username}");
            return Task.CompletedTask;
        }

        public Task<string> UserId()
        {
            return Task.FromResult(Context.ConnectionId);
        }

        public async Task SendMessage(string message)
        {
            string userId = Context.ConnectionId;
            string username = _usernames.GetValueOrDefault(userId, "Anônimo");

            var infoMsg = new
            {
                authorId = userId,
                author = username,
                text = message
            };

            Console.WriteLine($"Mensagem de {username}: {message}");

            // Envia a mensagem para todos os clientes conectados
            await Clients.All.SendAsync("receive_message", infoMsg);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_usernames.TryRemove(Context.ConnectionId, out var username))
            {
                Console.WriteLine($"{username} ({Context.ConnectionId}) saiu.");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }


}
