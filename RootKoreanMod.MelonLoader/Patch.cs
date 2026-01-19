using System.Collections.Generic;
using System.Reflection;
using Il2CppTMPro;
using Il2CppCanis.utils.localization;
using HarmonyLib;

namespace RootKoreanMod.MelonLoader
{
    public static class Patch
    {
        [HarmonyPatch]
        public class LocalizationLookup_SetPairs_Patch
        {
            public static MethodBase TargetMethod()
            {
                foreach (MethodInfo method in typeof(LocalizationLookup).GetMethods())
                {
                    if (method.Name == "SetPairs" && method.GetParameters().Length == 1)
                    {
                        return method;
                    }
                }
                return null;
            }

            public static bool Prefix(ref Il2CppSystem.Collections.Generic.IEnumerable<Il2CppSystem.Collections.Generic.KeyValuePair<string, string>> pairs)
            {
                var dict = pairs.Cast<Il2CppSystem.Collections.Generic.Dictionary<string, string>>();
                ModMain.LogMessage("Pairs count = " + dict.Count.ToString());

                if (ModMain.ModInstance.ExportLocale.Value)
                {
                    ModMain.LogMessage("Exporting locale...");
                    ModMain.Translation.ExportUpdatedCsvFile("locale.csv", dict, EnumerateIl2CppDict);
                    ModMain.ModInstance.ExportLocale.Value = false;
                }

                if (ModMain.Translation != null)
                {
                    var keys = new List<string>();
                    foreach (string key in dict.Keys)
                    {
                        keys.Add(key);
                    }

                    foreach (string key in keys)
                    {
                        if (ModMain.Translation.TryGetTranslation(key, out var value))
                        {
                            dict[key] = value;
                        }
                    }
                }

                return true;
            }

            private static IEnumerable<KeyValuePair<string, string>> EnumerateIl2CppDict(Il2CppSystem.Collections.Generic.Dictionary<string, string> dict)
            {
                foreach (var kvp in dict)
                {
                    yield return new KeyValuePair<string, string>(kvp.Key, kvp.Value);
                }
            }
        }

        [HarmonyPatch(typeof(TMP_FontAsset), nameof(TMP_FontAsset.Awake))]
        public class TMP_FontAsset_Awake_Patch
        {
            public static void Postfix(TMP_FontAsset __instance)
            {
                ModMain.LogMessage("TMP_FontAsset.Awake Postfix " + __instance.name);
                ModMain.AddFallbackFont(__instance);
            }
        }
    }
}
