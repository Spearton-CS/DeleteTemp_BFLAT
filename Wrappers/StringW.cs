using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Environment = System.Environment;

namespace DeleteTemp_BFLAT.Wrappers;

using Native;

[StructLayout(LayoutKind.Sequential)]
internal unsafe readonly struct StringW
{
    public StringW(char* ptr, nuint len)
    {
        Pointer = ptr;
        Length = len;
    }
    public StringW(char* ptr)
    {
        Pointer = ptr;
        Length = 0;
        while (*ptr != '\0')
        {
            Length++;
            ptr++;
        }    
    }

    public readonly char* Pointer;
    public readonly nuint Length;

    public readonly bool IsEmpty
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Pointer is null | Length == 0;
    }

    public readonly ref char this[nuint index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (index >= Length)
                Consts.EXCEPTIONS.Throw(Consts.EXCEPTIONS.IndexOutOfRange, Consts.EXCEPTIONS.StringW, Consts.EXCEPTIONS.thisIndexer);

            return ref Pointer[index];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly nuint IndexOf(char value)
    {
        for (nuint i = 0; i < Length; i++)
            if (Pointer[i] == value)
                return i;

        return Consts.NUIntMaxValue;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly nuint LastIndexOf(char value)
    {
        char* ptr = Pointer + Length;
        for (nuint i = Length; i > 0; i--)
            if (*(--ptr) == value)
                return i - 1;

        return Consts.NUIntMaxValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly StringW Slice(nuint start)
    {
        if (start >= Length)
            Consts.EXCEPTIONS.Throw(Consts.EXCEPTIONS.IndexOutOfRange, Consts.EXCEPTIONS.StringW, Consts.EXCEPTIONS.Slice);
        return new(Pointer + start, Length - start);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly StringW Slice(nuint start, nuint length)
    {
        if ((start + length) > Length)
            Consts.EXCEPTIONS.Throw(Consts.EXCEPTIONS.IndexOutOfRange, Consts.EXCEPTIONS.StringW, Consts.EXCEPTIONS.Slice);
        return new(Pointer + start, length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly nuint Split(char value, delegate* managed<StringW, bool> callback)
    {
        if (callback is null)
            Environment.FailFast(null);

        nuint count = 0;
        nuint index = 0;
        nuint start = 0;
        while (index < Length)
        {
            if (Pointer[index] == value)
            {
                if (start != index)
                {
                    if (!callback(new StringW(Pointer + start, index - start)))
                        break;
                    ++count;
                }
                start = index + 1;
            }
            ++index;
        }

        if (start < index)
        {
            callback(new StringW(Pointer + start, index - start));
            ++count;
        }

        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator true(StringW str) => str.Pointer is not null;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator false(StringW str) => str.Pointer is null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(StringW a, StringW b)
    {
        if (a.Pointer == b.Pointer)
            return a.Length == b.Length;
        else if (a.Length != b.Length)
            return false;
        else
        {
            for (nuint i = 0; i < a.Length; i++)
                if (a[i] != b[i])
                    return false;

            return true;
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(StringW a, StringW b)
    {
        if (a.Pointer == b.Pointer)
            return a.Length != b.Length;
        else if (a.Length != b.Length)
            return true;
        else
        {
            for (nuint i = 0; i < a.Length; i++)
                if (a[i] != b[i])
                    return true;

            return false;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(StringW a, char* b) => a.Pointer == b;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(StringW a, char* b) => a.Pointer != b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(StringW a, string b)
    {
        fixed (char* ptr = b)
            return a == new StringW(ptr, (nuint)b.Length);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(StringW a, string b)
    {
        fixed (char* ptr = b)
            return a != new StringW(ptr, (nuint)b.Length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator char*(StringW str) => str.Pointer;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator StringW(char* ptr) => new(ptr);
}

internal unsafe static class StringWExtensions
{
    public static StringW ToString(this ulong value, StringW output)
    {
        if (value == 0)
        {
            output[0] = '0';
            return output.Slice(0, 2);
        }
        else if (value == Consts.ULongMaxValue)
        {
            StringW maxStr = Consts.ULongMaxValue_STR;
            if (output.Length < maxStr.Length)
                output[Consts.NUIntMaxValue] = '\0'; //Cause IndexOutOfRangeException.
            NativeMemory.Copy(maxStr, output, maxStr.Length * 2);
            return output.Slice(0, maxStr.Length);
        }
        else
        {
            uint pos = 0;
            char* temp = stackalloc char[20];
            int tPos = 0;

            // Классический разворот целого числа
            while (value > 0)
            {
                temp[tPos++] = (char)('0' + (value % 10));
                value /= 10;
            }
            while (tPos > 0)
                output[pos++] = temp[--tPos];

            output[pos++] = ' ';

            return output.Slice(0, pos);
        }
    }

    public static StringW ToString(this double value, StringW output)
    {
        uint pos = 0;

        // 1. Обрабатываем число (упрощенный WriteFloat)
        // Выделяем целую часть
        ulong integral = (ulong)value;
        // 2 знака дробной части с округлением
        uint fractional = (uint)((value - integral) * 100.0 + 0.5);

        // Случай когда округление дало 100 (.999 -> 1.00)
        if (fractional >= 100)
        {
            integral++;
            fractional = 0;
        }

        // Пишем целую часть (реверсивно через стек)
        char* temp = stackalloc char[20];
        int tPos = 0;
        ulong tVal = integral;
        if (tVal == 0)
            temp[tPos++] = '0';
        else
            while (tVal > 0)
            {
                temp[tPos++] = (char)('0' + (tVal % 10));
                tVal /= 10;
            }
        while (tPos > 0)
            output[pos++] = temp[--tPos];

        // Пишем точку и дробную часть
        output[pos++] = '.';
        output[pos++] = (char)('0' + (fractional / 10));
        output[pos++] = (char)('0' + (fractional % 10));

        return output.Slice(0, pos);
    }
}