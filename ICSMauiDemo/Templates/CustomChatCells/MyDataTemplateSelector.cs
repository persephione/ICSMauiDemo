using ICSMauiDemo.Chat;

namespace ICSMauiDemo.Templates.CustomChatCells
{
    public class MyDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IncomingTemplate { get; set; }

        public DataTemplate OutgoingTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as ChatMessageModel;

            return messageVm.IsIncoming ? IncomingTemplate : OutgoingTemplate;
        }

        //public DataTemplate incomingDataTemplate;
        //public DataTemplate outgoingDataTemplate;

        //public MyDataTemplateSelector()
        //{
        //    // Retain instances!
        //    incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
        //    outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        //}

        //protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        //{
        //    var messageVm = item as ChatMessageModel;

        //    if (messageVm == null)
        //        return null;

        //    return messageVm.IsIncoming ? incomingDataTemplate : outgoingDataTemplate;
        //}
    }
}
