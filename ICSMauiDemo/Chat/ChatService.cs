using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ICSMauiDemo.Chat
{
    public class ChatService
    {
        HttpClient client;

        public delegate void MessageReceivedHandler(object sender, AzureChatMessageModel message);
        public delegate void ConnectionHandler(object sender, bool successful, string message);

        public event ConnectionHandler Connected;
        public event MessageReceivedHandler NewMessageReceived;
        public event ConnectionHandler ConnectionFailed;
        public bool IsConnected { get; private set; }
        public bool IsBusy { get; private set; }

        public ChatService()
        {
            client = new HttpClient();
        }

        public async Task ConnectAsync()
        {
            try
            {
                IsBusy = true;

                //string negotiateJson = await client.GetStringAsync($"{SignalRConstants.HostName}/api/negotiate");
                string negotiateJson = await client.GetStringAsync("https://icsdemochathub.azurewebsites.net/api/negotiate");
                var negotiate = JsonConvert.DeserializeObject<NegotiateInfoModel>(negotiateJson);
                HubConnection connection = new HubConnectionBuilder()
                    .WithUrl(negotiate.Url, options =>
                    {
                        options.AccessTokenProvider = async () => negotiate.AccessToken;
                    })
                    .Build();

                connection.Closed += Connection_Closed;
                connection.On<JObject>(SignalRConstants.MessageName, AddNewMessage);
                await connection.StartAsync();

                IsConnected = true;
                IsBusy = false;

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
            IsBusy = true;

            var newMessage = new AzureChatMessageModel
            {
                Name = username,
                Text = message
            };

            var json = JsonConvert.SerializeObject(newMessage);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync($"{SignalRConstants.HostName}/api/talk", content);

            IsBusy = false;
        }

        public void AddNewMessage(JObject message)
        {
            var messageModel = new AzureChatMessageModel
            {
                Name = message.GetValue("name").ToString(),
                Text = message.GetValue("text").ToString(),
                TimeReceived = DateTime.Now
            };

            NewMessageReceived?.Invoke(this, messageModel);
        }

        public Task Connection_Closed(Exception arg)
        {
            ConnectionFailed?.Invoke(this, false, arg.Message);
            IsConnected = false;
            IsBusy = false;
            return Task.CompletedTask;
        }
    }
}
