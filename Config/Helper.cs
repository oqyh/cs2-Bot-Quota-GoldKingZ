using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using System.Text.RegularExpressions;
using Bot_Quota_GoldKingZ.Config;
using CounterStrikeSharp.API.Core.Translations;

namespace Bot_Quota_GoldKingZ;

public class Helper
{
    public static void AdvancedPlayerPrintToChat(CCSPlayerController player, string message, params object[] args)
    {
        if (string.IsNullOrEmpty(message)) return;

        for (int i = 0; i < args.Length; i++)
        {
            message = message.Replace($"{{{i}}}", args[i].ToString());
        }
        if (Regex.IsMatch(message, "{nextline}", RegexOptions.IgnoreCase))
        {
            string[] parts = Regex.Split(message, "{nextline}", RegexOptions.IgnoreCase);
            foreach (string part in parts)
            {
                string trimmedPart = part.Trim();
                trimmedPart = trimmedPart.ReplaceColorTags();
                if (!string.IsNullOrEmpty(trimmedPart))
                {
                    player.PrintToChat(" " + trimmedPart);
                }
            }
        }
        else
        {
            message = message.ReplaceColorTags();
            player.PrintToChat(message);
        }
    }
    public static void AdvancedPlayerPrintToConsole(CCSPlayerController player, string message, params object[] args)
    {
        if (string.IsNullOrEmpty(message)) return;

        for (int i = 0; i < args.Length; i++)
        {
            message = message.Replace($"{{{i}}}", args[i].ToString());
        }
        if (Regex.IsMatch(message, "{nextline}", RegexOptions.IgnoreCase))
        {
            string[] parts = Regex.Split(message, "{nextline}", RegexOptions.IgnoreCase);
            foreach (string part in parts)
            {
                string trimmedPart = part.Trim();
                trimmedPart = trimmedPart.ReplaceColorTags();
                if (!string.IsNullOrEmpty(trimmedPart))
                {
                    player.PrintToConsole(" " + trimmedPart);
                }
            }
        }
        else
        {
            message = message.ReplaceColorTags();
            player.PrintToConsole(message);
        }
    }
    public static void AdvancedServerPrintToChatAll(string message, params object[] args)
    {
        if (string.IsNullOrEmpty(message)) return;

        for (int i = 0; i < args.Length; i++)
        {
            message = message.Replace($"{{{i}}}", args[i].ToString());
        }
        if (Regex.IsMatch(message, "{nextline}", RegexOptions.IgnoreCase))
        {
            string[] parts = Regex.Split(message, "{nextline}", RegexOptions.IgnoreCase);
            foreach (string part in parts)
            {
                string trimmedPart = part.Trim();
                trimmedPart = trimmedPart.ReplaceColorTags();
                if (!string.IsNullOrEmpty(trimmedPart))
                {
                    Server.PrintToChatAll(" " + trimmedPart);
                }
            }
        }
        else
        {
            message = message.ReplaceColorTags();
            Server.PrintToChatAll(message);
        }
    }
    public static List<CCSPlayerController> GetPlayersController(bool IncludeBots = false, bool IncludeSPEC = true, bool IncludeCT = true, bool IncludeT = true) 
    {
        var playerList = Utilities
            .FindAllEntitiesByDesignerName<CCSPlayerController>("cs_player_controller")
            .Where(p => p != null && p.IsValid && 
                        (IncludeBots || (!p.IsBot && !p.IsHLTV)) && 
                        p.Connected == PlayerConnectedState.PlayerConnected && 
                        ((IncludeCT && p.TeamNum == (byte)CsTeam.CounterTerrorist) || 
                        (IncludeT && p.TeamNum == (byte)CsTeam.Terrorist) || 
                        (IncludeSPEC && p.TeamNum == (byte)CsTeam.Spectator)))
            .ToList();

        return playerList;
    }
    public static int GetPlayersCount(bool IncludeBots = false, bool IncludeSPEC = true, bool IncludeCT = true, bool IncludeT = true)
    {
        return Utilities.GetPlayers().Count(p => 
            p != null && 
            p.IsValid && 
            p.Connected == PlayerConnectedState.PlayerConnected && 
            (IncludeBots || (!p.IsBot && !p.IsHLTV)) && 
            ((IncludeCT && p.TeamNum == (byte)CsTeam.CounterTerrorist) || 
            (IncludeT && p.TeamNum == (byte)CsTeam.Terrorist) || 
            (IncludeSPEC && p.TeamNum == (byte)CsTeam.Spectator))
        );
    }

