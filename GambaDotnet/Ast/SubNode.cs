using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamba.Ast
{
    public class SubNode : BinaryNode
    {
        public override AstKind Kind => AstKind.Sub;

        public SubNode(AstNode op1, AstNode op2) : base(op1, op2)
        {

        }
    }
}
