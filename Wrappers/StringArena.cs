using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DeleteTemp_BFLAT.Wrappers;

using Native;

[StructLayout(LayoutKind.Sequential)]
internal unsafe readonly struct StringArena
{
    public const uint InitialCapacity = 1024;

    private readonly Fields* fields;

    public StringArena()
    {
        fields = (Fields*)NativeMemory.AllocAligned(64, 8);
        fields->ArenaFreeOffset = fields->RecordsFreeOffset = 0;
        fields->Arena = NativeMemory.AllocAligned(fields->RawArenaLength = InitialCapacity * 2, 2);
        nuint rsz = (nuint)sizeof(Record);
        fields->Records = (Record*)NativeMemory.AllocAligned(fields->RawRecordsLength = InitialCapacity * rsz, rsz);
        fields->Capacity = InitialCapacity;
    }

    public readonly nuint CharCapacity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => fields->Capacity;
    }
    public readonly nuint CharCount
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => fields->ArenaFreeOffset;
    }
    public readonly nuint StringCount
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => fields->RecordsFreeOffset;
    }

    public readonly void Push(StringW str)
    {
        if (str.IsEmpty)
            Consts.EXCEPTIONS.Throw(Consts.EXCEPTIONS.NullReference, Consts.EXCEPTIONS.StringArena, Consts.EXCEPTIONS.Push);

        nuint rsz = (nuint)sizeof(Record);
        nuint recordsLen = fields->RawRecordsLength, recordsFreeOffset = fields->RecordsFreeOffset;

        nuint arenaLen = fields->RawArenaLength, arenaFreeOffset = fields->ArenaFreeOffset;
        if ((arenaLen - (arenaFreeOffset * 2)) < (str.Length * 2))
        {
            NativeMemory.ReallocAligned(ref fields->Arena, arenaLen *= 2, 2);

            fields->RawArenaLength = arenaLen;
            fields->Capacity *= 2;
        }
        if ((recordsFreeOffset * rsz) >= recordsLen)
        {
            NativeMemory.ReallocAligned(ref fields->Records, recordsLen *= 2, rsz);

            fields->RawRecordsLength = recordsLen;
        }

        ((Record*)fields->Records)[recordsFreeOffset] = new(arenaFreeOffset, str.Length);
        NativeMemory.Copy(str, (char*)fields->Arena + arenaFreeOffset, str.Length * 2);
        fields->ArenaFreeOffset += str.Length;
        fields->RecordsFreeOffset++;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Push(string str)
    {
        fixed (char* ptr = str)
            Push(new StringW(ptr, (nuint)str.Length));
    }

    public readonly bool Pop(out StringW str)
    {
        nuint strCount = StringCount;
        if (strCount == 0)
        {
            str = default;
            return false;
        }

        Record record = ((Record*)fields->Records)[strCount - 1];
        fields->ArenaFreeOffset -= record.Length;
        fields->RecordsFreeOffset--;
        str = new((char*)fields->Arena + record.Offset, record.Length);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Dispose()
    {
        NativeMemory.FreeAligned(fields->Arena);
        NativeMemory.FreeAligned(fields->Records);
        NativeMemory.FreeAligned(fields);
    }

    [StructLayout(LayoutKind.Sequential, Size = 64)]
    private struct Fields
    {
        public nuint ArenaFreeOffset;
        public nuint RecordsFreeOffset;

        public nuint RawArenaLength;
        public nuint RawRecordsLength;

        public nuint Capacity;

        public void* Arena;
        public void* Records;

        public readonly nuint __Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    private readonly struct Record(nuint offset, nuint length)
    {
        public readonly nuint Offset = offset;
        public readonly nuint Length = length;
    }
}