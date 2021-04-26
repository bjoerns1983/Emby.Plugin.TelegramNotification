using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Notifications;
using MediaBrowser.Model.Logging;
using Emby.Plugin.TelegramNotification.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

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
            string message = Uri.EscapeDataString(request.Name);

            if (!string.IsNullOrEmpty(request.Description) && options.SendDescription)
            {
                string filmAffinityInfo = "";

                if (options.FilmAffinityRating)
                {
                    try
                    {
                        string title = request.Name.ToLower().Split("ha sido agregado")[0].Trim().Replace(" ", "+");
                        var url = "http://localhost:5000/api/title/" + title;

                        using var client = new HttpClient();
                        client.BaseAddress = new Uri(url);

                        HttpResponseMessage response = await client.GetAsync(url);
                        string strResult = await response.Content.ReadAsStringAsync();

                        using JsonDocument doc = JsonDocument.Parse(strResult);
                        JsonElement root = doc.RootElement;

                        var filmRating = root.GetProperty("rating");
                        var filmTitle = root.GetProperty("title");
                        var filmId = root.GetProperty("id");

                        if (filmRating.ToString().Any(char.IsDigit))
                        {
                            filmAffinityInfo = "\n\nFilmAffinity: " + filmRating + "/10";
                        }
                        _logger.Info("FilmAffinity film : {0} - {1} - {2}", filmId, filmTitle, filmRating);
                    }
                    catch (Exception e)
                    {
                        _logger.Error("Error retrieving FilmAffinity rating", e);
                    }
                }

                message = Uri.EscapeDataString(request.Name + "\n\n" + request.Description + filmAffinityInfo); 
            }

            _logger.Info("TeleGram to Token : {0} - {1} - {2}", options.BotToken, options.ChatID, request.Name);

            var _httpRequest = new HttpRequestOptions
            {
                Url = "https://api.telegram.org/bot" + options.BotToken + "/sendmessage?chat_id=" + options.ChatID + "&text=" + message,
                CancellationToken = cancellationToken
            };

            using (await _httpClient.Post(_httpRequest).ConfigureAwait(false))
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
