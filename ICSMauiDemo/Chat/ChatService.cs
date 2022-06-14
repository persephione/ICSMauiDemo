using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSMauiDemo.Chat
{
    public class ChatService
    {
        HttpClient client;
        HubConnection connection;

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
                connection = new HubConnectionBuilder()
                    .WithUrl("https://icsdemochathub.service.signalr.net")
                    //.WithUrl("http://localhost:53353/ChatHub")
                    .Build();

                connection.Closed += async (error) =>
                {
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    await connection.StartAsync();
                };

                connection.On<string, string>("ReceiveMessage", (user, message) =>
                {
                    var finalMessage = $"{user} says {message}";
                    //this.Dispatcher.Invoke(() =>
                    //{
                    //    var newMessage = $"{user}: {message}";
                    //    messagesList.Items.Add(newMessage);
                    //});
                });

                try
                {
                    await connection.StartAsync();
                    //messagesList.Items.Add("Connection started");
                    //connectButton.IsEnabled = false;
                    //sendButton.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    var test = ex.Message;
                    // messagesList.Items.Add(ex.Message);
                }


                //IsBusy = true;

                ////string negotiateJson = await client.GetStringAsync($"{SignalRConstants.HostName}/api/negotiate");
                //string negotiateJson = await client.GetStringAsync("https://icsdemochathub.azurewebsites.net/api/negotiate");
                //var negotiate = JsonConvert.DeserializeObject<NegotiateInfoModel>(negotiateJson);

                //HubConnection connection = new HubConnectionBuilder()
                //    .WithUrl(negotiate.Url, options =>
                //    {
                //        options.AccessTokenProvider = async () => negotiate.AccessToken;
                //    })
                //    .Build();

                //connection.Closed += Connection_Closed;
                //connection.On<JObject>(SignalRConstants.MessageName, AddNewMessage);
                //await connection.StartAsync();

                //IsConnected = true;
                //IsBusy = false;

                //Connected?.Invoke(this, true, "Connection successful.");
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

                await connection.InvokeAsync("SendMessage", "tina", "test message");

            }
            catch (Exception ex)
            {
                var test = ex.Message;
                //messagesList.Items.Add(ex.Message);
            }

            //IsBusy = true;

            //var newMessage = new AzureChatMessageModel
            //{
            //    Name = username,
            //    Text = message,
            //    TimeReceived = DateTime.UtcNow
            //};

            //var json = JsonConvert.SerializeObject(newMessage);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            ////var result = await client.PostAsync($"{SignalRConstants.HostName}/api/talk", content);
            //var result = await client.PostAsync("https://icsdemochathub.azurewebsites.net/api/talk", content);

            //IsBusy = false;
        }

        public void AddNewMessage(JObject message)
        {
            var messageModel = new AzureChatMessageModel
            {
                Name = message.GetValue("name").ToString(),
                Text = message.GetValue("text").ToString(),
                TimeReceived = DateTime.UtcNow
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
