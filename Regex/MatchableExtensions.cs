using static Regex.TryMatchChar;

namespace Regex;

using RosC = ReadOnlySpan<Char>;

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

	public static Not<T> Not<T>(this T matchable)
	where T : struct, IMatchable =>
		new(matchable);

	public static Quantity<T> Quantify<T>(this T matchable, Quantifier quantifier)
	where T : struct, IMatchable =>
		new(matchable, quantifier);

	public static Quantity<T> Quantify<T>(this T matchable, UInt32 exactly)
	where T : struct, IMatchable =>
		new(matchable, Quantifier.Exactly(exactly));

	public static Then<T1, Literal> Literal<T1>(this T1 first, Literal literal)
	where T1 : struct, IMatchable =>
		new(first, literal);

	public static Then<T1, Literals> Literals<T1>(this T1 first, Literals literal)
	where T1 : struct, IMatchable =>
		new(first, literal);

	public static Then<T1, Quantity<Digit>> Digit<T1>(this T1 first, Quantifier quantifier)
	where T1 : struct, IMatchable =>
		new(first, new(new(), quantifier));

	public static Boolean TryMatch<T>(this T matchable, RosC input, out (Int32 start, Int32 end) matched, Boolean startAnchor = true, Boolean endAnchor = false)
	where T : struct, IMatchable =>
		matchable.TryMatch(input, new DummyVisitHandler(), out matched, startAnchor, endAnchor);

	public static Boolean TryMatch<T, TVh>(this T matchable, RosC input, TVh visitHandler, out (Int32 start, Int32 end) matched, Boolean startAnchor = true, Boolean endAnchor = false)
	where T : struct, IMatchable where TVh : IVisitHandler
	{
		var currentInput = input;
		Boolean matchFound;
		Int32 length = 0;
		for (;;)
		{
			matchFound = matchable.TryMatch(currentInput, visitHandler, out var l);
			length += l;
			if (matchFound | startAnchor)
				break;
			if (currentInput.Length <= 1)
			{
				matched = (0, 0);
				return false;
			}
			currentInput = currentInput[1..];
		}
		if (endAnchor)
		{
			if (currentInput.Length != length)
			{
				matched = (0, 0);
				return false;
			}
			//matched = (input.Length - currentInput.Length, input.Length - currentInput.Length + length);
			//return true;
		}
		matched = (input.Length - currentInput.Length, input.Length - currentInput.Length + length);
		return matchFound;
	}
	
	public static Boolean TryMatch<T>(this T matchable, RosC input, out Int32 length) where T : IMatchable =>
		matchable.TryMatch(input, new DummyVisitHandler(), out length);
	
	private struct DummyVisitHandler : IVisitHandler
	{
		public void Handle<T>(in T value, RosC input) where T : IMatchable {}
	}
}
