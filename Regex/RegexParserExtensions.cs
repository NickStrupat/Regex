using System;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using static RegexParser;
using Input = System.ReadOnlySpan<System.Char>;

namespace Regex;

public readonly ref struct Output
{
	public Output(Int32 start, Int32 end)
	{
		Start = start >= 0 ? start : throw new ArgumentOutOfRangeException(nameof(start));
		End = end >= 0 ? end : throw new ArgumentOutOfRangeException(nameof(end));
	}
	public readonly Int32 Start;
	public readonly Int32 End;

	public ReadOnlySpan<Char> SubStr(ReadOnlySpan<Char> text) => text[Start..End];

	public static implicit operator Output(Range range) => new(range.Start.Value, range.End.Value);
	public static implicit operator Range(Output output) => output.Start..output.End;
}

static class RegexParserExtensions
{
	public static T Parse<T>(this ITerminalNode node) where T : IParsable<T> => T.Parse(node.GetText(), provider:null);
	public static T Parse<T>(this IToken node) where T : IParsable<T> => T.Parse(node.Text, provider:null);
	public static T Parse<T>(this ParserRuleContext prc) where T : IParsable<T> => T.Parse(prc.GetText(), provider:null);

	public delegate Boolean TryMatchDel(Input input, out Output matched);

	public static Boolean TryMatch(this PatternContext ctx, Input input, out Output matched)
	{
		var currentInput = input;
		Boolean matchFound;
		for (;;)
		{
			matchFound = ctx.alternatives().TryMatch(currentInput, out matched);
			if (matchFound | ctx.startAnchor is not null)
				break;
			if (currentInput.Length <= 1)
				return false;
			currentInput = currentInput[1..];
		}
		if (ctx.endAnchor is not null)
			return matchFound & (matched.End == input.Length - 1);
		return matchFound;
	}

	public static Boolean TryMatch(this AlternativesContext ctx, Input input, out Output matched)
	{
		// return the first matched term, if there is one
		foreach (var term in ctx.term())
			if (term.TryMatch(input, out matched))
				return true;
			else
				input = input[matched.End..];
		matched = ..0;
		return false;
	}

	public static Boolean TryMatch(this TermContext ctx, Input input, out Output matched)
	{
		// try to match all factors in order
		matched = ..0;
		foreach (var factor in ctx.factor())
		{
			if (!factor.TryMatch(input, out var matchedFactor))
			{
				matched = ..0;
				return false;
			}
			input = input[matchedFactor.End..];
			matched = matched.Start..matchedFactor.End;
		}
		return true;
	}

	public static Boolean TryMatch(this FactorContext ctx, Input input, out Output matched)
	{
		return ctx.quantifier().TryMatch(ctx.atom().TryMatch, input, out matched);
	}

	private static Boolean TryMatch(this QuantifierContext? ctx, TryMatchDel tryMatch, Input input, out Output matched)
	{
		switch (ctx?.GetChild(0))
		{
			case null:
				return tryMatch(input, out matched);
			case ZeroOrOneContext:
				return TryMatchMultiple(0, 1, tryMatch, input, out matched);
			case ZeroOrManyContext:
				return TryMatchMultiple(0, null, tryMatch, input, out matched);
			case OneOrManyContext:
				return TryMatchMultiple(1, null, tryMatch, input, out matched);
			case ExactlyContext ec:
				return TryMatchMultiple(ec.count().Parse<UInt64>(), ec.count().Parse<UInt64>(), tryMatch, input, out matched);
			case AtLeastContext al:
				return TryMatchMultiple(al.min.Parse<UInt64>(), null, tryMatch, input, out matched);
			case BetweenContext bc:
				return TryMatchMultiple(bc.min.Parse<UInt64>(), bc.max.Parse<UInt64>(), tryMatch, input, out matched);
			default: throw new NotImplementedException();
		}

		static Boolean TryMatchMultiple(UInt64 min, UInt64? max, TryMatchDel tryMatch, Input input, out Output matched)
		{
			UInt64 matchesFound = 0;
			for (;;)
			{
				if (!tryMatch(input, out matched))
					break;
				matchesFound++;
				if (matchesFound > max)
					break;
				input = input[..matched.End];
			}
			return matchesFound >= min & (max is null || matchesFound <= max);
		}
	}

	public static Boolean TryMatch(this AtomContext ctx, Input input, out Output matched) =>
		ctx.GetChild(0) switch
		{
			AlternativesContext a => a.TryMatch(input, out matched),
			// LiteralContext l => l.TryMatch(input, out matched),
			AnyContext a => a.TryMatch(input, out matched),
			CharacterContext cc => cc.TryMatch(input, out matched),
			CharacterRangeContext crc => crc.TryMatch(input, out matched),
			_ => throw new NotImplementedException()
		};

	// public static Boolean TryMatch(this LiteralContext ctx, Input input, out Output matched)
	// {
	//
	// }

	public static Boolean TryMatch(this AnyContext ctx, Input input, out Output matched)
	{
		if (input.Length != 0)
		{
			matched = ..1;
			return true;
		}
		matched = ..0;
		return false;
	}

	public static Boolean TryMatch(this CharacterContext ctx, Input input, out Output matched)
	{
		var text = ctx.Character().Symbol.Text;
		Span<Char> unescaped = stackalloc Char[1];
		unescaped[0] = Unescape(text[0], text.Length > 1 ? text[1] : '\0');
		if (input.StartsWith(unescaped))
		{
			matched = ..1;
			return true;
		}
		matched = ..0;
		return false;
	}

	public static Boolean TryMatch(this CharacterRangeContext ctx, Input input, out Output matched)
	{
		// TODO: handle negative ranges

		// try to match all factors in order
		foreach (var factor in ctx.rangeFactor())
		{
			if (!factor.TryMatch(input, out matched))
				return false;
			input = input[..matched.End];
		}
		matched = ..0;
		return true;
	}

	public static Boolean TryMatch(this RangeFactorContext ctx, Input input, out Output matched)
	{
		return ctx.quantifier().TryMatch(ctx.rangeAtom().TryMatch, input, out matched);
	}

	public static Boolean TryMatch(this RangeAtomContext ctx, Input input, out Output matched) =>
		ctx.GetChild(0) switch
		{
			RangeContext rc => rc.TryMatch(input, out matched),
			CharacterContext cc => cc.TryMatch(input, out matched),
			_ => throw new NotImplementedException()
		};

	public static Boolean TryMatch(this RangeContext ctx, Input input, out Output matched)
	{
		var start = ctx.start.Text;
		var end = ctx.end.Text;
		if (input[0] >= start[0] & input[0] <= end[0])
		{
			matched = ..1;
			return true;
		}
		matched = ..0;
		return false;
	}

	private static Char Unescape(Char first, Char second) => (first, second) switch
	{
		('\\', 'n') => '\n',
		('\\', 'r') => '\r',
		('\\', 't') => '\t',
		('\\', _) => second,
		_ => first
	};

	private static Boolean TryUnescape(Char first, Char second, out Char unescaped)
	{
		unescaped = Unescape(first, second);
		return unescaped != first;
	}

	private static String Unescape(String text)
	{
		var unescaped = new StringBuilder(text.Length);
		for (var i = 0; i < text.Length; i++)
		{
			if (TryUnescape(text[i], i + 1 < text.Length ? text[i + 1] : '\0', out var unescapedChar))
			{
				unescaped.Append(unescapedChar);
				i++;
			}
			else
				unescaped.Append(text[i]);
		}
		return unescaped.ToString();
	}
}
