using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Regex;
using static RegexParser;

var regex = "ac";
var input = "ac";
var stream = new CodePointCharStream(regex);
var lexer = new RegexLexer(stream);
var tokens = new CommonTokenStream(lexer);
var parser = new RegexParser(tokens);
parser.character().TryMatch(input, out var asdf);
var pattern = parser.pattern();
//var visitor = new Visitor(input.AsMemory());
//var isMatch = visitor.Visit(pattern);


if (pattern.TryMatch(input, out var matched))
	Console.WriteLine("Match: " + input[matched.Start..matched.End]);
else
	Console.WriteLine("No match");

var listener = new Listener("a");
ParseTreeWalker.Default.Walk(listener, pattern);
;

// static Boolean IsMatch(ExpressionContext expression, String input)
// {
// 	expression.term().Any()
// 	var isMatch = false;
// 	foreach (var term in expression.term())
// 	{
// 		foreach (var factor in term.factor())
// 		{
// 			switch (factor.quantifier().GetChild(0))
// 			{
// 				case ZeroOrOneContext zoo:
// 					if (!IsMatch2(factor.atom()))
// 						continue;
// 					break;
// 				case ZeroOrManyContext zom:
//
// 					break;
// 				case OneOrManyContext oom:
// 					break;
// 				case ExactlyContext ec:
// 					break;
// 				case AtLeastContext al:
// 					break;
// 				case BetweenContext bc:
// 					break;
// 				default:
// 					throw new NotImplementedException();
// 			}
// 		}
// 		if (isMatch)
// 			return
// 	}
// }

public sealed class Listener : RegexBaseListener
{
	//private Int32 index;
	private readonly ReadOnlyMemory<Char> input;
	public Listener(String input) : this(input.AsMemory()) {}
	public Listener(ReadOnlyMemory<Char> input) => this.input = input;

	public Boolean IsMatch { get; private set; }
}

// public sealed class Visitor : RegexBaseVisitor<Boolean>
// {
// 	private ReadOnlyMemory<Char> input;
// 	public Visitor(ReadOnlyMemory<Char> input) => this.input = input;
//
// 	public override Boolean VisitAlternatives(AlternativesContext context) => context.term().Any(VisitTerm);
// 	public override Boolean VisitTerm(TermContext context) => context.factor().All(VisitFactor);
//
// 	public override Boolean VisitAtom(AtomContext context)
// 	{
// 		switch (context.GetChild(0))
// 		{
// 			case AlternativesContext e: return Visit(e);
// 			case LiteralContext l: return Visit(l);
// 			case CharacterContext cc: return Visit(cc);
// 			case CharacterRangeContext crc: return Visit(crc);
// 			default: throw new NotImplementedException();
// 		}
// 	}
//
// 	public override Boolean VisitFactor(FactorContext context)
// 	{
// 		switch (context.quantifier()?.GetChild(0))
// 		{
// 			case null:
// 				return Visit(context.atom());
// 			case ZeroOrOneContext:
// 				return Visit(context.atom()) || true;
// 			case ZeroOrManyContext:
// 				for (;;)
// 					if (!Visit(context.atom()))
// 						break;
// 				return true;
// 			case OneOrManyContext:
// 				if (!Visit(context.atom()))
// 					return false;
// 				for (;;)
// 					if (!Visit(context.atom()))
// 						break;
// 				return true;
// 			case ExactlyContext ec:
// 				for (var i = 0ul; i < ec.Number().Parse<UInt64>(); i++)
// 					if (!Visit(context.atom()))
// 						return false;
// 				return true;
// 			case AtLeastContext al:
// 				for (var i = 0ul; i < al.Number().Parse<UInt64>(); i++)
// 					if (!Visit(context.atom()))
// 						return false;
// 				for (;;)
// 					if (!Visit(context.atom()))
// 						break;
// 				return true;
// 			case BetweenContext bc:
// 				for (var i = 0ul; i < bc.Number()[0].Parse<UInt64>(); i++)
// 					if (!Visit(context.atom()))
// 						return false;
// 				for (var i = 0ul; i < bc.Number()[1].Parse<UInt64>(); i++)
// 					if (!Visit(context.atom()))
// 						break;
// 				return true;
// 			default: throw new NotImplementedException();
// 		}
// 	}
//
// 	// public override Boolean VisitLiteral(LiteralContext context)
// 	// {
// 	// 	var literal = context.GetText();
// 	// 	var length = literal.Length;
// 	// 	var buffer = length <= 1024 ? stackalloc Char[length] : new Char[length];
// 	// 	var bi = 0;
// 	// 	for (var i = 1; i < literal.Length - 1; i++)
// 	// 	{
// 	// 		var current = literal[i];
// 	// 		var next = i < length - 1 ? literal[i + 1] : '\0';
// 	// 		buffer[bi++] = (current, next) switch
// 	// 		{
// 	// 			('\\', 'n') => '\n',
// 	// 			('\\', 'r') => '\r',
// 	// 			('\\', 't') => '\t',
// 	// 			('\\', _) => next,
// 	// 			_ => current
// 	// 		};
// 	// 	}
// 	//
// 	// 	return input.Span.StartsWith(buffer[..bi]);
// 	// }
//
// 	public override Boolean VisitCharacter(CharacterContext context)
// 	{
// 		var character = context.Character().GetText();
// 		var first = character[0];
// 		var second = character.Length > 1 ? character[1] : '\0';
// 		var unescaped = Unescape(first, second);
// 		return input.Length > 0 && input.Span[0] == unescaped;
// 	}
//
// 	public override Boolean VisitCharacterRanges(CharacterRangesContext context)
// 	{
// 		if (context.not() != null
// 		return context.characterRange().Any(VisitCharacterRange);
// 	}
//
// 	public override Boolean VisitCharacterRange(CharacterRangeContext context)
// 	{
// 		var chars = context.Character();
// 		if (chars.Length == 1)
// 			return IsMatch(chars[0].GetText().AsSpan()[0..1]);
// 		if (chars.Length != 2)
// 			throw new NotImplementedException();
//
// 		var first = Unescape(chars[0].GetText());
// 		var second = Unescape(chars[1].GetText());
// 		return input.Length > 0 && input.Span[0] >= first && input.Span[0] <= second;
// 	}
//
// 	private Boolean IsMatch(ReadOnlySpan<Char> possiblyEscapedText)
// 	{
// 		for (var i = 0; i < possiblyEscapedText.Length; i++)
// 		{
// 			var c = possiblyEscapedText[i];
// 			var n = i < possiblyEscapedText.Length - 1 ? possiblyEscapedText[i + 1] : '\0';
// 			var unescaped = Unescape(c, n);
// 			if (input.Length <= 0 || input.Span[0] != unescaped)
// 				continue;
// 			input = input[1..];
// 			return true;
// 		}
// 		return false;
// 	}
//
// 	private static Char Unescape(ReadOnlySpan<Char> text) =>
// 		text.Length switch
// 		{
// 			1 => text[0],
// 			_ => Unescape(text[0], text[1])
// 		};
//
// 	private static Char Unescape(Char first, Char second) => (first, second) switch
// 	{
// 		('\\', 'n') => '\n',
// 		('\\', 'r') => '\r',
// 		('\\', 't') => '\t',
// 		('\\', _) => second,
// 		_ => first
// 	};
// }
