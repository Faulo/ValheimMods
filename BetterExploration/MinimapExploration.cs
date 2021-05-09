using UnityEngine;
using UnityEngine.Assertions;

namespace Slothsoft.BetterExploration {
    class MinimapExploration : MonoBehaviour {
        const float RAYCAST_RADIUS = 1;
        const float RAYCAST_DISTNCE = float.PositiveInfinity;
        const Layers RAYCAST_LAYERS = Layers.terrain;
        const int RAYCAST_BUDGET = 100;

        RaycastHit[] raycastHits = new RaycastHit[RAYCAST_BUDGET];
        int raycastHitCount;

        void Start() {
            for (int i = 0; i < 32; i++) {
                Assert.AreEqual(((Layers)(1 << i)).ToString(), LayerMask.LayerToName(i).Replace(" ", ""));
            }
        }
        void Update() {
            if (Player.m_localPlayer && Minimap.instance) {
                Explore(Player.m_localPlayer, Minimap.instance);
            }
        }
        void Explore(Player player, Minimap minimap) {
            var eyePosition = player.GetEyePoint();
            var lookDirection = player.GetLookDir();
            Debug.Log($"Looking from {eyePosition} in direction {lookDirection}");
            raycastHitCount = Physics.SphereCastNonAlloc(
                eyePosition, RAYCAST_RADIUS, lookDirection,
                raycastHits, RAYCAST_DISTNCE, (int)RAYCAST_LAYERS
            );
            for (int i = 0; i < raycastHitCount; i++) {
                Debug.Log($"We see {raycastHits[i].collider.gameObject} at position {raycastHits[i].point}!");
            }
        }
    }
}
