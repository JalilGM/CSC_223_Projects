
/**
* Implements a simple tokenizer for a subset of a programming language.
*
* The tokenizer breaks an input string into a sequence of {Token} objects
* representing integers, floats, variables, operators, separators, and keywords.
* It is intentionally small and educational, demonstrating manual scanning and
* handling of multi-character tokens.
*
* Bugs: The assignment detection logic previously handled ":" incorrectly, which
*       resulted in errors for the ":=" operator. Added more thorough comments.
*
* @Jalil Garvin-Mingo
* @date 2026-02-20
*/
using System.Numerics;

namespace Tokenizer;

public class TokenizerImpl
{
    /// <summary>
    /// Converts an input string into a list of tokens.
    /// </summary>
    /// <param name="input">Source text to tokenize.
    /// May contain numbers, identifiers, operators, and punctuation.</param>
    /// <returns>A list of <see cref="Token"/> instances in the order they appear.</returns>
    /// <exception cref="ArgumentException">Thrown when an unexpected character is
    /// encountered.</exception>
    public List<Token> Tokenize(string input)
    {
        List<Token> tokens = new List<Token>();

        int index = 0;
        // arrays used to recognize single-character tokens quickly
        string[] operators = { TokenConstants.PLUS, TokenConstants.MINUS, TokenConstants.TIMES, TokenConstants.FLOAT_DIVISION, TokenConstants.INTEGER_DIVISION, TokenConstants.MODULUS, TokenConstants.EXPONENTIATION };
        string[] assignments = { TokenConstants.ASSIGNMENT };
        string[] seperators = { TokenConstants.LEFT_PAREN, TokenConstants.RIGHT_PAREN, TokenConstants.LEFT_CURLY, TokenConstants.RIGHT_CURLY };

        // iterate through the entire input string
        while (index < input.Length)
        {
            char c = input[index];
            
            if (char.IsWhiteSpace(c))
            {
                // skip over whitespace characters
                index++;
            }
            else if (char.IsDigit(c))
            {
                // number literal (integer or float)
                tokens.Add(HandleNumeber(input, ref index));
            }
            else if (char.IsLetter(c))
            {
                // identifier or keyword
                tokens.Add(HandleVariable(input, ref index));
            }
            else if (operators.Contains(c.ToString()))
            {
                // arithmetic operator
                tokens.Add(HandleOperator(input, ref index));
            }
            else if (assignments.Contains(c.ToString()))
            {
                // assignment operator (only ":=")
                tokens.Add(HandleAssignment(input, ref index));
            }
            else if (seperators.Contains(c.ToString()))
            {
                // parentheses or curly braces
                tokens.Add(HandleSeperator(input, ref index));
            }
            else
            {
                // character not recognized by the tokenizer rules
                throw new ArgumentException($"Unexpected character: {c}");
            }
        }
        return tokens;
    }

    #region Helper routines for specific token types
    // these methods are private and only called by Tokenize

    /// <summary>
    /// Scans a number literal from the current index. Handles integers and floats
    /// by looking for a decimal point followed by digits.
    /// </summary>
    /// <param name="input">The complete input string being tokenized.</param>
    /// <param name="index">Reference to the current scanning position; this
    /// method advances it to the first character after the token.</param>
    /// <returns>A <see cref="Token"/> representing either an INTEGER or FLOAT.</returns>
    private Token HandleNumeber(string input, ref int index)
    {
        string number = "";

        // collect leading digits
        while (index < input.Length && char.IsDigit(input[index])) // Handle multi-digit numbers
        {
            number += input[index]; 
            index++;
        }

        // check for decimal part to classify as float
        if (index < input.Length && input[index] == '.') // Handle decimal point for floats
        {
            number += input[index];
            index++;

            // digits after the decimal point
            while (index < input.Length && char.IsDigit(input[index])) // digits after decimal point
            {
                number += input[index];
                index++;
            }

            return new Token(number, TokenType.FLOAT);
        }
        return new Token(number, TokenType.INTEGER);
    }

    /// <summary>
    /// Recognizes an identifier or the "return" keyword. Identifiers consist of
    /// letters only.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <param name="index">Current position reference, which will be advanced past
    /// the identifier.</param>
    /// <returns>A VARIABLE token or RETURN token for the keyword.</returns>
    private Token HandleVariable(string input, ref int index)
    {
        string variable = "";

        // gather consecutive letters
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

    /// <summary>
    /// Parses an operator token. Supports both single-character symbols and the
    /// two-character forms "//" and "**" for integer division and exponentiation.
    /// </summary>
    /// <param name="input">Input string to scan.</param>
    /// <param name="index">Reference to current index; advanced past operator.</param>
    /// <returns>An OPERATOR token.</returns>
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

    /// <summary>
    /// Handles assignment tokens. The only valid assignment symbol is ":=".
    /// </summary>
    /// <param name="input">Full input string.</param>
    /// <param name="index">Index passed by reference; moved past the token.</param>
    /// <returns>An ASSIGNMENT token.</returns>
    /// <exception cref="ArgumentException">Thrown if a lone ':' is encountered.</exception>
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

    /// <summary>
    /// Recognizes separators like parentheses and curly braces.
    /// </summary>
    /// <param name="input">Source text.</param>
    /// <param name="index">Reference index; advanced one character.</param>
    /// <returns>A token representing the separator.</returns>
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

    #endregion

}