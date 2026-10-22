namespace Lab2Variant2;

public class TextAnalyzer
{
    public Dictionary<string, int> GetWordFrequency(string text)
    {
        if (text is null)
            throw new ArgumentNullException(nameof(text));

        return text.GetWords()
            .Select(w => w.RemovePunctuation().ToLower())
            .Where(w => w.Length > 0)
            .GroupBy(w => w)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public IEnumerable<string> GetLongestWords(string text, int count)
    {
        if (text is null)
            throw new ArgumentNullException(nameof(text));
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        return text.GetWords()
            .Select(w => w.RemovePunctuation().ToLower())
            .Where(w => w.Length > 0)
            .Distinct()
            .OrderByDescending(w => w.Length)
            .Take(count);
    }

    public char? GetMostFrequentChar(string text)
    {
        if (text is null)
            throw new ArgumentNullException(nameof(text));

        return text
            .Where(c => !char.IsWhiteSpace(c))
            .Select(c => char.ToLower(c))
            .GroupBy(c => c)
            .OrderByDescending(g => g.Count())
            .Select(g => (char?)g.Key)
            .FirstOrDefault();
    }
}
