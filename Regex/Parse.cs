using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using static Regex.TryMatchChar;

namespace Regex;

using IM = IMatchable;
using RosC = ReadOnlySpan<Char>;

public static class Parser
{
	public static Digit Digit() => new();
	public static AnyExceptNewline Any() => new();
	public static Word Word() => new();
	public static Whitespace Whitespace() => new();
	public static Not<T> Not<T>(T matchable) where T : IM => new(matchable);
	public static Literal Literal(Char c) => new(c);
	public static Literals Literals(ReadOnlyMemory<Char> s) => new(s);
	public static Literals Literals(String s) => new(s);
	public static Range Range(CharRange charRange) => new(charRange);

	public static Quantity<Digit> Digit(Quantifier quantifier) => new(Digit(), quantifier);
	public static Quantity<AnyExceptNewline> Any(Quantifier quantifier) => new(Any(), quantifier);
	public static Quantity<Word> Word(Quantifier quantifier) => new(Word(), quantifier);
	public static Quantity<Whitespace> Whitespace(Quantifier quantifier) => new(Whitespace(), quantifier);
	public static Quantity<Not<T>> Not<T>(T matchable, Quantifier quantifier) where T : IM => new(Not(matchable), quantifier);
	public static Quantity<Literal> Literal(Char c, Quantifier quantifier) => new(Literal(c), quantifier);
	public static Quantity<Literals> Literals(ReadOnlyMemory<Char> s, Quantifier quantifier) => new(Literals(s), quantifier);
	public static Quantity<Range> Range(CharRange charRange, Quantifier quantifier) => new(Range(charRange), quantifier);
}

public interface IMatchable
{
	[Pure]
	Boolean TryMatch(RosC input, out Int32 length);
}

public readonly struct Quantity<T>(T matchable, Quantifier quantifier) : IM
where T : IM
{
	public Boolean TryMatch(RosC input, out Int32 length)
	{

		var (min, max) = quantifier;
		length = 0;
		UInt64 matchesFound = 0;
		for (;;)
		{
			if (!matchable.TryMatch(input, out var oneLength))
				break;
			if (matchesFound >= max)
				break;
			length += oneLength;
			matchesFound++;
			input = input[oneLength..];
		}
		return matchesFound >= min & (max is null || matchesFound <= max);
	}
}

public readonly struct Or<T1, T2>(T1 _1, T2 _2) : IM
where T1 : IM
where T2 : IM
{
	public Boolean TryMatch(RosC input, out Int32 length)
	{
		if (_1.TryMatch(input, out var l1))
		{
			length = l1;
			return true;
		}
		if (_2.TryMatch(input, out var l2))
		{
			length = l2;
			return true;
		}
		length = 0;
		return false;
	}
}

public readonly struct Then<T1, T2>(T1 _1, T2 _2) : IM
where T1 : IM
where T2 : IM
{
	public Boolean TryMatch(RosC input, out Int32 length)
	{
		length = 0;
		if (!_1.TryMatch(input, out var l1))
			return false;
		if (!_2.TryMatch(input[l1..], out var l2))
			return false;
		length = l1 + l2;
		return true;
	}
}

public readonly struct AnyExceptNewline : IM
{
	public Boolean TryMatch(RosC input, out Int32 length) =>
		MatchOne(AtLeastOne(input) && input[0] != '\n', out length);
}

public readonly struct Word : IM
{
	public Boolean TryMatch(RosC input, out Int32 length) =>
		MatchOne(AtLeastOne(input, out var x) && Char.IsLetterOrDigit(x) | x == '_', out length);
}

public readonly struct Digit : IM
{
	public Boolean TryMatch(RosC input, out Int32 length) =>
		MatchOne(AtLeastOne(input, out var x) && Char.IsDigit(x), out length);
}

public readonly struct Whitespace : IM
{
	public Boolean TryMatch(RosC input, out Int32 length) =>
		MatchOne(AtLeastOne(input, out var x) && Char.IsWhiteSpace(x), out length);
}

public readonly struct Not<T>(T matchable) : IM where T : IM
{
	public Boolean TryMatch(RosC input, out Int32 length) =>
		MatchOne(AtLeastOne(input) && !matchable.TryMatch(input, out length), out length);
}

// public readonly struct NotWord : IMatchable
// {
// 	public Boolean TryMatch(RosC input, out UInt32 length) =>
// 		MatchOne(AtLeastOne(input, out var x) && !(Char.IsLetterOrDigit(x) | x == '_'), out length);
// }
//
// public readonly struct NotDigit : IMatchable
// {
// 	public Boolean TryMatch(RosC input, out UInt32 length) =>
// 		MatchOne(AtLeastOne(input, out var x) && !Char.IsDigit(x), out length);
// }
//
// public readonly struct NotWhitespace : IMatchable
// {
// 	public Boolean TryMatch(RosC input, out UInt32 length) =>
// 		MatchOne(AtLeastOne(input, out var x) && !Char.IsWhiteSpace(x), out length);
// }

public readonly struct Literal(Char c) : IM
{
	public Boolean TryMatch(RosC input, out Int32 length) =>
		MatchOne(AtLeastOne(input) && input[0] == c, out length);
	public static implicit operator Literal(Char c) => new(c);
}

public readonly struct Literals(ReadOnlyMemory<Char> text) : IM
{
	public Literals(String text) : this(text.AsMemory()) {}
	public Boolean TryMatch(RosC input, out Int32 length) =>
		Match(AtLeast(text.Length, input) && input.SequenceEqual(text.Span), input.Length, out length);
}

public readonly struct Range(CharRange charRange) : IM
{
	public Boolean TryMatch(RosC input, out Int32 length) =>
		MatchOne(AtLeastOne(input) && charRange.Contains(input[0]), out length);
}

// public readonly struct StartsWith : IAnchor
// {
// 	public Boolean TryMatch(RosC input, out Int32 length)
// 	{
// 		if (matchable.TryMatch(input, out length) && length > 0)
// 			return true;
// 		length = 0;
// 		return false;
// 	}
// }

// public readonly struct Contains<T>(T matchable) : IM where T : IM
// {
// 	public Boolean TryMatch(RosC input, out Int32 length)
// 	{
// 		while (!input.IsEmpty)
// 		{
// 			if (matchable.TryMatch(input, out length) && length > 0)
// 				return true;
// 			input = input[1..];
// 		}
// 		length = 0;
// 		return false;
// 	}
// }
//
// public readonly struct EndAnchor : IM
// {
// 	public Boolean TryMatch(RosC input, out Int32 length)
// 	{
// 		length = 0;
// 		return input.IsEmpty;
// 	}
// }
