using NoAlloq;
using Regex;

namespace Tests;

using Ros = ReadOnlySpan<Char>;

public class TryMatch
{
	[Fact]
	public void AnyCharacterExceptNewline()
	{
		Span<Char> input = stackalloc[] {'\0'};
		for (var i = Char.MinValue; i != Char.MaxValue; i++)
		{
			if (Char.IsSurrogate(i))
				continue;
			input[0] = i;
			Assert.Equal(i != '\n', input.AnyCharacterExceptNewline(out _));
		}
	}

	[Fact]
	public void AnyOneOf()
	{
		Span<Char> input = stackalloc[] {'\0'};
		for (var i = Char.MinValue; i != Char.MaxValue; i++)
		{
			if (Char.IsSurrogate(i))
				continue;
			input[0] = i;
			Assert.Equal(
				i == 'a' | i == 'b',
				input.AnyOneOf("ab", out _)
			);
		}
	}

	[Fact]
	public void NotAnyOneOf()
	{
		Span<Char> input = stackalloc[] {'\0'};
		for (var i = Char.MinValue; i != Char.MaxValue; i++)
		{
			if (Char.IsSurrogate(i))
				continue;
			input[0] = i;
			Assert.Equal(
				i != 'a' & i != 'b',
				Regex.TryMatchChar.NotAnyOneOf(input, "ab", out _)
			);
		}
	}

	[Fact]
	public void AnyOneOfCharRanges()
	{
		Span<Char> input = stackalloc[] {'\0'};
		for (var i = Char.MinValue; i != Char.MaxValue; i++)
		{
			if (Char.IsSurrogate(i))
				continue;
			input[0] = i;
			Assert.Equal(
				i >= 'a' & i <= 'z',
				Regex.TryMatchChar.AnyOneOf(input, ('a', 'z'), out _)
			);
		}
	}
}
