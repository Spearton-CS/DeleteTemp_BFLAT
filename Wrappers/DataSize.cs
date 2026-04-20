using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DeleteTemp_BFLAT.Wrappers;

[StructLayout(LayoutKind.Sequential, Size = 8)]
internal readonly struct DataSize(ulong totalBytes)
{
    public const ulong Base = 1024;
    public const ulong
        Kilobyte = Base,
        Megabyte = Kilobyte * Base,
        Gigabyte = Megabyte * Base,
        Terabyte = Gigabyte * Base, //Maybe it can be for high-loaded servers
        Petabyte = Terabyte * Base, //Maybe it can be for high-loaded servers after 10 years of work
        Exabyte = Petabyte * Base; //Maybe it can be for idk what in +infinity year

    public DataSize(ulong b, ulong kb)
        : this(b + (kb * Kilobyte)) { }
    public DataSize(ulong b, ulong kb, ulong mb)
        : this(b + (kb * Kilobyte) + (mb * Megabyte)) { }
    public DataSize(ulong b, ulong kb, ulong mb, ulong gb)
        : this(b + (kb * Kilobyte) + (mb * Megabyte) + (gb * Gigabyte)) { }

    public readonly ulong TotalBytes = totalBytes;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DataSize operator +(DataSize a, DataSize b) => new(a.TotalBytes + b.TotalBytes);

    public readonly ulong Bytes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => TotalBytes % Kilobyte;
    }

    public readonly ulong Kilobytes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (TotalBytes % Megabyte) / Kilobyte;
    }
    public readonly double TotalKilobytes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => TotalBytes / (double)Kilobyte;
    }

    public readonly ulong Megabytes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (TotalBytes % Gigabyte) / Megabyte;
    }
    public readonly double TotalMegabytes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => TotalBytes / (double)Megabyte;
    }

    public readonly ulong Gigabytes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (TotalBytes % Terabytes) / Gigabyte;
    }
    public readonly double TotalGigabytes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => TotalBytes / (double)Gigabyte;
    }

    public readonly ulong Terabytes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => TotalBytes / Terabyte;
    }
    public readonly double TotalTerabytes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => TotalBytes / (double)Terabyte;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe readonly StringW ToString(StringFormat format, StringW output)
    {
        nuint pos;
        StringW name;
        if (TotalBytes >= Terabyte & (format & StringFormat.NoTB) == 0)
        {
            pos = (TotalBytes % Terabyte) < 100
                ? Terabytes.ToString(output).Length
                : TotalTerabytes.ToString(output).Length;
            name = Consts.TB_CSTR;
        }
        else if (TotalBytes >= Gigabyte & (format & StringFormat.NoGB) == 0)
        {
            pos = (TotalBytes % Gigabyte) < 100
                ? Gigabytes.ToString(output).Length
                : TotalGigabytes.ToString(output).Length;
            name = Consts.GB_CSTR;
        }
        else if (TotalBytes >= Megabyte & (format & StringFormat.NoMB) == 0)
        {
            pos = (TotalBytes % Megabyte) < 100
                ? Megabytes.ToString(output).Length
                : TotalMegabytes.ToString(output).Length;
            name = Consts.MB_CSTR;
        }
        else if (TotalBytes >= Kilobyte & (format & StringFormat.NoKB) == 0)
        {
            pos = (TotalBytes % Kilobyte) < 100
                ? Kilobytes.ToString(output).Length
                : TotalKilobytes.ToString(output).Length;
            name = Consts.KB_CSTR;
        }
        else
            if (TotalBytes < 10)
            {
                output[0] = (char)((byte)'0' + TotalBytes); output[1] = ' '; output[2] = 'B'; output[3] = '\0';
                return output.Slice(0, 4);
            }
            else
            {
                pos = TotalBytes.ToString(output).Length;
                name = Consts.B_CSTR;
            }

        output[pos++] = name.Pointer[0]; output[pos++] = name.Pointer[1];
        if (name.Length == 3)
            output[pos++] = name.Pointer[2];

        return output.Slice(0, pos);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator ulong(DataSize ds) => ds.TotalBytes;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator DataSize(ulong value) => new(value);

    public enum StringFormat : uint
    {
        Default = 0,

        NoB = 1 << 0,
        NoKB = 1 << 1,
        NoMB = 1 << 2,
        NoGB = 1 << 3,
        NoTB = 1 << 4,

        BOnly = NoKB | NoMB | NoGB | NoTB,
        KBOnly = NoB | NoMB | NoGB | NoTB,
        MBOnly = NoB | NoKB | NoGB | NoTB,
        GBOnly = NoB | NoKB | NoMB | NoTB,
        TBOnly = NoB | NoKB | NoMB | NoGB
    }
}