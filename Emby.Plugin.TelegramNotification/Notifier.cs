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

            var myJson = new Dictionary<string, string>
                {
                  {"chat_id", options.ChatID},
                };

            if (string.IsNullOrEmpty(request.Description) == false && options.SendDescription == true)
            {
                myJson.Add ("text", Uri.EscapeDataString(request.Name + "\n\n" + request.Description)); 
            }
            else
            {
                myJson.Add ("text", Uri.EscapeDataString(request.Name));
            }


            _logger.Debug("TeleGram to Token : {0} - {1} - {2}", options.BotToken, options.ChatID, request.Name);

            var httpRequestOptions = new HttpRequestOptions
            {
                Url = "https://api.telegram.org/bot" + options.BotToken + "/sendmessage",
                CancellationToken = cancellationToken
            };
            

            httpRequestOptions.SetPostData(myJson);

            using (await _httpClient.Post(httpRequestOptions).ConfigureAwait(false))
            {

            }
        }

        private bool IsValid(TeleGramOptions options)
        {
            return !string.IsNullOrEmpty(options.ChatID) &&
                !string.IsNullOrEmpty(options.BotToken);
        }
    }
}
