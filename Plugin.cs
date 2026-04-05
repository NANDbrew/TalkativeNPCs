using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Reflection;

namespace PlayerBeMean
{
    [BepInPlugin(PLUGIN_ID, PLUGIN_NAME, PLUGIN_VERSION)]
    //[BepInDependency("com.app24.sailwindmoddinghelper", "2.0.3")]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_ID = "com.yourname.npctalker";
        public const string PLUGIN_NAME = "Talkative NPCs";
        public const string PLUGIN_VERSION = "0.0.1";

        public static Plugin Instance { get; private set; }

        //--settings--
        //internal ConfigEntry<bool> someSetting;


        private void Awake()
        {
            Instance = this;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_ID);
            AssetTools.LoadAssetBundles();

            //someSetting = Config.Bind("Settings", "Some setting", false);
        }
    }
}
