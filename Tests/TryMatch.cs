// using System.Text;
// using Regex;
//
// namespace Tests;
//
// public class TryMatch
// {
// 	[Fact]
// 	public void AnyCharacterExceptNewline()
// 	{
// 		Span<Char> input = stackalloc[] {'\0'};
// 		for (var i = Char.MinValue; i != Char.MaxValue; i++)
// 		{
// 			if (Char.IsSurrogate(i))
// 				continue;
// 			input[0] = i;
// 			Assert.Equal(i != '\n', TryMatchRune.AnyCharacterExceptNewline((Span<Rune>) input, out _));
// 		}
// 	}
//
// 	[Fact]
// 	public void AnyOneOf()
// 	{
// 		Span<Char> input = stackalloc[] {'\0'};
// 		for (var i = Char.MinValue; i != Char.MaxValue; i++)
// 		{
// 			if (Char.IsSurrogate(i))
// 				continue;
// 			input[0] = i;
// 			Assert.Equal(
// 				i == 'a' | i == 'b',
// 				input.AnyOneOf("ab", out _)
// 			);
// 		}
// 	}
//
// 	[Fact]
// 	public void NotAnyOneOf()
// 	{
// 		Span<Char> input = stackalloc[] {'\0'};
// 		for (var i = Char.MinValue; i != Char.MaxValue; i++)
// 		{
// 			if (Char.IsSurrogate(i))
// 				continue;
// 			input[0] = i;
// 			Assert.Equal(
// 				i != 'a' & i != 'b',
// 				input.NotAnyOneOf("ab", out _)
// 			);
// 		}
// 	}
//
// 	[Fact]
// 	public void AnyOneOfCharRanges()
// 	{
// 		Span<Char> input = stackalloc[] {'\0'};
// 		for (var i = Char.MinValue; i != Char.MaxValue; i++)
// 		{
// 			if (Char.IsSurrogate(i))
// 				continue;
// 			input[0] = i;
// 			Assert.Equal(
// 				i >= 'a' & i <= 'z',
// 				input.AnyOneOf(('a', 'z'), out _)
// 			);
// 		}
// 	}
//
// 	[Theory]
// 	[InlineData("", 0, 1, true, 0)]
// 	[InlineData("a", 0, 1, true, 1)]
// 	[InlineData("a", 1, 1, true, 1)]
// 	[InlineData("f", 1, 1, false)]
// 	[InlineData("x", 0, 1, true, 1)]
// 	[InlineData("aa", 2, 2, true, 2)]
// 	[InlineData("bb", 3, 3, false)]
// 	[InlineData("abcd", 2, 4, true, 4)]
// 	[InlineData("abcde", 2, 4, true, 4)]
// 	[InlineData("aaaaaaaa", 0, -1, true, 8)]
// 	[InlineData("aaaaaaaa", 9, -1, false)]
// 	public void AnyMinToMaxOf(String input, UInt32 min, Int32 max, Boolean expected, UInt32 expectedLength = 0)
// 	{
// 		var fixedMax = max < 0 ? null : (UInt32?)max;
// 		Span<CharRange> ranges = stackalloc CharRange[] {('a', 'e'), ('x', 'z')};
// 		var actual = input.AsSpan().AnyMinToMaxOf(min, fixedMax, ranges, out var length);
// 		Assert.Equal(expected, actual);
// 		if (expected)
// 			Assert.Equal(expectedLength, length);
// 	}
// }
