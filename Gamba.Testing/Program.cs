using Gamba.Ast;
using Gamba.Interop;
using Gamba.LLVMInterop;
using Gamba.Parsing;
using Gamba.Simplification;
using Gamba.Symbolic;
using Gamba.Utility;
using LLVMSharp.Interop;
using Microsoft.Z3;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;

var input = "(x^y)-(x&~y^~z)+(~x|y)-(x&~y&~z)-(~x&~y&~z)+2*(~x|~y&~z)-3*(~x&y&z)-(~x|y|~z)-(x&~y)";
var ast = AstParser.Parse(input, 64);
Console.WriteLine($"Ast before simplification: {ast}");

var simplifier = new AstSimplifier(ast);
var simplified = simplifier.Simplify();

Console.WriteLine($"Ast after simplification: {simplified}");

/*
var converter = new AstToZ3(64);
var toZ3 = converter.Translate(ast, null);

Console.WriteLine($"Before simplification: {toZ3}");

// Simplify.
for (int i = 0; i < 10; i++)
    toZ3 = toZ3.Simplify();

Console.WriteLine($"After simplification: {toZ3.Simplify()}");

foreach(var child in ast.Children)
{
    var isLinear = FastSimba.CheckLinear(child);
    if(isLinear)
    {
        Console.WriteLine($"Linear subtree: {child}");
    }
}


//LinearSubtreeSimplifier.SimplifyLinearSubtrees(ast,);

//Console.WriteLine($"Simplified: {ast}");
*/

Debugger.Break();