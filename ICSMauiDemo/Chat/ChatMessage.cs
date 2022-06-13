using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSMauiDemo.Chat
{
    public class ChatMessage : BindableObject
    {
        public string MessageText { get; set; }

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

        //public MessageOwner Owner { get; set; }
    }
}
