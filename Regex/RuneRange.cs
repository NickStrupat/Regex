using System.Text;

namespace Regex;

public readonly struct RuneRange(Rune start, Rune end)
{
	public Boolean Contains(Rune c) => c >= start && c <= end;
	public static implicit operator RuneRange(Rune c) => new(c, c);
	public static implicit operator RuneRange((Rune start, Rune end) range) => new(range.start, range.end);
}
