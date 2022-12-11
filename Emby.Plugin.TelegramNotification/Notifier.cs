using System.Collections.Generic;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Notifications;
using MediaBrowser.Model.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Emby.Notifications;
using MediaBrowser.Controller;

namespace Emby.Plugin.TelegramNotification
{
    public class Notifier : IUserNotifier
    {
        private ILogger _logger;
        private IServerApplicationHost _appHost;
        private IHttpClient _httpClient;

        public Notifier(ILogger logger, IServerApplicationHost applicationHost, IHttpClient httpClient)
        {
            _logger = logger;
            _appHost = applicationHost;
            _httpClient = httpClient;
        }

        private Plugin Plugin => _appHost.Plugins.OfType<Plugin>().First();

        public string Name => Plugin.StaticName;

        public string Key => "telegramnotifications";

        public string SetupModuleUrl => Plugin.NotificationSetupModuleUrl;

        public async Task SendNotification(InternalNotificationRequest request, CancellationToken cancellationToken)
        {
            var options = request.Configuration.Options;

            options.TryGetValue("BotToken", out string botToken);
            options.TryGetValue("ChatID", out string chatID);

            string message = (request.Title);

            if (string.IsNullOrEmpty(request.Description) == false)
            {
                message = (request.Title + "\n\n" + request.Description); 
            }

            if (message.Length > 4096)
            {
                int chunkSize = 4096;
                int messageLenght = message.Length;
                for (int i = 0; i < messageLenght; i += chunkSize)
                {
                    if (i + chunkSize > messageLenght) chunkSize = messageLenght - i;
                    string TelegramMessage = Uri.EscapeDataString(message.Substring(i, chunkSize));
                    var httpRequestOptions = new HttpRequestOptions
                    {
                        Url = "https://api.telegram.org/bot" + botToken + "/sendmessage?chat_id=" + chatID + "&text=" + TelegramMessage,
                        CancellationToken = cancellationToken
                    };
                    using (await _httpClient.Post(httpRequestOptions).ConfigureAwait(false))
                    {

                    }
                }
            }
            else
            {
                string TelegramMessage = Uri.EscapeDataString(message);
                var httpRequestOptions = new HttpRequestOptions
                {
                    Url = "https://api.telegram.org/bot" + botToken + "/sendmessage?chat_id=" + chatID + "&text=" + TelegramMessage,
                    CancellationToken = cancellationToken
                };
                
                using (await _httpClient.Post(httpRequestOptions).ConfigureAwait(false))
                {

                }
            }
        }
    }
}
