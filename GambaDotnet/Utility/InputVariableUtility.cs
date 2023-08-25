using Gamba.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamba.Utility
{
    public class InputVariableUtility
    {
        private Dictionary<AstNode, HashSet<VarNode>> inputVariables = new();

        public void Collect(AstNode node, AstNode parent)
        {
            // Create an entry for the parent if it does not exist already.
            inputVariables.TryAdd(parent, new());

            foreach (var child in node.Operands)
            {
                // Collect input variables for the child node.
                Collect(child, node);

                // if(child is VarNode varNode)
                //  inputVariables.TryGe
            }
        }
    }
}
