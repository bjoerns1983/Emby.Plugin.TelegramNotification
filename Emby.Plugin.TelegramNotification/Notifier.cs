using System.Collections.Generic;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Notifications;
using MediaBrowser.Model.Logging;
using Emby.Plugin.TelegramNotification.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Emby.Plugin.TelegramNotification
{
    public class Notifier : INotificationService
    {
        private readonly ILogger _logger;
        private readonly IHttpClient _httpClient;

        public Notifier(ILogManager logManager, IHttpClient httpClient)
        {
            _logger = logManager.GetLogger(GetType().Name);
            _httpClient = httpClient;
        }

        public bool IsEnabledForUser(User user)
        {
            var options = GetOptions(user);

            return options != null && IsValid(options) && options.Enabled;
        }

        private TeleGramOptions GetOptions(User user)
        {
            return Plugin.Instance.Configuration.Options
                .FirstOrDefault(i => string.Equals(i.MediaBrowserUserId, user.Id.ToString("N"), StringComparison.OrdinalIgnoreCase));
        }

        public string Name
        {
            get { return Plugin.Instance.Name; }
        }

        public async Task SendNotification(UserNotification request, CancellationToken cancellationToken)
        {

            var options = GetOptions(request.User);
            string message = (request.Name);

            if (string.IsNullOrEmpty(request.Description) == false && options.SendDescription == true)
            {
                message = (request.Name + "\n\n" + request.Description); 
            }

            _logger.Debug("TeleGram to Token : {0} - {1} - {2}", options.BotToken, options.ChatID, request.Name);


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
                        Url = "https://api.telegram.org/bot" + options.BotToken + "/sendmessage?chat_id=" + options.ChatID + "&text=" + TelegramMessage,
                        CancellationToken = CancellationToken.None
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
                    Url = "https://api.telegram.org/bot" + options.BotToken + "/sendmessage?chat_id=" + options.ChatID + "&text=" + TelegramMessage,
                    CancellationToken = CancellationToken.None
                };
                using (await _httpClient.Post(httpRequestOptions).ConfigureAwait(false))
                {

                }
            }

        }

        private bool IsValid(TeleGramOptions options)
        {
            return !string.IsNullOrEmpty(options.ChatID) &&
                !string.IsNullOrEmpty(options.BotToken);
        }
    }
}
