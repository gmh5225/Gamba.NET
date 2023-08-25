﻿using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Gamba.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Gamba.Parsing
{
    public static class AstParser
    {
        public static AstNode Parse(string exprText, uint bitSize)
        {
            // Parse the expression AST.
            var charStream = new AntlrInputStream(exprText);
            var lexer = new ExprLexer(charStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new ExprParser(tokenStream);
            parser.BuildParseTree = true;
            var expr = parser.gamba();

            var set = new HashSet<string>();
            GetVariables(expr, set);

            // Throw if ANTLR has any errors.
            var errCount = parser.NumberOfSyntaxErrors;
            if (errCount > 0)
                throw new InvalidOperationException($"Parsing ast failed. Encountered {errCount} errors.");

            // Process the parse tree into a usable AST node.
            var visitor = new AstTranslationVisitor(bitSize);
            var result = visitor.Visit(expr);
            return result;
        }

        private static void GetVariables(IParseTree tree, HashSet<string> set)
        {
            if(tree is ExprParser.IdExpressionContext idExpr)
            {
                set.Add(idExpr.ID().GetText());
                return;
            }

            for(int i = 0; i < tree.ChildCount; i++)
            {
                var child = tree.GetChild(i);
                GetVariables(child, set);
            }
        }
    }
}
