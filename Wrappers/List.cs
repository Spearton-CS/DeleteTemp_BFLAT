using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DeleteTemp_BFLAT.Wrappers;

using Native;

[StructLayout(LayoutKind.Sequential)]
internal unsafe readonly struct List<T>
    where T : unmanaged
{
    private readonly Fields* fields;

    public List(nuint alignment = 64)
    {
        if (!alignment.IsPow2())
            Consts.EXCEPTIONS.Throw(Consts.EXCEPTIONS.InvalidAlignment, Consts.EXCEPTIONS.List, Consts.EXCEPTIONS.ctor);
        else
        {
            fields = (Fields*)NativeMemory.AllocAligned(64, 8);

            fields->Pointer = (T*)NativeMemory.AllocAligned(
                fields->RawLength = ((nuint)sizeof(T) * 20).ToPow2(),
                alignment);

            fields->Count = 0;
            fields->Capacity = 20;
            fields->Alignment = alignment;
        }
    }

    public readonly ref T this[nuint index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (index >= fields->Count)
                Consts.EXCEPTIONS.Throw(Consts.EXCEPTIONS.IndexOutOfRange, Consts.EXCEPTIONS.List, Consts.EXCEPTIONS.thisIndexer);

            return ref ((T*)fields->Pointer)[index];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Clear() => fields->Count = 0;
    public readonly nuint Capacity
    {
        get => fields->Capacity;
        set
        {
            NativeMemory.ReallocAligned(ref fields->Pointer,
                fields->RawLength = (value * (nuint)sizeof(T)).ToPow2(), fields->Alignment);
            fields->Capacity = value;
            if (fields->Count > value)
                fields->Count = value;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void RemoveAt(nuint index)
    {
        nuint count = fields->Count;
        if (index >= count)
            Consts.EXCEPTIONS.Throw(Consts.EXCEPTIONS.IndexOutOfRange, Consts.EXCEPTIONS.List, Consts.EXCEPTIONS.RemoveAt);
        else
        {
            T* ptr = (T*)fields->Pointer + index;
            if (index != (count - 1))
                NativeMemory.Copy(ptr + 1, ptr, (count - index) * (nuint)sizeof(T));
            fields->Count--;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly ref T Add(in T value)
    {
        nuint count = fields->Count, capacity = fields->Capacity;
        if (count == capacity)
        {
            NativeMemory.ReallocAligned(ref fields->Pointer,
                fields->RawLength = (count * 2 * (nuint)sizeof(T)).ToPow2(), fields->Alignment);
            fields->Capacity = fields->RawLength / (nuint)sizeof(T);
        }

        ref T dest = ref ((T*)fields->Pointer)[count];
        dest = value;
        fields->Count++;
        return ref dest;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly void Dispose()
    {
        NativeMemory.FreeAligned(fields->Pointer);
        NativeMemory.FreeAligned(fields);
    }

    [StructLayout(LayoutKind.Sequential, Size = 64)]
    private struct Fields
    {
        public void* Pointer;
        public nuint Count;
        public nuint Capacity;
        public nuint Alignment;
        public nuint RawLength;
    }
}