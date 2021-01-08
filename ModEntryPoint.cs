using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;

namespace OnixPack
{
    [BepInPlugin(MOD_GUID, MOD_NAME, MOD_VERSION)]
    public class ModEntryPoint : BaseUnityPlugin
    {
        const string MOD_NAME = "OnixPack";
        const string MOD_GUID = "org.bepinex.plugins." + MOD_NAME;
        const string MOD_VERSION = "1.0.4";

        public static BepInEx.Logging.ManualLogSource log;

        private ModEntryPoint()
        {
            log = Logger;
            log.LogMessage("ModEntryPoint loaded");
        }

        public static ConfigEntry<bool> instant3DMark;

        internal void Awake()
        {
            instant3DMark = base.Config.Bind("General", "Instant3DMark", true, "Instant 3DMark");

            try
            {
                if (FindObjectsOfType<ModEntryPoint>().Length > 1)
                {
                    Debug.Log(string.Format("[{0}] Another instance of {1} was instantiated. Will destroy this: {2}", MOD_NAME, typeof(ModEntryPoint), gameObject.GetInstanceID()));
                    DestroyImmediate(this);
                }
                else
                {
                    new Harmony(MOD_GUID).PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
                    Debug.Log(string.Format("[{0}] Successfully patched via Harmony.", MOD_NAME));
                }
            }
            catch (Exception arg)
            {
                Debug.Log(string.Format("[{0}] Failed to patch via Harmony. Error: {1}", MOD_NAME, arg));
            }
        }

        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "Splash")
                this.FPSBoost();
        }
        private void PrintGameObjects()
        {
            string text = "";
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                foreach (MonoBehaviour comp in obj.GetComponents<MonoBehaviour>())
                {
                    //if (comp is OutlineEffect)
                    //{
                    //    Destroy(comp);
                    //}
                    text = string.Concat(new string[]
                    {
                            text, obj.name, " - ", comp.GetType().Name, " - tag: ", comp.tag, " <> enabled: ", comp.enabled.ToString(), "\n"
                    });
                }
            }

            log.LogMessage(text);
        }

        private void FPSBoost()
        {
            QualitySettings.masterTextureLimit = 1;
            QualitySettings.antiAliasing = 0;
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            QualitySettings.shadows = ShadowQuality.Disable;
            QualitySettings.shadowDistance = 0f;
            QualitySettings.shadowCascades = 0;
            QualitySettings.pixelLightCount = 0;
            QualitySettings.vSyncCount = 0;
            QualitySettings.blendWeights = (BlendWeights)0;
            QualitySettings.lodBias = 0.3f;
            QualitySettings.resolutionScalingFixedDPIFactor = 0.5f;
            QualitySettings.maxQueuedFrames = 0;
            QualitySettings.realtimeReflectionProbes = false;
            QualitySettings.particleRaycastBudget = 0;
            QualitySettings.softParticles = false;
            QualitySettings.maximumLODLevel = 0;
            QualitySettings.softVegetation = false;
            QualitySettings.SetQualityLevel(0, true);

            GameController.Get().IsLightsOn = false;

            foreach (PostProcessingBehaviour behaviour in Resources.FindObjectsOfTypeAll<PostProcessingBehaviour>())
            {
                behaviour.enabled = false;
                Destroy(behaviour);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F11))
                this.FPSBoost();
        }

        /* private const float TitleWidth = 240f;
         private Vector2 m_ScrollPosition = Vector2.zero;

         private void OnGUI()
         {
             m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
             {
                 GUILayout.Label("<b>Cameras</b>");
                 GUILayout.BeginVertical("box");
                 {
                     for (int i = 0; i < Camera.allCamerasCount; i++)
                     {
                         Camera camera = Camera.allCameras[i];
                         DrawItem("Name:", camera.ToString());
                     }

                    GameObject main = GameObject.FindGameObjectWithTag("MainCamera");

                    if (main)
                        DrawItem("Name#2:", main.ToString());
                }
                 GUILayout.EndVertical();
             }
             GUILayout.EndScrollView();
         }

         protected void DrawItem(string title, string content)
         {
             GUILayout.BeginHorizontal();
             {
                 GUILayout.Label(title, GUILayout.Width(TitleWidth));
                 GUILayout.Label(content);
             }
             GUILayout.EndHorizontal();
         } */
    }
}