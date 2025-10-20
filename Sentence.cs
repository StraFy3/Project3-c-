using System;
using System.Collections.Generic;

public class Sentence : Token
{
    private List<Token> tokens = new List<Token>();

    // Value предложения - это объединение всех его токенов
    public override string Value
    {
        get
        {
            string result = "";
            foreach (var token in tokens)
            {
                result += token.Value;
            }
            return result;
        }
    }
    public IReadOnlyList<Token> Tokens => tokens;

    public List<Token> TokensPublic => tokens;
    public void AddToken(Token token)
    {
        if (token is Word || token is Punctuation)
        {
            tokens.Add(token);
        }

    }

    // Количество слов в предложении
    public int WordCount
    {
        get
        {
            // Счетчик слов
            int count = 0;

            // Проходим по всем токенам в предложении
            foreach (var token in tokens)
            {
                // Если токен является словом - увеличиваем счетчик
                if (token is Word)
                {
                    count++;
                }
            }
            return count;
        }
    }

    // Общая длина предложения в символах
    public override int Length
    {
        get
        {
            int totalLength = 0;

            foreach (var token in tokens)
            {
                // Добавляем длину токена
                totalLength += token.Length;
            }
            return totalLength;
        }
    }

    // Является ли предложение вопросительным
    public bool IsInterrogative
    {
        get
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];         
                // Если нашли знак препинания и это вопросительный знак
                if (token is Punctuation punctuation && punctuation.Value == "?") ////
                {
                    if (IsQuestionMarkEndOfSentence(i))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    private bool IsQuestionMarkEndOfSentence(int questionMarkIndex)
    {
        if (questionMarkIndex == tokens.Count - 1)
            return true;

        for (int i = questionMarkIndex + 1; i < tokens.Count; i++)
        {
            var nextToken = tokens[i];

            if (nextToken is Word nextWord)
            {
                return StartsWithUpperCase(nextWord.Value);
            }
        }

        return true;
    }

    private bool StartsWithUpperCase(string word)
    {
        if (string.IsNullOrEmpty(word))
            return false;

        char firstChar = word[0];
        return char.IsUpper(firstChar);
    }

    // Возвращает только слова из предложения
    public List<Word> GetWords()
    {
        // Создаем новый список для слов
        List<Word> words = new List<Word>();

        foreach (var token in tokens)
        {
            // Если токен является словом - добавляем в список
            if (token is Word word)
            {
                words.Add(word);
            }
        }

        // Возвращаем готовый список
        return words;
    }

    public override bool Equals(object obj)
    {
        if (obj is Sentence sentence)
        {
            if (tokens.Count != sentence.tokens.Count)
                return false;

            for (int i = 0; i < tokens.Count; i++)
            {
                if (!tokens[i].Equals(sentence.tokens[i]))
                    return false;
            }
            return true;
        }
        return false;
    }

}

