using System;
using System.Collections.Generic;

public class Sentence
{
    private List<object> tokens = new List<object>();
    public IReadOnlyList<object> Tokens => tokens;
    public void AddToken(object token)
    {
        if (token is Word || token is Punctuation)
        {
            tokens.Add(token);
        }
        else
        {
            // Если передан неправильный тип объекта - выбрасываем исключение
            throw new ArgumentException("Token must be Word or Punctuation");
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
    public int Length
    {
        get
        {
            int totalLength = 0;

            foreach (var token in tokens)
            {
                // Если токен - слово, добавляем его длину
                if (token is Word word)
                {
                    totalLength += word.Length;
                }
                // Если токен - знак препинания, добавляем длину знака
                else if (token is Punctuation punctuation)
                {
                    totalLength += punctuation.Value.Length;
                }
            }
            return totalLength;
        }
    }

    // Является ли предложение вопросительным
    public bool IsInterrogative
    {
        get
        {
            foreach (var token in tokens)
            {
                // Если нашли знак препинания и это вопросительный знак
                if (token is Punctuation punctuation && punctuation.Value == "?")
                {
                    return true; // Предложение вопросительное
                }
            }
            return false; // Вопросительного знака не найдено
        }
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

    // Метод преобразования предложения в строку
    public override string ToString()
    {
        var result = "";
        // Проходим по всем токенам и добавляем их текстовое представление к результату
        foreach (var token in tokens)
        {
            result += token.ToString();
        }
        return result;
    }
}

