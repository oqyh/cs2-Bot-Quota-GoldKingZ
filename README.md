## .:[ Join Our Discord For Support ]:.

<a href="https://discord.com/invite/U7AuQhu"><img src="https://discord.com/api/guilds/651838917687115806/widget.png?style=banner2"></a>

# [CS2] Bot-Quota-GoldKingZ (1.0.3)

Kick / Add Bots Depend How Many Players In The Server

![bot-quote](https://github.com/user-attachments/assets/c88a8ba3-dfaf-4265-9e22-1a4174370d8d)

---

## 📦 Dependencies
[![Metamod:Source](https://img.shields.io/badge/Metamod:Source-2.x-2d2d2d?logo=sourceengine)](https://www.sourcemm.net/downloads.php?branch=dev)

[![CounterStrikeSharp](https://img.shields.io/badge/CounterStrikeSharp-83358F)](https://github.com/roflmuffin/CounterStrikeSharp)

---

## 📥 Installation

1. Download latest release
2. Extract to `csgo` directory
3. Configure `Bot-Quota-GoldKingZ\config\config.json`
4. Restart server

---

## ⚙️ Configuration

> [!NOTE]
> Located In ..\Bot-Quota-GoldKingZ\config\config.json                                           
>

| Property | Description | Values | Required |  
|----------|-------------|--------|----------|  
| `DisablePluginOnWarmUp` | Disable Plugin On WarmUp | `true`/`false` | - |  
| `AddBotsWhenXOrLessPlayersInServer` | Add Bots When X Or Less Players In The Server | `Integer` (e.g., `5`) | - |  
| `IncludeCountingSpecPlayers` | Include Counting In `AddBotsWhenXOrLessPlayersInServer` Spectator Players | `true`/`false` | `AddBotsWhenXOrLessPlayersInServer=x` |  
| `HowManyBotsShouldAdd` | How Many Bots Should Add When `AddBotsWhenXOrLessPlayersInServer` Pass | `Integer` (e.g., `10`) | `AddBotsWhenXOrLessPlayersInServer=x` |    
| `BotAddMode` | Add Bots By Mode |`String` (e.g., `fill`)<br> `normal`-The Number Of Bots On The Server Equals HowManyBotsShouldAdd<br>`fill`-The Server Is Filled With Bots Until There Are At Least HowManyBotsShouldAdd Players On The Server (Humans + Bots). Human Players Joining Cause An Existing Bot To Be Kicked, Human Players Leaving Might Cause A Bot To Be Added<br>`match`-The Number Of Bots On The Server Equals The Number Of Human Players Times HowManyBotsShouldAdd | - |  
| `ExecConfigWhenBotsAdded` | Custom Cfg When Bots Added | `String` (e.g., `Bot-Quota-GoldKingZ/WhenBotsAdded.cfg`) | - |  
| `ExecConfigWhenBotsKicked` | Custom Cfg When Bots Kicked | `String` (e.g., `Bot-Quota-GoldKingZ/WhenBotsKicked.cfg`) | - |  
| `EnableDebug` | Debug mode | `true`/`false` | - |  


---

## 🌍 Language

> [!NOTE]
> Located In ..\Bot-Quota-GoldKingZ\lang\en.json                                           
>

<details>
<summary>🖼️ Preview Colors In Game (Click to expand 🔽)</summary>

![Color Preview](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/3df7caa9-34a7-47da-94aa-8d682f59e85d)
</details>

```json
{
	//==========================
	//        Colors
	//==========================
	//{Yellow} {Gold} {Silver} {Blue} {DarkBlue} {BlueGrey} {Magenta} {LightRed}
	//{LightBlue} {Olive} {Lime} {Red} {Purple} {Grey}
	//{Default} {White} {Darkred} {Green} {LightYellow}
	//==========================
	//        Other
	//==========================
	//{nextline} = Print On Next Line
	//{0} = How Many Bots Added/Kicked
	//{1} = Players In The Server
	//==========================

	"PrintChatToAll.LessPlayers": "{green}Gold KingZ {grey}| {grey}Server Has Less Players {lime}Adding {0} Bots",
	"PrintChatToAll.KickBots": "{green}Gold KingZ {grey}| {grey}Server Has More Players {darkred}Kicking All Bots"
}
```

---

## 📜 Changelog

<details>
<summary>📋 View Version History (Click to expand 🔽)</summary>

### [1.0.3]
- Fix Some Bugs
- Removed CheckPlayersByTimer
- Added In config.json info on each what it do

### [1.0.2]
- Fix Bug Counting
- Added Some Debugs Info

### [1.0.1]
- Fix Bug

### [1.0.0]
- Initial Release

</details>

---
