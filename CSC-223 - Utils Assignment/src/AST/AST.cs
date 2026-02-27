/**
 * Defines AST node hierarchy including expressions and statements for
 * the parser/compiler assignments. Contains literal, variable, operator
 * and statement types along with utility methods for unparsing.
 *
 * Bugs: None known
 *
 * @author <your name>
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
        // Base class for all expression nodes
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
public abstract class Operator: ExpressionNode
    {
        // Base class for all operator nodes
    }

    /// <summary>
    /// Represents a binary operator with two operands (left/right).
    /// </summary>
public abstract class BinaryOperator : Operator
    {
        public ExpressionNode Left { get; } 
        public ExpressionNode Right { get; }

        // Constructor to initialize left and right operands
        /// <summary>
        /// Creates a binary operator node with the given child expressions.
        /// </summary>
        /// <param name="left">Left operand expression.</param>
        /// <param name="right">Right operand expression.</param>
        protected BinaryOperator(ExpressionNode left, ExpressionNode right) 
        {
            Left = left;
            Right = right;
        }
    }


    /// <summary>
    /// Expression node representing an integer literal value.
    /// </summary>
public class LiteralNode : ExpressionNode
    {
        public int Value { get; }

        // Constructor to initialize the literal value
        /// <summary>
        /// Initializes a new LiteralNode with the specified integer.
        /// </summary>
        /// <param name="value">Integer value of the literal.</param>
        public LiteralNode(int value) 
        {
            Value = value;
        }
        public override string Unparse(int level = 0)
        {
            // Return the numeric literal as text
            return Value.ToString();
        }
    }

    /// <summary>
    /// Expression node representing a variable identifier.
    /// </summary>
public class VariableNode : ExpressionNode
    {
        public string Name { get; }

        // Constructor to initialize the variable name
        /// <summary>
        /// Creates a VariableNode with the given name.
        /// </summary>
        /// <param name="name">Identifier name of the variable.</param>
        public VariableNode(string name) 
        {
            Name = name;
        }

        public override string Unparse(int level = 0)
        {
            return Name;
        }
    }

    /// <summary>
    /// Binary addition operator node.
    /// </summary>
public class PlusNode : BinaryOperator
    {
        public PlusNode(ExpressionNode left, ExpressionNode right) : base(left, right)
        { 
        }

        public override string Unparse(int level = 0)
        {
            return $"({Left.Unparse(level)} + {Right.Unparse(level)})";
        }
    }

    /// <summary>
    /// Binary subtraction operator node.
    /// </summary>
public class MinusNode : BinaryOperator
    {
        public MinusNode(ExpressionNode left, ExpressionNode right) : base(left, right)
        {
        }

        public override string Unparse(int level = 0)
        {
            return $"({Left.Unparse(level)} - {Right.Unparse(level)})";
        }
    }

    /// <summary>
    /// Binary multiplication operator node.
    /// </summary>
public class TimesNode : BinaryOperator
    {
        public TimesNode(ExpressionNode left, ExpressionNode right) : base(left, right)
        {
        }

        public override string Unparse(int level = 0)
        {
            return $"({Left.Unparse(level)} * {Right.Unparse(level)})";
        }
    }

    /// <summary>
    /// Binary floating-point division operator node.
    /// </summary>
public class FloatDivNode : BinaryOperator
    {
        public FloatDivNode(ExpressionNode left, ExpressionNode right) : base(left, right)
        {
        }

        public override string Unparse(int level = 0)
        {
            return $"({Left.Unparse(level)} / {Right.Unparse(level)})";
        }
    }

    /// <summary>
    /// Binary integer division operator node.
    /// </summary>
public class IntDivNode : BinaryOperator
    {
        public IntDivNode(ExpressionNode left, ExpressionNode right) : base(left, right)
        {
        }

        public override string Unparse(int level = 0)
        {
            return $"({Left.Unparse(level)} // {Right.Unparse(level)})";
        }
    }

    /// <summary>
    /// Binary modulus/remainder operator node.
    /// </summary>
public class ModulusNode : BinaryOperator
    {
        public ModulusNode(ExpressionNode left, ExpressionNode right) : base(left, right)
        {
        }

        public override string Unparse(int level = 0)
        {
            return $"({Left.Unparse(level)} % {Right.Unparse(level)})";
        }
    }

    /// <summary>
    /// Binary exponentiation operator node.
    /// </summary>
public class ExponentiationNode : BinaryOperator
    {
        public ExponentiationNode(ExpressionNode left, ExpressionNode right) : base(left, right)
        {
        }

        public override string Unparse(int level = 0)
        {
            return $"({Left.Unparse(level)} ** {Right.Unparse(level)})";
        }
    }
    #endregion

    #region Statement Node Classes
    /// <summary>
    /// Base class for all statement nodes such as assignments and returns.
    /// </summary>
public abstract class Statement
    {
        // Base class for all statement nodes
        public abstract string Unparse(int level = 0); 

        // GetIndentation utility method you wrote in assignment 0? Use it to create readable code that preserves 
        // the hierarchical relationship of program elements.
        /// <summary>
        /// Returns a string of spaces used for indentation based on level.
        /// </summary>
        /// <param name="level">Number of indentation levels</param>
        /// <returns>String containing level*4 spaces</returns>
        protected string GetIndentation(int level)
        {
        return new string(' ', level * 4); // 4 spaces per level
    }
    }

    /// <summary>
    /// Statement representing a block of other statements scoped by a symbol table.
    /// </summary>
public class BlockStmt : Statement
    {
        public SymbolTable<string, object> SymbolTable { get; }

        public BlockStmt(SymbolTable<string, object> symbolTable) 
        {
            SymbolTable = symbolTable;
        }

        // BlockStmt use level to indent their opening and closing curly braces, then increment level when calling Unparse on 
        // their children. 
        /// <summary>
        /// Produce unparsed text for the block, recursively unparsing contained statements.
        /// </summary>
        /// <param name="level">Indentation level for this block.</param>
        /// <returns>Formatted block string including braces and child statements.</returns>
        public override string Unparse(int level = 0)
        {
            string indent = GetIndentation(level);
            string childindent = GetIndentation(level + 1);

            // Start the result string with an opening curly brace and a newline, indented according to the current level.
            string result = $"{indent}{{\n";
            foreach (var entry in SymbolTable)
            {
                //  unparse it with the child indentation and add it to the result string.
                result += $"{childindent}{((Statement)entry.Value).Unparse(level + 1)}\n"; // had to autofix (include Statement)
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
        public VariableNode Variable { get; }
        public ExpressionNode Expression { get; }

        /// <summary>
        /// Constructs an assignment statement with a variable target and expression.
        /// </summary>
        /// <param name="variable">Variable node being assigned to.</param>
        /// <param name="expression">Expression node to assign.</param>
        public AssignmentStmt(VariableNode variable, ExpressionNode expression) 
        {
            Variable = variable;
            Expression = expression;
        }

        public override string Unparse(int level = 0)
        {
            // the string be indented according to the current level
            return $"{GetIndentation(level)}{Variable.Unparse(level)} = {Expression.Unparse(level)}";
        }
    }

public class ReturnStmt: Statement
    {
        public ExpressionNode Expression { get; }

        /// <summary>
        /// Creates a return statement wrapping the given expression.
        /// </summary>
        /// <param name="expression">Expression to return.</param>
        public ReturnStmt(ExpressionNode expression)
        {
            Expression = expression;
        }

        public override string Unparse(int level = 0)
        {
            //same thing as AssignmentStmt in terms of indentation
            return $"{GetIndentation(level)}return {Expression.Unparse(level)}";
        }
    }

    #endregion

}

