
using Containers;

namespace AST
{
    
public abstract class ExpressionNode
    {
        // Base class for all expression nodes
        public abstract string Unparse(int level = 0);
    }

public abstract class Operator: ExpressionNode
    {
        // Base class for all operator nodes
    }

public abstract class BinaryOperator : Operator
    {
        public ExpressionNode Left { get; } 
        public ExpressionNode Right { get; }

        // Constructor to initialize left and right operands
        protected BinaryOperator(ExpressionNode left, ExpressionNode right) 
        {
            Left = left;
            Right = right;
        }
    }


public class LiteralNode : ExpressionNode
    {
        public int Value { get; }

        // Constructor to initialize the literal value
        public LiteralNode(int value) 
        {
            Value = value;
        }
        public override string Unparse(int level = 0)
        {
            return Value.ToString();
        }
    }

public class VariableNode : ExpressionNode
    {
        public string Name { get; }

        // Constructor to initialize the variable name
        public VariableNode(string name) 
        {
            Name = name;
        }

        public override string Unparse(int level = 0)
        {
            return Name;
        }
    }

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

public abstract class Statement
    {
        // Base class for all statement nodes
        public abstract string Unparse(int level = 0); 

        // GetIndentation utility method you wrote in assignment 0? Use it to create readable code that preserves 
        // the hierarchical relationship of program elements.
        protected string GetIndentation(int level)
        {
        return new string(' ', level * 4); // 4 spaces per level
    }
    }

public class BlockStmt : Statement
    {
        public SymbolTable<string, object> SymbolTable { get; }

        public BlockStmt(SymbolTable<string, object> symbolTable) 
        {
            SymbolTable = symbolTable;
        }

        // BlockStmt use level to indent their opening and closing curly braces, then increment level when calling Unparse on 
        // their children. 
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

public class AssignmentStmt : Statement
    {
        public VariableNode Variable { get; }
        public ExpressionNode Expression { get; }

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

}

