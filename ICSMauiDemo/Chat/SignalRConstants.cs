using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICSMauiDemo.Chat
{
    public static class SignalRConstants
    {
        // NOTE: If testing locally, use http://localhost:7071
        // otherwise enter your Azure Function App url  // For example: http://YOUR_FUNCTION_APP_NAME.azurewebsites.net
        public static string HostName { get; set; } = "https://icsdemochathub.azurewebsites.net";

        public static string MessageName { get; set; } = "newMessage";

    }
}
