using System.IO;
using System.Text.RegularExpressions;

public class TextParser
{
    public Text Parse(string text)
    {
        // Создаем пустой текст
        var result = new Text();

        // Регулярное выражение для разбиения на предложения
        // [^.!?…]* - любые символы кроме .!?… (0 или более раз)
        // [.!?…] - один из знаков конца предложения
        var sentencePattern = @"[^.!?…]*[.!?…]";
        // Ищем все совпадения с паттерном
        var sentenceMatches = Regex.Matches(text, sentencePattern);

        for (int i = 0; i < sentenceMatches.Count; i++)
        {
            var sentenceMatch = sentenceMatches[i];
            // Убираем пробелы в начале и конце
            var sentenceText = sentenceMatch.Value.Trim();
            // Пропускаем пустые предложения
            if (string.IsNullOrEmpty(sentenceText)) continue;

            // Парсим предложение на слова и знаки препинания
            var sentence = ParseSentence(sentenceText);
            result.AddSentence(sentence);
        }

        return result;
    }

    // Метод парсинга отдельного предложения
    private Sentence ParseSentence(string sentenceText)
    {
        var sentence = new Sentence();

        // Регулярное выражение для разбиения на токены:
        // (\w+) - слово (буквы, цифры, подчеркивание)
        // | - или
        // ([^\w\s]|…) - знак препинания (не буква и не пробел) или троеточие
        var pattern = @"(\w+)|([^\w\s]|…)";
        var matches = Regex.Matches(sentenceText, pattern);

        // Обрабатываем каждый найденный токен
        for (int i = 0; i < matches.Count; i++)
        {
            var match = matches[i];
            if (match.Groups[1].Success) // Если нашли слово
            {
                sentence.AddToken(new Word(match.Value));
            }
            else if (match.Groups[2].Success) // Если нашли знак препинания
            {
                sentence.AddToken(new Punctuation(match.Value));
            }
        }

        return sentence;
    }

    // Метод для парсинга текста из файла
    public Text ParseFromFile(string filePath)
    {
        // Проверяем существование файла
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        string text = File.ReadAllText(filePath);
        return Parse(text);
    }
}
