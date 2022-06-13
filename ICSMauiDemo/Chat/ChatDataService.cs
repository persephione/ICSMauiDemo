using System.Collections.ObjectModel;

namespace ICSMauiDemo.Chat
{
    public class ChatDataService
    {
        public const string BaseApiUrl = "https://icschatdemoapi.azurewebsites.net/";

        public string GetMostRecentMessagesEndpoint(int lang) => $"/getmostrecentmessages/{lang}";
        public const string SaveMessageEndpoint = "/savemessage";

        private readonly GenericRepository _genericRepository;
        private readonly LanguageService _languageService;

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
            }
        }


        public ChatDataService()
        {
            _genericRepository = new GenericRepository();
            _languageService = new LanguageService();

            UserName = Preferences.Get("username", "Unknown");
        }

        public async Task<ObservableCollection<ChatMessageModel>> GetChatMessagesAsync()
        {
            var preferredLanguage = UserName == "tina" ? 1 : 0;

            try
            {
                var list = await _genericRepository.GetAsync(preferredLanguage);

                var messages = new ObservableCollection<ChatMessageModel>(list);

                foreach (var message in messages)
                {
                    // update utc time to user's time for android
                    var timeZoneId = TimeZoneInfo.Local.Id;
                    var estTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                    var usersDateTime = TimeZoneInfo.ConvertTimeFromUtc(message.MessageDateTime, estTimeZone);
                    message.MessageDateTimeStr = ConvertDateTimeToReadableString(usersDateTime);

                    message.IsIncoming = message.UserName == UserName ? false : true;
                }

                return messages;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task SaveChatMessageAsync(ChatMessageToSaveModel model)
        {
            // detect language first
            var language = await _languageService.DetectLanguage(model.Text);

            // if text was written in english, translate it to span and vice versa
            var toLanguage = language == "en" ? 1 : 0;

            var translatedText = await _languageService.TranslateText(model.Text, toLanguage);

            // assign the correct text to the english and spanish properties of a chatmessage obj
            if (language == "en")
            {
                model.SpText = translatedText;
            }
            else
            {
                model.SpText = model.Text;
                model.Text = translatedText;
            }

            await _genericRepository.PostAsync(model);
        }

        private string ConvertDateTimeToReadableString(DateTime date)
        {
            var newDate = date.ToString("MM/dd/yyyy");
            var time = date.ToString(@"hh\:mm tt");
            return newDate + " " + time;
        }

    }
}
