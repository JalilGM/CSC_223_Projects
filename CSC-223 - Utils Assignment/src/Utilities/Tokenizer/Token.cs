
/**
* Definitions for the tokens produced by the tokenizer and related constants.
*
* Contains the <see cref="TokenType"/> enum enumerating the different categories of
* tokens, a collection of string constants used by the tokenizer implementation,
* and the <see cref="Token"/> class itself which holds a value/type pair.
*
* Bugs: 
*
* @Jalil Garvin-Mingo
* @date 2026-02-20
*/
using System;

namespace Tokenizer;

/// <summary>
/// All possible categories of token recognized by the tokenizer.
/// </summary>
public enum TokenType
{
    VARIABLE,
    RETURN,

    INTEGER,
    FLOAT,

    OPERATOR,
    ASSIGNMENT,

    LEFT_PAREN,
    RIGHT_PAREN,
    LEFT_CURLY,
    RIGHT_CURLY,
}

/// <summary>
/// String constants used to compare against characters and operators when parsing.
/// </summary>
public class TokenConstants
{
    public const string RETURN = "return";

    public const string PLUS = "+";
    public const string MINUS = "-";
    public const string TIMES = "*";
    public const string FLOAT_DIVISION = "/";
    public const string INTEGER_DIVISION = "//";
    public const string MODULUS = "%";
    public const string EXPONENTIATION = "**";
    public const string ASSIGNMENT = ":=";
    public const string DECIMAL_POINT = ".";

    public const string LEFT_PAREN = "(";
    public const string RIGHT_PAREN = ")";
    public const string LEFT_CURLY = "{";
    public const string RIGHT_CURLY = "}";
}

/// <summary>
/// Represents a single token produced by the tokenizer.  Equality is defined by
/// comparing both the <see cref="Value"/> and <see cref="Type"/>.
/// </summary>
public class Token
{
    /// <summary>Textual value of the token (e.g. "42", "x", "+").</summary>
    public string? Value { get; set; }

    /// <summary>Category of this token.</summary>
    public TokenType Type { get; set; }

    /// <summary>
    /// Construct a token with the given value and type.
    /// </summary>
    /// <param name="value">The literal string from the source text.</param>
    /// <param name="type">The category of token.</param>
    public Token(string? value, TokenType type)
    {
        Value = value;
        Type = type;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"({Value}, {Type})";
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is Token token)
        {
            return Value == token.Value && Type == token.Type;
        }
        return false;
    }
}
