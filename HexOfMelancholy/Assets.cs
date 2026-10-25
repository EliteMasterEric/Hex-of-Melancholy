using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace HexOfMelancholy
{
    internal class Assets
    {
        static AssetBundle bundle;

        public static Sprite hexMelancholyIcon;
        public static GameObject hexMelancholyPrefab;

        public static void Initialize()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Melancholy.assets"))
            {
                bundle = AssetBundle.LoadFromStream(stream);

                Log.Info("Loaded AssetBundle Melancholy.assets:");
                Log.Info(string.Join(", ", Assets.bundle.GetAllAssetNames()));

                hexMelancholyIcon = bundle.LoadAsset<Sprite>("assets/bundles/melancholy/items/melancholy/texicon.png");
                hexMelancholyPrefab = bundle.LoadAsset<GameObject>("assets/bundles/melancholy/items/melancholy/melancholy.prefab");
            }
        }
    }
}
