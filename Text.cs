using System;
using System.Collections.Generic;
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
