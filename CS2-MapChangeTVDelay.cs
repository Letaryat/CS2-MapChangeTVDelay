using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using static CounterStrikeSharp.API.Core.Listeners;
using Microsoft.Extensions.Logging;

namespace CS2MapChangeStopTV;
public class CS2MapChangeStopTV : BasePlugin
{
    public override string ModuleName => "CS2-MapChangeStopTV";
    public override string ModuleVersion => "0.0.1";
    public override string ModuleAuthor => "Letaryat";
    public override string ModuleDescription => "Delays map change and stops tv record";
    public string? fileName;
    public override void Load(bool hotReload)
    {
        Logger.LogInformation("CS2-MapChangeStopTV - Loaded");
        AddCommandListener("changelevel", ListenerChangeLevel, HookMode.Pre);
        AddCommandListener("map", ListenerChangeLevel, HookMode.Pre);
		AddCommandListener("host_workshop_map", ListenerChangeLevel, HookMode.Pre);
		AddCommandListener("ds_workshop_changelevel", ListenerChangeLevel, HookMode.Pre);
        RegisterListener<OnMapStart>((mapname) => {
            fileName = null;
            Logger.LogInformation($"Filename = null");
        });
        RegisterEventHandler<EventCsWinPanelMatch>((e, i) => {
            Server.ExecuteCommand("tv_stoprecord");
            Logger.LogInformation($"Stopped TV Record by end panel.");
            return HookResult.Continue;
        });
        RegisterEventHandler<EventRoundStart>((e, i) =>
        {
            var mapname = Server.MapName;
            if(fileName != null) {return HookResult.Continue;}
            fileName = $"auto-{DateTime.Now:yyyyMMdd-HHmm}-{mapname}-CS2_____Arena_1v1_____Pierdolnik.eu___1shot1kill.pl";
            Server.NextWorldUpdate(() => Server.ExecuteCommand($"tv_record \"replays/{fileName}.dem\""));
            Logger.LogInformation($"Started recording demo: {fileName}");
            return HookResult.Continue;
        });
    }
    public override void Unload(bool hotReload) 
    {
        Logger.LogInformation("CS2-MapChangeStopTV - Unloaded");
        Server.ExecuteCommand("tv_stoprecord");
    }
    public HookResult ListenerChangeLevel(CCSPlayerController? player, CommandInfo info)
    {
        Server.ExecuteCommand("tv_stoprecord");
        Logger.LogInformation($"Stopped TV Record. Delay before changing map.");
        return HookResult.Continue;
    }

}