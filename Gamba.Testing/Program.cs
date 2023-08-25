using Gamba.Ast;
using Gamba.Interop;
using Gamba.Parsing;
using Gamba.Simplification;
using Gamba.Utility;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;

// Remove white space from the MBA string and parse it.
var input = "2351193725*(~aux0^~x^y&x)+1596312586*((~aux1^aux0&x)&(aux0|y)&(aux0^aux1))+2290851828*~(aux1^x|~aux0)+2886596537*(x&x)+1943773571*aux0+2151036226*((~x^x^aux0)&((y|aux0)^aux0))+531942972*~(aux1&(aux1|y))+3835581895*~(x&aux1&aux0)+2342434347*(~aux0|aux1|~(aux0^x))+1275471292*~(y^~x)+2076673039*(aux0&aux1&~aux0|aux1&aux1|x)+1952150221*(x&aux0)+875156293*~y+1544730067*~(aux0|aux1|aux0^aux0)+1743616072*(y&((aux0|aux0)^~aux1))+3487232144*(y&aux0&(aux1|y)&~aux1)+2551351224*~(aux1&(y|y))+3887547142*(~(aux0&y)|~~x)+3687798283*((~y|x)^y^x^x)+2218294257*~(aux1&x)";
input = StringUtility.RemoveWhitespace(input);

/*
input = input.Replace("**", "#");
var matches = Regex.Matches(input, "^[a-zA-Z0-9-_]+$");
foreach(var match in matches)
{
    Console.WriteLine(match);
}

var parseTree = ShuttingYardParser.ShuttingYard(input);
foreach(var item in parseTree)
{
    Console.WriteLine($"Token: {StringMarshaler.AcquireString(item.str)}");
}

var variables = new List<AstNode>();
variables.Add(new VarNode("x", 64));
variables.Add(new VarNode("y", 64));
variables.Add(new VarNode("z", 64));
AstTranslator.Lift(parseTree.Select(x => new Token(x)).ToList(), variables, 64);
*/
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

/*
foreach (var (node, linearity) in classification)
{
    var isLinear = AstClassifier.IsLinear(linearity);
    Console.WriteLine($"(class: {linearity}) (isLinear: {isLinear}): {node}");
}
*/
Debugger.Break();