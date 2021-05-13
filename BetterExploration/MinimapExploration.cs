using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

namespace Slothsoft.BetterExploration {
    class MinimapExploration : MonoBehaviour {
        public static bool isDebug = false;
        public static bool usePlayerEyes = false;

        static readonly MethodInfo minimapExplore = typeof(Minimap)
            .GetMethod(
                name: "Explore",
                bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                binder: Type.DefaultBinder,
                modifiers: Array.Empty<ParameterModifier>(),
                types: new[] { typeof(Vector3), typeof(float) }
            );
        const float RAYCAST_RADIUS = 1;
        const float RAYCAST_HORIZONTAL_POV = 45;
        const float RAYCAST_VERTICAL_POV = 10;
        const int RAYCAST_HORIZONTAL_COUNT = 20;
        const int RAYCAST_VERTICAL_COUNT = 5;

        const float RAYCAST_EXPLORE_RADIUS = 10;

        const float RAYCAST_MAX_DISTANCE = 10;
        const Layers RAYCAST_LAYERS = Layers.terrain | Layers.viewblock | Layers.static_solid | Layers.Water | Layers.smoke | Layers.piece | Layers.piece_nonsolid;

        const float DEBUG_MARKER_DURATION = 0.1f;
        static readonly Vector3 DEBUG_MARKER_SCALE = Vector3.one / 2;

        Minimap attachedMinimap;
        float fogDensity;
        float currentViewDistance;


        void Start() {
            for (int i = 0; i < 32; i++) {
                Assert.AreEqual(((Layers)(1 << i)).ToString(), LayerMask.LayerToName(i).Replace(" ", ""), $"Layer names changed! Check if {nameof(RAYCAST_LAYERS)} is still the correct layer.");
            }
        }
        void Update() {
            if (Minimap.instance) {
                attachedMinimap = Minimap.instance;
                UpdateViewDistance();
                if (usePlayerEyes) {
                    if (Player.m_localPlayer) {
                        Explore(Player.m_localPlayer.GetEyePoint(), Player.m_localPlayer.GetLookDir());
                    }
                } else {
                    if (GameCamera.instance) {
                        Explore(GameCamera.instance.transform.position, GameCamera.instance.transform.forward);
                    }
                }
            }
        }
        void UpdateViewDistance() {
            var env = EnvMan.instance.GetCurrentEnvironment();
            fogDensity = EnvMan.instance.IsDay()
                ? env.m_fogDensityDay
                : env.m_fogDensityNight;

            currentViewDistance = RAYCAST_MAX_DISTANCE / fogDensity;
        }
        void Explore(Vector3 origin, Vector3 direction) {
            for (int x = -RAYCAST_HORIZONTAL_COUNT; x <= RAYCAST_HORIZONTAL_COUNT; x++) {
                for (int y = -RAYCAST_VERTICAL_COUNT; y <= RAYCAST_VERTICAL_COUNT; y++) {
                    var rotation = Quaternion.Euler(
                        y * RAYCAST_VERTICAL_POV / RAYCAST_VERTICAL_COUNT,
                        x * RAYCAST_HORIZONTAL_POV / RAYCAST_HORIZONTAL_COUNT,
                        0
                    );
                    var ray = new Ray(origin, rotation * direction);
                    ExploreRay(ray);
                }
            }
        }
        void ExploreRay(in Ray ray) {
            if (Physics.SphereCast(ray, RAYCAST_RADIUS, out var raycastHit, currentViewDistance, (int)RAYCAST_LAYERS)) {
                ExplorePosition(raycastHit.point);
                if (isDebug) {
                    CreateDebugMarker(raycastHit.point, true);
                }
            } else {
                if (isDebug) {
                    CreateDebugMarker(ray.GetPoint(currentViewDistance), false);
                }
            }
        }
        void ExplorePosition(Vector3 position) {
            minimapExplore.Invoke(attachedMinimap, new object[] { position, RAYCAST_EXPLORE_RADIUS });
        }
        void CreateDebugMarker(Vector3 position, bool isHit) {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.transform.position = position;
            obj.transform.localScale = DEBUG_MARKER_SCALE;
            if (obj.TryGetComponent<Renderer>(out var renderer)) {
                renderer.material.color = isHit
                    ? Color.green
                    : Color.red;
                Destroy(renderer.material, DEBUG_MARKER_DURATION);
            }
            if (obj.TryGetComponent<Collider>(out var collider)) {
                Destroy(collider);
            }
            Destroy(obj, DEBUG_MARKER_DURATION);
        }
    }
}
