# Emby.Plugin.TelegramNotification

Emby Server Plugin for sending notifications to a Telegram bot.

## Install
1. Install the Plugin by downloading the DLL (or build it yourself using VS2019) and putting it into your Emby Plugin folder
2. Restart your Emby Server
3. Talk to @Bodfather with your favorite Telegram Client to create your Telegram Bot
4. Start a chat with your bot (or place it in a channel) and send a message to it
5. Fire up your browser and type the following URL: `https://api.telegram.org/bot<BotTokenGoesHere>/getUpdates` 
   (without the braces) and find the ChatID 
6. Use the Settings Page of the Plugin to set Bot Token and Chat ID
7. Activate the Telegram Notifications Plugin in the desired Server notifications

Based on the raw Telegram Plugin of an unknown Author, spiced up with pieces of the Slack Notification Author and much googleling.
Please test it as much as you can. 
