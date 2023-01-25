using System.Collections;
using System.Text;

namespace Dictionary
{
    class Program
    {
        public Dictionary<string, string> dictionary = new Dictionary<string, string>();
        static readonly string allWordsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Files.txt\AllWords.txt");
        static readonly string unknownWordsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Files.txt\UnknownWords.txt");
        static readonly string learnedWordsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Files.txt\LearnedWords.txt");
        string word = null;
        public bool IsCorrect { get; set; }
        public bool IsUnknown { get; set; }
        public int correctGuesses = 0;
        public int guesses = 1;
        public ArrayList learnedWords = new ArrayList();
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Program program = new Program();
            program.CreateDictionary();
            program.PrintNextValue(program.learnedWords);
            CreatenewFile(program.learnedWords);
            ShowScore(program.dictionary.Count, program.correctGuesses);
        }
        public void CreateDictionary()
        {
            string[] lines = File.ReadAllLines(unknownWordsPath);
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
                        catch (Exception)
                        {
                            //Exeption
                        }
                    }
                }
            }
        }


        public void PrintNextValue(ArrayList learned)
        {
            foreach (var keyValuePair in dictionary)
            {
                ShowProgress(guesses, dictionary.Count);
                word = keyValuePair.Key;
                Console.Write($"{word} - ");
                CheckValue();
                AddLearnedWord(IsUnknown, word, dictionary, learned);
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
                            Console.WriteLine($"\n{word} - {correctAnswer}");
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
            //AddDelay(hasDoubleVal, answer, correctAnswer);
        }

        public static bool HasDoubleValue(string answer)
        {
            return answer.Contains(',');
        }

        public static string GetStringFromWord(string word, Dictionary<string, string> dictionary)
        {
            string[] arr = dictionary.ToArrayDashed();
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Contains(word))
                {
                    return arr[i];
                }
            }
            return null;
        }

        public static void AddLearnedWord(bool isUnkwn, string word, Dictionary<string, string> dictionary, ArrayList learned)
        {
            string words = GetStringFromWord(word, dictionary);
            if (words != null && !isUnkwn)
            {
                learned.Add(words);
            }
        }

        public static void CreatenewFile(ArrayList learned)
        {
            ArrayList alllearnedArray = AddLearnedFromTxt(learned);
            string[] allWordsArray = GetArrayFromFile(allWordsPath);
            ArrayList learnedSortedArray = new ArrayList();
            ArrayList unknownSortedArray = new ArrayList();

            if (alllearnedArray.Count != 0)
            {
                for (int i = 0; i < allWordsArray.Length; i++)
                {
                    for (int k = 0; k < alllearnedArray.Count; k++)
                    {
                        if (allWordsArray[i] == (string)alllearnedArray[k])
                        {
                            learnedSortedArray.Add(allWordsArray[i]);
                        }
                        else
                        {
                            unknownSortedArray.Add(allWordsArray[i]);
                        }
                    }
                }
            }
            else
            {
                unknownSortedArray.AddRange(allWordsArray);
            }
            File.WriteAllLines(unknownWordsPath,(string[])unknownSortedArray.ToArray(typeof(string)));
            File.AppendAllLines(learnedWordsPath, (string[])learnedSortedArray.ToArray(typeof(string)));

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
        public static string[] GetArrayFromFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            return lines.ToDiction().ToArrayDashed();
        }
        public static ArrayList AddLearnedFromTxt(ArrayList learned)
        {
            string[] learnedWordsArray = GetArrayFromFile(learnedWordsPath);
            ArrayList arrayList = new();
            if (learned.Capacity != 0)
            {
                arrayList.AddRange(learned);
            }
            if (learnedWordsArray.Length != 0)
            {
                arrayList.AddRange(learnedWordsArray);
            }
            return arrayList;
        }
    }
}