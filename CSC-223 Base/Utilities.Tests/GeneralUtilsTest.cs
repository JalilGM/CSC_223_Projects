/**
 * Comprehensive unit tests for GeneralUtils class. Tests all public methods
 * with various inputs including edge cases, duplicates, and error conditions.
 * 
 * Bugs: None known at this time.
 * 
 * @author Jail Garvin-Mingo
 * @date   January 21, 2026
 */

using Xunit;
using Utilities;


namespace Utilities.Tests;


public class GeneralUtilsTest
{
    private readonly GeneralUtils _utils = new GeneralUtils();

    #region Contains Tests
    [Theory]
    [InlineData(new int[] { 1, 2, 3, 4, 5 }, 3, true)]
    [InlineData(new int[] { 1, 2, 3, 4, 5 }, 6, false)]
    [InlineData(new int[] { }, 1, false)]
    [InlineData(new int[] { 1, 1, 1, 1 }, 1, true)] // duplicates
    [InlineData(new int[] { 5 }, 5, true)]
    [InlineData(new string[] { "hello", "world" }, "hello", true)]
    [InlineData(new string[] { "hello", "world" }, "goodbye", false)]
    [InlineData(new string[] { "test", "test", "test" }, "test", true)] // string duplicates
    public void Contains_WithVariousInputs_ReturnsExpectedResult<T>(T[] array, T item, bool expected)
    {
        var result = _utils.Contains(array, item);
        Assert.Equal(expected, result);
    }

    #endregion

    #region GetIndentation Tests
    [Theory]
    [InlineData(0, "")]
    [InlineData(1, "    ")]
    [InlineData(2, "        ")]
    [InlineData(3, "            ")]
    [InlineData(5, "                    ")]
    public void GetIndentation_WithVariousLevels_ReturnsCorrectSpaces(int level, string expected)
    {
        var result = _utils.GetIndentation(level);
        Assert.Equal(expected, result);
    }

    #endregion

    #region IsValidVariable Tests
    [Theory]
    [InlineData("validname", true)]
    [InlineData("test_var", true)]
    [InlineData("name123", true)]
    [InlineData("ValidName", false)] // uppercase
    [InlineData("CONSTANT", false)] // all uppercase
    [InlineData("", false)] // empty
    [InlineData("   ", true)] // spaces only (edge case)
    [InlineData("singleword", true)]
    [InlineData("multiple_words_here", true)]
    [InlineData("Has_Upper_Case", false)]
    [InlineData("a", true)] // single lowercase letter
    [InlineData("A", false)] // single uppercase letter
    public void IsValidVariable_WithVariousNames_ReturnsExpectedValidity(string name, bool expected)
    {
        var result = _utils.IsValidVariable(name);
        Assert.Equal(expected, result);
    }

    #endregion

    #region IsValidOperator Tests
    [Theory]
    [InlineData("+", true)]
    [InlineData("-", true)]
    [InlineData("*", true)]
    [InlineData("/", true)]
    [InlineData("//", true)]
    [InlineData("%", true)]
    [InlineData("**", true)]
    [InlineData("^", false)]
    [InlineData("&", false)]
    [InlineData("|", false)]
    [InlineData("", false)]
    [InlineData("+++", false)]
    [InlineData("**", true)] // power operator
    public void IsValidOperator_WithVariousOperators_ReturnsExpectedValidity(string op, bool expected)
    {
        var result = _utils.IsValidOperator(op);
        Assert.Equal(expected, result);
    }

    #endregion

    #region CountOccurences Tests
    [Theory]
    [InlineData("hello", 'l', 2)]
    [InlineData("hello", 'h', 1)]
    [InlineData("hello", 'z', 0)]
    [InlineData("aaa", 'a', 3)] // all same character
    [InlineData("", 'a', 0)] // empty string
    [InlineData("banana", 'a', 3)]
    [InlineData("Mississippi", 's', 4)]
    [InlineData("test", 't', 2)] // duplicate occurrences
    [InlineData("aaaa", 'a', 4)] // all duplicates
    public void CountOccurences_WithVariousStringsAndChars_ReturnsCorrectCount(string s, char c, int expected)
    {
        var result = _utils.CountOccurences(s, c);
        Assert.Equal(expected, result);
    }

