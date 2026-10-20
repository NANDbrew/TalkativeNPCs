using HarmonyLib;
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
        private static class Patch
        {
            [HarmonyPatch("Start")]
            [HarmonyPostfix]
            public static void StartPatch(NPCAnimations __instance)
            {
                if (__instance.transform.parent == Refs.observerMirror) { return; }

                UnityEngine.Object.Instantiate(GetPrefab(), __instance.transform, false);

            }
        }
    }

}
