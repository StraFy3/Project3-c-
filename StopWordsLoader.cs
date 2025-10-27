using System.Collections.Generic;
using System.IO;
    public class StopWordsLoader
    {
        public HashSet<string> LoadFromFile(string filePath)
        {
            var stopWords = new HashSet<string>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var word = line.Trim().ToLower();
                    if (!string.IsNullOrEmpty(word))
                        stopWords.Add(word);
                }
            }

            return stopWords;
        }

        public string LoadFromFile(string filePath, bool returnText = true)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                throw new System.Exception($"Файл не найден: {filePath}");
            }
        }

        public HashSet<string> LoadEnglishStopWords()
        {
            return LoadFromFile(@"C:\Users\Lenovo\source\repos\Project3(c#)\stopwords_en.txt");
        }

        public HashSet<string> LoadRussianStopWords()
        {
            return LoadFromFile(@"C:\Users\Lenovo\source\repos\Project3(c#)\stopwords_ru.txt");
        }
    }
