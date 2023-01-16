internal static class RandomExtensionsHelpers
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
}