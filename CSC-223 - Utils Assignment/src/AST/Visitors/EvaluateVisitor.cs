using System;
using System.Collections.Generic;
using System.Text;
using AST;
using Containers;

namespace AST
{
    /// <summary>
    /// Exception thrown when an evaluation error occurs
    /// </summary>
    public class EvaluationException : Exception
    {
        public EvaluationException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Visitor that evaluates an AST, executing the program and returning the final value
    /// Uses symbol tables to store variable values during execution
    /// </summary>
    public class EvaluateVisitor : IVisitor<SymbolTable<string, object>, object>
    {
        // Flag to indicate if a return statement has been encountered
        private bool _returnEncountered;
        
        // Value from the return statement
        private object _returnValue;

        /// <summary>
        /// Initializes a new instance of the EvaluateVisitor class
        /// </summary>
        public EvaluateVisitor()
        {
            _returnEncountered = false;
            _returnValue = null;
        }

        /// <summary>
        /// Evaluates the given AST and returns the result
        /// </summary>
        /// <param name="ast">The AST to evaluate</param>
        /// <returns>The result of the evaluation (typically from a return statement)</returns>
        public object Evaluate(Statement ast)
        {
            _returnEncountered = false;
            _returnValue = null;
            
            // Execute the AST with a null initial scope
            // (the BlockStmt will use its own symbol table)
            ast.Accept(this, null);
            
            return _returnValue;
        }

        private object GetVariableValue(string name, SymbolTable<string, object> symbolTable)
        {
            if (symbolTable == null)
            {
                throw new EvaluationException($"Variable '{name}' is not defined.");
            }

            if (symbolTable.TryGetValue(name, out object value))
            {
                return value;
            }
            else
            {
                throw new EvaluationException($"Variable '{name}' is not defined.");
            }
        }

        #region Expression Node Visit Methods

        public object Visit(PlusNode node, SymbolTable<string, object> symbolTable)
        {
            object leftValue = node.Left.Accept(this, symbolTable);
            object rightValue = node.Right.Accept(this, symbolTable);
            return Convert.ToDouble(leftValue) + Convert.ToDouble(rightValue);

        }

        public object Visit(MinusNode node, SymbolTable<string, object> symbolTable)
        {
            object leftValue = node.Left.Accept(this, symbolTable);
            object rightValue = node.Right.Accept(this, symbolTable);
            return Convert.ToDouble(leftValue) - Convert.ToDouble(rightValue);
        }

        public object Visit(TimesNode node, SymbolTable<string, object> symbolTable)
        {
            object leftValue = node.Left.Accept(this, symbolTable);
            object rightValue = node.Right.Accept(this, symbolTable);
            return Convert.ToDouble(leftValue) * Convert.ToDouble(rightValue);
        }

        public object Visit(FloatDivNode node, SymbolTable<string, object> symbolTable)
        {
            object leftValue = node.Left.Accept(this, symbolTable);
            object rightValue = node.Right.Accept(this, symbolTable);
            return Convert.ToDouble(leftValue) / Convert.ToDouble(rightValue);
        }

        public object Visit(IntDivNode node, SymbolTable<string, object> symbolTable)
        {
            object leftValue = node.Left.Accept(this, symbolTable);
            object rightValue = node.Right.Accept(this, symbolTable);
            return Convert.ToInt32(leftValue) / Convert.ToInt32(rightValue);
        }

        public object Visit(ModulusNode node, SymbolTable<string, object> symbolTable)
        {
            object leftValue = node.Left.Accept(this, symbolTable);
            object rightValue = node.Right.Accept(this, symbolTable);
            return Convert.ToInt32(leftValue) % Convert.ToInt32(rightValue);
        }

        public object Visit(ExponentiationNode node, SymbolTable<string, object> symbolTable)
        {
            object leftValue = node.Left.Accept(this, symbolTable);
            object rightValue = node.Right.Accept(this, symbolTable);
            return Math.Pow(Convert.ToDouble(leftValue), Convert.ToDouble(rightValue));
        }

        public object Visit(LiteralNode node, SymbolTable<string, object> symbolTable)
        {
            return node.Value;
        }

        public object Visit(VariableNode node, SymbolTable<string, object> symbolTable)
        {
            // Variables return their value from the symbol table
            return GetVariableValue(node.Name, symbolTable);
        }

        #endregion

        #region Statement Node Visit Methods

        public object Visit(AssignmentStmt node, SymbolTable<string, object> symbolTable)
        {
            // Evaluate the expression to get the value to assign
            object value = node.Expression.Accept(this, symbolTable);
            
            // Assign the value to the variable in the current symbol table
            symbolTable[node.Variable.Name] = value;
            
            return null; // Assignment statements do not return a value
        }

        public object Visit(ReturnStmt node, SymbolTable<string, object> symbolTable)
        {
            // Evaluate the expression to get the return value
            _returnValue = node.Expression.Accept(this, symbolTable);
            _returnEncountered = true;
            return null; // Return statements do not return a value directly
        }

        public object Visit(BlockStmt node, SymbolTable<string, object> symbolTable)
        {
            // Use this block's symbol table, which is already linked to its parent
            SymbolTable<string, object> currentScope = node.SymbolTable;
            
            // Execute each statement in the block
            foreach (Statement stmt in node.Statements)
            {
                stmt.Accept(this, currentScope);
                
                // If a return statement was encountered, stop executing further statements
                if (_returnEncountered)
                {
                    break;
                }
            }

            return null; // Block statements do not return a value directly
        }

        #endregion
    }
}