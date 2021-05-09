using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace Slothsoft.BetterExploration {
    [BepInPlugin("Slothsoft.BetterExploration", "Better Exploration", "0.0.1")]
    public partial class BepInExPlugin : BaseUnityPlugin {
        static ConfigEntry<bool> modEnabled;

        void Awake() {
            modEnabled = Config.Bind("General", "Enabled", true, "Enable this mod");

            Debug.Log($"{typeof(BepInExPlugin)} enabled: {modEnabled.Value}");

            if (!modEnabled.Value) {
                return;
            }

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }

        [HarmonyPatch(typeof(Character), nameof(Character.Jump))]
        static class CharacterJumpPatch {
            static void Prefix(ref float jumpForce) {
                if (!modEnabled.Value) {
                    return;
                }

                jumpForce *= 2;
                Debug.Log($"Increased jumpForce to '{jumpForce}'!");
            }
        }
    }
}
