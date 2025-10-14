using System;
public class Word
{
    public string Value { get; private set; }

    public bool IsStopWord { get; set; }

    // Свойство Length возвращает длину слова (количество символов)
    public int Length => Value.Length;

    // Принимает строку value и сохраняет ее в свойство Value
    public Word(string value)
    {
        Value = value;
    }

    // Когда мы пишем word.ToString(), вернется значение свойства Value
    public override string ToString() => Value;

    public override bool Equals(object obj)
    {
        return obj is Word word && Value.Equals(word.Value, StringComparison.OrdinalIgnoreCase);
    }
    public override int GetHashCode()
    {
        return Value.ToLower().GetHashCode();
    }
}

