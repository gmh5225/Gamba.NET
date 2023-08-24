using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamba.Ast
{
    public class ConstNode : AstNode
    {
        public override AstKind Kind => AstKind.Const;

        protected override int OpCount => 0;

        public ulong Value { get; }

        public ConstNode(ulong value, uint bitSize) : base(bitSize, out Action recalcHash)
        {
            Value = value;
            recalcHash();
        }

        protected override int ComputeHash()
        {
            return base.ComputeHash() * 23 + Value.GetHashCode();
        }
    }
}
