using BepInEx;
using BepInEx.Logging;
using R2API;

[assembly: HG.Reflection.SearchableAttribute.OptIn]

namespace HexOfMelancholy
{
    public static class PluginInfo
    {
        public const string PLUGIN_ID = "HexOfMelancholy";
        public const string PLUGIN_NAME = "Hex Of Melancholy";
        public const string PLUGIN_AUTHOR = "EliteMasterEric";
        public const string PLUGIN_VERSION = "1.0.0";
        public const string PLUGIN_GUID = "com.elitemastereric.melancholy";
    }

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency(ItemAPI.PluginGUID)] // For custom items
    [BepInDependency(LanguageAPI.PluginGUID)] // For custom language data
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }

        public PluginLogger PluginLogger;

        // The Awake() method is run at the very start when the game is initialized.
        public void Awake()
        {
            Instance = this;

            // Init our logging class so that we can properly log for debugging
            Log.Init(Logger);
            PluginLogger = new PluginLogger(Logger);

            Assets.Initialize();
            Items.Initialize();
            Hooks.Initialize();
        }
    }

    public class PluginLogger
    {
        ManualLogSource manualLogSource;

        public PluginLogger(ManualLogSource manualLogSource)
        {
            this.manualLogSource = manualLogSource;
        }

        public void LogFatal(object data)
        {
            manualLogSource.LogFatal(data);
        }

        public void LogError(object data)
        {
            manualLogSource.LogError(data);
        }

        public void LogWarning(object data)
        {
            manualLogSource.LogWarning(data);
        }

        public void LogMessage(object data)
        {
            manualLogSource.LogMessage(data);
        }

        public void LogInfo(object data)
        {
            manualLogSource.LogInfo(data);
        }

        public void LogDebug(object data)
        {
            manualLogSource.LogDebug(data);
        }
    }
}