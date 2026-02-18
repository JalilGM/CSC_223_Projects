
using System;

namespace Tokenizer;

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

public class Token
{
    public string? Value { get; set; }
    public TokenType Type { get; set; }

    public Token(string? value, TokenType type)
    {
        Value = value;
        Type = type;
    }

    public override string ToString()
    {
        return $"({Value}, {Type})";
    }

    public override bool Equals(object? obj)
    {
        if (obj is Token token)
        {
            return Value == token.Value && Type == token.Type;
        }
        return false;
    }

}