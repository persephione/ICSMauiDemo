
//using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICSMauiDemo.Chat
{
    public class ChatDataService
    {
//#if DEBUG
        //public const string BaseApiUrl = "https://10.0.2.2:44394/"; // for emulator debugging.
        public const string BaseApiUrl = "https://icschatdemoapi.azurewebsites.net/"; // device testing use the live test url
//#else
//        public const string BaseApiUrl = "https://laorejaapi.azurewebsites.net/"; // live prod api
//#endif
        public const string GetMostRecentMessagesEndpoint = "/getmostrecentmessages";
        public const string SaveMessageEndpoint = "/savemessage";

        private readonly GenericRepository _genericRepository;
        private readonly LanguageService _languageService;

        public ChatDataService(GenericRepository genericRepository, LanguageService languageService)
        {
            _genericRepository = genericRepository;
            _languageService = languageService;
        }

        //public async Task<ObservableRangeCollection<ChatMessageModel>> GetChatMessagesAsync(string userName)
        //{
        //    UriBuilder builder = new UriBuilder(BaseApiUrl)
        //    {
        //        //Path = UserPreferredLanguageSetting == 0
        //        //? GetMostRecentMessagesEndpoint
        //        //: GetMostRecentSpanishMessagesEndpoint
        //    };

        //    try
        //    {
        //        //var list = await _genericRepository.GetAsync<List<ChatMessageModel>>(builder.ToString());

        //        //var messages = new ObservableRangeCollection<ChatMessageModel>(list);

        //        foreach (var message in messages)
        //        {
        //            // update utc time to user's time for android
        //            var timeZoneId = TimeZoneInfo.Local.Id;
        //            var estTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        //            var usersDateTime = TimeZoneInfo.ConvertTimeFromUtc(message.MessageDateTime, estTimeZone);
        //            message.MessageDateTimeStr = ConvertDateTimeToReadableString(usersDateTime);

        //            message.IsIncoming = message.UserName == userName ? false : true;
        //        }

        //        return messages;
        //    }
        //    catch (Exception ex)
        //    {
        //        // todo: log error
        //        return null;
        //    }
        //}

        public async Task<bool> SaveChatMessageAsync(ChatMessageToSaveModel model)
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

            UriBuilder builder = new UriBuilder(BaseApiUrl)
            {
                Path = SaveMessageEndpoint
            };

            await _genericRepository.PostAsync(model);

            return false;
        }

        private string ConvertDateTimeToReadableString(DateTime date)
        {
            var newDate = date.ToString("MM/dd/yyyy");
            var time = date.ToString(@"hh\:mm tt");
            return newDate + " " + time;
        }

    }
}
