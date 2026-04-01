/**
 * Builder subclass that returns null for every creation call. Useful for
 * isolating parser logic without building actual node objects.
 *
 * Bugs: None known
 *
 * @author Jalil Garvin-Mingo
 * @date   February 27, 2026
 */

using System;
using System.Collections.Generic;
using Containers;

namespace AST
{
    /// <summary>
    /// Null implementation of the builder pattern; every method returns null.
    /// </summary>
    public class NullBuilder : DefaultBuilder
    {
        #region Overridden creation methods returning null
        // Override all creation methods to return null
        /// <summary>
        /// Always returns null regardless of operands.
        /// </summary>
        public override PlusNode CreatePlusNode(ExpressionNode left, ExpressionNode right)
        {
            return null;
        }

        /// <summary>
        /// Always returns null regardless of operands.
        /// </summary>
        public override MinusNode CreateMinusNode(ExpressionNode left, ExpressionNode right)
        {
            return null;
        }

        /// <summary>
        /// Always returns null regardless of operands.
        /// </summary>
        public override TimesNode CreateTimesNode(ExpressionNode left, ExpressionNode right)
        {
            return null;
        }

        /// <summary>
        /// Always returns null regardless of operands.
        /// </summary>
        public override FloatDivNode CreateFloatDivNode(ExpressionNode left, ExpressionNode right)
        {
            return null;
        }

        /// <summary>
        /// Always returns null regardless of operands.
        /// </summary>
        public override IntDivNode CreateIntDivNode(ExpressionNode left, ExpressionNode right)
        {
            return null;
        }

        /// <summary>
        /// Always returns null regardless of operands.
        /// </summary>
        public override ModulusNode CreateModulusNode(ExpressionNode left, ExpressionNode right)
        {
            return null;
        }

        /// <summary>
        /// Always returns null regardless of operands.
        /// </summary>
        public override ExponentiationNode CreateExponentiationNode(ExpressionNode left, ExpressionNode right)
        {
            return null;
        }

        /// <summary>
        /// Always returns null for literal creation.
        /// </summary>
        public override LiteralNode CreateLiteralNode(object value)
        {
            return null;
        }

        /// <summary>
        /// Always returns null for variable creation.
        /// </summary>
        public override VariableNode CreateVariableNode(string name)
        {
            return null;
        }

        /// <summary>
        /// Always returns null for assignment statement creation.
        /// </summary>
        public override AssignmentStmt CreateAssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            return null;
        }

        /// <summary>
        /// Always returns null for return statement creation.
        /// </summary>
        public override ReturnStmt CreateReturnStmt(ExpressionNode expression)
        {
            return null;
        }

        /// <summary>
        /// Always returns null for block statement creation.
        /// </summary>
        public override BlockStmt CreateBlockStmt(SymbolTable<string, object> symbolTable)
        {
            return null;
        }
        #endregion
    }
}