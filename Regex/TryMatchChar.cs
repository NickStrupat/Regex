using NoAlloq;

namespace Regex;

using RosC = ReadOnlySpan<Char>;
using RosCr = ReadOnlySpan<CharRange>;

public static class TryMatchChar
{
	public static Boolean AnyCharacterExceptNewline(this RosC i, out Range matched)
	{
		return MatchOne(AtLeastOne(i) && i[0] != '\n', out matched);
	}

	public static Boolean Word(this RosC i, out Range matched)
	{
		return MatchOne(AtLeastOne(i, out var x) && Char.IsLetterOrDigit(x) | x == '_', out matched);
	}

	public static Boolean Digit(this RosC i, out Range matched)
	{
		return MatchOne(AtLeastOne(i) && Char.IsDigit(i[0]), out matched);
	}

	public static Boolean Whitespace(this RosC i, out Range matched)
	{
		return MatchOne(AtLeastOne(i) && Char.IsWhiteSpace(i[0]), out matched);
	}

	public static Boolean NotWord(this RosC i, out Range matched)
	{
		return MatchOne(AtLeastOne(i, out var x) && !(Char.IsLetterOrDigit(x) | x == '_'), out matched);
	}

	public static Boolean NotDigit(this RosC i, out Range matched)
	{
		return MatchOne(AtLeastOne(i) && !Char.IsDigit(i[0]), out matched);
	}

	public static Boolean NotWhitespace(this RosC i, out Range matched)
	{
		return MatchOne(AtLeastOne(i) && !Char.IsWhiteSpace(i[0]), out matched);
	}

	public static Boolean Literal(this RosC i, Char c, out Range matched)
	{
		return MatchOne(AtLeastOne(i) && i[0] == c, out matched);
	}

	public static Boolean AnyOneOf(this RosC i, RosC chars, out Range matched)
	{
		return MatchOne(AtLeastOne(i) && chars.Contains(i[0]), out matched);
	}

	public static Boolean NotAnyOneOf(this RosC i, RosC chars, out Range matched)
	{
		return MatchOne(AtLeastOne(i) && !chars.Contains(i[0]), out matched);
	}

	public static Boolean AnyOneOf(this RosC i, CharRange range, out Range matched)
	{
		return MatchOne(AtLeastOne(i, out var one) && range.Contains(one), out matched);
	}

	public static Boolean NotAnyOneOf(this RosC i, CharRange range, out Range matched)
	{
		return MatchOne(AtLeastOne(i, out var one) && !range.Contains(one), out matched);
	}

	public static Boolean AnyOneOf(this RosC i, RosCr ranges, out Range matched)
	{
		return MatchOne(AtLeastOne(i, out var one) && ranges.Any(r => r.Contains(one)), out matched);
	}

	public static Boolean NotAnyOneOf(this RosC i, RosCr ranges, out Range matched)
	{
		return MatchOne(AtLeastOne(i, out var one) && !ranges.Any(r => r.Contains(one)), out matched);
	}

	private static Boolean AtLeastOne(this RosC i) => i.Length != 0;
	private static Boolean AtLeastOne(this RosC i, out Char one) => Match(i.Length != 0, i[0], out one);

	public static Boolean AnyCharacterExceptNewline(this Span<Char> i, out Range m) => ((RosC)i).AnyCharacterExceptNewline(out m);
	public static Boolean Word(this Span<Char> i, out Range matched) => ((RosC)i).Word(out matched);
	public static Boolean Digit(this Span<Char> i, out Range matched) => ((RosC)i).Digit(out matched);
	public static Boolean Whitespace(this Span<Char> i, out Range matched) => ((RosC)i).Whitespace(out matched);
	public static Boolean NotWord(this Span<Char> i, out Range matched) => ((RosC)i).NotWord(out matched);
	public static Boolean NotDigit(this Span<Char> i, out Range matched) => ((RosC)i).NotDigit(out matched);
	public static Boolean NotWhitespace(this Span<Char> i, out Range matched) => ((RosC)i).NotWhitespace(out matched);
	public static Boolean Literal(this Span<Char> i, Char c, out Range matched) => ((RosC)i).Literal(c, out matched);
	public static Boolean AnyOneOf(this Span<Char> i, CharRange range, out Range matched) => ((RosC)i).AnyOneOf(range, out matched);
	public static Boolean NotAnyOneOf(this Span<Char> i, CharRange range, out Range matched) => ((RosC)i).NotAnyOneOf(range, out matched);
	public static Boolean AnyOneOf(this Span<Char> i, RosCr ranges, out Range matched) => ((RosC)i).AnyOneOf(ranges, out matched);
	public static Boolean NotAnyOneOf(this Span<Char> i, RosCr ranges, out Range matched) => ((RosC)i).NotAnyOneOf(ranges, out matched);
	public static Boolean AnyOneOf(this Span<Char> i, RosC chars, out Range matched) => ((RosC)i).AnyOneOf(chars, out matched);
	public static Boolean NotAnyOneOf(this Span<Char> i, RosC chars, out Range matched) => ((RosC)i).NotAnyOneOf(chars, out matched);

	private static Boolean MatchOne(Boolean result, out Range matched) => Match(result, ..1, out matched);

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