    #endregion

    #region ToCamelCase Tests
    [Theory]
    [InlineData("hello world", "helloWorld")]
    [InlineData("hello", "hello")]
    [InlineData("hello world test", "helloWorldTest")]
    [InlineData("the quick brown fox", "theQuickBrownFox")]
    [InlineData("a b c", "aBC")]
    [InlineData("HELLO WORLD", "helloWORLD")] // uppercase input
    [InlineData("test test test", "testTestTest")] // duplicates
    [InlineData("single", "single")]
    public void ToCamelCase_WithVariousPhrases_ReturnsCorrectCamelCase(string s, string expected)
    {
        var result = _utils.ToCamelCase(s);
        Assert.Equal(expected, result);
    }

    #endregion

    #region IsPasswordStrong Tests
    [Theory]
    [InlineData("Str0ng!Pwd", true)] // meets all criteria
    [InlineData("Short1!", false)] // too short
    [InlineData("nouppercase1!", false)] // no uppercase
    [InlineData("NOLOWERCASE1!", false)] // no lowercase
    [InlineData("NoDigits!Abc", false)] // no digits
    [InlineData("NoSpecial1Abc", false)] // no special characters
    [InlineData("ValidPassword1!", true)]
    [InlineData("Another.Valid.1", true)]
    [InlineData("P@ssw0rd", true)]
    [InlineData("Weak", false)] // too short and missing requirements
    [InlineData("ValidPwd1!", true)] // minimal valid password
    [InlineData("VeryLongPasswordWith1SpecialChar!", true)] // long valid password
    public void IsPasswordStrong_WithVariousPasswords_ReturnsExpectedStrength(string pwd, bool expected)
    {
        var result = _utils.IsPasswordStrong(pwd);
        Assert.Equal(expected, result);
    }

    #endregion

