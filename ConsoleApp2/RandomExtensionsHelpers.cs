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
}