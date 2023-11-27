using NoAlloq;

namespace Regex;

using RosC = ReadOnlySpan<Char>;
using RosCr = ReadOnlySpan<CharRange>;

public static class TryMatchChar
{
	public static Boolean AnyCharacterExceptNewline(this RosC i, out UInt32 length)
	{
		return MatchOne(AtLeastOne(i) && i[0] != '\n', out length);
	}

	public static Boolean Word(this RosC i, out UInt32 length)
	{
		return MatchOne(AtLeastOne(i, out var x) && Char.IsLetterOrDigit(x) | x == '_', out length);
	}

	public static Boolean Digit(this RosC i, out UInt32 length)
	{
		return MatchOne(AtLeastOne(i) && Char.IsDigit(i[0]), out length);
	}

	public static Boolean Whitespace(this RosC i, out UInt32 length)
	{
		return MatchOne(AtLeastOne(i) && Char.IsWhiteSpace(i[0]), out length);
	}

	public static Boolean NotWord(this RosC i, out UInt32 length)
	{
		return MatchOne(AtLeastOne(i, out var x) && !(Char.IsLetterOrDigit(x) | x == '_'), out length);
	}

	public static Boolean NotDigit(this RosC i, out UInt32 length)
	{
		return MatchOne(AtLeastOne(i) && !Char.IsDigit(i[0]), out length);
	}

	public static Boolean NotWhitespace(this RosC i, out UInt32 length)
	{
		return MatchOne(AtLeastOne(i) && !Char.IsWhiteSpace(i[0]), out length);
	}

	public static Boolean Literal(this RosC i, Char c, out UInt32 length)
	{
		return MatchOne(AtLeastOne(i) && i[0] == c, out length);
	}

	public static Boolean AnyOneOf(this RosC i, RosC chars, out UInt32 length)
	{
		return MatchOne(AtLeastOne(i) && chars.Contains(i[0]), out length);
	}

	public static Boolean NotAnyOneOf(this RosC i, RosC chars, out UInt32 length)
	{
		return MatchOne(AtLeastOne(i) && !chars.Contains(i[0]), out length);
	}

	public static Boolean AnyOneOf(this RosC i, CharRange range, out UInt32 length)
	{
		return MatchOne(AtLeastOne(i, out var one) && range.Contains(one), out length);
	}

	public static Boolean NotAnyOneOf(this RosC i, CharRange range, out UInt32 length)
	{
		return MatchOne(AtLeastOne(i, out var one) && !range.Contains(one), out length);
	}

	public static Boolean AnyOneOf(this RosC i, RosCr ranges, out UInt32 length)
	{
		return MatchOne(AtLeastOne(i, out var one) && ranges.Any(r => r.Contains(one)), out length);
	}

	public static Boolean NotAnyOneOf(this RosC i, RosCr ranges, out UInt32 length)
	{
		return MatchOne(AtLeastOne(i, out var one) && !ranges.Any(r => r.Contains(one)), out length);
	}

	public static Boolean AnyMinToMaxOf(this RosC i, UInt32 min, UInt32? max, Char c, out UInt32 length)
	{
		return i.AnyMinToMaxOf(min, max, (CharRange)c, out length);
	}

	public static Boolean AnyMinToMaxOf(this RosC i, UInt32 min, UInt32? max, CharRange range, out UInt32 length)
	{
		return i.AnyMinToMaxOf(min, max, stackalloc CharRange[] { range }, out length);
	}

	public static Boolean AnyMinToMaxOf(this RosC i, UInt32 min, UInt32? max, RosCr ranges, out UInt32 length)
	{
		if (max < min)
			throw new ArgumentOutOfRangeException(nameof(max), $"{nameof(max)} must be greater than or equal to {nameof(min)}");
		length = 0;
		UInt64 matchesFound = 0;
		for (;;)
		{
			if (!i.AnyOneOf(ranges, out var oneLength))
				break;
			if (matchesFound + 1 > max)
				break;
			length += oneLength;
			matchesFound++;
			i = i[(Int32) oneLength..];
		}
		return matchesFound >= min & (max is null || matchesFound <= max);
	}

	private static Boolean AtLeastOne(this RosC i) => i.Length != 0;
	private static Boolean AtLeastOne(this RosC i, out Char one) => Match(i.Length != 0, i.FirstOrDefault(), out one);

	public static Boolean AnyCharacterExceptNewline(this Span<Char> i, out UInt32 length) => ((RosC)i).AnyCharacterExceptNewline(out length);
	public static Boolean Word(this Span<Char> i, out UInt32 length) => ((RosC)i).Word(out length);
	public static Boolean Digit(this Span<Char> i, out UInt32 length) => ((RosC)i).Digit(out length);
	public static Boolean Whitespace(this Span<Char> i, out UInt32 length) => ((RosC)i).Whitespace(out length);
	public static Boolean NotWord(this Span<Char> i, out UInt32 length) => ((RosC)i).NotWord(out length);
	public static Boolean NotDigit(this Span<Char> i, out UInt32 length) => ((RosC)i).NotDigit(out length);
	public static Boolean NotWhitespace(this Span<Char> i, out UInt32 length) => ((RosC)i).NotWhitespace(out length);
	public static Boolean Literal(this Span<Char> i, Char c, out UInt32 length) => ((RosC)i).Literal(c, out length);
	public static Boolean AnyOneOf(this Span<Char> i, CharRange range, out UInt32 length) => ((RosC)i).AnyOneOf(range, out length);
	public static Boolean NotAnyOneOf(this Span<Char> i, CharRange range, out UInt32 length) => ((RosC)i).NotAnyOneOf(range, out length);
	public static Boolean AnyOneOf(this Span<Char> i, RosCr ranges, out UInt32 length) => ((RosC)i).AnyOneOf(ranges, out length);
	public static Boolean NotAnyOneOf(this Span<Char> i, RosCr ranges, out UInt32 length) => ((RosC)i).NotAnyOneOf(ranges, out length);
	public static Boolean AnyOneOf(this Span<Char> i, RosC chars, out UInt32 length) => ((RosC)i).AnyOneOf(chars, out length);
	public static Boolean NotAnyOneOf(this Span<Char> i, RosC chars, out UInt32 length) => ((RosC)i).NotAnyOneOf(chars, out length);

	private static Boolean MatchOne(Boolean result, out UInt32 length) => Match(result, 1u, out length);

	private static Boolean Match<T>(Boolean result, T ifMatched, out T matched)
	{
		if (result)
		{
			matched = ifMatched;
			return true;
		}
		matched = default!;
		return false;
	}
}
