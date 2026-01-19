using System;
using System.Collections.Generic;
using System.IO;
using Csv;

namespace RootKoreanMod.Shared
{
    public class TranslationData
    {
        private readonly Dictionary<string, TranslationEntry> translationDict = new Dictionary<string, TranslationEntry>();

        public int Count => translationDict.Count;

        public string this[string key] => translationDict[key].Target;

        public bool TryGetTranslation(string key, out string value)
        {
            if (translationDict.TryGetValue(key, out var entry) && entry.Translated)
            {
                value = entry.Target;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        public void LoadCsvFile(string csvPath)
        {
            var csvOptions = new CsvOptions
            {
                AllowNewLineInEnclosedFieldValues = true
            };

            translationDict.Clear();

            using (var sr = new StreamReader(csvPath))
            {
                foreach (var item in CsvReader.Read(sr, csvOptions))
                {
                    if (item.ColumnCount >= 3 && !string.IsNullOrWhiteSpace(item[2]))
                    {
                        translationDict[item[0]] = new TranslationEntry(item[0], item[1], item[2]);
                    }
                }
            }
        }

        private string ConvertNewline(string text)
        {
            return text.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\"\"", "\"");
        }

        public void ExportUpdatedCsvFile<TDict>(string outputPath, TDict newData, Func<TDict, IEnumerable<KeyValuePair<string, string>>> enumerator)
        {
            var rows = new List<string[]>();

            foreach (var kvpair in enumerator(newData))
            {
                string key = kvpair.Key;
                string src = ConvertNewline(kvpair.Value);

                if (translationDict.TryGetValue(key, out var entry))
                {
                    string old_src = ConvertNewline(entry.Source);
                    if (old_src == src)
                    {
                        rows.Add(new string[] { key, src, entry.Target });
                    }
                    else
                    {
                        rows.Add(new string[] { key, src, string.Empty, entry.Source, entry.Target });
                    }
                }
                else
                {
                    rows.Add(new string[] { key, src });
                }
            }

            using (var sw = new StreamWriter(outputPath))
            {
                string[] header = new string[] { "키", "원문", "번역", "기존 원문", "기존 번역" };
                CsvWriter.Write(sw, header, rows);
            }
        }

        private class TranslationEntry
        {
            public TranslationEntry(string key, string source, string target)
            {
                Key = key;
                Source = source;
                Target = target;
            }

            public string Key { get; set; }
            public string Source { get; set; }
            public string Target { get; set; }
            public bool Translated => !string.IsNullOrEmpty(Target);
        }
    }
}
