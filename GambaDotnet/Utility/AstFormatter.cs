using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamba.Ast;

namespace Gamba.Utility
{
    public static class AstFormatter
    {
        public static string FormatAst(AstNode node)
        {
            var sb = new StringBuilder();
            FormatAstInternal(node, ref sb);
            return sb.ToString();
        }

        private static void FormatAstInternal(AstNode node, ref StringBuilder sb)
        {
            /*
            object _ = node switch
            {
                ConstNode constNode => builder.Append(constNode.Value.ToString("X")),
                VarNode varNode => builder.Append(varNode.Name),
                BinaryNode binaryNode => () =>
                {
                    FormatAst(binaryNode.Operands[0]);
                    builder.Append($" {GetOperatorName(binaryNode.Kind)} ");
                }
                ,
                UnaryNode unaryNode => null,
                _ => throw new InvalidOperationException()
            };
            */

            if (node is ConstNode constNode)
            {
                sb.Append(constNode.Value);
                return;
            }

            if (node is VarNode varNode)
            {
                sb.Append(varNode.Name);
                return;
            }

            if (node is BinaryNode)
            {
                sb.Append("(");

                // Pretty print "0 - x" as "-x";
                if (node.Kind == AstKind.Sub && node.Operands[0] is ConstNode subConst && subConst.Value == 0)
                {
                    sb.Append("-");
                    FormatAstInternal(node.Operands[1], ref sb);
                }

                // Otherwise print the AST as normal.
                else
                {
                    FormatAstInternal(node.Operands[0], ref sb);
                    sb.Append($" {GetOperatorName(node.Kind)} ");
                    FormatAstInternal(node.Operands[1], ref sb);
                }

                sb.Append(")");
                return;
            }

            if (node is UnaryNode)
            {
                sb.Append($"{GetOperatorName(node.Kind)}");
                FormatAstInternal(node.Operands[0], ref sb);
                return;
            }

            throw new InvalidOperationException($"Cannot print ast kind: {node.Kind}");
        }

        private static string GetOperatorName(AstKind kind)
        {
            return kind switch
            {
                AstKind.Power => "**",
                AstKind.Add => "+",
                AstKind.Sub => "-",
                AstKind.Mul => "*",
                AstKind.And => "&",
                AstKind.Or => "|",
                AstKind.Xor => "^",
                AstKind.Neg => "~",
                _ => throw new InvalidOperationException($"Unrecognized operator: {kind.ToString()}")
            };
        }
    }
}
