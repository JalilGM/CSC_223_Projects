using Containers;

namespace AST
{
    // <summary>
    /// DefaultBuilder that creates actual AST nodes; useful for normal operation and testing
    /// </summary>

    public class DefaultBuilder
    {
        public virtual PlusNode CreatePlusNode(ExpressionNode left, ExpressionNode right)
        {
            return new PlusNode(left, right);
        }

        public virtual MinusNode CreateMinusNode(ExpressionNode left, ExpressionNode right)
        {
            return new MinusNode(left, right);
        }

        public virtual TimesNode CreateTimesNode(ExpressionNode left, ExpressionNode right)
        {
            return new TimesNode(left, right);
        }

        public virtual FloatDivNode CreateFloatDivNode(ExpressionNode left, ExpressionNode right)
        {
            return new FloatDivNode(left, right);
        }

        public virtual IntDivNode CreateIntDivNode(ExpressionNode left, ExpressionNode right)
        {
            return new IntDivNode(left, right);
        }

        public virtual ModulusNode CreateModulusNode(ExpressionNode left, ExpressionNode right)
        {
            return new ModulusNode(left, right);
        }

        public virtual ExponentiationNode CreateExponentiationNode(ExpressionNode left, ExpressionNode right)
        {
            return new ExponentiationNode(left, right);
        }

        public virtual LiteralNode CreateLiteralNode(object value)
        {
            if (value is int intValue)
                return new LiteralNode(intValue);
            throw new ArgumentException("Unsupported literal type");
        }

        public virtual VariableNode CreateVariableNode(string name)
        {
            return new VariableNode(name);
        }

        public virtual AssignmentStmt CreateAssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            return new AssignmentStmt(variable, expression);
        }

        public virtual ReturnStmt CreateReturnStmt(ExpressionNode expression)
        {
            return new ReturnStmt(expression);
        }

        public virtual BlockStmt CreateBlockStmt(SymbolTable<string, object> symbolTable)
        {
            return new BlockStmt(symbolTable);
        }
    }
    
}