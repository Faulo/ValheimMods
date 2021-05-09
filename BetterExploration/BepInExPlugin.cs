using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace Slothsoft.BetterExploration {
    [BepInPlugin("Slothsoft.BetterExploration", "Better Exploration", "0.0.1")]
    public partial class BepInExPlugin : BaseUnityPlugin {
        static Harmony harmony = new Harmony(typeof(BepInExPlugin).Namespace);

        static ConfigEntry<bool> modEnabled;

        void Awake() {
            modEnabled = Config.Bind("General", "Enabled", true, "Enable this mod");

            Debug.Log($"{typeof(BepInExPlugin)} enabled: {modEnabled.Value}");

            if (!modEnabled.Value) {
                return;
            }

            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(Character), nameof(Character.Jump))]
        static class CharacterJumpPatch {
            static void Prefix(Character __instance) {
                if (!modEnabled.Value) {
                    return;
                }
                __instance.SetMaxHealth(1000);
                __instance.SetHealth(1000);
                __instance.m_jumpForce = 10;
                __instance.m_jumpForceForward = 10;
                __instance.m_jumpStaminaUsage = 0;
            }
        }
        [HarmonyPatch(typeof(Character), nameof(Character.IsOnGround))]
        static class CharacterIsOnGroundPatch {
            static void Postfix(ref bool __result) {
                if (!modEnabled.Value) {
                    return;
                }
                __result = true;
            }
        }
    }
}
