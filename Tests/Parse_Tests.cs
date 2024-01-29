using Regex;
using Xunit;

namespace Tests;

public class Parse_Tests
{
    [Fact]
    public void AnyExceptNewline()
    {
        for (var i = Char.MinValue; i != Char.MaxValue; i++)
        {
            if (Char.IsSurrogate(i))
                continue;
            var isNewline = i == '\n';
            Assert.Equal(!isNewline, Regex.Parser.Any().TryMatch([i], out var length));
            Assert.Equal(!isNewline ? 1 : 0, length);
        }
    }
    
    [Fact]
    public void Literal()
    {
        for (var i = Char.MinValue; i != Char.MaxValue; i++)
        {
            if (Char.IsSurrogate(i))
                continue;
            var a = i == 'a';
            Assert.Equal(
                a,
                Regex.Parser.Literal('a').TryMatch([i], out var length)
            );
            Assert.Equal(a ? 1 : 0, length);
        }
    }
    
    [Fact]
    public void Literals()
    {
        //var literals = Parser.Literal('a').Then(Parser.Literal('b'));
        var literals = Parser.Literals("ab");
        for (var i = Char.MinValue; i != Char.MaxValue; i++)
        {
            if (Char.IsSurrogate(i))
                continue;
            var a = i == 'a';
            Assert.Equal(
                a,
                literals.TryMatch([i, 'b'], out var length)
            );
            Assert.Equal(a ? 2 : 0, length);
        }
    }
}