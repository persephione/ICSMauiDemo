using ICSMauiDemo.Chat;

namespace ICSMauiDemo
{
    public class ChatMessageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ReceivedMessageTemplate { get; set; }
        public DataTemplate SentMessageTemplate { set; get; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            ChatMessage message = item as ChatMessage;

            if (message.UserName == "tina")
                return SentMessageTemplate;
            else
                return ReceivedMessageTemplate;
        }
    }
}
