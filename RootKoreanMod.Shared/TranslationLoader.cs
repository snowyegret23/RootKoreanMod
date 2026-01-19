using System.IO;

namespace RootKoreanMod.Shared
{
    public static class TranslationLoader
    {
        public static TranslationData LoadTranslation(string csvPath)
        {
            var translationData = new TranslationData();
            translationData.LoadCsvFile(csvPath);
            return translationData;
        }
    }
}
