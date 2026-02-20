
using System.Numerics;

namespace Tokenizer;

public class TokenizerImpl
{
    public List<Token> Tokenize(string input)
    {
        List<Token> tokens = new List<Token>();

        int index = 0;
        string[] operators = { TokenConstants.PLUS, TokenConstants.MINUS, TokenConstants.TIMES, TokenConstants.FLOAT_DIVISION, TokenConstants.INTEGER_DIVISION, TokenConstants.MODULUS, TokenConstants.EXPONENTIATION };
        string[] assignments = { TokenConstants.ASSIGNMENT };
        string[] seperators = { TokenConstants.LEFT_PAREN, TokenConstants.RIGHT_PAREN, TokenConstants.LEFT_CURLY, TokenConstants.RIGHT_CURLY };

        while (index < input.Length)
        {
            char c = input[index];
            

            if (char.IsWhiteSpace(c))
            {
                index++;
            }
            else if (char.IsDigit(c))
            {
                tokens.Add(HandleNumeber(input, ref index));
            }
            else if (char.IsLetter(c))
            {
                tokens.Add(HandleVariable(input, ref index));
            }
            else if (operators.Contains(c.ToString()))
            {
                tokens.Add(HandleOperator(input, ref index));
            }
            else if (assignments.Contains(c.ToString()))
            {
                tokens.Add(HandleAssignment(input, ref index));
            }
            else if (seperators.Contains(c.ToString()))
            {
                tokens.Add(HandleSeperator(input, ref index));
            }
            else
            {
                throw new ArgumentException($"Unexpected character: {c}");

            }
        }
        return tokens;
    }

    private Token HandleNumeber(string input, ref int index)
    {
        string number = "";

        while (index < input.Length && char.IsDigit(input[index])) // Handle multi-digit numbers
        {
            number += input[index]; 
            index++;
        }

        if (index < input.Length && input[index] == '.') // Handle decimal point for floats
        {
            number += input[index];
            index++;

            while (index < input.Length && char.IsDigit(input[index])) // digits after decimal point
            {
                number += input[index];
                index++;
            }

            return new Token(number, TokenType.FLOAT);
        }
        return new Token(number, TokenType.INTEGER);
    }

    private Token HandleVariable(string input, ref int index)
    {
        string variable = "";

        while (index < input.Length && char.IsLetter(input[index])) // Handle multi-letter variables
        {
            variable += input[index];
            index++;
        }

        if (variable == TokenConstants.RETURN) // check for "return" keyword
        {
            return new Token(variable, TokenType.RETURN);
        }
        return new Token(variable, TokenType.VARIABLE);
    }

    private Token HandleOperator(string input, ref int index)
    {
        string oper = input[index].ToString();

        if (index + 1 < input.Length) // Check for two-character operators
        {
            string twocharoper = oper + input[index + 1];

            if (twocharoper == TokenConstants.INTEGER_DIVISION || twocharoper == TokenConstants.EXPONENTIATION)
            {
                index += 2; // Skip both characters for two-character operator
                return new Token(twocharoper, TokenType.OPERATOR);
            }
        }

        // single-character operator
        index++;
        return new Token(oper, TokenType.OPERATOR);

        throw new ArgumentException($"Unexpected operator: {oper}");
    }

    private Token HandleAssignment(string input, ref int index)
    {
        string assign = input[index].ToString();
        // Check for two-character just assignment operator, since the only assignment operator is ":="
        if (index + 1 < input.Length) 
        {
            string twocharassign = assign + input[index + 1];

            if (twocharassign == TokenConstants.ASSIGNMENT)
            {
                index += 2; // Skip both characters for two-character assignment operator
                return new Token(twocharassign, TokenType.ASSIGNMENT);
            }
        }
        throw new ArgumentException($"Unexpected assignment operator: {assign}");        
    }

    private Token HandleSeperator(string input, ref int index)
    {
        char seperator = input[index];
        index++;
        
        switch (seperator) //switch statement to determine which seperator it is and return the appropriate token
        {
            case '(':
                return new Token(seperator.ToString(), TokenType.LEFT_PAREN);
            case ')':
                return new Token(seperator.ToString(), TokenType.RIGHT_PAREN);
            case '{':
                return new Token(seperator.ToString(), TokenType.LEFT_CURLY);
            case '}':
                return new Token(seperator.ToString(), TokenType.RIGHT_CURLY);
            default:
                throw new ArgumentException($"Unexpected seperator: {seperator}");
        }
    }

}