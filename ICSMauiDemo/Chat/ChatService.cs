using Microsoft.AspNetCore.SignalR.Client;

namespace ICSMauiDemo.Chat
{
    public class ChatService
    {
        HubConnection hubConnection;

        public delegate void MessageReceivedHandler(object sender, AzureChatMessageModel message);
        public delegate void ConnectionHandler(object sender, bool successful, string message);

        public event ConnectionHandler Connected;
        public event MessageReceivedHandler NewMessageReceived;
        public event ConnectionHandler ConnectionFailed;
        public bool IsConnected { get; private set; }
        public bool IsBusy { get; private set; }

        public ChatService()
        {
            
        }

        public async Task ConnectAsync()
        {
            try
            {
                hubConnection = new HubConnectionBuilder()
                    .WithUrl("https://icsdemochathub.azurewebsites.net/chathub")
                    .Build();

                hubConnection.Closed += Connection_Closed;

                hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
                {
                    AddNewMessage(user, message);
                });

                await hubConnection.StartAsync();

                //var isConnected = hubConnection.State;

                IsConnected = true;

                Connected?.Invoke(this, true, "Connection successful.");  
            }
            catch (Exception ex)
            {
                ConnectionFailed?.Invoke(this, false, ex.Message);
                IsConnected = false;
                IsBusy = false;
            }
        }

        public async Task SendMessageAsync(string username, string message)
        {
            try
            {
                await hubConnection.InvokeAsync("SendMessage", username, message);
            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
        }

        public void AddNewMessage(string user, string message)
        {
            var messageModel = new AzureChatMessageModel
            {
                Name = user,
                Text = message,
                TimeReceived = DateTime.UtcNow
            };

            NewMessageReceived?.Invoke(this, messageModel);
        }

        public Task Connection_Closed(Exception arg)
        {
            ConnectionFailed?.Invoke(this, false, arg.Message);
            IsConnected = false;
            return Task.CompletedTask;

            /*hubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };*/
        }
    }
}
