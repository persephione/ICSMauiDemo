using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ICSMauiDemo.Chat
{
    // https://docs.microsoft.com/en-us/azure/cognitive-services/Translator/quickstart-translate?pivots=programming-language-csharp
    public class LanguageService
    {
        private readonly ILogger _logger;

        private static readonly string subscriptionKey = "fd76ede0721248ed94b68eeace5365f0";
        private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com";
        string routeDetectLanguage = "/detect?api-version=3.0";
        string routeTranslateToSpanish = "/translate?api-version=3.0&to=es";
        string routeTranslateToEnglish = "/translate?api-version=3.0&to=en";

        public LanguageService(ILogger logger) 
        { 
            _logger = logger;
        }

        public async Task<string> DetectLanguage(string inputText)
        {
            var result = await SendRequest(inputText, routeDetectLanguage);

            var deserializedOutput = JsonConvert.DeserializeObject<DetectResult[]>(result);

            return deserializedOutput[0].Language;
        }

        /// <summary>
        /// Translates from Eng to Span and vice versa
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="toLanguage"></param>
        /// <returns>Returns the translated text</returns>
        public async Task<string> TranslateText(string inputText, int toLanguage)
        {
            var route = toLanguage == 0 ? routeTranslateToEnglish : routeTranslateToSpanish;

            var result = await SendRequest(inputText, route);

            var deserializedOutput = JsonConvert.DeserializeObject<TranslationResult[]>(result);

            return deserializedOutput[0].Translations[0].Text;
        }

        private async Task<string> SendRequest(string inputText, string route)
        {
            // serialize the detect request
            object[] body = new object[] { new { Text = inputText } };
            var requestBody = JsonConvert.SerializeObject(body);

            // Construct the request and print the response
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;

                // Construct the URI and add headers.
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                request.Headers.Add("Ocp-Apim-Subscription-Region", "eastus");

                // Send the request and get response.
                HttpResponseMessage responseMessage = await client.SendAsync(request).ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    return await responseMessage.Content.ReadAsStringAsync();
                }
                else if (responseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogError("LangServ1", responseMessage.StatusCode.ToString());
                    return string.Empty;
                }
                else if (responseMessage.StatusCode == HttpStatusCode.Forbidden || responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogError("LangServ2", responseMessage.StatusCode.ToString());
                    return string.Empty;
                }
                else
                {
                    _logger.LogError("LangServ3", responseMessage.StatusCode.ToString());
                    return string.Empty;
                }
            }
        }
    }

    public class DetectResult
    {
        public string Language { get; set; }
        public float Score { get; set; }
        public bool IsTranslationSupported { get; set; }
        public bool IsTransliterationSupported { get; set; }
    }

    public class TranslationResult
    {
        public Translation[] Translations { get; set; }
    }

    public class Translation
    {
        public string Text { get; set; }
        public string To { get; set; }
    }
}
