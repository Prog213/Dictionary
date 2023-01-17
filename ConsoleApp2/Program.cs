using System.Collections;
using System.Text;

namespace Dictionary
{
    class Program
    {
        public Dictionary<string, string> dictionary = new Dictionary<string, string>();
        readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Files.txt\AllWords.txt");
        static readonly string newFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Files.txt\UnknownWords.txtqwe");
        string word = null;
        public bool IsCorrect { get; set; }
        public bool IsUnknown { get; set; }
        public int correctGuesses = 0;
        public int guesses = 1;
        string[] lines = new string[1];
        public ArrayList newDiction = new ArrayList();
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Program program = new Program();
            program.CreateDictionary();
            program.PrintNextValue(program.newDiction);
            CreatenewFile(program.newDiction);
            ShowScore(program.dictionary.Count, program.correctGuesses);
        }
        public void CreateDictionary()
        {
            bool hasExeptions = false;
            lines = File.ReadAllLines(filePath);
            Random rng = new Random();
            rng.Shuffle(lines);
            foreach (string line in lines)
            {
                string[] KeysAndValues = line.ToLower().Split("-");
                for (int i = 0; i < KeysAndValues.Length; i++)
                {
                    KeysAndValues[i] = KeysAndValues[i].Trim();
                }
                if (line != "")
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
                            hasExeptions = true;
                        }
                    }
                }
            }
            if (hasExeptions)
            {
                char k;
                Console.WriteLine("\nPress Esc to continue");
                do
                {
                    k = Console.ReadKey().KeyChar;
                    Console.WriteLine("");
                }
                while (k != '\u001b');
                Console.Clear();
            }
        }


        public void PrintNextValue(ArrayList arr)
        {
            foreach (var keyValuePair in dictionary)
            {
                ShowProgress(guesses, dictionary.Count);
                word = keyValuePair.Key;
                Console.Write($"{word} - ");
                CheckValue();
                AddUnknownWord(IsUnknown, word, dictionary, arr);
                IsCorrect = IsUnknown = false;
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
                    Console.WriteLine($"\n{word} - {correctAnswer}");
                    IsUnknown = true;
                }
                else
                {
                    while (!IsCorrect)
                    {
                        Console.WriteLine("\nPlease try again or type \"0\" to get answer\n");
                        Console.Write(word + "- ");
                        answer = Convert.ToString(Console.ReadLine()).Replace("?", "і");
                        if (correctAnswer == answer)
                        {
                            IsCorrect = true;
                            correctGuesses++;
                        }
                        else if (answer == "0")
                        {
                            IsCorrect = true;
                            Console.WriteLine($"\n{word} - {correctAnswer}");
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
                    Console.WriteLine($"\n{word} - {correctAnswer}");
                }
                else if (answer == "")
                {
                    Console.WriteLine($"\n{word} - {correctAnswer}");
                    IsUnknown = true;
                }
                else
                {
                    while (!IsCorrect)
                    {
                        Console.WriteLine("\nPlease try again or type \"0\" to get answer\n");
                        Console.Write(word + "- ");
                        answer = Convert.ToString(Console.ReadLine()).Replace("?", "і");
                        if (answer == answers[0] || answer == answers[1])
                        {
                            correctGuesses++;
                            IsCorrect = true;
                            Console.WriteLine($"\n{word}- {correctAnswer}");
                        }
                        else if (answer == "0")
                        {
                            IsCorrect = IsUnknown = true;
                            Console.WriteLine($"\n{word} - {correctAnswer}");
                        }
                    }
                }
            }
            guesses++;
            AddDelay(hasDoubleVal, answer, correctAnswer);
        }

        public static bool HasDoubleValue(string answer)
        {
            return answer.Contains(',');
        }

        public static string GetUnkwownString(bool val, string word, Dictionary<string, string> dictionary)
        {
            string[] arr = dictionary.ToArrayDashed();
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

        public static void AddUnknownWord(bool val, string word, Dictionary<string, string> dictionary, ArrayList arrayList)
        {
            string words = GetUnkwownString(val, word, dictionary);
            if (words != null)
            {
                arrayList.Add(words);
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
        public static void AddDelay(bool doubval, string ans, string corans)
        {
            if (doubval)
            {
                if (!corans.Contains(ans))
                {
                    Task.Delay(1500).Wait();
                }
            }
            else
            {
                if (ans != corans)
                {
                    Task.Delay(1500).Wait();
                }
            }
        }
    }
}