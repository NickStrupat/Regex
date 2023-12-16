using Regex;
using Xunit;

namespace Tests;

public class Quantifier_Tests
{
	[Fact]
	public void IfDefaultInitialized_MatchExactlyOne()
	{
		Assert.Equal(1u, default(Quantifier).Min);
		Assert.Equal(1u, default(Quantifier).Max);
	}

	[Fact]
	public void IfDefaultInitialized_MatchExactlyOne_WhenDeconstructed()
	{
		var (min, max) = default(Quantifier);
		Assert.Equal(1u, min);
		Assert.Equal(1u, max);
	}

	[Fact]
	public void IfConstructedWithMaxLessThanMin_Throw()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() => new Quantifier(2, 1));
	}

	[Theory]
	[InlineData(0u, 1u)]
	[InlineData(0u, null)]
	[InlineData(1u, null)]
	[InlineData(1u, 1u)]
	[InlineData(2u, null)]
	[InlineData(2u, 2u)]
	[InlineData(2u, 3u)]
	[InlineData(3u, 3u)]
	[InlineData(3u, null)]
	public void IfConstructedWithValues_MatchExactlyThoseValues(UInt32 min, UInt32? max)
	{
		var quantifier = new Quantifier(min, max);
		Assert.Equal(min, quantifier.Min);
		Assert.Equal(max, quantifier.Max);
	}
}
