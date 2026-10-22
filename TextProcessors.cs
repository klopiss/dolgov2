using System.Text;
using System.Text.RegularExpressions;

namespace Lab2Variant2;

public static class TextProcessors
{
    private static readonly string[] DigitWords =
    {
        "ноль", "один", "два", "три", "четыре",
        "пять", "шесть", "семь", "восемь", "девять"
    };

    private const string AllowedPunctuation = ".,!?;:-'\"()";

    public static TextTransformer ToUpperCase()
    {
        return text => text.ToUpper();
    }

    public static TextTransformer ToLowerCase()
    {
        return text => text.ToLower();
    }

    public static TextTransformer RemoveExtraSpaces()
    {
        return text => string.Join(" ", text.GetWords());
    }

    public static TextTransformer ReplaceDigitsWithWords()
    {
        return text =>
        {
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (c >= '0' && c <= '9')
                    sb.Append(' ').Append(DigitWords[c - '0']).Append(' ');
                else
                    sb.Append(c);
            }
            return Regex.Replace(sb.ToString(), @"\s+", " ").Trim();
        };
    }

    public static TextValidator MinLength(int length)
    {
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length), "Длина не может быть отрицательной.");
        return text => text.Length >= length;
    }

    public static TextValidator MaxLength(int length)
    {
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length), "Длина не может быть отрицательной.");
        return text => text.Length <= length;
    }

    public static TextValidator ContainsWord(string word)
    {
        if (word is null)
            throw new ArgumentNullException(nameof(word));
        return text => text.GetWords()
        .Any(w => string.Equals(w.RemovePunctuation(), word, StringComparison.OrdinalIgnoreCase));
    }

    public static TextValidator NoSpecialCharacters()
    {
        return text =>
        {
            foreach (char c in text)
            {
                bool ok = char.IsLetterOrDigit(c)
                          || char.IsWhiteSpace(c)
                          || AllowedPunctuation.Contains(c);
                if (!ok)
                    return false;
            }
            return true;
        };
    }
}
