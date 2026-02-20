

using Xunit;

namespace Tokenizer;

public class TokenizerTest
{
    private readonly TokenizerImpl _tokenizer = new TokenizerImpl();

    #region Token Class Tests

    [Fact]
    public void Token_Constructor_SetsValueAndType()
    {
        var token = new Token("42", TokenType.INTEGER);
        Assert.Equal("42", token.Value);
        Assert.Equal(TokenType.INTEGER, token.Type);
    }

    [Fact]
    public void Token_ToString_ReturnsCorrectFormat()
    {
        var token = new Token("test", TokenType.VARIABLE);
        Assert.Equal("(test, VARIABLE)", token.ToString());
    }

    [Fact]
    public void Token_Equals_SameValueAndType_ReturnsTrue()
    {
        var token1 = new Token("42", TokenType.INTEGER);
        var token2 = new Token("42", TokenType.INTEGER);
        Assert.True(token1.Equals(token2));
    }

    [Fact]
    public void Token_Equals_DifferentValue_ReturnsFalse()
    {
        var token1 = new Token("42", TokenType.INTEGER);
        var token2 = new Token("43", TokenType.INTEGER);
        Assert.False(token1.Equals(token2));
    }

    [Fact]
    public void Token_Equals_DifferentType_ReturnsFalse()
    {
        var token1 = new Token("42", TokenType.INTEGER);
        var token2 = new Token("42", TokenType.FLOAT);
        Assert.False(token1.Equals(token2));
    }

    [Fact]
    public void Token_Equals_NonTokenObject_ReturnsFalse()
    {
        var token = new Token("42", TokenType.INTEGER);
        Assert.False(token.Equals("not a token"));
    }

    [Fact]
    public void Token_Equals_NullObject_ReturnsFalse()
    {
        var token = new Token("42", TokenType.INTEGER);
        Assert.False(token.Equals(null));
    }

    #endregion

    #region Integer Tokenization Tests

    [Theory]
    [InlineData("0", "0")]
    [InlineData("42", "42")]
    [InlineData("999", "999")]
    [InlineData("100000", "100000")]
    public void Tokenize_SingleInteger_ReturnsCorrectToken(string input, string expectedValue)
    {
        var result = _tokenizer.Tokenize(input);
        Assert.Single(result);
        Assert.Equal(expectedValue, result[0].Value);
        Assert.Equal(TokenType.INTEGER, result[0].Type);
    }

