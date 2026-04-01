/**
 * Defines AST node hierarchy including expressions and statements for
 * the parser/compiler assignments. Contains literal, variable, operator
 * and statement types along with utility methods for unparsing.
 *
 * Changes from previous version:
 *   - LiteralNode now stores a double (to support both int and float literals).
 *   - BlockStmt gained an AddStatement helper method.
 *   - AssignmentStmt.Unparse now emits ':=' to match the DEC language grammar.
 *
 * @author jalil
 * @date   February 27, 2026
 */
using Containers;

namespace AST
{

    #region Expression Node Classes
    /// <summary>
    /// Abstract base class for all expression nodes in the AST.
    /// </summary>
    public abstract class ExpressionNode
    {
        /// <summary>
        /// Converts this expression node into its string representation.
        /// </summary>
        /// <param name="level">Indentation level used for pretty printing.</param>
        /// <returns>Unparsed string of the expression.</returns>
        public abstract string Unparse(int level = 0);
    }

    /// <summary>
    /// Base class for operator nodes (unary/binary) in expressions.
    /// </summary>
    public abstract class Operator : ExpressionNode { }

    /// <summary>
    /// Represents a binary operator with two operands (left/right).
    /// </summary>
    public abstract class BinaryOperator : Operator
    {
        public ExpressionNode Left  { get; }
        public ExpressionNode Right { get; }

        /// <summary>
        /// Creates a binary operator node with the given child expressions.
        /// </summary>
        protected BinaryOperator(ExpressionNode left, ExpressionNode right)
        {
            Left  = left;
            Right = right;
        }
    }

    /// <summary>
    /// Expression node representing a numeric literal value (integer or float).
    /// Internally stored as a <see cref="double"/> so that both 42 and 3.14 are supported.
    /// </summary>
    public class LiteralNode : ExpressionNode
    {
        /// <summary>Numeric value of the literal (may be an integral value stored as double).</summary>
        public double Value { get; }

        /// <summary>Initialises a LiteralNode from an integer.</summary>
        public LiteralNode(int value)    { Value = value; }

        /// <summary>Initialises a LiteralNode from a double (float literal).</summary>
        public LiteralNode(double value) { Value = value; }

