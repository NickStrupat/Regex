namespace Regex;

public readonly struct Quantifier(UInt32 min, UInt32? max)
{
	public UInt32 Min { get; } = min;
	public UInt32? Max { get; } = max is null || max >= min ? max : throw MaxEx();

	private static ArgumentOutOfRangeException MaxEx() =>
		new(nameof(max), $"{nameof(max)} must be greater than or equals to {nameof(min)}");

	public void Deconstruct(out UInt32 min, out UInt32? max) => (min, max) = (Min, Max);

	public static Quantifier ZeroOrOne { get; } = new(0, 1);
	public static Quantifier ZeroOrMore { get; } = new(0, null);
	public static Quantifier OneOrMore { get; } = new(1, null);
	public static Quantifier Exactly(UInt32 count) => new(count, count);
	public static Quantifier AtLeast(UInt32 min) => new(min, null);
	public static Quantifier AtMost(UInt32 max) => new(0, max);
	public static Quantifier Between(UInt32 min, UInt32 max) => new(min, max);

	public static implicit operator Quantifier(UInt32 exactly) => new(exactly, exactly);
	public static implicit operator Quantifier((UInt32 min, UInt32 max) range) => new(range.min, range.max);
	public static implicit operator Quantifier(System.Range range)
	{
		if (range.Start.IsFromEnd)
			throw new ArgumentOutOfRangeException(nameof(range), "Range start must not be from end");
		if ((range.End.IsFromEnd & range.End.Value != 0))
			throw new ArgumentOutOfRangeException(nameof(range), "Range end must not be from end unless it is 0 (indicating unlimited)");
		var start = (UInt32) range.Start.Value;
		var end = range.End.IsFromEnd & range.End.Value == 0 ? null : (UInt32?) range.End.Value;
		return new Quantifier(start, end);
	}
}
