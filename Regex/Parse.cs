using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using static Regex.TryMatchChar;

namespace Regex;

using IM = IMatchable;
using RosC = ReadOnlySpan<Char>;

public static class Parser
{
	public static Sequence<T1, T2> Sequence<T1, T2>(T1 _1, T2 _2)
	where T1 : IM where T2 : IM =>
		new(_1, _2);

	public static Sequence<T1, T2, T3> Sequence<T1, T2, T3>(T1 _1, T2 _2, T3 _3)
	where T1 : IM where T2 : IM where T3 : IM =>
		new(_1, _2, _3);

	public static Sequence<T1, T2, T3, T4> Sequence<T1, T2, T3, T4>(T1 _1, T2 _2, T3 _3, T4 _4)
	where T1 : IM where T2 : IM where T3 : IM where T4 : IM =>
		new(_1, _2, _3, _4);

	public static Sequence<T1, T2, T3, T4, T5> Sequence<T1, T2, T3, T4, T5>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5)
	where T1 : IM where T2 : IM where T3 : IM where T4 : IM where T5 : IM =>
		new(_1, _2, _3, _4, _5);

	public static Sequence<T1, T2, T3, T4, T5, T6> Sequence<T1, T2, T3, T4, T5, T6>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5, T6 _6)
	where T1 : IM where T2 : IM where T3 : IM where T4 : IM where T5 : IM where T6 : IM =>
		new(_1, _2, _3, _4, _5, _6);

	public static Sequence<T1, T2, T3, T4, T5, T6, T7> Sequence<T1, T2, T3, T4, T5, T6, T7>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5, T6 _6, T7 _7)
	where T1 : IM where T2 : IM where T3 : IM where T4 : IM where T5 : IM where T6 : IM where T7 : IM =>
		new(_1, _2, _3, _4, _5, _6, _7);

	public static Sequence<T1, T2, T3, T4, T5, T6, T7, T8> Sequence<T1, T2, T3, T4, T5, T6, T7, T8>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5, T6 _6, T7 _7, T8 _8)
	where T1 : IM where T2 : IM where T3 : IM where T4 : IM where T5 : IM where T6 : IM where T7 : IM where T8 : IM =>
		new(_1, _2, _3, _4, _5, _6, _7, _8);

	public static Digit Digit() => new();
	public static AnyExceptNewline Any() => new();
	public static Word Word() => new();
	public static Whitespace Whitespace() => new();
	public static Not<T> Not<T>(T matchable) where T : IM => new(matchable);
	public static Literal Literal(Char c) => new(c);
	public static Literals Literals(ReadOnlyMemory<Char> s) => new(s);
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

public readonly struct Then<T1, T2, T3>(T1 _1, T2 _2, T3 _3) : IM
where T1 : IM
where T2 : IM
where T3 : IM
{
	public Boolean TryMatch(RosC input, out Int32 length)
	{
		length = 0;
		if (!_1.TryMatch(input, out var l1))
			return false;
		if (!_2.TryMatch(input[l1..], out var l2))
			return false;
		if (!_3.TryMatch(input[(l1 + l2)..], out var l3))
			return false;
		length = l1 + l2 + l3;
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
		MatchOne(input.SequenceEqual(text.Span), out length);
}

public readonly struct Range(CharRange charRange) : IM
{
	public Boolean TryMatch(RosC input, out Int32 length) =>
		MatchOne(AtLeastOne(input) && charRange.Contains(input[0]), out length);
}
