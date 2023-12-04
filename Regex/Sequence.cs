namespace Regex;

using IM = IMatchable;

public readonly struct Sequence<T1, T2>(T1 _1, T2 _2) : IM
where T1 : IM where T2 : IM
{
	public Boolean TryMatch(ReadOnlySpan<Char> input, out Int32 length) =>
		(length = 0).TM(_1, input)?.TM(_2, input).R(out length) ?? false;
}

public readonly struct Sequence<T1, T2, T3>(T1 _1, T2 _2, T3 _3) : IM
where T1 : IM where T2 : IM where T3 : IM
{
	public Boolean TryMatch(ReadOnlySpan<Char> input, out Int32 length) =>
		(length = 0).TM(_1, input)?.TM(_2, input)?.TM(_3, input).R(out length) ?? false;
}

public readonly struct Sequence<T1, T2, T3, T4>(T1 _1, T2 _2, T3 _3, T4 _4) : IM
where T1 : IM where T2 : IM where T3 : IM where T4 : IM
{
	public Boolean TryMatch(ReadOnlySpan<Char> i, out Int32 length) =>
		(length = 0).TM(_1, i)?.TM(_2, i)?.TM(_3, i)?.TM(_4, i).R(out length) ?? false;
}

public readonly struct Sequence<T1, T2, T3, T4, T5>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5) : IM
where T1 : IM where T2 : IM where T3 : IM where T4 : IM where T5 : IM
{
	public Boolean TryMatch(ReadOnlySpan<Char> i, out Int32 length) =>
		(length = 0).TM(_1, i)?.TM(_2, i)?.TM(_3, i)?.TM(_4, i)?.TM(_5, i).R(out length) ?? false;
}

public readonly struct Sequence<T1, T2, T3, T4, T5, T6>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5, T6 _6) : IM
where T1 : IM where T2 : IM where T3 : IM where T4 : IM where T5 : IM where T6 : IM
{
	public Boolean TryMatch(ReadOnlySpan<Char> i, out Int32 length) =>
		(length = 0).TM(_1, i)?.TM(_2, i)?.TM(_3, i)?.TM(_4, i)?.TM(_5, i)?.TM(_6, i).R(out length) ?? false;
}

public readonly struct Sequence<T1, T2, T3, T4, T5, T6, T7>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5, T6 _6, T7 _7) : IM
where T1 : IM where T2 : IM where T3 : IM where T4 : IM where T5 : IM where T6 : IM where T7 : IM
{
	public Boolean TryMatch(ReadOnlySpan<Char> i, out Int32 length) =>
		(length = 0).TM(_1, i)?.TM(_2, i)?.TM(_3, i)?.TM(_4, i)?.TM(_5, i)?.TM(_6, i)?.TM(_7, i).R(out length) ?? false;
}

public readonly struct Sequence<T1, T2, T3, T4, T5, T6, T7, T8>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5, T6 _6, T7 _7, T8 _8) : IM
where T1 : IM where T2 : IM where T3 : IM where T4 : IM where T5 : IM where T6 : IM where T7 : IM where T8 : IM
{
	public Boolean TryMatch(ReadOnlySpan<Char> i, out Int32 length) =>
		(length = 0).TM(_1, i)?.TM(_2, i)?.TM(_3, i)?.TM(_4, i)?.TM(_5, i)?.TM(_6, i)?.TM(_7, i)?.TM(_8, i).R(out length) ?? false;
}

file static class SequenceExtensions
{
	internal static Int32? TM<T>(this Int32 length, T matchable, ReadOnlySpan<Char> input) where T : IM
	{
		if (!matchable.TryMatch(input[length..], out var l))
			return null;
		length += l;
		return length;
	}

	internal static Boolean R(this Int32? _, out Int32 length)
	{
		length = _ ?? 0;
		return _ is not null;
	}
}
