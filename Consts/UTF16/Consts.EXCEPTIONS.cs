using System.Runtime.CompilerServices;

namespace DeleteTemp_BFLAT;

using Wrappers;

unsafe partial class Consts
{
    public static class EXCEPTIONS
    {
        public static void Throw(StringW exception, StringW type, StringW member)
        {
            if (!exception.IsEmpty)
                Native.Console.WriteError(exception);
            Native.Console.WriteError(Exception);
            if (!type.IsEmpty)
                Native.Console.WriteError(type);
            if (!member.IsEmpty)
                Native.Console.WriteLineError(member);
            System.Environment.FailFast(null);
        }

        public static StringW Exception
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 257, 9);
        }

        public static StringW IndexOutOfRange
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 266, 15);
        }
        public static StringW OutOfMemory
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 281, 11);
        }
        public static StringW NullReference
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 292, 13);
        }
        public static StringW InvalidAlignment
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 305, 16);
        }
        public static StringW TempPathUnavailable
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 321, 19);
        }

        public static StringW NativeMemory
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 340, 13);
        }
        public static StringW ALLOC
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 353, 7);
        }
        public static StringW REALLOC
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 360, 9);
        }
        public static StringW ALLOC_ALIGNED
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 369, 14);
        }
        public static StringW REALLOC_ALIGNED
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 383, 16);
        }

        public static StringW StringW
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 399, 8);
        }
        public static StringW thisIndexer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 407, 6);
        }
        public static StringW Slice
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 413, 7);
        }

        public static StringW List
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 420, 8);
        }
        public static StringW ctor
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 428, 6);
        }
        public static StringW RemoveAt
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 434, 10);
        }

        public static StringW Console
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 444, 8);
        }
        public static StringW Write
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 452, 7);
        }
        public static StringW WriteLine
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 459, 11);
        }

        public static StringW IO
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 610, 3);
        }
        public static StringW UseMaskNotPath
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 576, 18);
        }
        public static StringW EnumerateItems
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 594, 16);
        }

        public static StringW StringArena
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 618, 11);
        }
        public static StringW Push
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(blobptr + 629, 4);
        }
    }
}