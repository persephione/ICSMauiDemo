namespace ICSMauiDemo.Chat
{
    public class ChatMessageModel : BindableObject
    {
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

        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        private DateTime _messageDateTime;
        public DateTime MessageDateTime
        {
            get
            {
                return _messageDateTime;
            }
            set
            {
                _messageDateTime = value;
                OnPropertyChanged();
            }
        }

        private string _messageDateTimeStr;
        public string MessageDateTimeStr
        {
            get
            {
                return _messageDateTimeStr;
            }
            set
            {
                _messageDateTimeStr = value;
                OnPropertyChanged();
            }
        }

        private bool _isIncoming;
        public bool IsIncoming
        {
            get
            {
                return _isIncoming;
            }
            set
            {
                _isIncoming = value;
                OnPropertyChanged();
            }
        }

    }
}
