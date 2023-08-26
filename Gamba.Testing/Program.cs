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

var ast = "-4*(x^y)*(x&y)+7*(x^y)*(x&~y)+2*(x^y)*~(x|y)+8*(x^y)*~(x|~y)+1*(x^y)*~y-7*(x^y)*~(x&y)+5*(x^y)*~(x^y)";

LinearSubtreeSimplifier.SimplifyLinearSubtrees(ast, classification);

Console.WriteLine($"Simplified: {ast}");

Debugger.Break();