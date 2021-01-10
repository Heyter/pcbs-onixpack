using UnityEngine.PostProcessing;
using UnityEngine;
using HarmonyLib;

namespace OnixPack
{
    internal class PostProcessing
    {
        [HarmonyPatch(typeof(PostProcessingBehaviour), "OnEnable")]
        static class OnEnable_Patch
        {
            private static bool Prefix(PostProcessingBehaviour __instance)
            {
                if (!ModEntryPoint.autoFPSBoost.Value)
                    return true;

                __instance.enabled = false;
                Object.Destroy(__instance);

                return false;
            }
        }

        [HarmonyPatch(typeof(PostProcessingBehaviour), "OnDisable")]
        private static class OnDisable_Patch
        {
            private static bool Prefix(PostProcessingBehaviour __instance)
            {
                if (!ModEntryPoint.autoFPSBoost.Value)
                    return true;

                PostProcessingBehaviour[] behaviours = Resources.FindObjectsOfTypeAll<PostProcessingBehaviour>();
                for (int i = 0; i < behaviours.Length; i++)
                {
                    PostProcessingBehaviour behaviour = behaviours[i];
                    if (!behaviour.Equals(__instance))
                    {
                        behaviour.enabled = false;
                        Object.Destroy(behaviour);
                    }
                }

                GraphicsUtils.Dispose();
                return false;
            }
        }

        [HarmonyPatch(typeof(PostProcessingBehaviour), "OnPreCull")]
        private static class OnPreCull_Patch
        {
            private static bool Prefix() => !ModEntryPoint.autoFPSBoost.Value;
        }

        [HarmonyPatch(typeof(PostProcessingBehaviour), "OnPreRender")]
        private static class OnPreRender_Patch
        {
            private static bool Prefix() => !ModEntryPoint.autoFPSBoost.Value;
        }

        [HarmonyPatch(typeof(PostProcessingBehaviour), "OnPostRender")]
        private static class OnPostRender_Patch
        {
            private static bool Prefix() => !ModEntryPoint.autoFPSBoost.Value;
        }

        [HarmonyPatch(typeof(PostProcessingBehaviour), "OnRenderImage")]
        private static class OnRenderImage_Patch
        {
            private static bool Prefix() => !ModEntryPoint.autoFPSBoost.Value;
        }

        [HarmonyPatch(typeof(PostProcessingBehaviour), "OnGUI")]
        private static class OnGUI_Patch
        {
            private static bool Prefix() => !ModEntryPoint.autoFPSBoost.Value;
        }
    }
}
