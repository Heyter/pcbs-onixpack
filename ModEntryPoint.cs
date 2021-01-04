using System;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.PostProcessing;
using cakeslice;

namespace OnixPack
{
    [BepInPlugin(MOD_GUID, MOD_NAME, "1.0")]
    public class ModEntryPoint : BaseUnityPlugin
    {
        public const string MOD_NAME = "OnixPack";
        public const string MOD_GUID = "org.bepinex.plugins." + MOD_NAME;
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

		private void Update()
        {
			if (fpsBoost && Time.time > fpsBoostTime)
			{
				fpsBoostTime += 3;

				foreach (Camera camera in Camera.allCameras)
				{
					camera.gameObject.GetComponent<PostProcessingBehaviour>().enabled = false;
					camera.gameObject.GetComponent<HBAOControl>().hbao.enabled = false;
					camera.gameObject.GetComponent<HBAOControl>().enabled = false;
					camera.gameObject.GetComponent<FxaaComponent>().OnDisable();
					camera.gameObject.GetComponent<AmbientOcclusionComponent>().model.enabled = false;
					camera.gameObject.GetComponent<AmbientOcclusionModel>().enabled = false;
					camera.gameObject.GetComponent<Camera>().allowHDR = false;
					camera.gameObject.GetComponent<DustVolume>().enabled = false;
					camera.gameObject.GetComponent<RealIllumination>().enabled = false;
					camera.gameObject.GetComponent<ReflectionProbe>().enabled = false;
				}
			}

			if (Input.GetKey(KeyCode.F11))
			{
				QualitySettings.masterTextureLimit = 1;
				QualitySettings.antiAliasing = 0;
				QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
				QualitySettings.shadows = ShadowQuality.Disable;
				QualitySettings.shadowDistance = 0f;
				QualitySettings.shadowCascades = 0;
				QualitySettings.pixelLightCount = 0;
				QualitySettings.resolutionScalingFixedDPIFactor = 0.5f;
				QualitySettings.vSyncCount = 0;
				QualitySettings.blendWeights = (BlendWeights)0;
				QualitySettings.lodBias = 0.3f;
				QualitySettings.streamingMipmapsActive = false;
				OutlineEffect.Instance.lineThickness = 0.5f;
				OutlineEffect.Instance.lineIntensity = 0.05f;
				OutlineEffect.Instance.fillAmount = 0.05f;
				QualitySettings.particleRaycastBudget = 0;
				QualitySettings.softParticles = false;
				QualitySettings.maximumLODLevel = 0;
				QualitySettings.softVegetation = false;
				QualitySettings.SetQualityLevel(0);
				GameController.Get().IsLightsOn = false;
				PlayerPrefs.SetInt("AmbientOcclusion", 0);
				PlayerPrefs.SetInt("CameraEffects", 0);
				PlayerPrefs.SetInt("MotionBlur", 0);

				fpsBoost = true;
			}
		}

		public static ConfigEntry<bool> instant3DMark;
		private bool fpsBoost = false;
		private float fpsBoostTime = 0.0f;
	}
}
