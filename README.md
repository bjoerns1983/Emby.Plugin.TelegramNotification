

# TelegramNotification FilmAffinity Edition™ 
<p align="center">
  <img src="https://raw.githubusercontent.com/rafagale/Emby.Plugin.TelegramNotification/master/Emby.Plugin.TelegramNotification/thumb.png" width="300px" alt="Telegram plugin" />
</p>

## Fork of the popular emby plugin [TelegramNotification](https://github.com/bjoerns1983/Emby.Plugin.TelegramNotification)
- Stay safe using updated dependencies.

- Notify your users about new content with the [best movie ratings website ever created.](https://www.filmaffinity.com/)

- Your users will know at a glance whether a movie is good or bad thanks to the accuracy of FilmAffinity ratings.

## Screenshots
Plugin config + Telegram notification examples
<p align="left">
  <img src=" https://raw.githubusercontent.com/rafagale/Emby.Plugin.TelegramNotification/master/screenshots/Screenshot_20210425-194411_r.png" width="300px" alt="Plugin screenshot" />
  
 <img src="https://raw.githubusercontent.com/rafagale/Emby.Plugin.TelegramNotification/master/screenshots/Screenshot_20210425-185058_r.png" width="300px" alt="Telegram screenshot" />
</p>

## Requirements
- An instance of [Emby Server](https://emby.media/download.html) using spanish language running.
- An instance of [this FilmAffinity API](#) running on port 5000 in the same server as Emby.


## Installation

Place the dll in your emby plugin directory

```sh
cp /home/user/Download/Emby.Plugin.TelegramNotification.dll /var/lib/emby/plugins/Emby.Plugin.TelegramNotification.dll
```

Restart your server

```sh
sudo service emby-server restart
```

Use the settings page of the Plugin to set your bot token and your chat id (See [screenshot](https://github.com/rafagale/Emby.Plugin.TelegramNotification/blob/master/screenshots/Screenshot_20210425-194411_r.png?raw=true))

Activate the Telegram Notifications Plugin in the desired server notifications.


