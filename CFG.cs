using New_CSC.src.AST;
using New_CSC.src.Utilities.Containers;
namespace New_CSC.src.Optimizer
{
    public class CFG : DiGraph<Statement>
    {
        public Statement? Start {get; set;}

        public CFG(Statement? start)
        {
            Start = start;
        }
    }
}