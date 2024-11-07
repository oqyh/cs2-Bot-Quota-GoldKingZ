using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Utils;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Encodings.Web;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Cvars;
using System.Runtime.InteropServices;
using Bot_Quota_GoldKingZ.Config;
using System.Diagnostics;

namespace Bot_Quota_GoldKingZ;

public class Helper
{
    public static void AdvancedPlayerPrintToChat(CCSPlayerController player, string message, params object[] args)
    {
        if (string.IsNullOrEmpty(message))return;

        for (int i = 0; i < args.Length; i++)
        {
            message = message.Replace($"{{{i}}}", args[i].ToString());
        }
        if (Regex.IsMatch(message, "{nextline}", RegexOptions.IgnoreCase))
        {
            string[] parts = Regex.Split(message, "{nextline}", RegexOptions.IgnoreCase);
            foreach (string part in parts)
            {
                string messages = part.Trim();
                player.PrintToChat(" " + messages);
            }
        }else
        {
            player.PrintToChat(message);
        }
    }
    public static void AdvancedServerPrintToChatAll(string message, params object[] args)
    {
        if (string.IsNullOrEmpty(message))return;

        for (int i = 0; i < args.Length; i++)
        {
            message = message.Replace($"{{{i}}}", args[i].ToString());
        }
        if (Regex.IsMatch(message, "{nextline}", RegexOptions.IgnoreCase))
        {
            string[] parts = Regex.Split(message, "{nextline}", RegexOptions.IgnoreCase);
            foreach (string part in parts)
            {
                string messages = part.Trim();
                Server.PrintToChatAll(" " + messages);
            }
        }else
        {
            Server.PrintToChatAll(message);
        }
    }
    public static void AdvancedPlayerPrintToConsole(CCSPlayerController player, string message, params object[] args)
    {
        if (string.IsNullOrEmpty(message))return;
        
        for (int i = 0; i < args.Length; i++)
        {
            message = message.Replace($"{{{i}}}", args[i].ToString());
        }
        if (Regex.IsMatch(message, "{nextline}", RegexOptions.IgnoreCase))
        {
            string[] parts = Regex.Split(message, "{nextline}", RegexOptions.IgnoreCase);
            foreach (string part in parts)
            {
                string messages = part.Trim();
                player.PrintToConsole(" " + messages);
            }
        }else
        {
            player.PrintToConsole(message);
        }
    }
    
    public static bool IsPlayerInGroupPermission(CCSPlayerController player, string groups)
    {
        var excludedGroups = groups.Split(',');
        foreach (var group in excludedGroups)
        {
            switch (group[0])
            {
                case '#':
                    if (AdminManager.PlayerInGroup(player, group))
                        return true;
                    break;

                case '@':
                    if (AdminManager.PlayerHasPermissions(player, group))
                        return true;
                    break;

                default:
                    return false;
            }
        }
        return false;
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
        BotQuotaGoldKingZ.Instance.Onetime = false;
        Globals.BotCheckTimer?.Kill();
        Globals.BotCheckTimer = null;
    }
    
    public static string ReplaceMessages(string Message, string date, string time, string PlayerName, string SteamId, string ipAddress, string reason)
    {
        var replacedMessage = Message
                                    .Replace("{TIME}", time)
                                    .Replace("{DATE}", date)
                                    .Replace("{PLAYERNAME}", PlayerName.ToString())
                                    .Replace("{STEAMID}", SteamId.ToString())
                                    .Replace("{IP}", ipAddress.ToString())
                                    .Replace("{REASON}", reason);
        return replacedMessage;
    }
    public static string RemoveLeadingSpaces(string content)
    {
        string[] lines = content.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].TrimStart();
        }
        return string.Join("\n", lines);
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
    public static void DebugMessage(string message)
    {
        if(!Configs.GetConfigData().EnableDebug)return;
        Console.WriteLine($"================================= [ Debug Bot Quota ] =================================");
        Console.WriteLine(message);
        Console.WriteLine("=====================================================================================================");
    }

    public static void CheckPlayersAndAddBots()
    {
        if(Configs.GetConfigData().DisablePluginOnWarmUp && IsWarmup())return;
        bool counteSpec = Configs.GetConfigData().IncludeCountingSpecPlayers? true : false;
        var PlayersCounts = GetPlayersCount(false,counteSpec);

        string botmode = "";
        if(Configs.GetConfigData().BotAddMode.Contains("normal", StringComparison.OrdinalIgnoreCase))
        {
            botmode = "normal";
        }else if(Configs.GetConfigData().BotAddMode.Contains("fill", StringComparison.OrdinalIgnoreCase))
        {
            botmode = "fill";
        }else if(Configs.GetConfigData().BotAddMode.Contains("match", StringComparison.OrdinalIgnoreCase))
        {
            botmode = "match";
        }else
        {
            botmode = "fill";
        }
        if(Configs.GetConfigData().AddBotsWhenXOrLessPlayersInServer >= PlayersCounts && !BotQuotaGoldKingZ.Instance.Onetime)
        {
            Server.ExecuteCommand($"bot_quota_mode {botmode}; bot_quota {Configs.GetConfigData().HowManyBotsShouldAdd}");
            AdvancedServerPrintToChatAll(Configs.Shared.StringLocalizer![$"PrintChatToAll.LessPlayers"], Configs.GetConfigData().HowManyBotsShouldAdd,PlayersCounts);
            
            
            if(!string.IsNullOrEmpty(Configs.GetConfigData().ExecConfigWhenBotsAdded))
            {
                string FileWhenBotsAdded = Path.Combine(Server.GameDirectory, $"csgo/cfg/{Configs.GetConfigData().ExecConfigWhenBotsAdded}");
                try
                {
                    if (!File.Exists(FileWhenBotsAdded))
                    {
                        DebugMessage($"File not found: {FileWhenBotsAdded}");
                        return;
                    }
                    Server.ExecuteCommand($"exec {Configs.GetConfigData().ExecConfigWhenBotsAdded}");
                    
                }catch (Exception ex)
                {
                    DebugMessage(ex.Message);
                }
            }

            BotQuotaGoldKingZ.Instance.Onetime = true;
        }else if(Configs.GetConfigData().AddBotsWhenXOrLessPlayersInServer < PlayersCounts && BotQuotaGoldKingZ.Instance.Onetime)
        {
            Server.ExecuteCommand("bot_kick");
            AdvancedServerPrintToChatAll(Configs.Shared.StringLocalizer![$"PrintChatToAll.KickBots"], Configs.GetConfigData().HowManyBotsShouldAdd,PlayersCounts);

            if(!string.IsNullOrEmpty(Configs.GetConfigData().ExecConfigWhenBotsKicked))
            {
                string FileWhenBotsKicked = Path.Combine(Server.GameDirectory, $"csgo/cfg/{Configs.GetConfigData().ExecConfigWhenBotsKicked}");
                try
                {
                    if (!File.Exists(FileWhenBotsKicked))
                    {
                        DebugMessage($"File not found: {FileWhenBotsKicked}");
                        return;
                    }
                    Server.ExecuteCommand($"exec {Configs.GetConfigData().ExecConfigWhenBotsKicked}");
                    
                }catch (Exception ex)
                {
                    DebugMessage(ex.Message);
                }
            }

            BotQuotaGoldKingZ.Instance.Onetime = false;
        }
    }
}