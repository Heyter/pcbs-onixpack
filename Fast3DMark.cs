using HarmonyLib;

namespace OnixPack
{
    [HarmonyPatch(typeof(SaveLoadSystem), "LoadGame")]
    internal sealed class Fast3DMark
    {
        [HarmonyPostfix]
        private static void Postfix()
        {
            DebugVars.s_instant3DMark = ModEntryPoint.instant3DMark.Value;
        }
    }
}