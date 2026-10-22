namespace Lab2Variant2;
public class TextPipeline
{
    private readonly List<TextTransformer> transformers = new();
    private readonly List<TextValidator> validators = new();

    public event Action<string>? ValidationFailed;
    public event Action<string, string>? TransformationApplied;

    public void AddTransformer(TextTransformer transformer)
    {
        if (transformer is null)
            throw new ArgumentNullException(nameof(transformer));
        transformers.Add(transformer);
    }

    public void AddValidator(TextValidator validator)
    {
        if (validator is null)
            throw new ArgumentNullException(nameof(validator));
        validators.Add(validator);
    }

    public string? Process(string text)
    {
        if (text is null)
            throw new ArgumentNullException(nameof(text));

        foreach (TextValidator validator in validators)
        {
            if (!validator(text))
            {
                ValidationFailed?.Invoke(text);
                return null;
            }
        }

        string result = text;
        foreach (TextTransformer transformer in transformers)
        {
            string before = result;
            result = transformer(before);
            TransformationApplied?.Invoke(before, result);
        }
        return result;
    }

    public void ClearPipeline()
    {
        transformers.Clear();
        validators.Clear();
    }
}
