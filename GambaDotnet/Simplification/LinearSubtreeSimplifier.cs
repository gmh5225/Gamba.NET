using Gamba.Ast;
using Gamba.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamba.Simplification
{
    public static class LinearSubtreeSimplifier
    {
        public static void SimplifyLinearSubtrees(AstNode node, IReadOnlyDictionary<AstNode, AstClassification> classificationMapping)
        {
            // Otherwise the expression is nonlinear and we must recurse into simplifying child nodes.
            for(int i = 0; i < node.Operands.Count; i++)
            {
                // If the child is linear then we simplify it and update the current operand.
                var child = node.Operands[i];
                if (AstClassifier.IsLinear(classificationMapping[child]))
                {
                    // Simplify the expression.
                    var simplified = SimbaSimplifier.SimplifyLinearExpression(child);

                    // Replace the child expression only if SIMBA returns a shorter and syntactically different AST.
                    if(child.GetHashCode() != simplified.GetHashCode() && simplified.ToString().Length < child.ToString().Length)
                        node.SetOperand(i, simplified);
                }

                // If the child is nonlinear then we recurse into simplifying child nodes.
                else
                {
                    SimplifyLinearSubtrees(child, classificationMapping);
                }
            }
        }
    }
}
