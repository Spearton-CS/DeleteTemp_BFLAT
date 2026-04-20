using System.Runtime.CompilerServices;

namespace DeleteTemp_BFLAT;

unsafe partial class Consts
{
    public const byte ByteMaxValue = 0xFF;
    public const ushort UShortMaxValue = 0xFFFF;
    public const uint UIntMaxValue = 0xFFFFFFFFu;
    public const ulong ULongMaxValue = 0xFFFFFFFFFFFFFFFFul;
    public static nuint NUIntMaxValue
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => unchecked((nuint)ULongMaxValue);
    }
}