using System.Runtime.CompilerServices;

namespace DeleteTemp_BFLAT;

using Wrappers;

unsafe partial class Consts
{
    public static StringW ColonAndSpace
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 222, 2);
    }
    public static StringW Space
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 223, 1);
    }

    public static StringW DeInitWillTakeSomeMcSecs
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr, 91);
    }
    public static StringW InitWillTakeSomeMcSecs
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 2, 89);
    }
    //public static StringW ItWillTakeSomeMcSecs
    //{
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    get => new(blobptr + 8, 33);
    //}
    //public static StringW IfItStuckedItDied
    //{
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    get => new(blobptr + 42, 49);
    //}
    public static StringW CanUtilityCleanup
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 91, 92);
    }
    //public static StringW SYSTEM
    //{
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    get => new(blobptr + 183, 6);
    //}
    //public static StringW SYSTEM_TEMP
    //{
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    get => new(blobptr + 183, 11);
    //}
    public static StringW TEMP
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 190, 4);
    }
    public static StringW TEMP_CSTR
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 613, 4);
    }
    public static StringW SystemTempFound
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 183, 29);
    }
    public static StringW TempFound
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 190, 22);
    }
    //public static StringW directory
    //{
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    get => new(blobptr + 196, 9);
    //}
    //public static StringW found
    //{
    //    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    //    get => new(blobptr + 205, 5);
    //}
    public static StringW ConsoleTitle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 212, 31);
    }
    public static StringW InitFinished
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 243, 14);
    }

    public static StringW B_CSTR
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 634, 1);
    }
    public static StringW TB_CSTR
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 633, 2);
    }
    public static StringW GB_CSTR
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 636, 2);
    }
    public static StringW MB_CSTR
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 639, 2);
    }
    public static StringW KB_CSTR
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 642, 2);
    }

    public static StringW ULongMaxValue_STR
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 645, 14);
    }

    public static StringW AltStream__DATA
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 659, 7);
    }
    public static StringW AltStream__ZoneIdentifier
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 666, 22);
    }

    public static StringW CleaningCompleted
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 688, 38);
    }
    public static StringW StartingCleaning
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 726, 18);
    }
    public static StringW Processing
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 744, 11);
    }
    public static StringW FailedToDelete
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 755, 18);
    }
    public static StringW DirectoryCleaned
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 773, 17);
    }

    public static StringW Total
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 790, 6);
    }
    public static StringW Deleted
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 796, 8);
    }
    public static StringW size
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 804, 4);
    }
    public static StringW files
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 808, 5);
    }
    public static StringW file
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 808, 4);
    }
    public static StringW directories
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 813, 11);
    }
    public static StringW directory
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 824, 9);
    }

    public static StringW PathNotFound
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 830, 12);
    }
    public static StringW NotFound
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 834, 8);
    }
    public static StringW AccessDenied
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 842, 12);
    }
    public static StringW InvalidHandle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 854, 13);
    }
    public static StringW SharingViolation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 867, 16);
    }

    public static StringW Unknown
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(blobptr + 883, 7);
    }
}