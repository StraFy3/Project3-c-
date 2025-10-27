using System;
using System.Collections.Generic;
class Program
{
    static void Main(string[] args)
    {  
            
        var load = new StopWordsLoader();
        string sampleText = load.LoadFromFile(@"C:\Users\Lenovo\source\repos\Project3(c#)\test.txt", true);

        var parser = new TextParser();
        var text = parser.Parse(sampleText);
        var stopWordsLoader = new StopWordsLoader();

        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== ТОКЕНИЗАТОР ТЕКСТА ===");
            Console.WriteLine("Исходный текст:");
            Console.WriteLine(sampleText);
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("ВЫБЕРИТЕ ФУНКЦИЮ:");
            Console.WriteLine("1. Предложения по количеству слов");
            Console.WriteLine("2. Предложения по длине");
            Console.WriteLine("3. Слова в вопросительных предложениях");
            Console.WriteLine("4. Удалить слова (длина + согласная)");
            Console.WriteLine("5. Заменить слова в предложении");
            Console.WriteLine("6. Удалить стоп-слова");
            Console.WriteLine("0. Выход");
            Console.Write("\nВаш выбор: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ShowSentencesByWordCount(text);
                    break;
                case "2":
                    ShowSentencesByLength(text);
                    break;
                case "3":
                    ShowWordsInQuestions(text);
                    break;
                case "4":
                    RemoveConsonantWords(text);
                    break;
                case "5":
                    ReplaceWordsInSentence(text);
                    break;
                case "6":
                    RemoveStopWords(text, stopWordsLoader);
                    break;
                case "0":
                    exit = true;
                    Console.WriteLine("Выход из программы...");
                    break;
                default:
                    Console.WriteLine("Неверный выбор! Нажмите любую клавишу...");
                    Console.ReadKey();
                    break;
            }

            if (!exit && choice != "0")
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }

    static void ShowSentencesByWordCount(Text text)
    {
        Console.WriteLine("=== ПРЕДЛОЖЕНИЯ ПО КОЛИЧЕСТВУ СЛОВ (возрастание) ===");
        var sentences = text.GetSentencesOrderedByWordCount();
        foreach (var sentence in sentences)
        {
            Console.WriteLine($"Слов: {sentence.WordCount,2} -> {sentence.Value}");
        }
    }

    static void ShowSentencesByLength(Text text)
    {
        Console.WriteLine("=== ПРЕДЛОЖЕНИЯ ПО ДЛИНЕ (возрастание) ===");
        var sentences = text.GetSentencesOrderedByLength();
        foreach (var sentence in sentences)
        {
            Console.WriteLine($"Длина: {sentence.Length,3} -> {sentence.Value}");
        }
    }

    static void ShowWordsInQuestions(Text text)
    {
        Console.WriteLine("=== СЛОВА В ВОПРОСИТЕЛЬНЫХ ПРЕДЛОЖЕНИЯХ ===");
        Console.Write("Введите длину слова для поиска: ");
        if (int.TryParse(Console.ReadLine(), out int length))
        {
            var words = text.FindWordsInInterrogativeSentences(length);
            if (words.Count > 0)
            {
                Console.WriteLine($"Слова длиной {length}:");
                foreach (var word in words)
                {
                    Console.WriteLine($"  {word}");
                }
            }
            else
            {
                Console.WriteLine($"Слов длиной {length} в вопросительных предложениях не найдено.");
            }
        }
        else
        {
            Console.WriteLine("Неверный формат длины!");
        }
    }

    static void RemoveConsonantWords(Text text)
    {
        Console.WriteLine("=== УДАЛЕНИЕ СЛОВ ПО УСЛОВИЮ ===");
        Console.Write("Введите длину слов для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int length))
        {
            Console.WriteLine($"Удаление слов длиной {length}, начинающихся с согласной...");
            text.RemoveWordsStartingWithConsonant(length);
            Console.WriteLine("Текст после удаления:");
            Console.WriteLine(text.Value);
        }
        else
        {
            Console.WriteLine("Неверный формат длины!");
        }
    }

    static void ReplaceWordsInSentence(Text text)
    {
        Console.WriteLine("=== ЗАМЕНА СЛОВ В ПРЕДЛОЖЕНИИ ===");

        Console.Write("Введите номер предложения (0-based): ");
        if (!int.TryParse(Console.ReadLine(), out int sentenceIndex))
        {
            Console.WriteLine("Неверный формат номера!");
            return;
        }

        Console.Write("Введите длину слов для замены: ");
        if (!int.TryParse(Console.ReadLine(), out int wordLength))
        {
            Console.WriteLine("Неверный формат длины!");
            return;
        }

        Console.Write("Введите строку для замены: ");
        string replacement = Console.ReadLine();

        try
        {
            text.ReplaceWordsInSentence(sentenceIndex, wordLength, replacement);
            Console.WriteLine("Текст после замены:");
            Console.WriteLine(text.Value);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void RemoveStopWords(Text text, StopWordsLoader stopWordsLoader)
    {
        Console.WriteLine("=== УДАЛЕНИЕ СТОП-СЛОВ ===");
        Console.WriteLine("Выберите язык стоп-слов:");
        Console.WriteLine("1. Английские");
        Console.WriteLine("2. Русские");
        Console.Write("Ваш выбор: ");

        string langChoice = Console.ReadLine();
        HashSet<string> stopWords;

        if (langChoice == "1")
        {
            stopWords = stopWordsLoader.LoadEnglishStopWords();
            Console.WriteLine("Загружены английские стоп-слова");
        }
        else if (langChoice == "2")
        {
            stopWords = stopWordsLoader.LoadRussianStopWords();
            Console.WriteLine("Загружены русские стоп-слова");
        }
        else
        {
            Console.WriteLine("Неверный выбор!");
            return;
        }

        text.RemoveStopWords(stopWords);
        Console.WriteLine("Текст после удаления стоп-слов:");
        Console.WriteLine(text.Value);
    }
}
