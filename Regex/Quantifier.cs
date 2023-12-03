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
	public static implicit operator Quantifier(System.Range range) =>
		range.Start.IsFromEnd | range.End.IsFromEnd
			? throw new ArgumentOutOfRangeException(nameof(range), "range must be from start to end")
			: new Quantifier((UInt32) range.Start.Value, (UInt32) range.End.Value);
}
