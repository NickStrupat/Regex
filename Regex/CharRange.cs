namespace Regex;

public readonly record struct CharRange(Char Start, Char End)
{
	public Boolean Contains(Char c) => c >= Start && c <= End;
	public static implicit operator CharRange(Char c) => new(c, c);
	public static implicit operator CharRange((Char start, Char end) range) => new(range.start, range.end);
}
