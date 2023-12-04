namespace Regex;

using IM = IMatchable;

public readonly struct Alternation<T1, T2>(T1 _1, T2 _2) : IM
where T1 : IM where T2 : IM
{
	public Boolean TryMatch(ReadOnlySpan<Char> i, out Int32 l) =>
		_1.TM(i, out l) ?? _2.TM(i, out l) ?? false;
}

public readonly struct Alternation<T1, T2, T3>(T1 _1, T2 _2, T3 _3) : IM
where T1 : IM where T2 : IM where T3 : IM
{
	public Boolean TryMatch(ReadOnlySpan<Char> i, out Int32 l) =>
		_1.TM(i, out l) ?? _2.TM(i, out l) ?? _3.TM(i, out l) ?? false;
}

public readonly struct Alternation<T1, T2, T3, T4>(T1 _1, T2 _2, T3 _3, T4 _4) : IM
where T1 : IM where T2 : IM where T3 : IM where T4 : IM
{
	public Boolean TryMatch(ReadOnlySpan<Char> i, out Int32 l) =>
		_1.TM(i, out l) ?? _2.TM(i, out l) ?? _3.TM(i, out l) ?? _4.TM(i, out l) ?? false;
}

public readonly struct Alternation<T1, T2, T3, T4, T5>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5) : IM
where T1 : IM where T2 : IM where T3 : IM where T4 : IM where T5 : IM
{
	public Boolean TryMatch(ReadOnlySpan<Char> i, out Int32 l) =>
		_1.TM(i, out l) ?? _2.TM(i, out l) ?? _3.TM(i, out l) ?? _4.TM(i, out l) ?? _5.TM(i, out l) ?? false;
}

public readonly struct Alternation<T1, T2, T3, T4, T5, T6>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5, T6 _6) : IM
where T1 : IM where T2 : IM where T3 : IM where T4 : IM where T5 : IM where T6 : IM
{
	public Boolean TryMatch(ReadOnlySpan<Char> i, out Int32 l) =>
		_1.TM(i, out l) ?? _2.TM(i, out l) ?? _3.TM(i, out l) ?? _4.TM(i, out l) ?? _5.TM(i, out l) ?? _6.TM(i, out l) ?? false;
}

public readonly struct Alternation<T1, T2, T3, T4, T5, T6, T7>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5, T6 _6, T7 _7) : IM
where T1 : IM where T2 : IM where T3 : IM where T4 : IM where T5 : IM where T6 : IM where T7 : IM
{
	public Boolean TryMatch(ReadOnlySpan<Char> i, out Int32 l) =>
		_1.TM(i, out l) ?? _2.TM(i, out l) ?? _3.TM(i, out l) ?? _4.TM(i, out l) ?? _5.TM(i, out l) ?? _6.TM(i, out l) ?? _7.TM(i, out l) ?? false;
}

public readonly struct Alternation<T1, T2, T3, T4, T5, T6, T7, T8>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5, T6 _6, T7 _7, T8 _8) : IM
where T1 : IM where T2 : IM where T3 : IM where T4 : IM where T5 : IM where T6 : IM where T7 : IM where T8 : IM
{
	public Boolean TryMatch(ReadOnlySpan<Char> i, out Int32 l) =>
		_1.TM(i, out l) ?? _2.TM(i, out l) ?? _3.TM(i, out l) ?? _4.TM(i, out l) ?? _5.TM(i, out l) ?? _6.TM(i, out l) ?? _7.TM(i, out l) ?? _8.TM(i, out l) ?? false;
}

file static class SequenceExtensions
{
	internal static Boolean? TM<T>(this T matchable, ReadOnlySpan<Char> input, out Int32 length) where T : IM
	{
		if (matchable.TryMatch(input, out length))
			return true;
		return null;
	}

	internal static Boolean R(this Int32? _, out Int32 length)
	{
		length = _ ?? 0;
		return _ is not null;
	}
}
