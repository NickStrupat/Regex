namespace Regex;

public readonly struct CharRange(Char start, Char end)
{
	public Char Start { get; } = start;
	public Char End { get; } = end >= start ? end : throw new ArgumentOutOfRangeException(nameof(end), "end must be greater than or equal to start");
	public Boolean Contains(Char c) => c >= Start && c <= End;
	public static implicit operator CharRange(Char c) => new(c, c);
	public static implicit operator CharRange((Char start, Char end) range) => new(range.start, range.end);
}
