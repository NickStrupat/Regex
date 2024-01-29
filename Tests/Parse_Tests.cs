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
    
    [Fact]
    public void Range()
    {
        var range = Parser.Range(('a', 'z'));
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
    public void Not()
    {
        var not = Parser.Not(Parser.Literal('a'));
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
    public void Digit()
    {
        var digit = Parser.Digit();
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
    public void Whitespace()
    {
        var whitespace = Parser.Whitespace();
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
    public void Word()
    {
        var word = Parser.Word();
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
    public void AnyOneOf()
    {
        var anyOneOf = Parser.Literal('a').Or(Parser.Literal('b')).Or(Parser.Literal('c'));
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
}