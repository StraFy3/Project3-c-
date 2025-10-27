using System.IO;
using System.Text.RegularExpressions;

public class TextParser
{
    public Text Parse(string text)
    {
        var result = new Text();

        if (string.IsNullOrWhiteSpace(text))
            return result;

        // Улучшенное регулярное выражение, которое игнорирует точки после цифр (нумерацию)
        // (?<!\d\.) - Не должно быть цифры с точкой перед разделителем
        string sentencePattern = @"(?<!\d\.)(?<=[.!?])\s+";

        // Разбиваем текст на предложения
        string[] sentenceStrings = Regex.Split(text, sentencePattern);

        foreach (string sentenceText in sentenceStrings)
        {
            string trimmedSentence = sentenceText.Trim();

            // Пропускаем пустые строки и отдельные цифры с точками
            if (string.IsNullOrEmpty(trimmedSentence) || IsJustNumbering(trimmedSentence))
                continue;

            // Убедимся, что предложение заканчивается знаком препинания
            if (!Regex.IsMatch(trimmedSentence, @"[.!?]$"))
            {
                trimmedSentence += ".";
            }

            // Парсим предложение и добавляем в результат
            var sentence = ParseSentence(trimmedSentence);
            result.AddSentence(sentence);
        }

        return result;
    }

    // Проверяет, является ли строка просто нумерацией (например, "1.", "2.")
    private bool IsJustNumbering(string text)
    {
        return Regex.IsMatch(text, @"^\d+\.$");
    }

    // Метод парсинга отдельного предложения
    private Sentence ParseSentence(string sentenceText)
    {
        var sentence = new Sentence();
        // Улучшенный паттерн для разбиения на слова и знаки препинания
        // Теперь учитывает цифры с точками как часть слова
        var pattern = @"(\w+[.]?\w*)|([^\w\s]|…)";
        var matches = Regex.Matches(sentenceText, pattern);

        // Обрабатываем каждый найденный токен
        for (int i = 0; i < matches.Count; i++)
        {
            var match = matches[i];
            if (match.Groups[1].Success) // Если нашли слово
            {
                string value = match.Value;
                if (value.EndsWith(".") && IsNumberWithDot(value))
                {
                    // Это номер с точкой - добавляем как слово
                    sentence.AddToken(new Word(value));
                }
                else
                {
                    sentence.AddToken(new Word(match.Value));
                }
            }
            else if (match.Groups[2].Success) // Если нашли знак препинания
            {
                sentence.AddToken(new Punctuation(match.Value));
            }
        }

        return sentence;
    }

    // Проверяет, является ли строка номером с точкой (например, "1.", "2.3")
    private bool IsNumberWithDot(string value)
    {
        if (string.IsNullOrEmpty(value) || !value.EndsWith("."))
            return false;

        string withoutDot = value.Substring(0, value.Length - 1); // Берем подстроку с начала до предпоследнего символа
        return double.TryParse(withoutDot, out _); // Символ _ означает, что не важно полученное числовое значение, только true или false
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
