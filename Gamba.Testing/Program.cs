using Gamba.Parsing;
using Gamba.Simplification;
using Gamba.Utility;
using System.Data;
using System.Diagnostics;

// Remove white space from the MBA string and parse it.
var input = "(((x&y) + (x&y)) & z)";
input = StringUtility.RemoveWhitespace(input);
Console.WriteLine(input);
var ast = AstParser.Parse(input, 32);

// Convert the parsed AST back into a string representation, then dump it for comparison.
var parsed = AstFormatter.FormatAst(ast);
parsed = StringUtility.RemoveWhitespace(parsed);
for(int i = 0; i < 5; i++)
    Console.WriteLine("");

Console.WriteLine(parsed);

var sw = Stopwatch.StartNew();
var classification = AstClassifier.Classify(ast);
sw.Stop();
Console.WriteLine($"Took {sw.ElapsedMilliseconds}");

foreach (var (node, linearity) in classification)
{
    var isLinear = AstClassifier.IsLinear(linearity);
    Console.WriteLine($"(class: {linearity}) (isLinear: {isLinear}): {node}");
}

Debugger.Break();