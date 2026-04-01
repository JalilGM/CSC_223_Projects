using System;
using System.Collections.Generic;
using System.Linq;
using AST;
using Tokenizer;
using Containers;

/**
* Parser for the DEC language. Converts tokenized source code into an AST.
*
* The DEC language supports: 
* - Expressions: parenthesised variables, literals, and binary operations.
* - Statements: variable assignment (':='), return statements, and nested blocks.
* The parser performs recursive descent parsing, with separate methods for expressions, statements, 
* and blocks. It also maintains a symbol table for variable declarations.
*
* Bugs: The expression parsing logic previously did not handle missing closing parentheses correctly, 
*       resulting in parsing errors when parentheses were mismatched. 
*
* @Jalil Garvin-Mingo
* @date 2026-03-30
*/

// The DEC language syntax is defined as follows:
/// 
/// Grammar for DEC language expressions:
/// expr       ::= '(' variable ')' | '(' literal ')' | '(' expr binary_op expr ')'
/// binary_op  ::= '+' | '-' | '*' | '/' | '//' | '%' | '**'
/// variable   ::= [a-z]+
/// literal    ::= integer | float
/// integer    ::= [0-9]+
/// float      ::= [0-9]+ '.' [0-9]+
/// 
/// Grammar for DEC language statements:
/// stmt        ::= assign_stmt | return_stmt | block_stmt
/// assign_stmt ::= variable ':=' expr
/// return_stmt ::= 'return' expr
/// block_stmt  ::= '{' stmt* '}'
///
/// Programs must start with '{' and end with '}'

namespace Parser
{
    /// <summary>
    /// Custom exception for parsing errors.
    /// </summary>
    public class ParseException : Exception
    {
        public ParseException(string message) : base(message) { }
    }

    /// <summary>
    /// Static parser class for the DEC language. Entry point is <see cref="Parse"/>.
    /// All methods are static because no instance state is required.
    /// </summary>
    public static class Parser
    {
        /// <summary>
        /// Parses a complete DEC program text and returns the root <see cref="BlockStmt"/>.
        /// </summary>
        /// <param name="program">Source text of the program.</param>
        /// <returns>AST root as a <see cref="BlockStmt"/>.</returns>
        /// <exception cref="ParseException">
        /// Thrown when the program does not begin with '{'.
        /// </exception>
        public static AST.BlockStmt Parse(string program)
        {
            // Split the program into trimmed, non-empty lines.
            var lines = program
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(l => l.Trim())
                .Where(l => l.Length > 0)
                .ToList();

            // Validate that the program starts with '{'.
            if (lines.Count == 0 || lines[0] != "{")
                throw new ParseException("Program must start with '{'.");

            var symbolTable = new SymbolTable<string, object>();
            return ParseBlockStmt(lines, symbolTable);
        }

        // ------------------------------------------------------------------ //
        //  Expression parsing
        // ------------------------------------------------------------------ //

        /// <summary>
        /// Parses an expression enclosed in parentheses from the token list.
        /// Consumes the opening '(' and the matching closing ')'.
        /// Supports:
        ///   (variable), (literal), (expr binary_op expr)
        /// where each operand may itself be a parenthesised expression.
        /// </summary>
        /// <param name="tokens">Mutable token list; tokens are removed as consumed.</param>
        /// <returns>The parsed <see cref="ExpressionNode"/>.</returns>
        /// <exception cref="ParseException">
        /// Thrown when the expression does not begin with '(' or does not end with ')'.
        /// </exception>
        private static AST.ExpressionNode ParseExpression(List<Token> tokens)
        {
            // Every expression must start with '('.
            if (tokens.Count == 0 || tokens[0].Type != TokenType.LEFT_PAREN)
                throw new ParseException("must begin with a (");

            tokens.RemoveAt(0); // consume '('

            // Peek: if the next token is another '(', recurse to obtain the left operand.
            AST.ExpressionNode left;
            if (tokens.Count > 0 && tokens[0].Type == TokenType.LEFT_PAREN)
            {
                left = ParseExpression(tokens);
            }
            else
            {
                // Parse a single token (variable or literal) as the content.
                left = ParseExpressionContent(tokens);
            }

            // After the left side, expect either ')' (single-value expression) or an operator.
            if (tokens.Count == 0)
                throw new ParseException("Expression must end with a ')'.");

            if (tokens[0].Type == TokenType.RIGHT_PAREN)
            {
                tokens.RemoveAt(0); // consume ')'
                return left;
            }

            // There should be a binary operator next.
            if (tokens[0].Type != TokenType.OPERATOR)
                throw new ParseException(
                    $"Expected a binary operator or ')', but found '{tokens[0].Value}'.");

            string op = tokens[0].Value!;
            tokens.RemoveAt(0); // consume operator

            // Parse the right operand; it may be a nested expression or a single token.
            AST.ExpressionNode right;
            if (tokens.Count > 0 && tokens[0].Type == TokenType.LEFT_PAREN)
            {
                right = ParseExpression(tokens);
            }
            else
            {
                right = ParseExpressionContent(tokens);
            }

            // Expect closing ')'.
            if (tokens.Count == 0 || tokens[0].Type != TokenType.RIGHT_PAREN)
                throw new ParseException("must end with a )");

            tokens.RemoveAt(0); // consume ')'

            return CreateBinaryOperatorNode(op, left, right);
        }

