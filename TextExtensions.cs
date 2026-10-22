using System.Text;

namespace Lab2Variant2;

public static class TextExtensions
{
    public static string RemovePunctuation(this string text)
    {
        if (text is null)
            throw new ArgumentNullException(nameof(text));

        var sb = new StringBuilder();
        foreach (char c in text)
        {
            if (!char.IsPunctuation(c))
                sb.Append(c);
        }
        return sb.ToString();
    }

    public static string Capitalize(this string text)
    {
        if (text is null)
            throw new ArgumentNullException(nameof(text));

        var sb = new StringBuilder(text.Length);
        bool startOfWord = true;
        foreach (char c in text)
        {
            if (char.IsWhiteSpace(c))
            {
                startOfWord = true;
                sb.Append(c);
            }
            else if (startOfWord)
            {
                sb.Append(char.ToUpper(c));
                startOfWord = false;
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    public static string ReverseWords(this string text)
    {
        if (text is null)
            throw new ArgumentNullException(nameof(text));

        string[] words = text.GetWords().ToArray();
        Array.Reverse(words);
        return string.Join(" ", words);
    }

    public static int WordCount(this string text)
    {
        if (text is null)
            throw new ArgumentNullException(nameof(text));

        return text.GetWords().Count();
    }

    public static IEnumerable<string> GetWords(this string text)
    {
        if (text is null)
            throw new ArgumentNullException(nameof(text));
        return text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
    }
}
