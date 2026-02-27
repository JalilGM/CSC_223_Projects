/**
 * Decorator builder that logs creation events to the console before
 * invoking DefaultBuilder behavior. Helpful for debugging parser output.
 *
 * Bugs: None noted
 *
 * @author <your name>
 * @date   February 27, 2026
 */

using Containers;

namespace AST
{
    /// <summary>
    /// Builder subclass which prints debug information for every node created.
    /// </summary>
    public class DebugBuilder : DefaultBuilder
    {
        #region Overridden creation methods with logging
        /// <summary>
        /// Logs creation of a plus node then delegates to base builder.
        /// </summary>
        public override PlusNode CreatePlusNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Creating PlusNode with left: " + left.Unparse() + " and right: " + right.Unparse());
            return base.CreatePlusNode(left, right);
        }

        /// <summary>
        /// Logs creation of a minus node then delegates to base builder.
        /// </summary>
        public override MinusNode CreateMinusNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Creating MinusNode with left: " + left.Unparse() + " and right: " + right.Unparse());
            return base.CreateMinusNode(left, right);
        }

        /// <summary>
        /// Logs creation of a times node then delegates to base builder.
        /// </summary>
        public override TimesNode CreateTimesNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Creating TimesNode with left: " + left.Unparse() + " and right: " + right.Unparse());
            return base.CreateTimesNode(left, right);
        }

        /// <summary>
        /// Logs creation of a float division node then delegates to base builder.
        /// </summary>
        public override FloatDivNode CreateFloatDivNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Creating FloatDivNode with left: " + left.Unparse() + " and right: " + right.Unparse());
            return base.CreateFloatDivNode(left, right);
        }

        /// <summary>
        /// Logs creation of an integer division node then delegates to base builder.
        /// </summary>
        public override IntDivNode CreateIntDivNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Creating IntDivNode with left: " + left.Unparse() + " and right: " + right.Unparse());
            return base.CreateIntDivNode(left, right);
        }

        /// <summary>
        /// Logs creation of a modulus node then delegates to base builder.
        /// </summary>
        public override ModulusNode CreateModulusNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Creating ModulusNode with left: " + left.Unparse() + " and right: " + right.Unparse());
            return base.CreateModulusNode(left, right);
        }

        /// <summary>
        /// Logs creation of an exponentiation node then delegates to base builder.
        /// </summary>
        public override ExponentiationNode CreateExponentiationNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Creating ExponentiationNode with left: " + left.Unparse() + " and right: " + right.Unparse());
            return base.CreateExponentiationNode(left, right);
        }

        /// <summary>
        /// Logs literal creation then delegates to base builder.
        /// </summary>
        public override LiteralNode CreateLiteralNode(object value)
        {
            Console.WriteLine("Creating LiteralNode with value: " + value.ToString());
            return base.CreateLiteralNode(value);
        }

        /// <summary>
        /// Logs variable creation then delegates to base builder.
        /// </summary>
        public override VariableNode CreateVariableNode(string name)
        {
            Console.WriteLine("Creating VariableNode with name: " + name);
            return base.CreateVariableNode(name);
        }

        /// <summary>
        /// Logs assignment statement creation then delegates to base builder.
        /// </summary>
        public override AssignmentStmt CreateAssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            Console.WriteLine("Creating AssignmentStmt with variable: " + variable.Unparse() + " and expression: " + expression.Unparse());
            return base.CreateAssignmentStmt(variable, expression);
        }

        /// <summary>
        /// Logs return statement creation then delegates to base builder.
        /// </summary>
        public override ReturnStmt CreateReturnStmt(ExpressionNode expression)
        {
            Console.WriteLine("Creating ReturnStmt with expression: " + expression.Unparse());
            return base.CreateReturnStmt(expression);
        }

        /// <summary>
        /// Logs block statement creation then delegates to base builder.
        /// </summary>
        public override BlockStmt CreateBlockStmt(SymbolTable<string, object> symbolTable)
        {
            Console.WriteLine("Creating BlockStmt with symbol table: " + symbolTable.ToString());
            return base.CreateBlockStmt(symbolTable);
        }
        #endregion
    }
}
