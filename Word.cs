using System;
public class Word : Token 
{
    public override string Value { get; }

    public bool IsStopWord { get; set; }

    // Свойство Length возвращает длину слова (количество символов)
    public int Length => Value.Length;

    // Принимает строку value и сохраняет ее в свойство Value
    public Word(string value)
    {
        Value = value;
    }

    public override bool Equals(object obj)
    {
        return obj is Word word && Value.Equals(word.Value, StringComparison.OrdinalIgnoreCase);
    }
}