        /// <summary>
        /// Parses the content of an expression: a single variable or literal token.
        /// Consumes exactly one token.
        /// </summary>
        /// <param name="tokens">Mutable token list.</param>
        /// <returns>A <see cref="LiteralNode"/> or <see cref="VariableNode"/>.</returns>
        /// <exception cref="ParseException">
        /// Thrown when the token list is empty or the token is not a valid expression atom.
        /// </exception>
        private static AST.ExpressionNode ParseExpressionContent(List<Token> tokens)
        {
            if (tokens.Count == 0)
                throw new ParseException("Unexpected end of expression.");

            var token = tokens[0];

            if (token.Type == TokenType.INTEGER)
            {
                tokens.RemoveAt(0);
                return new AST.LiteralNode(int.Parse(token.Value!));
            }
            else if (token.Type == TokenType.FLOAT)
            {
                tokens.RemoveAt(0);
                return new AST.LiteralNode(double.Parse(token.Value!,
                    System.Globalization.CultureInfo.InvariantCulture));
            }
            else if (token.Type == TokenType.VARIABLE)
            {
                tokens.RemoveAt(0);
                return new AST.VariableNode(token.Value!);
            }
            else if (token.Type == TokenType.RIGHT_PAREN)
            {
                // Empty parentheses or trailing junk—caught by caller.
                throw new ParseException("Unexpected ')': expression content is missing.");
            }
            else
            {
                throw new ParseException(
                    $"Unexpected token in expression: '{token.Value}'.");
            }
        }

        /// <summary>
        /// Handles a single-token expression (variable or integer/float literal).
        /// </summary>
        /// <param name="token">The single token to evaluate.</param>
        /// <returns>The corresponding expression node.</returns>
        /// <exception cref="ParseException">Thrown when the token type is invalid.</exception>
        private static AST.ExpressionNode HandleSingleToken(Token token)
        {
            if (token.Type == TokenType.INTEGER)
                return new AST.LiteralNode(int.Parse(token.Value!));

            if (token.Type == TokenType.FLOAT)
                return new AST.LiteralNode(double.Parse(token.Value!,
                    System.Globalization.CultureInfo.InvariantCulture));

            if (token.Type == TokenType.VARIABLE)
                return new AST.VariableNode(token.Value!);

            throw new ParseException($"Unexpected token: '{token.Value}'.");
        }

        /// <summary>
        /// Creates the binary operator AST node that corresponds to the given operator string.
        /// </summary>
        /// <param name="op">Operator symbol (e.g. "+", "//").</param>
        /// <param name="left">Left operand node.</param>
        /// <param name="right">Right operand node.</param>
        /// <returns>The appropriate <see cref="BinaryOperator"/> subclass instance.</returns>
        /// <exception cref="ParseException">Thrown for unknown operators.</exception>
        private static AST.ExpressionNode CreateBinaryOperatorNode(
            string op, AST.ExpressionNode left, AST.ExpressionNode right)
        {
            return op switch
            {
                "+"  => new AST.PlusNode(left, right),
                "-"  => new AST.MinusNode(left, right),
                "*"  => new AST.TimesNode(left, right),
                "/"  => new AST.FloatDivNode(left, right),
                "//" => new AST.IntDivNode(left, right),
                "%"  => new AST.ModulusNode(left, right),
                "**" => new AST.ExponentiationNode(left, right),
                _    => throw new ParseException($"Invalid operator")
            };
        }

