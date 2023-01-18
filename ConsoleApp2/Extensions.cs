namespace Dictionary
{
    internal static class Extensions
    {
        public static void Shuffle(this Random rgn, string[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rgn.Next(n--);
                (array[k], array[n]) = (array[n], array[k]);
            }
        }
        public static string[] ToArrayDashed(this Dictionary<string, string> dictionary)
        {
            string[] array = new string[dictionary.Count];
            var Keys = dictionary.Keys.ToArray();
            var Values = dictionary.Values.ToArray();
            for (int i = 0; i < dictionary.Count; i++)
            {
                array[i] = $"{Keys[i]} - {Values[i]}";
            }
            return array;
        }
        public static Dictionary<string, string> ToDiction(this string[] lines)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
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
            return dictionary;
        }
    }
}