namespace Parser;

public static class ParseException : Exception
{
    public ParseException(string message) : base(message) { }
}

public static class Parser
{
    public static AST.BlockStmt Parse(string program);

    public AST.ExpressionNode ParseExpression(List<Token> tokens)
    {
        if (tokens[0].Type != TokenType.LEFT_PAREN)
        {
            throw new ParseException("Invalid Expression");
        }
        var content = new List<Token>();
        for (int i = 1; i < tokens.Count - 1; i++)
        {
            content.Add(tokens[i]);
        }
        if (tokens.Type != TokenType.Right_Paren)
        {
            return new ParseException("Invalid Expression");
        }
        return ParseExpressionContent(content);
    }
    public static AST.ExpressionNode ParseSubContent(List<Tokenizer.Token> tokens)
    {
        if (tokens.Count == 0)
        {
            throw new ParseException("Empty subcontent");
        }
        if (tokens.Count == 1) { return HandleSingleToken(tokens[0]); }

        return ParseExpression(tokens);

    }
    public AST.ExpressionNode ParseExpressionContent(List<Tokenizer.Token> tokens)
    {
        //Cases: Empty -> Exception

        if (tokens.Count == 0)
        {
            throw new ParseException("No valid content to Parse");

        }
        //Single Token
        if (tokens.Count = 1)
        {
            return HandleSingleToken(token[0]);
        }
        //Binary operator cases - lit/var op lit/var
        // - expr op expr - litvar op expr - expr op litvar
        // Most unwrapped to least unwrapped
        int NestLevel = 1;
        for (int i = 0; i < tokens.Count - 1; i++)
        {
            if (tokens[i].Type == LEFT_PAREN)
            {
                NestLevel += 1;
            }
            if (tokens[i].Type == RIGHT_PAREN)
            {
                NestLevel -= 1;
            }
            if (tokens[i].Type == TokenType.OPERATOR && NestLevel == 1)
            {
                var left = new List<Token>();
                var right = new List<Token>();
                for (int j = 0; j < 1; j++)
                {
                    left.Add(tokens[j]);
                }
                for (int k = i + 1; k < tokens.Count - 1; k++)
                {
                    right.Add(tokens[k]);
                }

                leftNode = ParseSubContent(left);
                rightNode = ParseSubContent(right);
                return CreateBinaryOperatorNode(tokens[i], leftNode, rightNode);
            }



        }
    }




    public AST.ExpressionNode HandleSingeToken(Tokenizer.Token token)
    {
        if (token.Type == TokenType.VARIABLE)
        {
            return ParseVariableNode(token.Value);
        }
        if (token.Type == TokenType.INTEGER)
        {
            return AST.LiteralNode(token);
        }
        if (token.Type == TokenType.FLOAT)
        {
            return AST.LiteralNode(token);
        }
        else
        {
            throw ParseException("Invalid Token");
        }
    }
    public AST.ExpressionNode CreateBinaryOperatorNode(string op, AST.EXpressionNode node, AST.ExpressionNode r)
    {
        if (op == "+")
        {
            return AST.PlusNode(node, r);

        }
        if (op == "-")
        {
            return AST.MinusNode(node, r);
        }
        if (op == "*")
        {
            return AST.TimesNode(node, r);
        }
        if (op == "/")
        {
            return AST.FloatDivNode(node, r);
        }
        if (op == "//")
        {
            return AST.IntDivNode(node, r);
        }
        if (op == "%")
        {
            return AST.ModulusNode(node, r);
        }
        if (op == "**")
        {
            return AST.ExponentiationNode(node, r);
        }
        else
        {
            throw new ParseException("Invalid Node");
        }
    }
    public AST.VariableNode ParseVariableNode(string s)
    {
        List<string> alphabet = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'];
        foreach (char c in s)
        {
            if (!alphabet.Contains(s))
            {
                throw new ParseException("Invalid Node");
            }
        }

        return AST.VariableNode(s);
    }


    public AST.AssignmentStmt ParseAssignmentStmt(List<Tokenizer.Token> tokens, SymbolTable table)
    {
        pass;
    }
    public AST.ReturnStmt ParseReturnStatement(List<Tokenizer.Token> tokens)
    {
        pass;
    }
    public AST.Statement ParseStatement(List<Tokenizer.Token> tokens)
    {
        pass;
    }
    public void ParseStmtList(List<string> lines, BlockStmt block)
    {
        pass;
    }
    public AST.BlockStmt ParseBlockStmt(List<string> lines)
    {
        pass;
    }

}
