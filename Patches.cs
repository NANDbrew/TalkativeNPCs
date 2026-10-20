using HarmonyLib;
using MeanieBridge;
using UnityEngine;

namespace PlayerBeMean
{
    internal class Patches
    {
        static GameObject prefab;
        static GameObject GetPrefab()
        {
            if (prefab == null)
            {
                prefab = (GameObject)AssetTools.bundle.LoadAsset("Assets/PlayerBeMean/speech ui.prefab");
            }
            return prefab;
        }
        /*        [HarmonyPatch(typeof(Shopkeeper))]
                private static class ShopKeeperPatch
                {
                    [HarmonyPatch("Start")]
                    [HarmonyPostfix]
                    public static void StartPatch(Shopkeeper __instance)
                    {
                        UnityEngine.Object.Instantiate(GetPrefab(), __instance.transform, false);
                    }
                }*/

        /*        [HarmonyPatch(typeof(NPCPlayerCol))]
                private static class ComponentAdder
                {
                    [HarmonyPatch("RegisterAnimations")]
                    [HarmonyPostfix]
                    public static void StartPatch(NPCPlayerCol __instance)
                    {
                        UnityEngine.Object.Instantiate(GetPrefab(), __instance.transform, false);
                    }
                }*/

        [HarmonyPatch(typeof(NPCAnimations))]
        internal static class Patch
        {
            [HarmonyPatch("Start")]
            [HarmonyPostfix]
            public static void StartPatch(NPCAnimations __instance)
            {
                if (__instance.transform.parent == Refs.observerMirror) { return; }

                UnityEngine.Object.Instantiate(GetPrefab(), __instance.transform, false);

            }
        }
        [HarmonyPatch(typeof(TavernRumorsDude))]
        internal static class RumorStopper
        {
            static int minRumorDisposition = 90;
            static string unhappyGreeting = "You owe me a drink.";

            //[HarmonyPatch("OnTriggerEnter")]
            //[HarmonyPostfix]
            public static void Patch(Collider other, TavernRumorsDude __instance, string ___initialText)
            {
                if (other.CompareTag("Player"))
                {
                    if (__instance.GetComponentInChildren<NPCHitTalker>() is NPCHitTalker talker)
                    {
                        if (talker.disposition < minRumorDisposition)
                        {
                            __instance.text.text = unhappyGreeting;
                        }
                        else if (__instance.text.text == unhappyGreeting)
                        {
                            __instance.text.text = ___initialText;
                        }
                    }
                }
            }

            //[HarmonyPatch("ClickDrinkButton")]
            //[HarmonyPrefix]
            internal static bool ClickDrinkButton(TavernRumorsDude __instance, ref ShipItemBottle ___currentDrink, string ___initialText)
            {
                if ((bool)___currentDrink && !(___currentDrink.health < ___currentDrink.GetCapacity()) && ___currentDrink.sold)
                {
                    if (__instance.GetComponentInChildren<NPCHitTalker>() is NPCHitTalker talker && talker.disposition < minRumorDisposition)
                    {
                        talker.disposition += 20;
                        UISoundPlayer.instance.PlayUISound(UISounds.smallPop, 0.75f, 1f);
                        Juicebox.juice.TweenScale(__instance.speechUI, Vector3.one * 0.9f, 0.1f, JuiceboxTween.bounceInOut, returnToOrigin: true);

                        ___currentDrink.DestroyItem();
                        ___currentDrink = null;
                        __instance.drinkButton.SetActive(value: false);
                        if (talker.disposition >= minRumorDisposition && __instance.text.text == unhappyGreeting)
                        {
                            __instance.text.text = ___initialText;
                        }
                        return false;
                    }
                }
                return true;
            }
        }
    }

}
