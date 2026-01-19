using System.Collections.Generic;

namespace RootKoreanMod.Shared
{
    public static class FontHelper
    {
        public const string ASSETBUNDLE_FILENAME = "sourcehanserifk.unity3d";
        public const string TRANSLATION_FILENAME = "translation.csv";

        public const string REGULAR = "SourceHanSerifK-Regular SDF";
        public const string BOLD = "SourceHanSerifK-Bold SDF";

        public static readonly IDictionary<string, string> FontMapping = new Dictionary<string, string>()
        {
            { "SourceHanSerifSC-Regular SDF", REGULAR },
            { "SourceHanSerifSC-Bold SDF", BOLD },
            { "SourceHanSerifSC-Regular SDF_BlackStroke", BOLD },
        };
    }
}
