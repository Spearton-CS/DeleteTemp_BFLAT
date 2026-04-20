using System.Runtime.CompilerServices;

namespace DeleteTemp_BFLAT.Native;

internal unsafe static class Math
{
    #region IsPow2

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPow2(this uint v) => v != 0 & (v & (v - 1)) == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPow2(this ulong v) => v != 0 & (v & (v - 1)) == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPow2(this nuint v) => v != 0 & (v & (v - 1)) == 0;

    #endregion

    #region ToPow2

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ToPow2(this uint v)
    {
        if (v == 0)
            return 1;
        else
        {
            --v;
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            return v + 1;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ToPow2(this ulong v)
    {
        if (v == 0)
            return 1;
        else
        {
            v--;
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            v |= v >> 32;
            return v + 1;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint ToPow2(this nuint v)
    {
        if (sizeof(nuint) == sizeof(ulong))
            return (nuint)ToPow2((ulong)v);
        else
            return ToPow2((uint)v);
    }

    #endregion

    #region Align

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nuint Align(this nuint addr, nuint alignment) => (addr + alignment - 1) & ~(alignment - 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* Align(void* ptr, nuint alignment) => (void*)((nuint)ptr).Align(alignment);

    #endregion
}