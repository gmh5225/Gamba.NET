

using Gamba.Parsing;
using System.Diagnostics;

var ast = AstParser.Parse("x~", 64);
Debugger.Break();