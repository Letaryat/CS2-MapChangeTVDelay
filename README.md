# CS2-MapChangeTVDelay
Changing map could cause Counter-Strike 2 servers to crash if they are using GOTV. This plugin should fix that. <br>
Thanks to [K4-GOTV-Discord](https://github.com/KitsuneLab-Development/K4-GOTV-Discord/tree/dev) for a way to listen changelevel and also for general idea how to fix this problem.
<br><img src="https://i.imgur.com/TQP4lYn.gif" height="200px">

## [📌] Setup
- Download latest release,
- Drag files to /plugins/
- Restart your server,
- Config file should be created in configs/plugins/CS2-MapChangeTVDelay,
- Edit to your liking,

```
{
  "ReplayName": "demo-{mapname}-{servername}-{date}-{time}-{timedate}" - You can use placeholders listed below (string),
  "DemoDirectory": "replays" - Folder name where to store demos (string),
  "Debug": true - if it should log everything that plugin does (bool),
  "ConfigVersion": 1
}
```

## [📌] Recommended GOTV settings:
```
tv_enable 1
tv_name "Your GOTV Name"
tv_relayvoice 1
tv_delaymapchange 1
tv_delay 0
tv_autorecord 0
```

## [📜] Placeholders:
| Placeholder  | Description |
| ------------- | ------------- |
| {mapname}  | Name of played map  |
| {servername}  | Server name (if it cannot fetch it, it returns "Unknown")  |
| {date}  | Returns date in format yyyy-mm-dd  |
| {time}  | Returns time in format HH-mm-ss  |
| {timedate}  | Returns time and date in format yyyy-MM-dd_HH-mm-ss  |
| {1s1kformat}  | Demo format that is used by 1s1k.pl to store demos. Returns auto-**Date-yyyyMMdd-HHmm**-ServerName |

### [🛠️] How does it work?
Plugin listens if changelevel was executed, if so it stops recording the map and then changes the level. When next map starts it start to record a new demo.

### [🚨] Plugin might be poorly written and have some issues. I have no idea what I am doing. Even so, when tested it works as intended.