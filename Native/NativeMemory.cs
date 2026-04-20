using System.Runtime.CompilerServices;

namespace DeleteTemp_BFLAT.Native;

internal unsafe static class NativeMemory
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* Alloc(nuint length)
    {
        void* ptr = PInvoke.MSVCRT.malloc(length);
        if (ptr is null)
            Consts.EXCEPTIONS.Throw(Consts.EXCEPTIONS.OutOfMemory, Consts.EXCEPTIONS.NativeMemory, Consts.EXCEPTIONS.ALLOC);
        return ptr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Free(void* ptr) => PInvoke.MSVCRT.free(ptr);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Realloc(ref void* ptr, nuint length)
    {
        ptr = PInvoke.MSVCRT.realloc(ptr, length);
        if (ptr is null)
            Consts.EXCEPTIONS.Throw(Consts.EXCEPTIONS.OutOfMemory, Consts.EXCEPTIONS.NativeMemory, Consts.EXCEPTIONS.REALLOC);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* AllocAligned(nuint length, nuint alignment)
    {
        void* ptr = PInvoke.MSVCRT._aligned_malloc(length, alignment);
        if (ptr is null)
            Consts.EXCEPTIONS.Throw(Consts.EXCEPTIONS.OutOfMemory, Consts.EXCEPTIONS.NativeMemory, Consts.EXCEPTIONS.ALLOC_ALIGNED);
        return ptr;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void FreeAligned(void* ptr) => PInvoke.MSVCRT._aligned_free(ptr);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReallocAligned(ref void* ptr, nuint length, nuint alignment)
    {
        ptr = PInvoke.MSVCRT._aligned_realloc(ptr, length, alignment);
        if (ptr is null)
            Consts.EXCEPTIONS.Throw(Consts.EXCEPTIONS.OutOfMemory, Consts.EXCEPTIONS.NativeMemory, Consts.EXCEPTIONS.REALLOC_ALIGNED);
    }

    public static void Copy(void* src, void* dest, nuint byteLength)
    {
        if (src == dest | byteLength == 0)
            return;

        nuint wsz = (nuint)sizeof(nuint),
            wlen = byteLength / wsz,
            rem = byteLength % wsz;

        // Копируем основную часть словами (x64: 8 байт)
        for (nuint i = 0; i < wlen; i++)
            ((nuint*)dest)[i] = ((nuint*)src)[i];

        // Добиваем остаток по байтам
        if (rem > 0)
        {
            byte* s = (byte*)src + (byteLength - rem),
                d = (byte*)dest + (byteLength - rem);

            for (nuint i = 0; i < rem; i++)
                d[i] = s[i];
        }
    }

    public static void Move(void* src, void* dest, nuint byteLength)
    {
        if (src == dest | byteLength == 0)
            return;
        else
        {
            nuint wsz = (nuint)sizeof(nuint),
                wlen = byteLength / wsz,
                rem = byteLength % wsz;

            // Если источник "дальше" приемника, то при копировании вперед мы ничего не затрем.
            // Обычный Copy работает корректно.
            if (src > dest)
            {
                // Копируем основную часть словами (x64: 8 байт)
                for (nuint i = 0; i < wlen; i++)
                    ((nuint*)dest)[i] = ((nuint*)src)[i];

                // Добиваем остаток по байтам
                if (rem > 0)
                {
                    byte* s = (byte*)src + (byteLength - rem),
                        d = (byte*)dest + (byteLength - rem);

                    for (nuint i = 0; i < rem; i++)
                        d[i] = s[i];
                }
            }
            else
            {
                // Если приемник "дальше" (src < dest), нужно копировать С КОНЦА.

                // 1. Сначала копируем "хвост" из байтов
                if (rem > 0)
                {
                    byte* s = (byte*)src + byteLength,
                        d = (byte*)dest + byteLength;

                    for (nuint i = 0; i < rem; i++)
                        *(--d) = *(--s);
                }

                // 2. Затем копируем слова (nuint) в обратном порядке
                if (wlen > 0)
                {
                    nuint* sW = (nuint*)((byte*)src + (byteLength - rem)),
                        dW = (nuint*)((byte*)dest + (byteLength - rem));

                    for (nuint i = wlen; i > 0; i--)
                        *(--dW) = *(--sW);
                }
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InitBlock(void* ptr, nuint length, byte value) => InitBlock((byte*)ptr, length, value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InitBlock<T>(T* ptr, nuint length, in T value)
        where T : unmanaged
    {
        for (nuint i = 0; i < length; i++)
            ptr[i] = value;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ClearBlock(void* ptr, nuint length)
    {
        nuint rem = length / (nuint)sizeof(nuint);

        for (nuint i = 0; i < rem; i++)
            ((nuint*)ptr)[i] = 0;

        ptr = (nuint*)ptr + rem;
        rem = length % (nuint)sizeof(nuint);

        for (nuint i = 0; i < rem; i++)
            ((byte*)ptr)[i] = 0;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void FillBlock(void* ptr, nuint length)
    {
        nuint rem = length / (nuint)sizeof(nuint),
            fillw = Consts.NUIntMaxValue;

        for (nuint i = 0; i < rem; i++)
            ((nuint*)ptr)[i] = fillw;

        ptr = (nuint*)ptr + rem;
        rem = length % (nuint)sizeof(nuint);

        for (nuint i = 0; i < rem; i++)
            ((byte*)ptr)[i] = Consts.ByteMaxValue;
    }
}