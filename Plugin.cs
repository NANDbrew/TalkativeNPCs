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
        public const string PLUGIN_ID = "com.nandbrew.npctalker";
        public const string PLUGIN_NAME = "Talkative NPCs";
        public const string PLUGIN_VERSION = "1.0.0";

        public static Plugin Instance { get; private set; }

        //--settings--
        internal ConfigEntry<bool> patchRumorDude;


        private void Awake()
        {
            Instance = this;
            patchRumorDude = Config.Bind("Settings", "Patch rumor dude", true, new ConfigDescription("Integrate tavern rumor NPCs with the mod's disposition system\nREQUIRES A RELOAD"));
            //Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_ID);
            AssetTools.LoadAssetBundles();

            //PATCHES INFO
            Harmony harmony = new Harmony(PLUGIN_ID);

            MethodInfo original = AccessTools.Method(typeof(NPCAnimations), "Start");
            MethodInfo patch = AccessTools.Method(typeof(Patches.Patch), "StartPatch");
            harmony.Patch(original, postfix: new HarmonyMethod(patch));

            if (patchRumorDude.Value)
            {
                MethodInfo original1 = AccessTools.Method(typeof(TavernRumorsDude), "OnTriggerEnter");
                MethodInfo patch1 = AccessTools.Method(typeof(Patches.RumorStopper), "Patch");
                harmony.Patch(original1, postfix: new HarmonyMethod(patch1));

                MethodInfo original2 = AccessTools.Method(typeof(TavernRumorsDude), "ClickDrinkButton");
                MethodInfo patch2 = AccessTools.Method(typeof(Patches.RumorStopper), "ClickDrinkButton");
                harmony.Patch(original2, prefix: new HarmonyMethod(patch2));
            }
        }
    }
}
