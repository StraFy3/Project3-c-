using System;
using System.Collections.Generic;
using System.Linq;
public class Text : Token
{
    // Приватное поле sentences - список всех предложений текста
    private List<Token> sentences = new List<Token>();

    // Cвойство для доступа к предложениям
    public IReadOnlyList<Token> Sentences => sentences;

    public void AddSentence(Token sentence)
    {
        if (sentence is Sentence)
        {
            sentences.Add(sentence);
        }
        else
        {
            throw new ArgumentException("Only sentence tokens can be added to text");
        }
    }

    // Value текста - это объединение всех предложений
    public override string Value
    {
        get
        {
            string result = "";
            for (int i = 0; i < sentences.Count; i++)
            {
                result += sentences[i].Value;
                if (i < sentences.Count - 1)
                {
                    result += " ";
                }
            }
            return result;
        }
    }

    // Длина текста - сумма длин всех предложений
    public override int Length
    {
        get
        {
            int totalLength = 0;
            foreach (var sentence in sentences)
            {
                totalLength += sentence.Length;
            }
            return totalLength;
        }
    }

    // Метод 1: Сортировка предложений по количеству слов
    public List<Sentence> GetSentencesOrderedByWordCount()
    {
        // Создаем список только для предложений (отфильтровываем не-Sentence токены)
        var sentenceList = new List<Sentence>();
        foreach (var token in sentences)
        {
            if (token is Sentence sentence)
            {
                sentenceList.Add(sentence);
            }
        }

        // Сортируем предложения по количеству слов (возрастание)
        sentenceList.Sort((s1, s2) => s1.WordCount.CompareTo(s2.WordCount));
        return sentenceList;
    }

    // Метод 2: Сортировка предложений по длине
    public List<Sentence> GetSentencesOrderedByLength()
    {
        var sentenceList = new List<Sentence>();
        foreach (var token in sentences)
        {
            if (token is Sentence sentence)
            {
                sentenceList.Add(sentence);
            }
        }

        // Сортируем по общей длине предложения
        sentenceList.Sort((s1, s2) => s1.Length.CompareTo(s2.Length));
        return sentenceList;
    }

    // Метод 3: Поиск слов заданной длины в вопросительных предложениях
    public HashSet<string> FindWordsInInterrogativeSentences(int wordLength)
    {
        // HashSet автоматически удаляет дубликаты
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var token in sentences)
        {
            // Проверяем, является ли токен предложением и вопросительным
            if (token is Sentence sentence && sentence.IsInterrogative)
            {
                // Получаем все слова предложения
                List<Word> words = sentence.GetWords();
                foreach (var word in words)
                {
                    // Если длина слова совпадает с заданной - добавляем в результат
                    if (word.Length == wordLength)
                    {
                        result.Add(word.Value);
                    }
                }
            }
        }

        return result;
    }

    // Метод 4: Удаление слов заданной длины, начинающихся с согласной буквы
    public void RemoveWordsStartingWithConsonant(int length)
    {
        // Множество согласных букв для русского и английского алфавитов
        var consonants = new HashSet<char>("bcdfghjklmnpqrstvwxyzBCDFGHJKLMNPQRSTVWXYZ" +
                                         "бвгджзйклмнпрстфхцчшщБВГДЖЗЙКЛМНПРСТФХЦЧШЩ");

        foreach (var token in sentences)
        {
            if (token is Sentence sentence)
            {
                var wordsToRemove = new List<Word>();
                List<Word> words = sentence.GetWords();

                foreach (var word in words)
                {
                    // Проверяем условия: длина совпадает, слово не пустое, начинается с согласной
                    if (word.Length == length && word.Value.Length > 0)
                    {
                        if (consonants.Contains(word.Value[0]))
                        {
                            wordsToRemove.Add(word);
                        }
                    }
                }

                // Если есть слова для удаления - удаляем их из предложения
                if (wordsToRemove.Count > 0)
                {
                    RemoveTokensFromSentence(sentence, wordsToRemove);
                }
            }
        }
    }

    // Метод 5: Замена слов заданной длины в указанном предложении
    public void ReplaceWordsInSentence(int sentenceIndex, int wordLength, string replacement)
    {
        // Проверяем корректность индекса предложения
        if (sentenceIndex < 0 || sentenceIndex >= sentences.Count)
            throw new ArgumentOutOfRangeException(nameof(sentenceIndex));

        // Получаем предложение по индексу
        if (sentences[sentenceIndex] is Sentence sentence)
        {
            var newTokens = new List<Token>();
            // Проходим по всем токенам предложения
            foreach (var token in sentence.Tokens)
            {
                // Если токен - слово нужной длины, заменяем его
                if (token is Word word && word.Length == wordLength)
                {
                    newTokens.Add(new Word(replacement));
                }
                else
                {
                    // Иначе оставляем токен без изменений
                    newTokens.Add(token);
                }
            }
            // Заменяем токены в предложении
            ReplaceTokensInSentence(sentence, newTokens);
        }
    }

    // Метод 6: Удаление стоп-слов из всего текста
    public void RemoveStopWords(HashSet<string> stopWords)
    {
        foreach (var token in sentences)
        {
            if (token is Sentence sentence)
            {
                var wordsToRemove = new List<Word>();
                List<Word> words = sentence.GetWords();

                // Ищем стоп-слова в предложении
                foreach (var word in words)
                {
                    if (stopWords.Contains(word.Value.ToLower()))
                    {
                        wordsToRemove.Add(word);
                    }
                }

                // Удаляем найденные стоп-слова
                if (wordsToRemove.Count > 0)
                {
                    RemoveTokensFromSentence(sentence, wordsToRemove);
                }
            }
        }
    }

    private void RemoveTokensFromSentence(Sentence sentence, List<Word> tokensToRemove)
    {
        sentence.TokensPublic.RemoveAll(t => tokensToRemove.Contains(t));
    }

    private void ReplaceTokensInSentence(Sentence sentence, List<Token> newTokens)
    {
        sentence.TokensPublic.Clear();
        sentence.TokensPublic.AddRange(newTokens);
    }

    public override bool Equals(object obj)
    {
        if (obj is Text text)
        {
            if (sentences.Count != text.sentences.Count)
                return false;

            for (int i = 0; i < sentences.Count; i++)
            {
                if (!sentences[i].Equals(text.sentences[i]))
                    return false;
            }
            return true;
        }
        return false;
    }
}
