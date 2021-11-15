using System;
using System.Configuration;
using System.Linq;
using m3md2;

namespace KeyboardMaster
{
    internal class ConfigurationRequest
    {
        internal static IDictonary GetDictonary()
        {
            StaticVariables.LanguageDictonary = (LanguageDictonary)Enum.Parse(typeof(LanguageDictonary), GetValueByKey("Lang"));
            return StaticVariables.LanguageDictonary switch
            {
                LanguageDictonary.RU => new RuDictonary(),
                LanguageDictonary.EN => new EnDictonary(),
                _ => throw new ArgumentException("В конфигурации обнаружено неожиданное значение"),
            };
        }

        private static void WriteValueByKey(string key, string value)
        {
            var appSettings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var item = Array.Find(appSettings.AppSettings.Settings.OfType<KeyValueConfigurationElement>().ToArray(), x => x.Key == key);
            item.Value = value;
            appSettings.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private static string GetValueByKey(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }
    }
}