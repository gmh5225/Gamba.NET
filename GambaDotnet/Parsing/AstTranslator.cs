using Gamba.Ast;
using Gamba.Interop;
using LLVMSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Gamba.Parsing
{
    public enum TokenType
    {
        Unknown,
        Number,
        Operator,
        LeftParen,
        RightParen,
        Variable
    };

    public static class AstTranslator
    {
        public static void Lift(IReadOnlyList<Token> tokens, IReadOnlyList<AstNode> variables, uint bitSize)
        {
            var queue = new Queue<Token>(tokens.Reverse());

            var stack = new Queue<AstNode>();

            while(queue.Any())
            {
                var token = queue.Dequeue();
                switch(token.Type)
                {
                    case TokenType.Variable:
                        stack.Enqueue(variables[token.ArgIndex]);
                        break;
                    case TokenType.Number:
                        var text = token.Str;
                        var value = ulong.Parse(text, text.Contains("0x") ? NumberStyles.HexNumber : NumberStyles.Number);
                        stack.Enqueue(new ConstNode(value, bitSize));
                        break;
                    case TokenType.Operator:
                        if(token.Unary)
                        {
                            var rhsAP = stack.Dequeue();
                            if (token.Str[0] == 'm')
                                stack.Enqueue(new MulNode(rhsAP, new ConstNode(-1, bitSize)));
                            else if (token.Str[0] == '~')
                                stack.Enqueue(new NegNode(rhsAP));
                            else
                                throw new InvalidOperationException();
                        }

                        else
                        {
                            var op2 = stack.Dequeue();
                            var op1 = stack.Dequeue();
                            AstNode node = token.Str[0].ToString() switch
                            {
                                "#" => new PowerNode(op1, op2),
                                "*" => new MulNode(op1, op2),
                                // Write "a << b" as "a * 2**b".
                                "<<" => new MulNode(op1, new PowerNode(new ConstNode(2, bitSize), op2)),
                                "+" => new AddNode(op1, op2),
                                "-" => new AddNode(op1, new MulNode(op2, new ConstNode(-1, bitSize))),
                                "&" => new AndNode(op1, op2),
                                "|" => new OrNode(op1, op2),
                                "^" => new XorNode(op1, op2),
                                _ => throw new InvalidOperationException($"Unrecognized binary operator: {token.Str}")
                            };
                        }

                        break;
                    default:
                        break;
                        //throw new InvalidOperationException($"Cannot handle token: {token}");

                }
            }

            Debugger.Break();
            var foobar = stack.Dequeue();
        }
    }

    public class Token
    {
        public TokenType Type { get; }

        public string Str { get; }

        public int Precedence { get; }

        public bool RightAssociative { get; }

        public bool Unary { get; }

        public int ArgIndex { get; }

        public Token(TokenOutput token) 
            : this((TokenType)token.type, StringMarshaler.AcquireString(token.str), token.precedence, token.rightAssociative, token.unary, token.argIndex)
        {
        }

        public Token(TokenType type, string str, int precedence, bool rightAssociative, bool unary, int argIndex)
        {
            Type = type;
            Str = str;
            Precedence = precedence;
            RightAssociative = rightAssociative;
            Unary = unary;
            ArgIndex = argIndex;
        }
    }
}
