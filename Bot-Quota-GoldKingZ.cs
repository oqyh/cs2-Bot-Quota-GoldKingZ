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

namespace Bot_Quota_GoldKingZ;


public class BotQuotaGoldKingZ : BasePlugin
{
    public override string ModuleName => "Bot Quota (Kick/Add Bots Depend How Many Players In The Server)";
    public override string ModuleVersion => "1.0.4";
    public override string ModuleAuthor => "Gold KingZ";
    public override string ModuleDescription => "https://github.com/oqyh";
    public static BotQuotaGoldKingZ Instance { get; set; } = new();
    public Globals g_Main = new();
    public override void Load(bool hotReload)
    {
        Instance = this;
        Configs.Load(ModuleDirectory);
        Configs.Shared.CookiesModule = ModuleDirectory;
        Configs.Shared.StringLocalizer = Localizer;

        RegisterListener<Listeners.OnMapStart>(OnMapStart);
        RegisterListener<Listeners.OnMapEnd>(OnMapEnd);

        Server.ExecuteCommand("sv_hibernate_when_empty false");
        g_Main.BotCheckTimer?.Kill();
        g_Main.BotCheckTimer = null;
        g_Main.BotCheckTimer = AddTimer(1.30f, Helper.CheckPlayersAndAddBots, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
    }
    private void OnMapStart(string Map)
    {
        Server.ExecuteCommand("sv_hibernate_when_empty false");
        g_Main.BotCheckTimer?.Kill();
        g_Main.BotCheckTimer = null;
        g_Main.BotCheckTimer = AddTimer(1.30f, Helper.CheckPlayersAndAddBots, TimerFlags.REPEAT | TimerFlags.STOP_ON_MAPCHANGE);
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