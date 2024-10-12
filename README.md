## .:[ Join Our Discord For Support ]:.
<a href="https://discord.com/invite/U7AuQhu"><img src="https://discord.com/api/guilds/651838917687115806/widget.png?style=banner2"></a>

***
# [CS2] Bot-Quota-GoldKingZ (1.0.0)

### Kick / Add Bots Depend How Many Players In The Server


## .:[ Dependencies ]:.
[Metamod:Source (2.x)](https://www.sourcemm.net/downloads.php/?branch=master)

[CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/releases)

[Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json)



## .:[ Configuration ]:.

> [!CAUTION]
> Config Located In ..\addons\counterstrikesharp\plugins\Bot-Quota-GoldKingZ\config\config.json                                           
>

```json
{
  //Disable Plugin On WarmUp?
  "DisablePluginOnWarmUp": false,

  //Check Players By Timer?
  "CheckPlayersByTimer": true,

  //Added Bots When 5 Or Less Players In The Server
  "AddBotsWhenXOrLessPlayersInServer": 5,

  //Do You Want Spec Players Count With AddBotsWhenXOrLessPlayersInServer?
  //true = Yes
  //false = No Exclude Them From Counting
  "IncludeCountingSpecPlayers": true,

  //How Many Bots Should Add When AddBotsWhenXOrLessPlayersInServer Happens
  "HowManyBotsShouldAdd": 10,

  //Add Bot By Mode? (normal,fill,match)
  "BotAddMode": "fill",

  //Exec cfg When Bots Added (Note Need cfg To Be Inside csgo/cfg/
  //Example : Bot-Quota-GoldKingZ/WhenBotsAdded.cfg 
  "ExecConfigWhenBotsAdded": "",

  //Exec cfg When Bots Kicked (Note Need cfg To Be Inside csgo/cfg/
  //Example : Bot-Quota-GoldKingZ/WhenBotsKicked.cfg 
  "ExecConfigWhenBotsKicked": "",

//----------------------------[ ↓ Debug ↓ ]----------------------------

  //Enable Debug Will Print Server Console If You Face Any Issue
  "EnableDebug": false,
}
```

![329846165-ba02c700-8e0b-4ebe-bc28-103b796c0b2e](https://github.com/oqyh/cs2-Game-Manager/assets/48490385/3df7caa9-34a7-47da-94aa-8d682f59e85d)


## .:[ Language ]:.
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
	//{0} = How Many Bots Added
	//{1} = Players Counts
	//==========================
	
    "PrintChatToAll.LessPlayers": "{green}Gold KingZ {grey}| {grey}Server Has Less Players {lime}Adding {0} Bots",
    "PrintChatToAll.KickBots": "{green}Gold KingZ {grey}| {grey}Server Has More Players {darkred}Kicking All Bots"
}
```


## .:[ Change Log ]:.
```
(1.0.0)
-Initial Release
```

## .:[ Donation ]:.

If this project help you reduce time to develop, you can give me a cup of coffee :)

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://paypal.me/oQYh)
