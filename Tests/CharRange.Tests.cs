using Regex;
using Xunit;

namespace Tests;

public class CharRange_Tests
{
	[Fact]
	public void IfDefaultInitialized_StartIsZero() => Assert.Equal('\0', default(CharRange).Start);

	[Fact]
	public void IfDefaultInitialized_EndIsZero() => Assert.Equal('\0', default(CharRange).End);

	[Fact]
	public void IfConstructedWithEndLessThanStart_Throw() => Assert.Throws<ArgumentOutOfRangeException>(() => new CharRange('b', 'a'));

	[Theory]
	[InlineData('a', 'a')]
	[InlineData('a', 'b')]
	[InlineData('a', 'c')]
	[InlineData('b', 'b')]
	[InlineData('b', 'c')]
	[InlineData('c', 'c')]
	public void IfConstructedWithValues_StartAndEndMatchValues(Char start, Char end)
	{
		var charRange = new CharRange(start, end);
		Assert.Equal(start, charRange.Start);
		Assert.Equal(end, charRange.End);
	}

	[Theory]
	[InlineData('a', 'a', 'a')]
	[InlineData('a', 'b', 'a')]
	[InlineData('a', 'b', 'b')]
	[InlineData('a', 'c', 'a')]
	[InlineData('a', 'c', 'b')]
	[InlineData('a', 'c', 'c')]
	[InlineData('1', '3', '2')]
	public void IfConstructedWithValues_Contains(Char start, Char end, Char c)
	{
		var charRange = new CharRange(start, end);
		Assert.True(charRange.Contains(c));
	}

	[Theory]
	[InlineData('a', 'a', 'A')]
	[InlineData('a', 'b', 'A')]
	[InlineData('a', 'b', 'B')]
	[InlineData('a', 'c', 'A')]
	[InlineData('a', 'c', 'B')]
	[InlineData('a', 'c', 'C')]
	[InlineData('1', '3', '4')]
	public void IfConstructedWithValues_DoesNotContain(Char start, Char end, Char c)
	{
		var charRange = new CharRange(start, end);
		Assert.False(charRange.Contains(c));
	}
}
