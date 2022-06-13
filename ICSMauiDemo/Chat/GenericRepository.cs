namespace ICSMauiDemo.Chat
{
    public class GenericRepository
    {
        public GenericRepository()
        {
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
            }
        }
    }
}
