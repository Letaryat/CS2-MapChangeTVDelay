using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using static CounterStrikeSharp.API.Core.Listeners;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Modules.Cvars;

namespace CS2MapChangeStopTV;

public class MapChangeStopTV : BasePluginConfig
{
    [JsonPropertyName("ReplayName")] public string ReplayName { get; set; } = "demo-{mapname}-{servername}-{date}-{time}-{timedate}";
    [JsonPropertyName("Debug")] public bool Debug { get; set; } = true;
}
public class CS2MapChangeStopTV : BasePlugin, IPluginConfig<MapChangeStopTV>
{
    public override string ModuleName => "CS2-MapChangeStopTV";
    public override string ModuleVersion => "0.0.3";
    public override string ModuleAuthor => "Letaryat";
    public override string ModuleDescription => "Delays map change and stops tv record";
    public string? fileName;
    public MapChangeStopTV Config { get; set; }
    public void OnConfigParsed(MapChangeStopTV config)
    {
        Config = config;
    }

    public override void Load(bool hotReload)
    {
        LogDebug("CS2-MapChangeStopTV - Loaded");
        //Listeners:
        AddCommandListener("changelevel", ListenerChangeLevel, HookMode.Pre);
        AddCommandListener("map", ListenerChangeLevel, HookMode.Pre);
        AddCommandListener("host_workshop_map", ListenerChangeLevel, HookMode.Pre);
        AddCommandListener("ds_workshop_changelevel", ListenerChangeLevel, HookMode.Pre);

        RegisterListener<OnMapStart>((mapname) => {
            fileName = null;
            LogDebug($"New map start ({mapname}). Filename = null");
        });
        RegisterEventHandler<EventCsWinPanelMatch>((e, i) => {
            Server.ExecuteCommand("tv_stoprecord");
            LogDebug("Recording stopped because of EventCsWinPanelMatch");
            return HookResult.Continue;
        });
        RegisterEventHandler<EventRoundStart>((e, i) =>
        {
            if(fileName != null) {return HookResult.Continue;}
            fileName = Config.ReplayName;
            var placeholdersReplacement = new Dictionary<string, string>
            {
                {"{mapname}", Server.MapName},
                {"{servername}", ConVar.Find("hostname")?.StringValue ?? "Unknown"},
                {"{date}", DateTime.Now.ToString("yyyy-MM-dd")},
                {"{time}", DateTime.Now.ToString("HH-mm-ss")},
                {"{timedate}", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")},
                {"{1s1kformat}", $"auto-{DateTime.Now:yyyyMMdd-HHmm}-{Server.MapName}"}
            };
            foreach(var placeholder in placeholdersReplacement)
            {
                fileName = fileName.Replace(placeholder.Key, placeholder.Value);
            }
            Server.NextWorldUpdate(() => Server.ExecuteCommand($"tv_record \"replays/{fileName}.dem\""));
            LogDebug($"Recording demo: {fileName}");
            return HookResult.Continue;
        });
    }
    public override void Unload(bool hotReload) 
    {
        LogDebug("CS2-MapChangeStopTV - Unloaded");
        Server.ExecuteCommand("tv_stoprecord");
    }
    public HookResult ListenerChangeLevel(CCSPlayerController? player, CommandInfo info)
    {
        Server.ExecuteCommand("tv_stoprecord");
        LogDebug("Stopped TV Record before changing map.");
        return HookResult.Continue;
    }
    public void LogDebug(string message)
    {
        if(Config.Debug == false) {return;}
        Logger.LogInformation($"{DateTime.Now} | {message}");
    }

}