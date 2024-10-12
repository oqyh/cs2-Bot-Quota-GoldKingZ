using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Localization;

namespace Bot_Quota_GoldKingZ.Config
{
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
            }
            else
            {
                _configData = new ConfigData();
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
            {
                throw new Exception("Config not yet loaded.");
            }
            string json = JsonSerializer.Serialize(configData, SerializationOptions);
            
            json = System.Text.RegularExpressions.Regex.Unescape(json);
            File.WriteAllText(_configFilePath, json, System.Text.Encoding.UTF8);
        }

        public class ConfigData
        {
            public bool DisablePluginOnWarmUp { get; set; }
            public bool CheckPlayersByTimer { get; set; }
            public int AddBotsWhenXOrLessPlayersInServer { get; set; }
            public bool IncludeCountingSpecPlayers { get; set; }
            public int HowManyBotsShouldAdd { get; set; }
            public string BotAddMode { get; set; }
            public string ExecConfigWhenBotsAdded { get; set; }
            public string ExecConfigWhenBotsKicked { get; set; }
            public string empty { get; set; }
            public bool EnableDebug { get; set; }
            public string empty2 { get; set; }
            public string Information_For_You_Dont_Delete_it { get; set; }
            
            public ConfigData()
            {
                DisablePluginOnWarmUp = false;
                CheckPlayersByTimer = true;
                AddBotsWhenXOrLessPlayersInServer = 5;
                IncludeCountingSpecPlayers = true;
                HowManyBotsShouldAdd = 10;
                BotAddMode = "fill";
                ExecConfigWhenBotsAdded = "";
                ExecConfigWhenBotsKicked = "";
                empty = "----------------------------[ ↓ Debug ↓ ]----------------------------";
                EnableDebug = false;
                empty2 = "----------------------------[ ↓ Info For All Configs Above ↓ ]----------------------------";
                Information_For_You_Dont_Delete_it = " Vist  [https://github.com/oqyh/cs2-Bot-Quota-GoldKingZ/tree/main?tab=readme-ov-file#-configuration-] To Understand All Above";
            }
        }
    }
}