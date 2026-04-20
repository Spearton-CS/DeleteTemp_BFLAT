using System.Runtime.CompilerServices;

namespace DeleteTemp_BFLAT;

using Wrappers;

unsafe partial class Consts
{
    public static StringW OptionsHELP
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(optionsHelpBlobptr, 294);
    }

    public static StringW SilentArg
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(optionsHelpBlobptr + 100, 8);
    }
    public static StringW NoCalcArg
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(optionsHelpBlobptr + 170, 9);
    }
    public static StringW NoCountArg
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(optionsHelpBlobptr + 207, 10);
    }
    public static StringW NoErrorsArg
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(optionsHelpBlobptr + 251, 11);
    }
}