        /// <summary>
        /// Validates the given name and returns a new <see cref="VariableNode"/>.
        /// </summary>
        /// <param name="name">Variable name string.</param>
        /// <returns>A <see cref="VariableNode"/> for the name.</returns>
        /// <exception cref="ParseException">Thrown when the name is null or empty.</exception>
        private static AST.VariableNode ParseVariableNode(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ParseException("Invalid variable name.");

            return new AST.VariableNode(name);
        }

        // ------------------------------------------------------------------ //
        //  Statement parsing
        // ------------------------------------------------------------------ //

        /// <summary>
        /// Parses an assignment statement of the form:  variable ':=' expr
        /// Also adds the variable to the supplied symbol table with a null value.
        /// </summary>
        /// <param name="tokens">Token list for the line (already tokenized).</param>
        /// <param name="symbolTable">Current scope's symbol table.</param>
        /// <returns>The parsed <see cref="AssignmentStmt"/>.</returns>
        /// <exception cref="ParseException">
        /// Thrown when the variable name is invalid or the ':=' operator is missing.
        /// </exception>
        private static AST.AssignmentStmt ParseAssignmentStmt(
            List<Token> tokens, SymbolTable<string, object> symbolTable)
        {
            // First token must be a VARIABLE.
            if (tokens.Count == 0 || tokens[0].Type != TokenType.VARIABLE)
                throw new ParseException(
                    $"Invalid variable name: '{(tokens.Count > 0 ? tokens[0].Value : "<empty>")}'.");

            var varNode = ParseVariableNode(tokens[0].Value!);
            tokens.RemoveAt(0);

            // Second token must be ':='; one token after variable is invalid.
            if (tokens.Count == 0)
                throw new ParseException("Invalid token");

            if (tokens[0].Type != TokenType.ASSIGNMENT)
                throw new ParseException(
                    $"Expected assignment operator ':=', but found " +
                    $"'{tokens[0].Value}'");

            tokens.RemoveAt(0); // consume ':='

            if (tokens.Count == 1)
                throw new ParseException("Invalid token");

            // Remaining tokens form the expression.
            if (tokens.Count == 0)
                throw new ParseException("Assignment statement is missing an expression.");

            var expr = ParseExpression(tokens);

            // Register the variable in the current scope (value is unknown at parse time).
            symbolTable[varNode.Name] = null;

            return new AST.AssignmentStmt(varNode, expr);
        }

        /// <summary>
        /// Parses a return statement of the form:  'return' expr
        /// </summary>
        /// <param name="tokens">Token list for the line.</param>
        /// <returns>The parsed <see cref="ReturnStmt"/>.</returns>
        /// <exception cref="ParseException">
        /// Thrown when the RETURN keyword is missing or the expression is absent/malformed.
        /// </exception>
        private static AST.ReturnStmt ParseReturnStatement(List<Token> tokens)
        {
            // The leading RETURN token is expected but we just skip it if present.
            if (tokens.Count > 0 && tokens[0].Type == TokenType.RETURN)
                tokens.RemoveAt(0);

            // There must be an expression after 'return'.
            if (tokens.Count == 0)
                throw new ParseException(
                    "Return statement is missing expression after 'return'.");

            var expr = ParseExpression(tokens);
            return new AST.ReturnStmt(expr);
        }

        /// <summary>
        /// Dispatches to <see cref="ParseAssignmentStmt"/> or <see cref="ParseReturnStatement"/>
        /// based on the leading token of the line.
        /// </summary>
        /// <param name="tokens">Tokenized line.</param>
        /// <param name="symbolTable">Current scope's symbol table.</param>
        /// <returns>The parsed <see cref="Statement"/>.</returns>
        /// <exception cref="ParseException">Thrown for unrecognised statement types.</exception>
        private static AST.Statement ParseStatement(
            List<Token> tokens, SymbolTable<string, object> symbolTable)
        {
            if (tokens.Count == 0)
                throw new ParseException("Empty statement.");

            return tokens[0].Type switch
            {
                TokenType.VARIABLE => ParseAssignmentStmt(tokens, symbolTable),
                TokenType.RETURN   => ParseReturnStatement(tokens),
                _ => throw new ParseException(
                         $"Unknown statement starting with '{tokens[0].Value}'.")
            };
        }

        // ------------------------------------------------------------------ //
        //  Block parsing
        // ------------------------------------------------------------------ //

