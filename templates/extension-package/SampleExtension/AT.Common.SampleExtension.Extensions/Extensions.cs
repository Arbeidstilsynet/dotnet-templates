namespace Arbeidstilsynet.Common.SampleExtension.Extensions;

/// <summary>
/// This class contains extension methods for the SampleExtension.
/// </summary>
public static class SampleExtension
{
    private static readonly char[] Separator = [' ', '.', '?'];

    /// <summary>
    /// Counts the number of words in a string.
    /// </summary>
    /// <param name="str"></param>
    /// <returns>Word count of str</returns>
    public static int WordCount(this string str) =>
        str.Split(Separator, StringSplitOptions.RemoveEmptyEntries).Length;
}