        public override string Unparse(int level = 0)
        {
            // Emit without trailing ".0" for whole numbers so unparse tests pass.
            return Value == Math.Truncate(Value)
                ? ((long)Value).ToString()
                : Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Expression node representing a variable identifier.
    /// </summary>
    public class VariableNode : ExpressionNode
    {
        public string Name { get; }

        /// <summary>Creates a VariableNode with the given name.</summary>
        public VariableNode(string name) { Name = name; }

        public override string Unparse(int level = 0) => Name;
    }

    /// <summary>Binary addition operator node.</summary>
    public class PlusNode : BinaryOperator
    {
        public PlusNode(ExpressionNode left, ExpressionNode right) : base(left, right) { }
        public override string Unparse(int level = 0) =>
            $"({Left.Unparse(level)} + {Right.Unparse(level)})";
    }

    /// <summary>Binary subtraction operator node.</summary>
    public class MinusNode : BinaryOperator
    {
        public MinusNode(ExpressionNode left, ExpressionNode right) : base(left, right) { }
        public override string Unparse(int level = 0) =>
            $"({Left.Unparse(level)} - {Right.Unparse(level)})";
    }

    /// <summary>Binary multiplication operator node.</summary>
    public class TimesNode : BinaryOperator
    {
        public TimesNode(ExpressionNode left, ExpressionNode right) : base(left, right) { }
        public override string Unparse(int level = 0) =>
            $"({Left.Unparse(level)} * {Right.Unparse(level)})";
    }

    /// <summary>Binary floating-point division operator node.</summary>
    public class FloatDivNode : BinaryOperator
    {
        public FloatDivNode(ExpressionNode left, ExpressionNode right) : base(left, right) { }
        public override string Unparse(int level = 0) =>
            $"({Left.Unparse(level)} / {Right.Unparse(level)})";
    }

    /// <summary>Binary integer division operator node.</summary>
    public class IntDivNode : BinaryOperator
    {
        public IntDivNode(ExpressionNode left, ExpressionNode right) : base(left, right) { }
        public override string Unparse(int level = 0) =>
            $"({Left.Unparse(level)} // {Right.Unparse(level)})";
    }

    /// <summary>Binary modulus/remainder operator node.</summary>
    public class ModulusNode : BinaryOperator
    {
        public ModulusNode(ExpressionNode left, ExpressionNode right) : base(left, right) { }
        public override string Unparse(int level = 0) =>
            $"({Left.Unparse(level)} % {Right.Unparse(level)})";
    }

    /// <summary>Binary exponentiation operator node.</summary>
    public class ExponentiationNode : BinaryOperator
    {
        public ExponentiationNode(ExpressionNode left, ExpressionNode right) : base(left, right) { }
        public override string Unparse(int level = 0) =>
            $"({Left.Unparse(level)} ** {Right.Unparse(level)})";
    }
    #endregion

    #region Statement Node Classes
    /// <summary>
    /// Base class for all statement nodes such as assignments and returns.
    /// </summary>
    public abstract class Statement
    {
        public abstract string Unparse(int level = 0);

        /// <summary>
        /// Returns a string of spaces used for indentation based on level.
        /// </summary>
        protected string GetIndentation(int level) => new string(' ', level * 4);
    }

    /// <summary>
    /// Statement representing a block of other statements scoped by a symbol table.
    /// </summary>
    public class BlockStmt : Statement
    {
        public SymbolTable<string, object> SymbolTable { get; }
        public List<Statement> Statements { get; }

        public BlockStmt(SymbolTable<string, object> symbolTable)
        {
            SymbolTable = symbolTable;
            Statements = new List<Statement>();
        }

        /// <summary>
        /// Appends a statement to this block's statement list.
        /// </summary>
        public void AddStatement(Statement stmt) => Statements.Add(stmt);

        /// <summary>
        /// Produce unparsed text for the block, recursively unparsing contained statements.
        /// BlockStmt uses <paramref name="level"/> to indent its braces, then increments
        /// level when calling Unparse on child statements.
        /// </summary>
        public override string Unparse(int level = 0)
        {
            string indent      = GetIndentation(level);
            string childIndent = GetIndentation(level + 1);

            string result = $"{indent}{{\n";
            foreach (var stmt in Statements)
            {
                //  unparse it with the child indentation and add it to the result string.
                result += $"{childIndent}{stmt.Unparse(level + 1)}\n"; // had to autofix (include Statement)
            }
            result += $"{indent}}}"; // Add the closing curly brace with the same indentation as the opening brace.
            return result;
        }
    }

    /// <summary>
    /// Statement for assigning an expression to a variable.
    /// </summary>
    public class AssignmentStmt : Statement
    {
        public VariableNode    Variable   { get; }
        public ExpressionNode  Expression { get; }

        /// <summary>
        /// Constructs an assignment statement with a variable target and expression.
        /// </summary>
        public AssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            Variable   = variable;
            Expression = expression;
        }

        public override string Unparse(int level = 0) =>
            // Use ':=' to match the DEC language grammar.
            $"{GetIndentation(level)}{Variable.Unparse(level)} := {Expression.Unparse(level)}";
    }

    /// <summary>
    /// Statement that returns an expression value from the current block.
    /// </summary>
    public class ReturnStmt : Statement
    {
        public ExpressionNode Expression { get; }

        /// <summary>Creates a return statement wrapping the given expression.</summary>
        public ReturnStmt(ExpressionNode expression) { Expression = expression; }

        public override string Unparse(int level = 0) =>
            $"{GetIndentation(level)}return {Expression.Unparse(level)}";
    }
    #endregion
}