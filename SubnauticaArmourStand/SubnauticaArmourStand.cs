using System.Reflection;
using Harmony;
using UnityEngine;

namespace SubnauticaArmourStand
{
    public class Main
    {
        public const string AssetFolder = "SubnauticaArmourStand/Assets";
        public const string AssetsBundle = "./QMods/" + AssetFolder + "/resources";
        public static GameObject ArmourStandModel { private set; get; }

        public static void Init()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("ovyerus.ArmourStand.mod");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            new ArmourStand().Patch();

            Load();
        }

        private static void Load()
        {
            /*AssetBundle bundle = AssetBundle.LoadFromFile(AssetsBundle);

            ArmourStandModel = bundle.LoadAsset("ArmourStandModel") as GameObject;*/
        }
    }
}
