using System.IO;
using System.Reflection;
using UnityEngine;

namespace PlayerBeMean
{
    internal class AssetTools
    {
        public static AssetBundle bundle;
        const string assetFile = "playerbemean.assets";
        const string libFile = "MeanieBridge.dll";

        public static void LoadAssetBundles()    //Load the bundle
        {
            string dataPath = Directory.GetParent(Plugin.Instance.Info.Location).FullName;
            //string firstTry = Path.Combine(dataPath, assetDir, assetFile);
            string secondTry = Path.Combine(dataPath, assetFile);
            //else { Debug.LogError("TowableBoats: can't find asset file"); return; }

            string libFirstTry = Path.Combine(dataPath, libFile);

            if (File.Exists(libFirstTry))
            {
                Assembly.LoadFrom(libFirstTry);

            }
            else { Debug.LogError("PlayerBeMean: Can't find lib"); }

            if (File.Exists(secondTry)) bundle = AssetBundle.LoadFromFile(secondTry);
            else { Debug.LogError("BULLSHITT!!"); }
            if (bundle == null)
            {
                Debug.LogError("PlayerBeMean: Bundle not loaded! Did you place it in the correct folder?");
            }
            else { Debug.Log("PlayerBeMean: loaded bundle " + bundle.ToString()); }

            //bundle2 = AssetBundle.LoadFromFile(Path.Combine(dataPath, assetFile2));

        }
    }
}
