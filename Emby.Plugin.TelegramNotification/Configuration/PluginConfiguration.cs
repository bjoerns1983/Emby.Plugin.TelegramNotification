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
        public String DeviceName { get; set; }
        public List<Sound> SoundList { get; set; }
        public int Priority { get; set; }
        public string MediaBrowserUserId { get; set; }

        public TeleGramOptions()
        {
            SoundList = new List<Sound>
            {
                new Sound() {Name = "Telegram", Value = "telegram"},
                new Sound() {Name = "Bike", Value = "bike"},
                new Sound() {Name = "Bugle", Value = "bugle"}
            };
        }
    }

    public class Sound
    {
        public String Name { get; set; }
        public String Value { get; set; }
    }
}
