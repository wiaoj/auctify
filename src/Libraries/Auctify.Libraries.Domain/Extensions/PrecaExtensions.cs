using System.Diagnostics;
using System.Runtime.CompilerServices;
using Wiaoj;

namespace Auctify.Libraries.Domain.Extensions;
internal static class PrecaExtensions {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerNonUserCode, DebuggerHidden, DebuggerStepThrough]
    public static void ThrowIfLeftOrRightNull<T>(this Wiaoj.PrecaExtensions _, T left, T right) {
        Preca.ThrowIfNull(left);
        Preca.ThrowIfNull(right);
    }
}