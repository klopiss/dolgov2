using System.Text;

namespace Lab2Variant2;

public class Program
{
    private static readonly TextPipeline Pipeline = new();
    private static readonly TextAnalyzer Analyzer = new();

    private const string SampleText =
        "Быстрая коричневая лиса прыгает через ленивую собаку. Лиса быстрая, собака ленивая!";

    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        TextTransformer t = text => text.ToUpper();
        Console.WriteLine(t("мяу"));

        TextValidator v = TextProcessors.MinLength(5);
        Console.WriteLine(v("Привет"));

        var steps = new (string Name, Action Run)[]
        {
            ("1. Создание конвейера, подписка на события", SetupPipeline),
            ("2. Обработка разных строк", ProcessStrings),
            ("3. Методы расширения", DemoExtensions),
            ("4. Анонимные методы и лямбда-выражения", DemoCustomFunctions),
            ("5. Анализ текста (LINQ)", DemoAnalyzer),
            ("6. Ошибочная ситуация (null на входе)", DemoError),
            ("7. ClearPipeline", DemoClear),
        };

        foreach (var (name, run) in steps)
        {
            if (!RunStep(name, run))
            {
                Console.WriteLine("Выполнение программы завершено пользователем");
                return;
            }
        }

        Console.WriteLine("\nВсе шаги выполнены.");
    }

    private static bool RunStep(string name, Action run)
    {
        Console.WriteLine($"\n{name}");
        try
        {
            run();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ОШИБКА] Шаг «{name}»: {ex.GetType().Name} — {ex.Message}");
            return AskToContinue();
        }
    }

    private static bool AskToContinue()
    {
        while (true)
        {
            Console.Write("Продолжить выполнение, проигнорировав ошибку? (y — продолжить, n — завершить): ");
            string? answer = Console.ReadLine()?.Trim().ToLower();
            if (answer is null || answer == "n")
                return false;
            if (answer == "y")
                return true;
            Console.WriteLine("Введите y или n.");
        }
    }

    private static void SetupPipeline()
    {
        Pipeline.AddValidator(TextProcessors.MinLength(5));
        Pipeline.AddValidator(TextProcessors.MaxLength(200));
        Pipeline.AddValidator(TextProcessors.NoSpecialCharacters());

        Pipeline.AddTransformer(TextProcessors.RemoveExtraSpaces());
        Pipeline.AddTransformer(TextProcessors.ReplaceDigitsWithWords());
        Pipeline.AddTransformer(TextProcessors.ToUpperCase());

        Pipeline.ValidationFailed += text =>
            Console.WriteLine($" событие ValidationFailed не прошла проверку: \"{text}\"");
        Pipeline.TransformationApplied += (before, after) =>
            Console.WriteLine($" событие TransformationApplied \"{before}\" -> \"{after}\"");

        Console.WriteLine("Конвейер настроен: 3 валидатора, 3 преобразователя, 2 подписки");
    }

    private static void ProcessStrings()
    {
        string[] inputs =
        {
            "  Купил   3 яблока и 12 груш  ",  
            "Hi",                               
            "Привет@мир #1",                  
        };

        foreach (string input in inputs)
        {
            Console.WriteLine($"\nВход: \"{input}\"");
            string? result = Pipeline.Process(input);
            Console.WriteLine(result is null ? "Результат: null (валидация не пройдена)" : $"Результат: \"{result}\"");
        }
    }

    private static void DemoExtensions()
    {
        string text = "привет, мир! это тест";
        Console.WriteLine($"Исходный текст:     {text}");
        Console.WriteLine($"RemovePunctuation:  {text.RemovePunctuation()}");
        Console.WriteLine($"Capitalize:         {text.Capitalize()}");
        Console.WriteLine($"ReverseWords:       {text.ReverseWords()}");
        Console.WriteLine($"WordCount:          {text.WordCount()}");
        Console.WriteLine($"GetWords:           {string.Join(" | ", text.GetWords())}");
    }

    private static void DemoCustomFunctions()
    {
        var custom = new TextPipeline();
        custom.TransformationApplied += (b, a) => Console.WriteLine($"  \"{b}\" -> \"{a}\"");

        custom.AddTransformer(delegate (string t) { return t.Trim(); });

        custom.AddTransformer(t => t + "!!!");
        custom.AddTransformer(t => t.Capitalize());

        custom.AddValidator(t => !t.ToLower().Contains("дгту"));

        custom.ValidationFailed += t => Console.WriteLine($"  отклонено: \"{t}\"");

        Console.WriteLine($"Результат: {custom.Process("  hello world  ")}");
        Console.WriteLine($"Результат: {custom.Process("купи дгту") ?? "null"}");
    }

    private static void DemoAnalyzer()
    {
        Console.WriteLine($"Текст: {SampleText}\n");

        Console.WriteLine("Частота слов:");
        foreach (var pair in Analyzer.GetWordFrequency(SampleText))
            Console.WriteLine($"  {pair.Key}: {pair.Value}");

        Console.WriteLine($"\nСамые длинные слова (3): {string.Join(", ", Analyzer.GetLongestWords(SampleText, 3))}");
        Console.WriteLine($"Самый частый символ: '{Analyzer.GetMostFrequentChar(SampleText)}'");
    }

    private static void DemoError()
    {
        string? nothing = null;
        Pipeline.Process(nothing!);
    }

    private static void DemoClear()
    {
        Pipeline.ClearPipeline();
        string? result = Pipeline.Process("a");
        Console.WriteLine($"После ClearPipeline строка не меняется и не проверяется: \"{result}\"");
    }
}
