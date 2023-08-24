using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Gamba.Ast;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamba.Parsing
{
    public class AstTranslationVisitor : ExprBaseVisitor<AstNode>
    {
        private readonly uint bitSize;

        public AstTranslationVisitor(uint bitSize)
        {
            this.bitSize = bitSize;
        }

        public override AstNode VisitGamba([NotNull] ExprParser.GambaContext context)
        {
            return Visit(context.expression());
        }

        public override AstNode VisitExpression([NotNull] ExprParser.ExpressionContext context)
        {
            var result = base.VisitExpression(context);
            return result;
        }

        public override AstNode VisitBinaryExpression([NotNull] ExprParser.BinaryExpressionContext context)
        {
            var op1 = Visit(context.expression()[0]);
            var op2 = Visit(context.expression()[1]);
            var binaryOperator = context.children[1].GetText();

            AstNode node = binaryOperator switch
            {
                "**" => new PowerNode(op1, op2),
                "*" => new MulNode(op1, op2),
                // Write "a << b" as "a * 2**b".
                "<<" => new MulNode(op1, new PowerNode(new ConstNode(2, bitSize), op2)),
                "+" => new AddNode(op1, op2),
                "-" => new SubNode(op1, op2),
                "&" => new AndNode(op1, op2),
                "|" => new OrNode(op1, op2),
                "^" => new XorNode(op1, op2),
                _ => throw new InvalidOperationException($"Unrecognized binary operator: {binaryOperator}")
            };

            return node;
        }

        public override AstNode VisitParenthesizedExpression([NotNull] ExprParser.ParenthesizedExpressionContext context)
        {
            return Visit(context.expression());
        }

        public override AstNode VisitUnaryExpression([NotNull] ExprParser.UnaryExpressionContext context)
        {
            var op1 = Visit(context.expression());
            var unaryOperator = context.children[0].GetText();

            AstNode node = unaryOperator switch
            {
                "~" => new NegNode(op1),
                // Write "-x" as "0 - x".
                "-" => new SubNode(new ConstNode(0, bitSize), op1),
                _ => throw new InvalidOperationException($"Unrecognized unary operator: {unaryOperator}")
            };

            return node;
        }

        public override AstNode VisitNumberExpression([NotNull] ExprParser.NumberExpressionContext context)
        {
            var text = context.NUMBER().GetText();
            var value = ulong.Parse(text, text.Contains("0x") ? NumberStyles.HexNumber : NumberStyles.Number);
            return new ConstNode(value, bitSize);
        }

        public override AstNode VisitIdExpression([NotNull] ExprParser.IdExpressionContext context)
        {
            var text = context.ID().GetText();
            return new VarNode(text, bitSize);
        }
    }
}
