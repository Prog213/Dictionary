using System.Collections;
using System.Text;

class Program
{
    public Dictionary<string, string> dictionary = new Dictionary<string, string>();
    readonly string filePath = @"C:\Users\maste\Downloads\23.txt";
    static readonly string newFilePath = @"C:\Users\maste\Downloads\23.txt";
    string word = null;
    public bool IsCorrect { get; set; }
    public bool IsUnknown { get; set; }
    static int correctGuesses = 0;
    static int guesses = 1;
    string[] lines = new string[1];
    public static ArrayList newDiction = new ArrayList();
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Program program = new Program();
        program.CreateDictionary();
        program.PrintNextValue();
        CreatenewFile(newDiction);
        ShowScore(program.dictionary.Count, correctGuesses);
    }
    public void CreateDictionary()
    {
        lines = File.ReadAllLines(filePath);
        Random rng = new Random();
        rng.Shuffle(lines);
        foreach (string line in lines)
        {
            string[] KeysAndValues = line.Trim().ToLower().Split("-");
            if (!(line == ""))
            {
                for (int i = 0; i < KeysAndValues.Length; i += 2)
                {
                    try
                    {
                        dictionary.Add(KeysAndValues[i], KeysAndValues[i + 1]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }
    public void PrintNextValue()
    {
        foreach (var keyValuePair in dictionary)
        {
            ShowProgress(guesses, dictionary.Count);
            word = keyValuePair.Key;
            Console.Write($"{word}- ");
            CheckValue();
            AddUnknownWord(IsUnknown, word, lines);
            Console.Clear();
        }
    }
    public void CheckValue()
    {
        string answer = Convert.ToString(Console.ReadLine()).Replace("?", "і");
        string correctAnswer = dictionary[$"{word}"].Trim();

        bool hasDoubleVal = HasDoubleValue(correctAnswer);
        if (!hasDoubleVal)
        {
            if (answer == correctAnswer)
            {
                correctGuesses++;
            }
            else if (answer == "")
            {
                Console.WriteLine($"\n{word}- {correctAnswer}");
                IsUnknown = true;
            }
            else
            {
                while (!IsCorrect)
                {
                    Console.WriteLine("\nPlease try again or type \"0\" to get answer\n");
                    Console.Write(word + "- ");
                    string Newanswer = Convert.ToString(Console.ReadLine()).Replace("?", "і");
                    if (correctAnswer == Newanswer)
                    {
                        IsCorrect = true;
                        correctGuesses++;
                    }
                    else if (Newanswer == "0")
                    {
                        IsCorrect = true;
                        Console.WriteLine($"\n{word}- {correctAnswer}");
                        IsUnknown = true;
                    }
                }
            }
        }
        else
        {
            string[] answers = correctAnswer.Split(',');
            for (int i = 0; i < answers.Length; i++)
            {
                answers[i] = answers[i].Trim();
            }
            if (answer == answers[0] || answer == answers[1])
            {
                correctGuesses++;
                Console.WriteLine($"\n{word}- {correctAnswer}");
            }
            else if (answer == "")
            {
                Console.WriteLine($"\n{word}- {correctAnswer}");
                IsUnknown = true;
            }
            else
            {
                while (!IsCorrect)
                {
                    Console.WriteLine("\nPlease try again or type \"0\" to get answer\n");
                    Console.Write(word + "- ");
                    string newanswer = Convert.ToString(Console.ReadLine()).Replace("?", "і");
                    if (newanswer == answers[0] || newanswer == answers[1])
                    {
                        correctGuesses++;
                        IsCorrect = true;
                        Console.WriteLine($"\n{word}- {correctAnswer}");
                    }
                    else if (newanswer == "0")
                    {
                        IsCorrect = IsUnknown = true;
                        Console.WriteLine($"\n{word}- {correctAnswer}");
                    }
                }
            }
        }
        IsCorrect = IsUnknown = false;
        guesses++;
        Task.Delay(1500).Wait();
    }

    public static bool HasDoubleValue(string answer)
    {
        return answer.Contains(',');
    }

    public static string GetUnkwownString(bool val, string word, string[] arr)
    {
        if (val)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Contains(word))
                {
                    return arr[i];
                }
            }
        }
        return null;
    }

    public static void AddUnknownWord(bool val, string word, string[] arr)
    {
        string words = GetUnkwownString(val, word, arr);
        if (!(words == null))
        {
            newDiction.Add(words);
        }
    }

    public static void CreatenewFile(ArrayList arr)
    {
        string[] newArray = (string[])arr.ToArray(typeof(string));
        File.WriteAllLines(newFilePath, newArray);
    }
    public static void ShowScore(int count, int correct)
    {
        int countOfAll = count;
        int score = correct;
        Console.Clear();
        Console.WriteLine($"Your score is {score}/{countOfAll}");
        Console.ReadKey();
        Console.Clear();
    }
    public static void ShowProgress(int current, int all)
    {
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Write($"{current} / {all}");
        Console.SetCursorPosition(0, 0);
    }
}