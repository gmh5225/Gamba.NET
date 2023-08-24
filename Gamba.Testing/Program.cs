using Gamba.Parsing;
using Gamba.Utility;
using System.Diagnostics;

// Remove white space from the MBA string and parse it.
var input = "(((((((2 * (2063597568 & (-16777216 ^ ((-1073741823 + a)  + (4278190080 | (a ^ -1) ) ) ) ) )  - ((((( (a & 4294967295))  & -16777216)  + -1073741824)  ^ -16777216) ) )  * 1099511628211)  + -5808590958014384161)  | b)  + (((((2 * (2063597568 & (-16777216 ^ ((-1073741823 + a)  + (4278190080 | (a ^ -1) ) ) ) ) )  - ((((( (a & 4294967295))  & -16777216)  + -1073741824)  ^ -16777216) ) )  * 1099511628211)  + -5808590958014384161)  ^ (b ^ -1) ) )  * 3298534884633)";
input = StringUtility.RemoveWhitespace(input);
Console.WriteLine(input);
var ast = AstParser.Parse(input, 64);

// Convert the parsed AST back into a string representation, then dump it for comparison.
var parsed = AstFormatter.FormatAst(ast);
parsed = StringUtility.RemoveWhitespace(parsed);
for(int i = 0; i < 5; i++)
    Console.WriteLine("");
Console.WriteLine(parsed);
Debugger.Break();