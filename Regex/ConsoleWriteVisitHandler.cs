namespace Regex;

public struct ConsoleWriteVisitHandler : IVisitHandler
{
    public void Handle<T>(ref T value, ReadOnlySpan<Char> input) where T : IMatchable
    {
        Console.Out.Write($"{typeof(T).Name}: ");
        foreach (var c in input)
            Console.Out.Write(c switch
            {
                '\"' => "\\\"",
                '\\' => "\\\\",
                '\0' => "\\0",
                '\a' => "\\a",
                '\b' => "\\b",
                '\f' => "\\f",
                '\n' => "\\n",
                '\r' => "\\r",
                '\t' => "\\t",
                '\v' => "\\v",
                _ => c
            });
        Console.Out.WriteLine();
    }
}