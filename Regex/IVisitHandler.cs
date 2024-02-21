using System.Runtime.CompilerServices;

namespace Regex;

public interface IVisitHandler
{
    void Handle<T>(ref T value, ReadOnlySpan<Char> input) where T : IMatchable;
}

// public interface IVisitHandler<TMatchable> : IVisitHandler
// where TMatchable : IMatchable
// {
//     void IVisitHandler.Handle<T>(ref readonly T value, ReadOnlySpan<Char> input)
//     {
//         if (typeof(T) != typeof(TMatchable))
//             return;
//         ref var matchable = ref Unsafe.As<T, TMatchable>(ref Unsafe.AsRef(in value));
//         unsafe
//         {
//             var p1 = Unsafe.AsPointer(ref matchable);
//             var p2 = Unsafe.AsPointer(ref Unsafe.AsRef(in value));
//             var b = p1 == p2;
//         }
//
//         Handle(in matchable, input);
//     }
//
//     void Handle(in TMatchable value, ReadOnlySpan<Char> input);
// }