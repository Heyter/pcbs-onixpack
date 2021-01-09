using UnityEngine.PostProcessing;
using UnityEngine;
using HarmonyLib;

namespace OnixPack
{
    class PostProcessing
    {
        [HarmonyPatch(typeof(PostProcessingBehaviour), "OnEnable")]
        static class OnEnable_Patch
        {
            private static bool Prefix(PostProcessingBehaviour __instance)
            {
                if (ModEntryPoint.autoFPSBoost.Value)
                {
                    __instance.enabled = false;
                    Object.Destroy(__instance);
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(PostProcessingBehaviour), "OnDisable")]
        static class OnDisable_Patch
        {
            private static bool Prefix(PostProcessingBehaviour __instance)
            {
                if (ModEntryPoint.autoFPSBoost.Value)
                {
                    foreach (PostProcessingBehaviour behaviour in Resources.FindObjectsOfTypeAll<PostProcessingBehaviour>())
                    {
                        if (!behaviour.Equals(__instance))
                        {
                            behaviour.enabled = false;
                            Object.Destroy(behaviour);
                        }
                    }

                    GraphicsUtils.Dispose();
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(PostProcessingBehaviour), "OnPreCull")]
        static class OnPreCull_Patch
        {
            private static bool Prefix()
            {
                if (ModEntryPoint.autoFPSBoost.Value)
                    return false;

                return true;
            }
        }

        [HarmonyPatch(typeof(PostProcessingBehaviour), "OnPreRender")]
        static class OnPreRender_Patch
        {
            private static bool Prefix()
            {
                if (ModEntryPoint.autoFPSBoost.Value)
                    return false;

                return true;
            }
        }

        [HarmonyPatch(typeof(PostProcessingBehaviour), "OnPostRender")]
        static class OnPostRender_Patch
        {
            private static bool Prefix()
            {
                if (ModEntryPoint.autoFPSBoost.Value)
                    return false;

                return true;
            }
        }

        [HarmonyPatch(typeof(PostProcessingBehaviour), "OnRenderImage")]
        static class OnRenderImage_Patch
        {
            private static bool Prefix()
            {
                if (ModEntryPoint.autoFPSBoost.Value)
                    return false;

                return true;
            }
        }

        [HarmonyPatch(typeof(PostProcessingBehaviour), "OnGUI")]
        static class OnGUI_Patch
        {
            private static bool Prefix()
            {
                if (ModEntryPoint.autoFPSBoost.Value)
                    return false;

                return true;
            }
        }
    }
}
