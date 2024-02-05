using Regex;
using Xunit;
using static Regex.Parser;

namespace Tests;

public class Parse_Tests
{
    [Fact]
    public void AnyExceptNewline_Test()
    {
        for (var i = Char.MinValue; i != Char.MaxValue; i++)
        {
            if (Char.IsSurrogate(i))
                continue;
            var isNewline = i == '\n';
            Assert.Equal(!isNewline, Any().TryMatch([i], out var length));
            Assert.Equal(!isNewline ? 1 : 0, length);
        }
    }
    
    [Fact]
    public void Literal_Test()
    {
        for (var i = Char.MinValue; i != Char.MaxValue; i++)
        {
            if (Char.IsSurrogate(i))
                continue;
            var a = i == 'a';
            Assert.Equal(
                a,
                Literal('a').TryMatch([i], out var length)
            );
            Assert.Equal(a ? 1 : 0, length);
        }
    }
    
    [Fact]
    public void Literals_Test()
    {
        var literals = Literals("ab");
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

    [Theory]
    [InlineData("a", "a", true)]
    [InlineData("a", "b", false)]
    [InlineData("a", "ab", true)]
    [InlineData("a", "ba", false)]
    [InlineData("a", "abc", true)]
    [InlineData("a", "bac", false)]
    public void Literals_MoreTests(String literal, String input, Boolean isMatchExpected)
    {
        var literals = Literals(literal);
        Assert.Equal(
            isMatchExpected,
            literals.TryMatch(input, out var length)
        );
    }
    
    [Fact]
    public void Range_Test()
    {
        var range = Range(('a', 'z'));
        for (var i = Char.MinValue; i != Char.MaxValue; i++)
        {
            if (Char.IsSurrogate(i))
                continue;
            var inRange = i is >= 'a' and <= 'z';
            Assert.Equal(
                inRange,
                range.TryMatch([i], out var length)
            );
            Assert.Equal(inRange ? 1 : 0, length);
        }
    }
    
    [Fact]
    public void Not_Test()
    {
        var not = Not(Literal('a'));
        for (var i = Char.MinValue; i != Char.MaxValue; i++)
        {
            if (Char.IsSurrogate(i))
                continue;
            var notA = i != 'a';
            Assert.Equal(
                notA,
                not.TryMatch([i], out var length)
            );
            Assert.Equal(notA ? 1 : 0, length);
        }
    }
    
    [Fact]
    public void Digit_Test()
    {
        var digit = Digit();
        for (var i = Char.MinValue; i != Char.MaxValue; i++)
        {
            if (Char.IsSurrogate(i))
                continue;
            var isDigit = Char.IsDigit(i);
            Assert.Equal(
                isDigit,
                digit.TryMatch([i], out var length)
            );
            Assert.Equal(isDigit ? 1 : 0, length);
        }
    }
    
    [Fact]
    public void Whitespace_Test()
    {
        var whitespace = Whitespace();
        for (var i = Char.MinValue; i != Char.MaxValue; i++)
        {
            if (Char.IsSurrogate(i))
                continue;
            var isWhitespace = Char.IsWhiteSpace(i);
            Assert.Equal(
                isWhitespace,
                whitespace.TryMatch([i], out var length)
            );
            Assert.Equal(isWhitespace ? 1 : 0, length);
        }
    }
    
    [Fact]
    public void Word_Test()
    {
        var word = Word();
        for (var i = Char.MinValue; i != Char.MaxValue; i++)
        {
            if (Char.IsSurrogate(i))
                continue;
            var isWord = Char.IsLetterOrDigit(i) | i == '_';
            Assert.Equal(
                isWord,
                word.TryMatch([i], out var length)
            );
            Assert.Equal(isWord ? 1 : 0, length);
        }
    }
    
    [Fact]
    public void AnyOneOf_Test()
    {
        var anyOneOf = Literal('a').Or(Literal('b')).Or(Literal('c'));
        for (var i = Char.MinValue; i != Char.MaxValue; i++)
        {
            if (Char.IsSurrogate(i))
                continue;
            var isAnyOneOf = i is 'a' or 'b' or 'c';
            Assert.Equal(
                isAnyOneOf,
                anyOneOf.TryMatch([i], out var length)
            );
            Assert.Equal(isAnyOneOf ? 1 : 0, length);
        }
    }
    
    [Fact]
    public void NotAnyOneOf_Test()
    {
        var notAnyOneOf = Not(Literal('a').Or(Literal('b')).Or(Literal('c')));
        for (var i = Char.MinValue; i != Char.MaxValue; i++)
        {
            if (Char.IsSurrogate(i))
                continue;
            var isNotAnyOneOf = i is not 'a' and not 'b' and not 'c';
            Assert.Equal(
                isNotAnyOneOf,
                notAnyOneOf.TryMatch([i], out var length)
            );
            Assert.Equal(isNotAnyOneOf ? 1 : 0, length);
        }
    }
}