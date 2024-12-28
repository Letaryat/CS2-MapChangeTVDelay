# CS2-MapChangeTVDelay
Changing map could cause Counter-Strike 2 servers to crash if they are using GOTV. This plugin should fix that. <br>
Thanks to [K4-GOTV-Discord](https://github.com/KitsuneLab-Development/K4-GOTV-Discord/tree/dev) for a way to listen changelevel and also for general idea how to fix this problem.
<br><img src="https://i.imgur.com/TQP4lYn.gif" height="200px">

## How does it work?
Plugin listens if changelevel was executed, if so it stops recording the map and then changes the level. When next map starts it start to record a new demo.

### [🚨] Plugin might be poorly written and have some issues. I have no idea what I am doing.