        /// <summary>
        /// Parses statements from <paramref name="lines"/> into <paramref name="blockStmt"/>
        /// until a lone '}' line is reached (which is NOT consumed by this method).
        /// Nested blocks starting with '{' are handled by recursive calls to
        /// <see cref="ParseBlockStmt"/>.
        /// </summary>
        /// <param name="lines">
        /// Mutable list of remaining source lines. Lines are removed from the front as consumed.
        /// </param>
        /// <param name="blockStmt">The block statement to populate.</param>
        /// <exception cref="ParseException">
        /// Thrown when the source ends unexpectedly or an invalid line is encountered.
        /// </exception>
        private static void ParseStmtList(List<string> lines, AST.BlockStmt blockStmt)
        {
            var tokenizer = new TokenizerImpl();

            while (true)
            {
                if (lines.Count == 0)
                    throw new ParseException(
                        "Unexpected end of program: missing closing '}'.");

                string line = lines[0];

                // A lone '}' signals the end of this block; leave it for the caller.
                if (line == "}")
                    return;

                // A lone '{' starts a nested block.
                if (line == "{")
                {
                    var childSymbolTable =
                        new SymbolTable<string, object>(blockStmt.SymbolTable);
                    var nestedBlock = ParseBlockStmt(lines, childSymbolTable);
                    blockStmt.AddStatement(nestedBlock);
                    continue;
                }

                // Otherwise tokenize and parse as a statement.
                lines.RemoveAt(0);
                List<Token> tokens = tokenizer.Tokenize(line);

                if (tokens.Count == 0)
                    continue;

                // A line beginning with '{' after tokenization is also a nested block opener,
                // but we handle raw-line detection above; if we get here the token check is
                // a safety net for odd whitespace situations.
                if (tokens[0].Type == TokenType.LEFT_CURLY)
                {
                    // Re-insert the line-equivalent and let the loop handle it.
                    lines.Insert(0, "{");
                    continue;
                }

                var stmt = ParseStatement(tokens, blockStmt.SymbolTable);
                blockStmt.AddStatement(stmt);
            }
        }

        /// <summary>
        /// Parses a block statement beginning with '{' and ending with '}'.
        /// Validates that the first line is exactly '{' and the last consumed line is exactly '}'.
        /// </summary>
        /// <param name="lines">
        /// Mutable list of source lines. The '{' and '}' delimiters are consumed.
        /// </param>
        /// <param name="symbolTable">Symbol table for this block's scope.</param>
        /// <returns>The fully parsed <see cref="BlockStmt"/>.</returns>
        /// <exception cref="ParseException">
        /// Thrown when the block does not begin with '{' or end with '}'.
        /// </exception>
        private static AST.BlockStmt ParseBlockStmt(
            List<string> lines, SymbolTable<string, object> symbolTable)
        {
            if (lines.Count == 0)
                throw new ParseException("Block must begin with '{', but no lines remain.");

            // ---- Validate and consume the opening '{' line ----
            var tokenizer = new TokenizerImpl();
            string firstLine = lines[0];
            var firstTokens = tokenizer.Tokenize(firstLine);

            if (firstTokens.Count == 0 || firstTokens[0].Type != TokenType.LEFT_CURLY)
                throw new ParseException(
                    $"Block must begin with '{{', but found '{firstLine}'.");

            if (firstTokens.Count != 1)
                throw new ParseException(
                    $"Expected 1 token ('{{}}') on opening line, but found {firstTokens.Count}.");

            lines.RemoveAt(0); // consume '{'

            // ---- Build the block and fill it with statements ----
            var block = new AST.BlockStmt(symbolTable);
            ParseStmtList(lines, block);

            // ---- Validate and consume the closing '}' line ----
            if (lines.Count == 0)
                throw new ParseException(
                    "Block is missing its closing '}'.");

            string lastLine = lines[0];
            var lastTokens = tokenizer.Tokenize(lastLine);

            if (lastTokens.Count == 0 || lastTokens[0].Type != TokenType.RIGHT_CURLY)
                throw new ParseException(
                    $"Invalid token at end of block: expected '}}', but found '{lastLine}'.");

            if (lastTokens.Count != 1)
                throw new ParseException(
                    $"Expected only '}}' on closing line, but found extra tokens: '{lastLine}'.");

            lines.RemoveAt(0); // consume '}'

            return block;
        }
    }
}