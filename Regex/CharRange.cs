namespace Regex;

public readonly struct CharRange(Char start, Char end)
{
	public Boolean Contains(Char c) => c >= start && c <= end;
	public static implicit operator CharRange(Char c) => new(c, c);
	public static implicit operator CharRange((Char start, Char end) range) => new(range.start, range.end);
}