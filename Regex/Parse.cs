using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
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
	Boolean TryMatch<TVh>(RosC input, ref TVh visitHandler, out Int32 length) where TVh : IVisitHandler;
	
	//void HandleVisitFrom<TV>(ref TV visitor) where TV : IVisitor;// => visitor.Visit(this);
}

// public interface IVisitor
// {
// 	void Visit<T>(ref readonly T matchable);
// }
//
// public interface IMutatingVisitor : IVisitor
// {
// 	void IVisitor.Visit<T>(ref readonly T matchable)
// 	{
// 		var copy = matchable;
// 		Visit(ref Unsafe.AsRef(in copy));
// 	}
//
// 	void Visit<T>(ref T matchable);
// }

public readonly struct Quantity<T>(T matchable, Quantifier quantifier) : IM
where T : struct, IM
{
	public Boolean TryMatch<TVh>(RosC input, ref TVh visitHandler, out Int32 length) where TVh : IVisitHandler
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
			if (!matchable.TryMatch(input, ref visitHandler, out _))
				throw new ArgumentException("Input changed during matching", nameof(input));
			//visitHandler.Handle(matchable, input[..oneLength]);
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
	public Boolean TryMatch<TVh>(RosC input, ref TVh visitHandler, out Int32 length) where TVh : IVisitHandler
	{
		if (_1.TryMatch(input, ref visitHandler, out var l1))
		{
			length = l1;
			return true;
		}
		if (_2.TryMatch(input, ref visitHandler, out var l2))
		{
			length = l2;
			return true;
		}
		length = 0;
		return false;
	}
}

public readonly struct Then<T1, T2>(T1 _1, T2 _2) : IM
where T1 : struct, IM
where T2 : struct, IM
{
	public Boolean TryMatch<TVh>(RosC input, ref TVh visitHandler, out Int32 length) where TVh : IVisitHandler
	{
		length = 0;
		if (!_1.TryMatch(input, ref visitHandler, out var l1))
			return false;
		if (!_2.TryMatch(input[l1..], ref visitHandler, out var l2))
			return false;
		length = l1 + l2;
		return true;
	}
}

public struct AnyExceptNewline : IM
{
	public Boolean TryMatch<TVh>(RosC input, ref TVh visitHandler, out Int32 length) where TVh : IVisitHandler
	{
		if (!MatchOne(AtLeastOne(input) && input[0] != '\n', out length))
			return false;
		visitHandler.Handle(ref this, input[..length]);
		return true;
	}
}

public struct Word : IM
{
	public Boolean TryMatch<TVh>(RosC input, ref TVh visitHandler, out Int32 length) where TVh : IVisitHandler
	{
		if (!MatchOne(AtLeastOne(input, out var x) && Char.IsLetterOrDigit(x) | x == '_', out length))
			return false;
		visitHandler.Handle(ref this, input[..length]);
		return true;
	}
}

public struct Digit : IM
{
	public Boolean TryMatch<TVh>(RosC input, ref TVh visitHandler, out Int32 length) where TVh : IVisitHandler
	{
		if (!MatchOne(AtLeastOne(input, out var x) && Char.IsDigit(x), out length))
			return false;
		visitHandler.Handle(ref this, input[..length]);
		return true;
	}
}

public struct Whitespace : IM
{
	public Boolean TryMatch<TVh>(RosC input, ref TVh visitHandler, out Int32 length) where TVh : IVisitHandler
	{
		if (!MatchOne(AtLeastOne(input, out var x) && Char.IsWhiteSpace(x), out length))
			return false;
		visitHandler.Handle(ref this, input[..length]);
		return true;
	}
}

public struct Not<T>(T matchable) : IM where T : IM
{
	public Boolean TryMatch<TVh>(RosC input, ref TVh visitHandler, out Int32 length) where TVh : IVisitHandler
	{
		if (!MatchOne(AtLeastOne(input) && !matchable.TryMatch(input, out length), out length))
			return false;
		visitHandler.Handle(ref this, input[..length]);
		return true;
	}
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

public struct Literal(Char c) : IM
{
	public Boolean TryMatch<TVh>(RosC input, ref TVh visitHandler, out Int32 length) where TVh : IVisitHandler
	{
		if (!MatchOne(AtLeastOne(input) && input[0] == c, out length))
			return false;
		visitHandler.Handle(ref this, input[..length]);
		return true;
	}
	
	public static implicit operator Literal(Char c) => new(c);
}

public struct Literals(ReadOnlyMemory<Char> text) : IM
{
	public Literals(String text) : this(text.AsMemory()) {}

	public Boolean TryMatch<TVh>(RosC input, ref TVh visitHandler, out Int32 length) where TVh : IVisitHandler
	{
		if (!Match(input.StartsWith(text.Span), text.Length, out length))
			return false;
		visitHandler.Handle(ref this, input[..length]);
		return true;
	}
}

public struct Range(CharRange charRange) : IM
{
	public Boolean TryMatch<TVh>(RosC input, ref TVh visitHandler, out Int32 length) where TVh : IVisitHandler
	{
		if (!MatchOne(AtLeastOne(input) && charRange.Contains(input[0]), out length))
			return false;
		visitHandler.Handle(ref this, input[..length]);
		return true;
	}
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
