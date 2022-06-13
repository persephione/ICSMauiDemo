using ICSMauiDemo.Chat;

namespace ICSMauiDemo
{
    public class ChatMessageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ReceivedMessageTemplate { get; set; }
        public DataTemplate SentMessageTemplate { set; get; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as ChatMessageModel;

            if (messageVm == null)
                return null;

            return messageVm.IsIncoming ? ReceivedMessageTemplate : SentMessageTemplate;
        }
    }
}
