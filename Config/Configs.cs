using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Localization;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Text;


namespace Bot_Quota_GoldKingZ.Config
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RangeAttribute : Attribute
    {
        public int Min { get; }
        public int Max { get; }
        public int Default { get; }
        public string Message { get; }

        public RangeAttribute(int min, int max, int defaultValue, string message)
        {
            Min = min;
            Max = max;
            Default = defaultValue;
            Message = message;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CommentAttribute : Attribute
    {
        public string Comment { get; }

        public CommentAttribute(string comment)
        {
            Comment = comment;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class BreakLineAttribute : Attribute
    {
        public string BreakLine { get; }

        public BreakLineAttribute(string breakLine)
        {
            BreakLine = breakLine;
        }
    }
    public static class Configs
    {
        public static class Shared {
            public static string? CookiesModule { get; set; }
            public static IStringLocalizer? StringLocalizer { get; set; }
        }
        private static readonly string ConfigDirectoryName = "config";
        private static readonly string ConfigFileName = "config.json";
        private static string? _configFilePath;
        private static ConfigData? _configData;

        private static readonly JsonSerializerOptions SerializationOptions = new()
        {
            Converters =
            {
                new JsonStringEnumConverter()
            },
            WriteIndented = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
        };

        public static bool IsLoaded()
        {
            return _configData is not null;
        }

        public static ConfigData GetConfigData()
        {
            if (_configData is null)
            {
                throw new Exception("Config not yet loaded.");
            }
            
            return _configData;
        }

        public static ConfigData Load(string modulePath)
        {
            var configFileDirectory = Path.Combine(modulePath, ConfigDirectoryName);
            if(!Directory.Exists(configFileDirectory))
            {
                Directory.CreateDirectory(configFileDirectory);
            }

            _configFilePath = Path.Combine(configFileDirectory, ConfigFileName);
            if (File.Exists(_configFilePath))
            {
                _configData = JsonSerializer.Deserialize<ConfigData>(File.ReadAllText(_configFilePath), SerializationOptions);
                _configData!.Validate();
            }
            else
            {
                _configData = new ConfigData();
                _configData.Validate();
            }

            if (_configData is null)
            {
                throw new Exception("Failed to load configs.");
            }

            SaveConfigData(_configData);
            
            return _configData;
        }

        private static void SaveConfigData(ConfigData configData)
        {
            if (_configFilePath is null)
                throw new Exception("Config not yet loaded.");

            string json = JsonSerializer.Serialize(configData, SerializationOptions);
            json = Regex.Unescape(json);

            var lines = json.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var newLines = new List<string>();

            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"^\s*""(\w+)""\s*:.*");
                bool isPropertyLine = false;
                PropertyInfo? propInfo = null;

                if (match.Success)
                {
                    string propName = match.Groups[1].Value;
                    propInfo = typeof(ConfigData).GetProperty(propName);

                    var breakLineAttr = propInfo?.GetCustomAttribute<BreakLineAttribute>();
                    if (breakLineAttr != null)
                    {
                        string breakLine = breakLineAttr.BreakLine;

                        if (breakLine.Contains("{space}"))
                        {
                            breakLine = breakLine.Replace("{space}", "").Trim();

                            if (breakLineAttr.BreakLine.StartsWith("{space}"))
                            {
                                newLines.Add("");
                            }

                            newLines.Add("// " + breakLine);
                            newLines.Add("");
                        }
                        else
                        {
                            newLines.Add("// " + breakLine);
                        }
                    }

                    var commentAttr = propInfo?.GetCustomAttribute<CommentAttribute>();
                    if (commentAttr != null)
                    {
                        var commentLines = commentAttr.Comment.Split('\n');
                        foreach (var commentLine in commentLines)
                        {
                            newLines.Add("// " + commentLine.Trim());
                        }
                    }

                    isPropertyLine = true;
                }
                newLines.Add(line);
                if (isPropertyLine && propInfo?.GetCustomAttribute<CommentAttribute>() != null)
                {
                    newLines.Add("");
                }
            }

            File.WriteAllText(_configFilePath, string.Join(Environment.NewLine, newLines), Encoding.UTF8);
        }

        public class ConfigData
        {
            private string? _Version;
            private string? _Link;
            [BreakLine("----------------------------[ ↓ Plugin Info ↓ ]----------------------------{space}")]
            public string Version
            {
                get => _Version!;
                set
                {
                    _Version = value;
                    if (_Version != BotQuotaGoldKingZ.Instance.ModuleVersion)
                    {
                        Version = BotQuotaGoldKingZ.Instance.ModuleVersion;
                    }
                }
            }

            public string Link
            {
                get => _Link!;
                set
                {
                    _Link = value;
                    if (_Link != "https://github.com/oqyh/cs2-Bot-Quota-GoldKingZ")
                    {
                        Link = "https://github.com/oqyh/cs2-Bot-Quota-GoldKingZ";
                    }
                }
            }

            [BreakLine("{space}----------------------------[ ↓ Main Config ↓ ]----------------------------{space}")]
            [Comment("Disable Plugin On WarmUp?\ntrue = Yes\nfalse = No")]
            public bool DisablePluginOnWarmUp { get; set; }

            [Comment("Add Bots When X Or Less Players In The Server\nx = Numbers Of Players From 1 To 64")]
            [Range(1, 64, 5, "[Bot Quota] AddBotsWhenXOrLessPlayersInServer: Is Invalid, Setting To Default Value (5) Please Choose From 1 To 64.")]

            public int AddBotsWhenXOrLessPlayersInServer { get; set; }

            [Comment("Include Counting In [AddBotsWhenXOrLessPlayersInServer] Spectator Players?\ntrue = Yes\nfalse = No")]
            public bool IncludeCountingSpecPlayers { get; set; }

            [Comment("How Many Bots Should Add When [AddBotsWhenXOrLessPlayersInServer] Pass\nx = Numbers Of Players From 1 To 64")]
            [Range(1, 64, 10, "[Bot Quota] HowManyBotsShouldAdd: Is Invalid, Setting To Default Value (10) Please Choose From 1 To 64.")]
            public int HowManyBotsShouldAdd { get; set; }

            [Comment("Add Bots By Mode?\nnormal = The Number Of Bots On The Server Equals HowManyBotsShouldAdd.\nfill = The Server Is Filled With Bots Until There Are At Least HowManyBotsShouldAdd Players On The Server (Humans + Bots). Human Players Joining Cause An Existing Bot To Be Kicked, Human Players Leaving Might Cause A Bot To Be Added.\nmatch = The Number Of Bots On The Server Equals The Number Of Human Players Times HowManyBotsShouldAdd.")]
            public string BotAddMode { get; set; }

            [Comment("Note: Must Not Start With cfg/\nCustom Cfg When Bots Added (Located in csgo/cfg/)")]
            public string ExecConfigWhenBotsAdded { get; set; }

            [Comment("Note: Must Not Start With cfg/\nCustom Cfg When Bots Kicked (Located in csgo/cfg/)")]
            public string ExecConfigWhenBotsKicked { get; set; }
            [Comment("Enable Debug Plugin In Server Console (Helps You To Debug Issues You Facing)?\ntrue = Yes\nfalse = No")]
            [BreakLine("{space}----------------------------[ ↓ Utilities  ↓ ]----------------------------{space}")]
            public bool EnableDebug { get; set; }
            
            public ConfigData()
            {
                Version = BotQuotaGoldKingZ.Instance.ModuleVersion;
                Link = "https://github.com/oqyh/cs2-Bot-Quota-GoldKingZ";
                DisablePluginOnWarmUp = false;
                AddBotsWhenXOrLessPlayersInServer = 5;
                IncludeCountingSpecPlayers = true;
                HowManyBotsShouldAdd = 10;
                BotAddMode = "fill";
                ExecConfigWhenBotsAdded = "Bot-Quota-GoldKingZ/WhenBotsAdded.cfg";
                ExecConfigWhenBotsKicked = "Bot-Quota-GoldKingZ/WhenBotsKicked.cfg";
                EnableDebug = false;
            }

            public void Validate()
            {
                foreach (var prop in GetType().GetProperties())
                {
                    var rangeAttr = prop.GetCustomAttribute<RangeAttribute>();
                    if (rangeAttr != null && prop.PropertyType == typeof(int))
                    {
                        int value = (int)prop.GetValue(this)!;
                        if (value < rangeAttr.Min || value > rangeAttr.Max)
                        {
                            prop.SetValue(this, rangeAttr.Default);
                            Helper.DebugMessage(rangeAttr.Message,false);
                        }
                    }
                }
            }
        }
    }
}