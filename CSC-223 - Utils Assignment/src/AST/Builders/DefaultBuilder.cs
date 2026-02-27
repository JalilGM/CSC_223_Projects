/**
 * Factory class for constructing concrete AST nodes.  Each method
 * corresponds to a type of node and returns a fresh instance.
 *
 * Bugs: None known
 *
 * @author <your name>
 * @date   February 27, 2026
 */

using Containers;

namespace AST
{
    /// <summary>
    /// Default implementation of a builder that constructs real AST node objects.
    /// </summary>
    public class DefaultBuilder
    {
        #region Factory methods
        /// <summary>
        /// Create a plus operator node with the specified operands.
        /// </summary>
        /// <param name="left">Left operand expression.</param>
        /// <param name="right">Right operand expression.</param>
        /// <returns>A new <see cref="PlusNode"/> instance.</returns>
        public virtual PlusNode CreatePlusNode(ExpressionNode left, ExpressionNode right)
        {
            return new PlusNode(left, right);
        }

        /// <summary>
        /// Create a minus operator node with the specified operands.
        /// </summary>
        /// <param name="left">Left operand expression.</param>
        /// <param name="right">Right operand expression.</param>
        /// <returns>A new <see cref="MinusNode"/> instance.</returns>
        public virtual MinusNode CreateMinusNode(ExpressionNode left, ExpressionNode right)
        {
            return new MinusNode(left, right);
        }

        /// <summary>
        /// Create a times operator node with the specified operands.
        /// </summary>
        /// <param name="left">Left operand expression.</param>
        /// <param name="right">Right operand expression.</param>
        /// <returns>A new <see cref="TimesNode"/> instance.</returns>
        public virtual TimesNode CreateTimesNode(ExpressionNode left, ExpressionNode right)
        {
            return new TimesNode(left, right);
        }

        /// <summary>
        /// Create a floating-point division operator node.
        /// </summary>
        /// <param name="left">Left operand expression.</param>
        /// <param name="right">Right operand expression.</param>
        /// <returns>A new <see cref="FloatDivNode"/> instance.</returns>
        public virtual FloatDivNode CreateFloatDivNode(ExpressionNode left, ExpressionNode right)
        {
            return new FloatDivNode(left, right);
        }

        /// <summary>
        /// Create an integer division operator node.
        /// </summary>
        /// <param name="left">Left operand expression.</param>
        /// <param name="right">Right operand expression.</param>
        /// <returns>A new <see cref="IntDivNode"/> instance.</returns>
        public virtual IntDivNode CreateIntDivNode(ExpressionNode left, ExpressionNode right)
        {
            return new IntDivNode(left, right);
        }

        /// <summary>
        /// Create a modulus operator node.
        /// </summary>
        /// <param name="left">Left operand expression.</param>
        /// <param name="right">Right operand expression.</param>
        /// <returns>A new <see cref="ModulusNode"/> instance.</returns>
        public virtual ModulusNode CreateModulusNode(ExpressionNode left, ExpressionNode right)
        {
            return new ModulusNode(left, right);
        }

        /// <summary>
        /// Create an exponentiation operator node.
        /// </summary>
        /// <param name="left">Left operand expression.</param>
        /// <param name="right">Right operand expression.</param>
        /// <returns>A new <see cref="ExponentiationNode"/> instance.</returns>
        public virtual ExponentiationNode CreateExponentiationNode(ExpressionNode left, ExpressionNode right)
        {
            return new ExponentiationNode(left, right);
        }

        /// <summary>
        /// Create a literal node from an object value.
        /// </summary>
        /// <param name="value">Object representing the literal; must be int.</param>
        /// <returns>A new <see cref="LiteralNode"/> or throws if invalid.</returns>
        /// <exception cref="ArgumentException">Thrown when value is not an integer.</exception>
        public virtual LiteralNode CreateLiteralNode(object value)
        {
            if (value is int intValue)
                return new LiteralNode(intValue);
            throw new ArgumentException("Unsupported literal type");
        }

        /// <summary>
        /// Create a variable node with the given identifier name.
        /// </summary>
        /// <param name="name">Name of the variable.</param>
        /// <returns>A new <see cref="VariableNode"/> instance.</returns>
        public virtual VariableNode CreateVariableNode(string name)
        {
            return new VariableNode(name);
        }

        /// <summary>
        /// Create an assignment statement node.
        /// </summary>
        /// <param name="variable">Target variable node.</param>
        /// <param name="expression">Expression to assign.</param>
        /// <returns>A new <see cref="AssignmentStmt"/> instance.</returns>
        public virtual AssignmentStmt CreateAssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            return new AssignmentStmt(variable, expression);
        }

        /// <summary>
        /// Create a return statement node.
        /// </summary>
        /// <param name="expression">Expression to return.</param>
        /// <returns>A new <see cref="ReturnStmt"/> instance.</returns>
        public virtual ReturnStmt CreateReturnStmt(ExpressionNode expression)
        {
            return new ReturnStmt(expression);
        }

        /// <summary>
        /// Create a block statement with the provided symbol table.
        /// </summary>
        /// <param name="symbolTable">Symbol table for the block scope.</param>
        /// <returns>A new <see cref="BlockStmt"/> instance.</returns>
        public virtual BlockStmt CreateBlockStmt(SymbolTable<string, object> symbolTable)
        {
            return new BlockStmt(symbolTable);
        }
        #endregion
    }
    
}