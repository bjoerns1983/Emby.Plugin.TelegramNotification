using System;
using System.Collections.Generic;
using MediaBrowser.Model.Plugins;

namespace Emby.Plugin.TelegramNotification.Configuration
{
    /// <summary>
    /// Class PluginConfiguration
    /// </summary>
    public class PluginConfiguration : BasePluginConfiguration
    {
        public TeleGramOptions[] Options { get; set; }

        public PluginConfiguration()
        {
            Options = new TeleGramOptions[] { };
        }
    }

    public class TeleGramOptions
    {
        public Boolean Enabled { get; set; }
        public String ChatID { get; set; }
        public String BotToken { get; set; }
        public Boolean SendDescription { get; set; }
        public Boolean FilmAffinityRating { get; set; }
        public String DeviceName { get; set; }
        public int Priority { get; set; }
        public string MediaBrowserUserId { get; set; }

    }

}
