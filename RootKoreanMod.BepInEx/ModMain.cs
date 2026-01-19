using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using TMPro;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;
using RootKoreanMod.Shared;

namespace RootKoreanMod.BepInEx
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class ModMain : BasePlugin
    {
        const string PLUGIN_GUID = "waldo.rootkoreanmod";
        const string PLUGIN_NAME = "RootKoreanMod";
        const string PLUGIN_VERSION = "2.0.1";

        internal static ModMain ModInstance { get; private set; }
        internal static ManualLogSource ModLogger { get; private set; }

        internal string PluginDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        internal static AssetBundle FontBundle { get; private set; }
        internal static TranslationData Translation { get; private set; }

        internal ConfigEntry<bool> ExportLocale;

        private Harmony _harmony;

        internal static void LogMessage(string message)
        {
            ModLogger.LogInfo(message);
        }

        internal static void LogError(string message)
        {
            ModLogger.LogError(message);
        }

        public override void Load()
        {
            ModInstance = this;
            ModLogger = Log;

            ExportLocale = Config.Bind("General", "ExportLocale", false, "Export current locale to csv file");

            Log.LogInfo("RootKoreanMod loading...");

            try
            {
                Translation = LoadTranslation();

                _harmony = new Harmony(PLUGIN_GUID);
                _harmony.PatchAll(Assembly.GetExecutingAssembly());

                Log.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
            }
            catch (Exception e)
            {
                Log.LogError(e.Message);
                Log.LogError(e.StackTrace);
            }

            AddComponent<ModMainBehaviour>();
        }

        public override bool Unload()
        {
            _harmony?.UnpatchSelf();
            return true;
        }

        private TranslationData LoadTranslation()
        {
            Log.LogInfo("Loading translation file");
            string csvPath = Path.Combine(PluginDirectory, FontHelper.TRANSLATION_FILENAME);
            var translationData = TranslationLoader.LoadTranslation(csvPath);
            Log.LogInfo($"Loaded {translationData.Count} translations");
            return translationData;
        }

        internal void LoadFontBundle()
        {
            if (FontBundle == null)
            {
                string assetBundlePath = Path.Combine(PluginDirectory, FontHelper.ASSETBUNDLE_FILENAME);

                Log.LogInfo("Assetbundle file path : " + assetBundlePath);
                Log.LogInfo($"Assetbundle file exists : {File.Exists(assetBundlePath)}");

                FontBundle = AssetBundle.LoadFromFile(assetBundlePath);
                Log.LogInfo($"Assetbundle loaded : {FontBundle != null}");
            }

            Il2CppReferenceArray<UnityEngine.Object> fonts = Resources.FindObjectsOfTypeAll(Il2CppType.Of<TMP_FontAsset>());
            foreach (UnityEngine.Object item in fonts)
            {
                AddFallbackFont(item.Cast<TMP_FontAsset>());
            }
        }

        public static void AddFallbackFont(TMP_FontAsset font)
        {
            if (FontBundle == null)
            {
                return;
            }

            if (FontHelper.FontMapping.TryGetValue(font.name, out var krfontname))
            {
                var krfont = FontBundle.LoadAsset<TMP_FontAsset>(krfontname);
                if (!font.fallbackFontAssetTable.Contains(krfont))
                {
                    font.fallbackFontAssetTable.Add(krfont);
                }
            }
        }
    }

    public class ModMainBehaviour : MonoBehaviour
    {
        private void Start()
        {
            try
            {
                ModMain.ModInstance.LoadFontBundle();
            }
            catch (Exception e)
            {
                ModMain.LogError(e.Message);
                ModMain.LogError(e.StackTrace);
            }
        }
    }
}
