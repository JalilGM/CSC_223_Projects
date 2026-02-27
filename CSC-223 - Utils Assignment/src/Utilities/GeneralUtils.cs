/**
 * A utilities class providing generic helper methods for common operations such as
 * searching, validation, text transformation, and statistical calculations.
 * 
 * Bugs: In Duplicates method, used >= 2 instead of == 2, causing multiple entries of the same duplicate.
 * 
 * @author Jalil Garvin-Mingo
 * @date   January 21, 2026
 */

using System.Collections.Concurrent;

namespace Utilities;

/// <summary>
/// A utilities class providing generic helper methods for common operations.
/// </summary>
public class GeneralUtils
{
    /// <summary>
    /// Searches for an item in a generic array using equality comparison.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array</typeparam>
    /// <param name="array">The array to search through</param>
    /// <param name="item">The item to search for</param>
    /// <returns>True if the item is found in the array, false otherwise</returns>
    public bool Contains<T>(T[] array, T item)
{
    // Iterate through array using index counter
    int index = 0;
    while (index < array.Length)
    {
        // Return true if equality match is found
        if (EqualityComparer<T>.Default.Equals(array[index], item)) return true;
        index++;
    }
    return false;
}

/// <summary>
/// Generates an indentation string of spaces based on the specified level.
/// Each level represents 4 spaces.
/// </summary>
/// <param name="level">The indentation level (0 or greater)</param>
/// <returns>A string of spaces with length = level * 4</returns>
public string GetIndentation(int level)
{
    return new string(' ', level * 4);
}

/// <summary>
/// Validates that a variable name contains only lowercase characters,
/// digits, and underscores.
/// </summary>
/// <param name="name">The variable name to validate</param>
/// <returns>True if the name is valid (no uppercase letters), false otherwise</returns>
public bool IsValidVariable(string name)
{
    // Empty names are invalid
    if (string.IsNullOrEmpty(name)) return false;
    
    // Check each character for uppercase letters
    foreach (char c in name)
    {
        if (char.IsUpper(c)) return false;
    }
    return true;
}

/// <summary>
/// Validates that an operator is in the set of supported operators.
/// </summary>
/// <param name="op">The operator string to validate</param>
/// <returns>True if the operator is supported, false otherwise</returns>
public bool IsValidOperator(string op)
{
    string[] operators = { "+", "-", "*", "/", "//", "%", "**"};
    return Contains(operators, op);
}

/// <summary>
/// Counts the number of occurrences of a character in a string.
/// </summary>
/// <param name="s">The string to search</param>
/// <param name="c">The character to count</param>
/// <returns>The number of times the character appears in the string</returns>
public int CountOccurences(string s, char c)
{
    int count = 0;
    // Iterate through string and count matching characters
    foreach (char ch in s)
    {
        if (ch == c) count++;
    }
    return count;
}

/// <summary>
/// Converts a string to camel case format. The first word is lowercase,
/// and subsequent words have their first letter capitalized.
/// </summary>
/// <param name="s">The input string with space-separated words</param>
/// <returns>The string converted to camel case</returns>
public string ToCamelCase(string s)
{
    // Split input string into words separated by spaces
    string [] words = s.Split(' ');
    for (int i = 0; i < words.Length; i++)
    {
        // Convert first word to lowercase
        if (i == 0) words[i] = words[i].ToLower();
        // Capitalize first letter of following words
        else if (words[i].Length > 0) words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
    }
    return string.Join("", words);
}

/// <summary>
/// Validates that a password meets strength requirements. A strong password
/// must be at least 8 characters long and contain uppercase, lowercase,
/// digit, and special characters.
/// </summary>
/// <param name="pwd">The password string to validate</param>
/// <returns>True if the password is strong, false otherwise</returns>
public bool IsPasswordStrong(string pwd)
{
    if (pwd.Length < 8) return false;
    
    // Check for required character types
    bool upper = pwd.Any(char.IsUpper);
    bool lower = pwd.Any(char.IsLower);
    bool digit = pwd.Any(char.IsDigit);
    bool special = pwd.Any(c => !char.IsLetterOrDigit(c));

    return upper && lower && digit && special;
}

/// <summary>
/// Extracts unique items from a list, removing all duplicates.
/// </summary>
/// <typeparam name="T">The type of elements in the list</typeparam>
/// <param name="list">The list to extract unique items from</param>
/// <returns>A list containing only unique items from the input</returns>
/// <exception cref="ArgumentException">Thrown when the list is null</exception>
public List<T> GetUniqueItems<T>(List<T> list)
{
    if (list == null) throw new ArgumentException("list has nothing in it");

    List<T> uniquelist = new List<T>();
    foreach (T item in list)
    {
        if (!uniquelist.Contains(item)) uniquelist.Add(item);
    }
    return uniquelist;
}

/// <summary>
/// Calculates the average of a set of numbers.
/// </summary>
/// <param name="numbers">An array of integers to calculate the average from</param>
/// <returns>The average as a double</returns>
/// <exception cref="ArgumentException">Thrown when the array is null or 
/// empty</exception>
public double CalculateAverage(int[] numbers)
{
    if (numbers == null || numbers.Length == 0) throw new ArgumentException("numbers has nothing in it");

    double sum = 0;
    foreach (int num in numbers)
    {
        sum += num;
    }
    return sum / numbers.Length;
}

/// <summary>
/// Identifies all duplicate items in an array. Returns items that appear
/// more than once, with each duplicate value returned only once.
/// </summary>
/// <typeparam name="T">The type of elements in the array</typeparam>
/// <param name="array">The array to search for duplicates</param>
/// <returns>An array of duplicate items found in the input</returns>
public T[] Duplicates<T>(T[] array)
{
    Dictionary<T, int> countofcopies = new Dictionary<T, int>();
    List<T> duplicates = new List<T>();
    foreach (T item in array)
    {
        if (countofcopies.ContainsKey(item))
        {
            countofcopies[item]++;
            if (countofcopies[item] == 2) duplicates.Add(item);
        }
        else countofcopies[item] = 1;
    }
    return duplicates.ToArray();
}
}