    public static void ClearVariables()
    {
        var g_Main = BotQuotaGoldKingZ.Instance.g_Main;
        g_Main.BotCheckTimer?.Kill();
        g_Main.BotCheckTimer = null;
        g_Main.onetime = false;
    }
    public static CCSGameRules? GetGameRules()
    {
        try
        {
            var gameRulesEntities = Utilities.FindAllEntitiesByDesignerName<CCSGameRulesProxy>("cs_gamerules");
            return gameRulesEntities.First().GameRules;
        }
        catch
        {
            return null;
        }
    }
    public static bool IsWarmup()
    {
        return GetGameRules()?.WarmupPeriod ?? false;
    }

    public static void DebugMessage(string message, bool prefix = true)
    {
        if (!Configs.GetConfigData().EnableDebug) return;
        Console.ForegroundColor = ConsoleColor.Magenta;
        string Prefix = "[Bot Quota]: ";
        Console.WriteLine(prefix?Prefix:"" + message);
        Console.ResetColor();
    }

    public static void CheckPlayersAndAddBots()
    {
        if (Configs.GetConfigData().DisablePluginOnWarmUp && IsWarmup()) 
        {
            DebugMessage("DisablePluginOnWarmUp is Enabled");
            DebugMessage("WarmUp Is Active Result Plugin Will Disable On WarmUp");
            return;
        }
        
        var PlayersCounts = GetPlayersCount(false, Configs.GetConfigData().IncludeCountingSpecPlayers);
        var Bots_InGame = Utilities.GetPlayers().Count(p => p != null && p.IsValid && p.IsBot && !p.IsHLTV && (p.TeamNum == (byte)CsTeam.Terrorist || p.TeamNum == (byte)CsTeam.CounterTerrorist));

        string botmode = Configs.GetConfigData().BotAddMode.ToLower() switch
        {
            "normal" => "normal",
            "fill" => "fill",
            "match" => "match",
            _ => "fill"
        };


        if (Configs.GetConfigData().AddBotsWhenXOrLessPlayersInServer > PlayersCounts)
        {
            if(Bots_InGame == 0 && Configs.GetConfigData().HowManyBotsShouldAdd > 0)
            {
                ExecuteConfig(Configs.GetConfigData().ExecConfigWhenBotsAdded);
                Server.ExecuteCommand($"bot_quota_mode {botmode}; bot_quota {Configs.GetConfigData().HowManyBotsShouldAdd}");
                if(BotQuotaGoldKingZ.Instance.g_Main.onetime == false)
                {
                    AdvancedServerPrintToChatAll(Configs.Shared.StringLocalizer![$"PrintChatToAll.LessPlayers"], Configs.GetConfigData().HowManyBotsShouldAdd, PlayersCounts);
                }
                BotQuotaGoldKingZ.Instance.g_Main.onetime = true;
            }
        }
        else
        {
            Server.ExecuteCommand("bot_kick");
            ExecuteConfig(Configs.GetConfigData().ExecConfigWhenBotsKicked);
            if(BotQuotaGoldKingZ.Instance.g_Main.onetime == true)
            {
                AdvancedServerPrintToChatAll(Configs.Shared.StringLocalizer![$"PrintChatToAll.KickBots"], Configs.GetConfigData().HowManyBotsShouldAdd, PlayersCounts);
            }
            BotQuotaGoldKingZ.Instance.g_Main.onetime = false;
        }
        
    }

    private static void ExecuteConfig(string configName)
    {
        if (!string.IsNullOrEmpty(configName))
        {
            string configPath = Path.Combine(Server.GameDirectory, $"csgo/cfg/{configName}");
            try
            {
                if (File.Exists(configPath))
                {
                    Server.ExecuteCommand($"execifexists {configName}");
                }
                else
                {
                    DebugMessage($"File not found: {configPath}");
                }
            }
            catch (Exception ex)
            {
                DebugMessage(ex.Message);
            }
        }
    }
}