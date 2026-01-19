using System;
using System.Collections.Generic;
using System.IO;
using MelonLoader;
using Il2CppTMPro;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;
using RootKoreanMod.Shared;

[assembly: MelonInfo(typeof(RootKoreanMod.MelonLoader.ModMain), "RootKoreanMod", "2.0.1", "waldo")]
[assembly: MelonGame(null, "Root")]

namespace RootKoreanMod.MelonLoader
{
    public class ModMain : MelonMod
    {
        internal static ModMain ModInstance { get; private set; }
        internal static MelonLogger.Instance ModLoggerInstance { get; private set; }

        internal string PluginDirectory => Path.GetDirectoryName(MelonAssembly.Location);

        internal static AssetBundle FontBundle { get; private set; }
        internal static TranslationData Translation { get; private set; }

        private MelonPreferences_Category prefCategoryGeneral;
        internal MelonPreferences_Entry<bool> ExportLocale;

        internal static void LogMessage(string message)
        {
            ModLoggerInstance.Msg(message);
        }

        internal static void LogError(string message)
        {
            ModLoggerInstance.Error(message);
        }

        public override void OnInitializeMelon()
        {
            ModInstance = this;
            ModLoggerInstance = LoggerInstance;

            prefCategoryGeneral = MelonPreferences.CreateCategory("General");
            ExportLocale = prefCategoryGeneral.CreateEntry<bool>("ExportLocale", false, null, "Export current locale to csv file", false, false, null, null);

            LoggerInstance.Msg("RootKoreanMod awake");

            try
            {
                Translation = LoadTranslation();
            }
            catch (Exception e)
            {
                LoggerInstance.Error(e.Message);
                LoggerInstance.Error(e.StackTrace);
            }
        }

        public override void OnLateInitializeMelon()
        {
            try
            {
                LoadFontBundle();
            }
            catch (Exception e)
            {
                LoggerInstance.Error(e.Message);
                LoggerInstance.Error(e.StackTrace);
            }
        }

        private TranslationData LoadTranslation()
        {
            LoggerInstance.Msg("Loading translation file");
            string csvPath = Path.Combine(PluginDirectory, FontHelper.TRANSLATION_FILENAME);
            var translationData = TranslationLoader.LoadTranslation(csvPath);
            LoggerInstance.Msg($"Loaded {translationData.Count} translations");
            return translationData;
        }

        public void LoadFontBundle()
        {
            if (FontBundle == null)
            {
                string assetBundlePath = Path.Combine(PluginDirectory, FontHelper.ASSETBUNDLE_FILENAME);

                LoggerInstance.Msg("Assetbundle file path : " + assetBundlePath);
                LoggerInstance.Msg($"Assetbundle file exists : {File.Exists(assetBundlePath)}");

                FontBundle = AssetBundle.LoadFromFile(assetBundlePath);
                LoggerInstance.Msg($"Assetbundle loaded : {FontBundle != null}");
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
}
