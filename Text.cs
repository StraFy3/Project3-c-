using System;
using System.Collections.Generic;
public class Text
{
    // Приватное поле sentences - список всех предложений текста
    private List<Sentence> sentences = new List<Sentence>();

    // Cвойство для доступа к предложениям
    public IReadOnlyList<Sentence> Sentences => sentences;

    public void AddSentence(Sentence sentence)
    {
        sentences.Add(sentence);
    }

    // Преобразование всего текста в строку
    public override string ToString()
    {
        var result = "";
        // Проходим по всем предложениям
        for (int i = 0; i < sentences.Count; i++)
        {
            // Добавляем предложение к результату
            result += sentences[i].ToString();

            // Добавляем пробел между предложениями (кроме последнего)
            if (i < sentences.Count - 1)
            {
                result += " ";
            }
        }
        return result;
    }
}
