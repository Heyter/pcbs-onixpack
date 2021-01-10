using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace OnixPack
{
    [BepInPlugin(MOD_GUID, MOD_NAME, MOD_VERSION)]
    public class ModEntryPoint : BaseUnityPlugin
    {
        private const string MOD_NAME = "OnixPack";
        private const string MOD_GUID = "org.bepinex.plugins." + MOD_NAME;
        private const string MOD_VERSION = "1.0.5";

        internal new static BepInEx.Logging.ManualLogSource Logger;
        public static ConfigEntry<bool> instant3DMark, autoFPSBoost;

        private ModEntryPoint()
        {
            instant3DMark = base.Config.Bind("General", "Instant3DMark", true, "Instant 3DMark");
            autoFPSBoost = base.Config.Bind("General", "AutoFPSBoost", true, "Auto FPS Boost");

            Logger = base.Logger;
            Logger.LogInfo("ModEntryPoint loaded");
        }

        internal void Awake()
        {
            this.FPSBoost();

            try
            {
                if (FindObjectsOfType<ModEntryPoint>().Length > 1)
                {
                    Logger.LogWarning(string.Format("Another instance of {0} was instantiated. Will destroy this: {1}",
                        typeof(ModEntryPoint),
                        gameObject.GetInstanceID()
                    ));
                    DestroyImmediate(this);
                }
                else
                {
                    new Harmony(MOD_GUID).PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
                    Logger.LogInfo("Successfully patched via Harmony.");
                }
            }
            catch (Exception arg)
            {
                Logger.LogError(string.Format("Failed to patch via Harmony. Error: {0}", arg));
            }
        }

        private void PrintGameObjects()
        {
            string text = "";
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                foreach (MonoBehaviour comp in obj.GetComponents<MonoBehaviour>())
                {
                    text = string.Concat(new string[]
                    {
                            text, obj.name, " - ", comp.GetType().Name, " - tag: ", comp.tag, " <> enabled: ", comp.enabled.ToString(), "\n"
                    });
                }
            }

            Logger.LogMessage(text);
        }

        private void FPSBoost(bool bClear = false)
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

            if (bClear)
            {
                PostProcessingBehaviour[] behaviours = Resources.FindObjectsOfTypeAll<PostProcessingBehaviour>();
                for (int i = 0; i < behaviours.Length; i++)
                {
                    PostProcessingBehaviour behaviour = behaviours[i];
                    behaviour.enabled = false;
                    Destroy(behaviour);
                }
            }
        }

        private void Update()
        {
#if DEBUG
            if (Input.GetKeyDown(KeyCode.F10))
                PrintGameObjects();
#endif
            if (Input.GetKeyDown(KeyCode.F11))
                this.FPSBoost(true);
        }
    }
}