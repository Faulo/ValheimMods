using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;

namespace Slothsoft.BetterExploration {
    [BepInPlugin("Slothsoft.BetterExploration", "Better Exploration", "0.0.1")]
    public partial class BepInExPlugin : BaseUnityPlugin {
        static Harmony harmony = new Harmony(typeof(BepInExPlugin).Namespace);

        static ConfigEntry<bool> modEnabled;

        void Awake() {
            modEnabled = Config.Bind("General", "Enabled", true, "Enable this mod");

            if (!modEnabled.Value) {
                return;
            }

            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(Minimap), "Awake")]
        static class MinimapAwake {
            static void Postfix(Minimap __instance) {
                if (!modEnabled.Value) {
                    return;
                }
                __instance.gameObject.AddComponent<MinimapExploration>();
            }
        }
    }
}
