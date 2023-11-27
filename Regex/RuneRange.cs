using System.Text;

namespace Regex;

public readonly record struct RuneRange(Rune Start, Rune End)
{
	public Boolean Contains(Rune c) => c >= Start && c <= End;
	public static implicit operator RuneRange(Rune c) => new(c, c);
	public static implicit operator RuneRange((Rune start, Rune end) range) => new(range.start, range.end);
}
