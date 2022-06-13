using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ICSMauiDemo.Chat
{
    public class GenericRepository
    {
        //private readonly ILogger _logger;

        public GenericRepository()
        {
            //_logger = logger;
        }

        public async Task<List<ChatMessageModel>> GetAsync(int lang)
        {
            try
            {
                HttpClient client = new HttpClient();
                List<ChatMessageModel> list = null;

                HttpResponseMessage response = await client.GetAsync($"https://icschatdemoapi.azurewebsites.net/api/GetMostRecentMessages/{lang}");

                if (response.IsSuccessStatusCode)
                {
                    list = await response.Content.ReadAsAsync<List<ChatMessageModel>>();
                }

                return list;
            }
            catch (Exception ex)
            {
               // _logger.LogError($"Exception: {ex.Message}");
                return null;
            }
        }

        public async Task PostAsync(ChatMessageToSaveModel chatMessage)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsJsonAsync("https://icschatdemoapi.azurewebsites.net/api/savemessage", chatMessage);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
               // _logger.LogError($"Exception: {ex.Message}");
            }
        }


        //public async Task<T> GetAsync<T>(string uri)
        //{
        //    try
        //    {
        //        HttpClient httpClient = CreateHttpClient();

        //        string jsonResult = string.Empty;

        //        var responseMessage = await Policy
        //            .Handle<WebException>(ex =>
        //            {
        //                Debug.WriteLine($"{ex.GetType().Name + " : " + ex.Message}");
        //                return true;
        //            })
        //            .WaitAndRetryAsync
        //            (
        //                5,
        //                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
        //            )
        //            .ExecuteAsync(async () => await httpClient.GetAsync(uri));

        //        if (responseMessage.IsSuccessStatusCode)
        //        {
        //            // TODO: Sometimes the result just comes back as a simple string, figure out if this is the best way to handle that
        //            jsonResult = await responseMessage.Content.ReadAsStringAsync();

        //            if (IsJson(jsonResult))
        //                return JsonConvert.DeserializeObject<T>(jsonResult);
        //            else if (IsBool(jsonResult))
        //                return default(T);
        //            else
        //                return (T)(object)jsonResult;
        //        }

        //        if (responseMessage.StatusCode == HttpStatusCode.Forbidden ||
        //            responseMessage.StatusCode == HttpStatusCode.Unauthorized)
        //        {
        //            ClearSettings();
        //            await LogRepoErrorAsync($"1GetAsync<T>(string uri): {responseMessage.StatusCode}");
        //            return default(T);
        //        }

        //        await LogRepoErrorAsync($"2GetAsync<T>(string uri): {responseMessage.StatusCode}");
        //        return default(T);
        //    }
        //    catch (Exception ex)
        //    {
        //        await LogRepoErrorAsync($"3GetAsync<T>(string uri): {ex.GetType().Name} - {ex.Message}");
        //        return default(T);
        //    }
        //}

        //public async Task<bool> PostAsyncReturnBool<T>(string uri, T data)
        //{
        //    try
        //    {
        //        HttpClient httpClient = CreateHttpClient();

        //        var content = new StringContent(JsonConvert.SerializeObject(data));
        //        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //        string jsonResult = string.Empty;

        //        var responseMessage = await Policy
        //            .Handle<WebException>(ex =>
        //            {
        //                Debug.WriteLine($"{ex.GetType().Name + " : " + ex.Message}");
        //                return true;
        //            })
        //            .WaitAndRetryAsync
        //            (
        //                5,
        //                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
        //            )
        //            .ExecuteAsync(async () => await httpClient.PostAsync(uri, content));

        //        if (responseMessage.IsSuccessStatusCode)
        //        {
        //            jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        //            var json = (bool)JsonConvert.DeserializeObject(jsonResult);
        //            return json;
        //        }

        //        if (responseMessage.StatusCode == HttpStatusCode.Forbidden ||
        //            responseMessage.StatusCode == HttpStatusCode.Unauthorized)
        //        {
        //            _logger.LogError($"13PostAsyncReturnBool<T>(string uri, T data) : {responseMessage.StatusCode}");
        //            return false;
        //        }

        //        _logger.LogError($"14PostAsyncReturnBool<T>(string uri, T data) : {responseMessage.StatusCode}");
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"15PostAsyncReturnBool<T>(string uri, T data) : {ex.GetType().Name} - {ex.Message}");
        //        return false;
        //    }
        //}







        //private HttpClient CreateHttpClient()
        //{
        //    // https://stackoverflow.com/questions/56597401/how-can-i-install-certificate-for-visual-studio-emulator-for-androids-emulato
        //    var httpClient = new HttpClient(new HttpClientHandler());

        //    //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    return httpClient;
        //}

        protected bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}") || input.StartsWith("[") && input.EndsWith("]");
        }

        protected bool IsBool(string input)
        {
            if (bool.TryParse(input, out bool value))
                return true;
            else
                return false;
        }

    }
}
