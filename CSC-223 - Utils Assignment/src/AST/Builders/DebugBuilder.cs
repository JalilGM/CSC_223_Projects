using Containers;

namespace AST
{
    // <summary>
    /// DebugBuilder that creates AST nodes with debug information; 
    /// useful for tracing the parsing process and understanding how nodes are created
    /// </summary>
    
    public class DebugBuilder : DefaultBuilder
    {
        public override PlusNode CreatePlusNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Creating PlusNode with left: " + left.Unparse() + " and right: " + right.Unparse());
            return base.CreatePlusNode(left, right);
        }

        public override MinusNode CreateMinusNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Creating MinusNode with left: " + left.Unparse() + " and right: " + right.Unparse());
            return base.CreateMinusNode(left, right);
        }

        public override TimesNode CreateTimesNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Creating TimesNode with left: " + left.Unparse() + " and right: " + right.Unparse());
            return base.CreateTimesNode(left, right);
        }

        public override FloatDivNode CreateFloatDivNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Creating FloatDivNode with left: " + left.Unparse() + " and right: " + right.Unparse());
            return base.CreateFloatDivNode(left, right);
        }

        public override IntDivNode CreateIntDivNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Creating IntDivNode with left: " + left.Unparse() + " and right: " + right.Unparse());
            return base.CreateIntDivNode(left, right);
        }

        public override ModulusNode CreateModulusNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Creating ModulusNode with left: " + left.Unparse() + " and right: " + right.Unparse());
            return base.CreateModulusNode(left, right);
        }

        public override ExponentiationNode CreateExponentiationNode(ExpressionNode left, ExpressionNode right)
        {
            Console.WriteLine("Creating ExponentiationNode with left: " + left.Unparse() + " and right: " + right.Unparse());
            return base.CreateExponentiationNode(left, right);
        }

        public override LiteralNode CreateLiteralNode(object value)
        {
            Console.WriteLine("Creating LiteralNode with value: " + value.ToString());
            return base.CreateLiteralNode(value);
        }

        public override VariableNode CreateVariableNode(string name)
        {
            Console.WriteLine("Creating VariableNode with name: " + name);
            return base.CreateVariableNode(name);
        }

        public override AssignmentStmt CreateAssignmentStmt(VariableNode variable, ExpressionNode expression)
        {
            Console.WriteLine("Creating AssignmentStmt with variable: " + variable.Unparse() + " and expression: " + expression.Unparse());
            return base.CreateAssignmentStmt(variable, expression);
        }

        public override ReturnStmt CreateReturnStmt(ExpressionNode expression)
        {
            Console.WriteLine("Creating ReturnStmt with expression: " + expression.Unparse());
            return base.CreateReturnStmt(expression);
        }

        public override BlockStmt CreateBlockStmt(SymbolTable<string, object> symbolTable)
        {
            Console.WriteLine("Creating BlockStmt with symbol table: " + symbolTable.ToString());
            return base.CreateBlockStmt(symbolTable);
        }
    }
}
