public class Punctuation : Token
{
    public override string Value { get; }

    public Punctuation(string value)
    {
        Value = value;
    }

    public bool IsSentenceEnding => Value == "." || Value == "!" || Value == "?" || Value == "..."; ///

    public override bool Equals(object obj)
    {
        return obj is Punctuation punctuation && Value == punctuation.Value;
    }
}