    #region GetUniqueItems Tests
    [Fact]
    public void GetUniqueItems_WithNullList_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _utils.GetUniqueItems<int>(null));
    }

    [Theory]
    [InlineData(new int[] { 1, 2, 3 })]
    [InlineData(new int[] { 1, 1, 2, 2, 3, 3 })] // all duplicates
    public void GetUniqueItems_WithVariousLists_ReturnsOnlyUniqueItems(int[] inputArray)
    {
        var input = new List<int>(inputArray);
        var result = _utils.GetUniqueItems(input);

        // Verify all items are unique
        var uniqueSet = new HashSet<int>(result);
        Assert.Equal(uniqueSet.Count, result.Count);
    }

    [Fact]
    public void GetUniqueItems_WithDuplicateIntegers_RemovesDuplicates()
    {
        var input = new List<int> { 1, 2, 2, 3, 3, 3 };
        var result = _utils.GetUniqueItems(input);
        
        Assert.Contains(1, result);
        Assert.Contains(2, result);
        Assert.Contains(3, result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void GetUniqueItems_WithDuplicateStrings_RemovesDuplicates()
    {
        var input = new List<string> { "apple", "banana", "apple", "cherry", "banana", "apple" };
        var result = _utils.GetUniqueItems(input);

        Assert.Contains("apple", result);
        Assert.Contains("banana", result);
        Assert.Contains("cherry", result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void GetUniqueItems_WithEmptyList_ReturnsEmptyList()
    {
        var input = new List<int> { };
        var result = _utils.GetUniqueItems(input);
        
        Assert.Empty(result);
    }

    [Fact]
    public void GetUniqueItems_WithAllSameElements_ReturnsOneElement()
    {
        var input = new List<int> { 5, 5, 5, 5, 5 };
        var result = _utils.GetUniqueItems(input);
        
        Assert.Single(result);
        Assert.Equal(5, result[0]);
    }

    #endregion

    #region CalculateAverage Tests
    [Theory]
    [InlineData(new int[] { 1, 2, 3, 4, 5 }, 3.0)]
    [InlineData(new int[] { 10 }, 10.0)]
    [InlineData(new int[] { 0, 0, 0 }, 0.0)]
    [InlineData(new int[] { -5, -10, 5, 10 }, 0.0)]
    [InlineData(new int[] { 100, 200, 300 }, 200.0)]
    [InlineData(new int[] { 1, 1, 1, 1 }, 1.0)] // all same (duplicates)
    [InlineData(new int[] { 2, 2, 2, 2, 2 }, 2.0)] // all duplicates
    public void CalculateAverage_WithVariousArrays_ReturnsCorrectAverage(int[] numbers, double expected)
    {
        var result = _utils.CalculateAverage(numbers);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CalculateAverage_WithEmptyArray_ThrowsArgumentException()
    {
        var emptyArray = new int[] { };
        Assert.Throws<ArgumentException>(() => _utils.CalculateAverage(emptyArray));
    }

    [Fact]
    public void CalculateAverage_WithNullArray_ThrowsArgumentException()
    {
        int[] nullArray = null;
        Assert.Throws<ArgumentException>(() => _utils.CalculateAverage(nullArray));
    }

    #endregion

    #region Duplicates Tests
    [Theory]
    [InlineData(new int[] { 1, 2, 3, 4, 5 })] // no duplicates
    public void Duplicates_WithNoDuplicates_ReturnsEmptyArray(int[] input)
    {
        var result = _utils.Duplicates(input);
        Assert.Empty(result);
    }

    [Fact]
    public void Duplicates_WithSimpleDuplicates_ReturnsDuplicateItems()
    {
        var input = new int[] { 1, 2, 2, 3, 4, 4 };
        var result = _utils.Duplicates(input);

        Assert.Contains(2, result);
        Assert.Contains(4, result);
        Assert.Equal(2, result.Length);
    }

    [Fact]
    public void Duplicates_WithAllDuplicates_ReturnsAllDuplicates()
    {
        var input = new int[] { 1, 1, 2, 2, 3, 3 };
        var result = _utils.Duplicates(input);

        Assert.Contains(1, result);
        Assert.Contains(2, result);
        Assert.Contains(3, result);
    }

    [Fact]
    public void Duplicates_WithTripleDuplicates_ReturnsEachDuplicateOnce()
    {
        var input = new int[] { 1, 1, 1, 2, 2, 2 };
        var result = _utils.Duplicates(input);

        // Should contain duplicates (items that appear 2+ times)
        Assert.NotEmpty(result);
        var uniqueDuplicates = new HashSet<int>(result);
        Assert.Equal(2, uniqueDuplicates.Count); // two unique duplicate values
    }

    [Fact]
    public void Duplicates_WithStringDuplicates_ReturnsStringDuplicates()
    {
        var input = new string[] { "apple", "banana", "apple", "cherry", "banana" };
        var result = _utils.Duplicates(input);

        Assert.Contains("apple", result);
        Assert.Contains("banana", result);
        Assert.Equal(2, result.Length);
    }

    [Fact]
    public void Duplicates_WithEmptyArray_ReturnsEmptyArray()
    {
        var input = new int[] { };
        var result = _utils.Duplicates(input);

        Assert.Empty(result);
    }

    [Fact]
    public void Duplicates_WithSingleElement_ReturnsEmptyArray()
    {
        var input = new int[] { 1 };
        var result = _utils.Duplicates(input);

        Assert.Empty(result);
    }

    [Fact]
    public void Duplicates_WithManyDuplicatesOfOneValue_ReturnsOnlyOneValue()
    {
        var input = new int[] { 5, 5, 5, 5, 5 };
        var result = _utils.Duplicates(input);

        Assert.Single(result);
        Assert.Equal(5, result[0]);
    }

    #endregion
}
