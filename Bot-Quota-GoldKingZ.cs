using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Localization;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Timers;
using Bot_Quota_GoldKingZ.Config;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Bot_Quota_GoldKingZ;


[MinimumApiVersion(260)]
public class BotQuotaGoldKingZ : BasePlugin
{
    public override string ModuleName => "Bot Quota (Kick/Add Bots Depend How Many Players In The Server)";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "https://github.com/oqyh";
	internal static IStringLocalizer? Stringlocalizer;
    public bool Onetime = false;
    public static BotQuotaGoldKingZ Instance { get; set; } = new();

    public override void Load(bool hotReload)
    {
        Instance = this;
        Configs.Load(ModuleDirectory);
        Stringlocalizer = Localizer;
        Configs.Shared.CookiesModule = ModuleDirectory;
        Configs.Shared.StringLocalizer = Localizer;
        RegisterListener<Listeners.OnMapEnd>(OnMapEnd);
        RegisterListener<Listeners.OnClientConnected>(OnClientConnected);
        RegisterListener<Listeners.OnClientDisconnectPost>(OnClientDisconnectPost);
        RegisterListener<Listeners.OnMapStart>(OnMapStart);

        AddCommandListener("jointeam", OnJoinTeam);

        if(Configs.GetConfigData().CheckPlayersByTimer)
        {
            Server.ExecuteCommand("sv_hibernate_when_empty false");
            Globals.BotCheckTimer?.Kill();
            Globals.BotCheckTimer = null;
            Globals.BotCheckTimer = AddTimer(1.30f, Helper.CheckPlayersAndAddBots, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
        }
    }
    private void OnMapStart(string Map)
    {
        if(Configs.GetConfigData().CheckPlayersByTimer)
        {
            Server.ExecuteCommand("sv_hibernate_when_empty false");
            Globals.BotCheckTimer?.Kill();
            Globals.BotCheckTimer = null;
            Globals.BotCheckTimer = AddTimer(1.30f, Helper.CheckPlayersAndAddBots, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
        }
    }

    private void OnClientConnected(int playerSlot)
    {
        if(Configs.GetConfigData().CheckPlayersByTimer)return;
        Helper.CheckPlayersAndAddBots();
    }
    private void OnClientDisconnectPost(int playerSlot)
    {
        if(Configs.GetConfigData().CheckPlayersByTimer)return;
        Helper.CheckPlayersAndAddBots();
    }
    public HookResult OnJoinTeam(CCSPlayerController? player, CommandInfo info)
    {
        if(Configs.GetConfigData().CheckPlayersByTimer)return HookResult.Continue;
        Helper.CheckPlayersAndAddBots();
        return HookResult.Continue;
    }


    private void OnMapEnd()
    {
        Helper.ClearVariables();
    }
    public override void Unload(bool hotReload)
    {
        Helper.ClearVariables();
    }
    
}