using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamba.Ast
{
    public class VarNode : AstNode
    {
        public override AstKind Kind => AstKind.Var;

        protected override int OpCount => 0;

        public uint Id { get; }

        public VarNode(uint id, uint bitSize) : base(bitSize)
        {
            Id = id;
        }

        protected override int ComputeHash()
        {
            return base.ComputeHash() * 23 + Id.GetHashCode();
        }
    }
}
