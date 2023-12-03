using static Regex.TryMatchChar;

namespace Regex;

public static class MatchableExtensions
{
	public static Or<T1, T2> Or<T1, T2>(this T1 first, T2 second)
	where T1 : struct, IMatchable
	where T2 : struct, IMatchable =>
		new(first, second);

	public static Then<T1, T2> Then<T1, T2>(this T1 first, T2 second)
	where T1 : struct, IMatchable
	where T2 : struct, IMatchable =>
		new(first, second);

	// public static Then<T1, Literal> Then<T1>(this T1 first, Char literal)
	// where T1 : struct, IMatchable =>
	// 	new(first, new(literal));
	//
	// public static Then<T1, Literals> Then<T1>(this T1 first, ReadOnlyMemory<Char> literal)
	// where T1 : struct, IMatchable =>
	// 	new(first, new(literal));

	public static Not<T> Not<T>(this T matchable)
	where T : struct, IMatchable =>
		new(matchable);

	public static Quantity<T> Quantify<T>(this T matchable, Quantifier quantifier)
	where T : struct, IMatchable =>
		new(matchable, quantifier);

	public static Quantity<T> Quantify<T>(this T matchable, UInt32 exactly)
	where T : struct, IMatchable =>
		new(matchable, Quantifier.Exactly(exactly));
}