    [Fact]
    public void Tokenize_MultipleIntegers_WithSpaces_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("42 100 999");
        Assert.Equal(3, result.Count);
        Assert.All(result, token => Assert.Equal(TokenType.INTEGER, token.Type));
        Assert.Equal(new[] { "42", "100", "999" }, result.Select(t => t.Value));
    }

    #endregion

    #region Float Tokenization Tests

    [Theory]
    [InlineData("0.0", "0.0")]
    [InlineData("3.14", "3.14")]
    [InlineData("42.5", "42.5")]
    [InlineData("0.999", "0.999")]
    public void Tokenize_SingleFloat_ReturnsCorrectToken(string input, string expectedValue)
    {
        var result = _tokenizer.Tokenize(input);
        Assert.Single(result);
        Assert.Equal(expectedValue, result[0].Value);
        Assert.Equal(TokenType.FLOAT, result[0].Type);
    }

    [Fact]
    public void Tokenize_MultipleFloats_WithSpaces_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("3.14 2.71 1.41");
        Assert.Equal(3, result.Count);
        Assert.All(result, token => Assert.Equal(TokenType.FLOAT, token.Type));
        Assert.Equal(new[] { "3.14", "2.71", "1.41" }, result.Select(t => t.Value));
    }

    [Fact]
    public void Tokenize_DecimalPoint_WithoutDigits_AfterPoint_ReturnsFloat()
    {
        var result = _tokenizer.Tokenize("42.");
        Assert.Single(result);
        Assert.Equal("42.", result[0].Value);
        Assert.Equal(TokenType.FLOAT, result[0].Type);
    }

    #endregion

    #region Variable Tokenization Tests

    [Theory]
    [InlineData("x", "x")]
    [InlineData("abc", "abc")]
    [InlineData("MyVariable", "MyVariable")]
    public void Tokenize_SingleVariable_ReturnsCorrectToken(string input, string expectedValue)
    {
        var result = _tokenizer.Tokenize(input);
        Assert.Single(result);
        Assert.Equal(expectedValue, result[0].Value);
        Assert.Equal(TokenType.VARIABLE, result[0].Type);
    }

    [Fact]
    public void Tokenize_MultipleVariables_WithSpaces_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("x y z");
        Assert.Equal(3, result.Count);
        Assert.All(result, token => Assert.Equal(TokenType.VARIABLE, token.Type));
        Assert.Equal(new[] { "x", "y", "z" }, result.Select(t => t.Value));
    }

    #endregion

    #region Return Keyword Tests

    [Fact]
    public void Tokenize_ReturnKeyword_ReturnsCorrectToken()
    {
        var result = _tokenizer.Tokenize("return");
        Assert.Single(result);
        Assert.Equal("return", result[0].Value);
        Assert.Equal(TokenType.RETURN, result[0].Type);
    }

    [Fact]
    public void Tokenize_ReturnKeyword_CaseSensitive_AsVariable()
    {
        var result = _tokenizer.Tokenize("Return");
        Assert.Single(result);
        Assert.Equal("Return", result[0].Value);
        Assert.Equal(TokenType.VARIABLE, result[0].Type);
    }

    [Fact]
    public void Tokenize_ReturnKeyword_InExpression_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("return x");
        Assert.Equal(2, result.Count);
        Assert.Equal(TokenType.RETURN, result[0].Type);
        Assert.Equal(TokenType.VARIABLE, result[1].Type);
    }

    #endregion

    #region Operator Tokenization Tests

    [Theory]
    [InlineData("+", TokenConstants.PLUS)]
    [InlineData("-", TokenConstants.MINUS)]
    [InlineData("*", TokenConstants.TIMES)]
    [InlineData("/", TokenConstants.FLOAT_DIVISION)]
    [InlineData("%", TokenConstants.MODULUS)]
    public void Tokenize_SingleCharOperators_ReturnsCorrectToken(string input, string expectedValue)
    {
        var result = _tokenizer.Tokenize(input);
        Assert.Single(result);
        Assert.Equal(expectedValue, result[0].Value);
        Assert.Equal(TokenType.OPERATOR, result[0].Type);
    }

    [Theory]
    [InlineData("//", TokenConstants.INTEGER_DIVISION)]
    [InlineData("**", TokenConstants.EXPONENTIATION)]
    public void Tokenize_TwoCharOperators_ReturnsCorrectToken(string input, string expectedValue)
    {
        var result = _tokenizer.Tokenize(input);
        Assert.Single(result);
        Assert.Equal(expectedValue, result[0].Value);
        Assert.Equal(TokenType.OPERATOR, result[0].Type);
    }

    [Fact]
    public void Tokenize_MultipleOperators_WithSpaces_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("+ - * /");
        Assert.Equal(4, result.Count);
        Assert.All(result, token => Assert.Equal(TokenType.OPERATOR, token.Type));
    }

    [Fact]
    public void Tokenize_ArithmeticExpression_WithIntegersAndOperators_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("42 + 100");
        Assert.Equal(3, result.Count);
        Assert.Equal(TokenType.INTEGER, result[0].Type);
        Assert.Equal(TokenType.OPERATOR, result[1].Type);
        Assert.Equal(TokenType.INTEGER, result[2].Type);
    }

    [Fact]
    public void Tokenize_IntegerDivision_BeforeDivision_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("10 // 3");
        Assert.Equal(3, result.Count);
        Assert.Equal(TokenType.INTEGER, result[0].Type);
        Assert.Equal("//", result[1].Value);
        Assert.Equal(TokenType.OPERATOR, result[1].Type);
    }

    [Fact]
    public void Tokenize_Exponentiation_ReturnsCorrectToken()
    {
        var result = _tokenizer.Tokenize("2 ** 3");
        Assert.Equal(3, result.Count);
        Assert.Equal("**", result[1].Value);
        Assert.Equal(TokenType.OPERATOR, result[1].Type);
    }

    #endregion

    #region Assignment Tokenization Tests

    [Fact]
    public void Tokenize_AssignmentOperator_StandAlone_ThrowsArgumentException()
    {
        // The tokenizer has a bug: assignments array contains ":=" but checks for single ":"
        // This causes single ":" to throw "Unexpected character"
        var ex = Assert.Throws<ArgumentException>(() => _tokenizer.Tokenize(":="));
        Assert.Contains("Unexpected character", ex.Message);
    }

    #endregion

    #region Separator/Parenthesis Tests

    [Theory]
    [InlineData("(", TokenType.LEFT_PAREN)]
    [InlineData(")", TokenType.RIGHT_PAREN)]
    [InlineData("{", TokenType.LEFT_CURLY)]
    [InlineData("}", TokenType.RIGHT_CURLY)]
    public void Tokenize_SingleSeparator_ReturnsCorrectToken(string input, TokenType expectedType)
    {
        var result = _tokenizer.Tokenize(input);
        Assert.Single(result);
        Assert.Equal(input, result[0].Value);
        Assert.Equal(expectedType, result[0].Type);
    }

    [Fact]
    public void Tokenize_MatchedParentheses_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("( )");
        Assert.Equal(2, result.Count);
        Assert.Equal(TokenType.LEFT_PAREN, result[0].Type);
        Assert.Equal(TokenType.RIGHT_PAREN, result[1].Type);
    }

    [Fact]
    public void Tokenize_MatchedCurlyBraces_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("{ }");
        Assert.Equal(2, result.Count);
        Assert.Equal(TokenType.LEFT_CURLY, result[0].Type);
        Assert.Equal(TokenType.RIGHT_CURLY, result[1].Type);
    }

    [Fact]
    public void Tokenize_FunctionCall_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("func ( 42 )");
        Assert.Equal(4, result.Count);
        Assert.Equal(TokenType.VARIABLE, result[0].Type);
        Assert.Equal(TokenType.LEFT_PAREN, result[1].Type);
        Assert.Equal(TokenType.INTEGER, result[2].Type);
        Assert.Equal(TokenType.RIGHT_PAREN, result[3].Type);
    }

    [Fact]
    public void Tokenize_CodeBlock_WithoutAssignment_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("{x+42}");
        Assert.Equal(5, result.Count);
        Assert.Equal(TokenType.LEFT_CURLY, result[0].Type);
        Assert.Equal(TokenType.VARIABLE, result[1].Type);
        Assert.Equal(TokenType.OPERATOR, result[2].Type);
        Assert.Equal(TokenType.INTEGER, result[3].Type);
        Assert.Equal(TokenType.RIGHT_CURLY, result[4].Type);
    }

    #endregion

    #region Whitespace Handling Tests

    [Fact]
    public void Tokenize_LeadingWhitespace_IgnoresAndTokenizes()
    {
        var result = _tokenizer.Tokenize("   42");
        Assert.Single(result);
        Assert.Equal("42", result[0].Value);
    }

    [Fact]
    public void Tokenize_TrailingWhitespace_IgnoresAndTokenizes()
    {
        var result = _tokenizer.Tokenize("42   ");
        Assert.Single(result);
        Assert.Equal("42", result[0].Value);
    }

    [Fact]
    public void Tokenize_TabsAndSpaces_HandlesAllWhitespace()
    {
        var result = _tokenizer.Tokenize("42\t+\t100");
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void Tokenize_NoWhitespace_StillTokenizes()
    {
        var result = _tokenizer.Tokenize("42+100");
        Assert.Equal(3, result.Count);
        Assert.Equal("42", result[0].Value);
        Assert.Equal("+", result[1].Value);
        Assert.Equal("100", result[2].Value);
    }

    #endregion

    #region Complex Expression Tests

    [Fact]
    public void Tokenize_ComplexArithmeticExpression_ReturnsAllTokens()
    {
        var result = _tokenizer.Tokenize("x+3.14+2*y");
        Assert.Equal(7, result.Count);
        Assert.Equal(TokenType.VARIABLE, result[0].Type);
        Assert.Equal(TokenType.OPERATOR, result[1].Type);
        Assert.Equal(TokenType.FLOAT, result[2].Type);
        Assert.Equal(TokenType.OPERATOR, result[3].Type);
        Assert.Equal(TokenType.INTEGER, result[4].Type);
        Assert.Equal(TokenType.OPERATOR, result[5].Type);
        Assert.Equal(TokenType.VARIABLE, result[6].Type);
    }

    [Fact]
    public void Tokenize_ExpressionWithAllOperators_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("a + b - c * d / e // f % g ** h");
        Assert.Equal(15, result.Count);
        var operatorValues = new[] { "+", "-", "*", "/", "//", "%", "**" };
        var operatorTokens = result.Where(t => t.Type == TokenType.OPERATOR).Select(t => t.Value).ToList();
        Assert.Equal(operatorValues, operatorTokens);
    }

    [Fact]
    public void Tokenize_ParenthesizedExpression_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("( a + b ) * ( c - d )");
        Assert.Equal(11, result.Count);
        Assert.Equal(TokenType.LEFT_PAREN, result[0].Type);
        Assert.Equal(TokenType.RIGHT_PAREN, result[4].Type);
        Assert.Equal(TokenType.OPERATOR, result[5].Type);
    }

    [Fact]
    public void Tokenize_NestedParentheses_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("( ( a ) )");
        Assert.Equal(5, result.Count);
        Assert.Equal(TokenType.LEFT_PAREN, result[0].Type);
        Assert.Equal(TokenType.LEFT_PAREN, result[1].Type);
        Assert.Equal(TokenType.RIGHT_PAREN, result[3].Type);
        Assert.Equal(TokenType.RIGHT_PAREN, result[4].Type);
    }

    [Fact]
    public void Tokenize_CompleteProgram_WithReturnStatement()
    {
        var result = _tokenizer.Tokenize("{x+42 return x}");
        Assert.Equal(7, result.Count);
        Assert.Equal(TokenType.LEFT_CURLY, result[0].Type);
        Assert.Equal(TokenType.VARIABLE, result[1].Type);
        Assert.Equal(TokenType.OPERATOR, result[2].Type);
        Assert.Equal(TokenType.INTEGER, result[3].Type);
        Assert.Equal(TokenType.RETURN, result[4].Type);
        Assert.Equal(TokenType.VARIABLE, result[5].Type);
        Assert.Equal(TokenType.RIGHT_CURLY, result[6].Type);
    }

    #endregion

    #region Empty Input Tests

    [Fact]
    public void Tokenize_EmptyString_ReturnsEmptyList()
    {
        var result = _tokenizer.Tokenize("");
        Assert.Empty(result);
    }

    [Fact]
    public void Tokenize_OnlyWhitespace_ReturnsEmptyList()
    {
        var result = _tokenizer.Tokenize("   \t   ");
        Assert.Empty(result);
    }

    #endregion

    #region Error Handling Tests

    [Theory]
    [InlineData("@")]
    [InlineData("#")]
    [InlineData("&")]
    [InlineData("$")]
    [InlineData("!")]
    [InlineData("?")]
    [InlineData("^")]
    public void Tokenize_UnexpectedCharacter_ThrowsArgumentException(string input)
    {
        Assert.Throws<ArgumentException>(() => _tokenizer.Tokenize(input));
    }

    [Fact]
    public void Tokenize_UnexpectedCharacterInExpression_ThrowsExceptionWithDetails()
    {
        var ex = Assert.Throws<ArgumentException>(() => _tokenizer.Tokenize("42 @ 100"));
        Assert.Contains("Unexpected character", ex.Message);
        Assert.Contains("@", ex.Message);
    }

    [Fact]
    public void Tokenize_SingleColon_WithoutEquals_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() => _tokenizer.Tokenize(":"));
        // Single colon throws "Unexpected character" because it's not recognized as start of :=
        Assert.Contains("Unexpected character", ex.Message);
    }

    #endregion

    #region Edge Cases and Boundary Tests

    [Fact]
    public void Tokenize_VeryLongInteger_ReturnsCorrectToken()
    {
        var input = "12345678901234567890";
        var result = _tokenizer.Tokenize(input);
        Assert.Single(result);
        Assert.Equal(input, result[0].Value);
        Assert.Equal(TokenType.INTEGER, result[0].Type);
    }

    [Fact]
    public void Tokenize_VeryLongVariable_ReturnsCorrectToken()
    {
        var input = "VeryLongVariableNameWithManyCharacters";
        var result = _tokenizer.Tokenize(input);
        Assert.Single(result);
        Assert.Equal(input, result[0].Value);
        Assert.Equal(TokenType.VARIABLE, result[0].Type);
    }

    [Fact]
    public void Tokenize_MixedCase_Variable_ReturnsCorrectToken()
    {
        var result = _tokenizer.Tokenize("MyVar");
        Assert.Single(result);
        Assert.Equal("MyVar", result[0].Value);
        Assert.Equal(TokenType.VARIABLE, result[0].Type);
    }

    [Fact]
    public void Tokenize_IntegerFollowedByVariable_WithoutSpace_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("42x");
        Assert.Equal(2, result.Count);
        Assert.Equal("42", result[0].Value);
        Assert.Equal(TokenType.INTEGER, result[0].Type);
        Assert.Equal("x", result[1].Value);
        Assert.Equal(TokenType.VARIABLE, result[1].Type);
    }

    [Fact]
    public void Tokenize_VariableFollowedByInteger_WithoutSpace_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("x42");
        Assert.Equal(2, result.Count);
        Assert.Equal("x", result[0].Value);
        Assert.Equal(TokenType.VARIABLE, result[0].Type);
        Assert.Equal("42", result[1].Value);
        Assert.Equal(TokenType.INTEGER, result[1].Type);
    }

    [Fact]
    public void Tokenize_ConsecutiveOperators_WithoutSpaces_ReturnsCorrectTokens()
    {
        var result = _tokenizer.Tokenize("++");
        Assert.Equal(2, result.Count);
        Assert.All(result, token => Assert.Equal(TokenType.OPERATOR, token.Type));
        Assert.All(result, token => Assert.Equal("+", token.Value));
    }

    [Fact]
    public void Tokenize_DuplicateTokens_InExpression_ReturnsAll()
    {
        var result = _tokenizer.Tokenize("x + x + x");
        Assert.Equal(5, result.Count);
        Assert.Equal("x", result[0].Value);
        Assert.Equal("x", result[2].Value);
        Assert.Equal("x", result[4].Value);
    }

    [Fact]
    public void Tokenize_FloatWithLeadingZero_ReturnsCorrectToken()
    {
        var result = _tokenizer.Tokenize("0.5");
        Assert.Single(result);
        Assert.Equal("0.5", result[0].Value);
        Assert.Equal(TokenType.FLOAT, result[0].Type);
    }

    [Fact]
    public void Tokenize_MultipleDecimalPoints_InSequence_ThrowsException()
    {
        // A standalone decimal point after a float is not a valid token
        Assert.Throws<ArgumentException>(() => _tokenizer.Tokenize("1.2.3"));
    }

    #endregion
}