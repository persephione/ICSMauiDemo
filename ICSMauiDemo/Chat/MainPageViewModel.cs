﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ICSMauiDemo.Chat
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ChatDataService _chatDataService;
        ChatService signalR;
        public Command SendMessageCommand { get; }
        public Command ConnectCommand { get; }
        public Command DisconnectCommand { get; }
        public Action RefreshScrollDown;
        public Action RefreshScrollDownAnimated;

        public MainPageViewModel()
        {
            _chatDataService = new ChatDataService();

            signalR = new ChatService();
            signalR.Connected += SignalR_ConnectionChanged;
            signalR.ConnectionFailed += SignalR_ConnectionChanged;
            signalR.NewMessageReceived += SignalR_NewMessageReceived;

            Messages = new ObservableCollection<ChatMessageModel>();

            Messages.Add(new ChatMessageModel
            {
                UserName = "tina",
                Text = "test message",
                MessageDateTimeStr = "06/12/2022"
            });

            UserName = "What's your name?";

            SendMessageCommand = new Command(async () => await SendMessage());
            ConnectCommand = new Command(async () => await Connect());


            
        }

        #region Properties
        private ObservableCollection<ChatMessageModel> _messages;
        public ObservableCollection<ChatMessageModel> Messages

        {
            get
            {
                return _messages;
            }
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }

        private string _userName;
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        private string _outgoingText;
        public string OutGoingText
        {
            get
            {
                return _outgoingText;
            }
            set
            {
                _outgoingText = value;
                OnPropertyChanged();
            }
        }

        private string _messageToSave;
        public string MessageToSave
        {
            get
            {
                return _messageToSave;
            }
            set
            {
                _messageToSave = value;
                OnPropertyChanged();
            }
        }

        private bool _isConnectedToChatHub;
        public bool IsConnectedToChatHub
        {
            get
            {
                return _isConnectedToChatHub;
            }
            set
            {
                _isConnectedToChatHub = value;
                OnPropertyChanged();
            }
        }

        private string _pageTitle;
        public string PageTitle
        {
            get
            {
                return _pageTitle;
            }
            set
            {
                _pageTitle = value;
                OnPropertyChanged();
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }
        #endregion

        public virtual Task InitializeAsync(object data)
        {
            PageTitle = "ICS Maui Demo Chat";          

            return Task.FromResult(false);
        }


        







        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsConnected()
        {
            var current = Connectivity.NetworkAccess;
            return current == NetworkAccess.Internet ? true : false;
        }

        private async Task Connect()
        {
            IsBusy = true;

            try
            {
                // get recent messages from db and display
                await GetRecentMessages();

                await signalR.ConnectAsync();
                IsConnectedToChatHub = true;

                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
            }
        }

        private async Task SendMessage()
        {
            try
            {
                IsBusy = true;

                // copy the outgoing text to another string and empty it out to quickly remove from entry box on view
                MessageToSave = string.Copy(OutGoingText);
                OutGoingText = string.Empty;

                if (string.IsNullOrWhiteSpace(MessageToSave))
                {
                    IsBusy = false;
                    return;
                }

                // save to db. if error, show dialog box and dont display message
                bool savedToDb = await SaveMessageToDatabase();
                if (!savedToDb)
                {
                    IsBusy = false;
                }
                else
                {
                    //await signalR.SendMessageAsync(_settingsService.UserNameSetting, MessageToSave);
                    MessageToSave = string.Empty;

                    IsBusy = false;
                }
            }
            catch (Exception ex)
            {
                IsBusy = false;
            }
        }

        void SignalR_ConnectionChanged(object sender, bool success, string message)
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    _dialogService.ShowDialog($"Server connection changed: {message}", "Info", "OK");
            //});
        }

        void SignalR_NewMessageReceived(object sender, AzureChatMessageModel message)
        {
            if (message == null)
                return;

            var finalMessage = new ChatMessageModel
            {
                UserName = message.Name,
                Text = message.Text,
                MessageDateTimeStr = ConvertDateTimeToReadableString(DateTime.Now),
                //IsIncoming = message.Name.Equals(_settingsService.UserNameSetting) ? false : true
            };

            AddLocalMessage(finalMessage);
        }

        void AddLocalMessage(ChatMessageModel message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Messages.Add(message);
            });

            RefreshScrollDownAnimated();
        }

        private async Task GetRecentMessages()
        {
            Messages = await _chatDataService.GetChatMessagesAsync();

            //RefreshScrollDown();
        }

        private async Task<bool> SaveMessageToDatabase()
        {
            bool result = await _chatDataService.SaveChatMessageAsync(new ChatMessageToSaveModel
            {
                //UserName = _settingsService.UserNameSetting,
                Text = MessageToSave,
                MessageDateTime = DateTime.Now
            });

            return result;
        }

        private string ConvertDateTimeToReadableString(DateTime date)
        {
            var currentDate = date.ToString("MM/dd/yyyy");
            var time = date.ToString(@"hh\:mm tt");
            return currentDate + " " + time;
        }
    }
}