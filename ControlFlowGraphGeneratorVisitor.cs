using New_CSC.src.AST;
using New_CSC.src.Optimizer;
using New_CSC.src.Utilities.Containers;
namespace New_CSC.src.Visitors
{
    public class CFGVisitor : IVisitor<Statement, Statement>
    {
        public CFG CFG;

        public CFG Generate(Statement ast)
        {
            CFG = new CFG(null);
            Statement start = ast.Accept(this, null);
            CFG.Start = start;
            return CFG;
        }

        //Assignment stmt
        public Statement Visit(AssignmentStmt node, Statement pretext)
        {   
            //Visit new statement aand add it as a node in cfg
            CFG.AddVertex(node);

            //If there is a previous statement, add it as an edge from the pretext to new node
            if (pretext != null)
            {
                CFG.AddEdge(pretext, node);
            }
            return node;
        }
        
        //Return Statement
        public Statement Visit(ReturnStmt node, Statement pretext)
        {
            //add return stmt to cfg
            CFG.AddVertex(node);
            
            //add edge
            if (pretext != null)
            {
                CFG.AddEdge(pretext, node);
            }
            //Change to null
            return node;
            //Cut off point 
        }
        //Visit Block
        public Statement Visit(BlockStmt node, Statement pretext)
        {
            Statement currPretext = pretext;

            foreach (var statement in node.Statements)
            {
                //Handle when pretext is null when we return
                currPretext = statement.Accept(this, currPretext);
            }

            return currPretext;
        }
    
    public Statement Visit(PlusNode node, Statement pretext)
        {
            return pretext;
        }
    public Statement Visit(MinusNode node, Statement pretext)
        {
            return pretext;
        }
    public Statement Visit(TimesNode node, Statement pretext)
        {
            return pretext;
        }
    public Statement Visit(FloatDivNode node, Statement pretext)
        {
            return pretext;
        }
    public Statement Visit(IntDivNode node, Statement pretext)
        {
            return pretext;
        }
    public Statement Visit(ModulusNode node, Statement pretext)
        {
            return pretext;
        }
    public Statement Visit(ExponentiationNode node, Statement pretext)
        {
            return pretext;
        }
    public Statement Visit(LiteralNode node, Statement pretext)
        {
            return pretext;
        }
    public Statement Visit(VariableNode node, Statement pretext)
        {
            return pretext;
        }
    }
    
}
