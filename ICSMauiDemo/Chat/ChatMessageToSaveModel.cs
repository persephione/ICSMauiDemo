namespace ICSMauiDemo.Chat
{
    public class ChatMessageToSaveModel : BindableObject
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

        private string _spText;
        public string SpText
        {
            get
            {
                return _spText;
            }
            set
            {
                _spText = value;
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
    }
}
