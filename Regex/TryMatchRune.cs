using System.Text;
using NoAlloq;

namespace Regex;

using S = Span<Rune>;
using RosR = ReadOnlySpan<Rune>;

public static class TryMatchRune
{
	private static readonly Rune Nl = new('\n');
	private static readonly Rune Us = new('_');

	public static Boolean AnyCharacterExceptNewline(this RosR i, out Range matched)
	{
		return MatchOne(AtLeastOne(i) && i[0] != Nl, out matched);
	}

	public static Boolean Word(this RosR i, out Range matched)
	{
		return MatchOne(AtLeastOne(i, out var x) && Rune.IsLetterOrDigit(x) | x == Us, out matched);
	}

	public static Boolean Digit(this RosR i, out Range matched)
	{
		return MatchOne(AtLeastOne(i) && Rune.IsDigit(i[0]), out matched);
	}

	public static Boolean Whitespace(this RosR i, out Range matched)
	{
		return MatchOne(AtLeastOne(i) && Rune.IsWhiteSpace(i[0]), out matched);
	}

	public static Boolean NotWord(this RosR i, out Range matched)
	{
		return MatchOne(AtLeastOne(i, out var x) && !(Rune.IsLetterOrDigit(x) | x == Us), out matched);
	}

	public static Boolean NotDigit(this RosR i, out Range matched)
	{
		return MatchOne(AtLeastOne(i) && !Rune.IsDigit(i[0]), out matched);
	}

	public static Boolean NotWhitespace(this RosR i, out Range matched)
	{
		return MatchOne(AtLeastOne(i) && !Rune.IsWhiteSpace(i[0]), out matched);
	}

	public static Boolean Literal(this RosR i, Rune r, out Range matched)
	{
		return MatchOne(AtLeastOne(i) && i[0] == r, out matched);
	}

	public static Boolean AnyOneOf(this RosR i, RosR runes, out Range matched)
	{
		return MatchOne(AtLeastOne(i) && runes.Contains(i[0]), out matched);
	}

	public static Boolean NotAnyOneOf(this RosR i, RosR runes, out Range matched)
	{
		return MatchOne(AtLeastOne(i) && !runes.Contains(i[0]), out matched);
	}

	public static Boolean AnyOneOf(this RosR i, RuneRange range, out Range matched)
	{
		return MatchOne(AtLeastOne(i, out var one) && range.Contains(one), out matched);
	}

	public static Boolean NotAnyOneOf(this RosR i, RuneRange range, out Range matched)
	{
		return MatchOne(AtLeastOne(i, out var one) && !range.Contains(one), out matched);
	}

	public static Boolean AnyOneOf(this RosR i, ReadOnlySpan<RuneRange> ranges, out Range matched)
	{
		return MatchOne(AtLeastOne(i, out var one) && ranges.Any(r => r.Contains(one)), out matched);
	}

	public static Boolean NotAnyOneOf(this RosR i, ReadOnlySpan<RuneRange> ranges, out Range matched)
	{
		return MatchOne(AtLeastOne(i, out var one) && !ranges.Any(r => r.Contains(one)), out matched);
	}

	private static Boolean AtLeastOne(this RosR i) => i.Length != 0;
	private static Boolean AtLeastOne(this RosR i, out Rune one) => Match(i.Length != 0, i[0], out one);

	public static Boolean AnyCharacterExceptNewline(this S i, out Range matched) => AnyCharacterExceptNewline((RosR)i, out matched);
	public static Boolean Word(this S i, out Range matched) => Word((RosR)i, out matched);
	public static Boolean Digit(this S i, out Range matched) => Digit((RosR)i, out matched);
	public static Boolean Whitespace(this S i, out Range matched) => Whitespace((RosR)i, out matched);
	public static Boolean NotWord(this S i, out Range matched) => NotWord((RosR)i, out matched);
	public static Boolean NotDigit(this S i, out Range matched) => NotDigit((RosR)i, out matched);
	public static Boolean NotWhitespace(this S i, out Range matched) => NotWhitespace((RosR)i, out matched);
	public static Boolean Literal(this S i, Rune c, out Range matched) => Literal((RosR)i, c, out matched);
	public static Boolean AnyOneOf(this S i, RuneRange range, out Range matched) => AnyOneOf((RosR)i, range, out matched);
	public static Boolean NotAnyOneOf(this S i, RuneRange range, out Range matched) => NotAnyOneOf((RosR)i, range, out matched);
	public static Boolean AnyOneOf(this S i, ReadOnlySpan<RuneRange> ranges, out Range matched) => AnyOneOf((RosR)i, ranges, out matched);
	public static Boolean NotAnyOneOf(this S i, ReadOnlySpan<RuneRange> ranges, out Range matched) => NotAnyOneOf((RosR)i, ranges, out matched);
	public static Boolean AnyOneOf(this S i, RosR runes, out Range matched) => AnyOneOf((RosR)i, runes, out matched);
	public static Boolean NotAnyOneOf(this S i, RosR runes, out Range matched) => NotAnyOneOf((RosR)i, runes, out matched);

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
