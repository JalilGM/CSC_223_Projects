using System;
using System.Collections.Generic;
using System.Text;
using AST;
using Containers;

namespace AST
{
    /// <summary>
    /// Visitor that performs name analysis on an AST, checking for undefined variables
    /// Uses symbol tables to track variable declarations and scopes
    /// </summary>
    public class NameAnalysisVisitor : IVisitor<Tuple<SymbolTable<string, object>, Statement>, bool>
    {
        /// <summary>
        /// Performs name analysis on the given AST, returning true if all variables are defined
        /// </summary>
        /// <param name="ast">The AST to analyze</param>
        /// <returns>True if name analysis succeeds, false if any undefined variables are found</returns>
        public bool Analyze(Statement ast)
        {
            // Start with an empty symbol table and the root statement as the current scope
            var initialContext = Tuple.Create(new SymbolTable<string, object>(), ast);
            return ast.Accept(this, initialContext);
        } 

        private bool IsVariableDefined(string name, SymbolTable<string, object> symbolTable)
        {
            if (symbolTable == null)
            {
                return false;
            }
            return symbolTable.ContainsKey(name) || IsVariableDefined(name, symbolTable.Parent);
        } 

        #region Visitor Methods
        public bool Visit(PlusNode node, Tuple<SymbolTable<string, object>, Statement> context)
        {
            return node.Left.Accept(this, context) && node.Right.Accept(this, context);
        }

        public bool Visit(MinusNode node, Tuple<SymbolTable<string, object>, Statement> context)
        {
            return node.Left.Accept(this, context) && node.Right.Accept(this, context);
        }

        public bool Visit(TimesNode node, Tuple<SymbolTable<string, object>, Statement> context)
        {
            return node.Left.Accept(this, context) && node.Right.Accept(this, context);
        }

        public bool Visit(FloatDivNode node, Tuple<SymbolTable<string, object>, Statement> context)
        {
            return node.Left.Accept(this, context) && node.Right.Accept(this, context);
        }

        public bool Visit(IntDivNode node, Tuple<SymbolTable<string, object>, Statement> context)
        {
            return node.Left.Accept(this, context) && node.Right.Accept(this, context);
        }

        public bool Visit(ModulusNode node, Tuple<SymbolTable<string, object>, Statement> context)
        {
            return node.Left.Accept(this, context) && node.Right.Accept(this, context);
        }

        public bool Visit(ExponentiationNode node, Tuple<SymbolTable<string, object>, Statement> context)
        {
            return node.Left.Accept(this, context) && node.Right.Accept(this, context);
        }

        public bool Visit(LiteralNode node, Tuple<SymbolTable<string, object>, Statement> context)
        {
            // Literals are always defined
            return true;
        }

        public bool Visit(VariableNode node, Tuple<SymbolTable<string, object>, Statement> context)
        {
            // Check if the variable is defined in the current symbol table or any parent tables
            return IsVariableDefined(node.Name, context.Item1);
        }

        public bool Visit(AssignmentStmt node, Tuple<SymbolTable<string, object>, Statement> context)
        {
            // First analyze the expression on the right-hand side
            bool exprValid = node.Expression.Accept(this, context);
            if (!exprValid)
            {
                return false;
            }

            // Then add the variable to the symbol table (assuming all variables are implicitly declared)
            context.Item1[node.Variable.Name] = null; // Value is not important for name analysis
            return true;
        }

        public bool Visit(ReturnStmt node, Tuple<SymbolTable<string, object>, Statement> context)
        {
            return node.Expression.Accept(this, context);
        }

        public bool Visit(BlockStmt node, Tuple<SymbolTable<string, object>, Statement> context)
        {
            // Create a new symbol table for the block scope, with the current table as its parent
            var blockSymbolTable = new SymbolTable<string, object>(context.Item1);
            var blockContext = Tuple.Create(blockSymbolTable, (Statement)node);

            // Analyze each statement in the block with the new symbol table
            foreach (var stmt in node.Statements)
            {
                if (!stmt.Accept(this, blockContext))
                {
                    return false; // If any statement in the block is invalid, return false
                }
            }
            return true; // All statements in the block are valid
        }

        #endregion
    }
}