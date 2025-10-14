public class Punctuation
{
    public string Value { get; private set; }

    public Punctuation(string value)
    {
        Value = value;
    }

    public bool IsSentenceEnding => Value == "." || Value == "!" || Value == "?";

    public override string ToString() => Value;

    public override bool Equals(object obj)
    {
        return obj is Punctuation punctuation && Value == punctuation.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
