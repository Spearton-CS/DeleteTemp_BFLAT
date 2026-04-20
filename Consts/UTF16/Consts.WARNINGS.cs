using System.Runtime.CompilerServices;

namespace DeleteTemp_BFLAT;

using Wrappers;

unsafe partial class Consts
{
    public static class WARNINGS
    {
        public static StringW SystemTempUnavailable
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 470, 106);
        }
    }
}