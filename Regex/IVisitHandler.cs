namespace Regex;

public interface IVisitHandler
{
    void Handle<T>(in T value, ReadOnlySpan<Char> input) where T : IMatchable;
}