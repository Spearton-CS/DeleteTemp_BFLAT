using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DeleteTemp_BFLAT.Native;

using Wrappers;

internal unsafe static class IO
{
    public static bool SkipZoneIdentifier = true;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool DeleteFile(StringW path, out ErrorType error)
    {
        bool result = PInvoke.Kernel32.DeleteFileW(path);
        error = result
            ? ErrorType.NoError
            : (ErrorType)PInvoke.Kernel32.GetLastError();
        return result;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool DeleteDirectory(StringW path, out ErrorType error)
    {
        bool result = PInvoke.Kernel32.RemoveDirectoryW(path);
        error = result
            ? ErrorType.NoError
            : (ErrorType)PInvoke.Kernel32.GetLastError();
        return result;
    }

    public static nuint EnumerateItems(StringW dirMask, delegate* managed<ItemInfo, bool> callback, out ErrorType error)
    {
        if (dirMask[dirMask.Length - 1] == '\0')
        {
            if (dirMask[dirMask.Length - 3] != '\\' | dirMask[dirMask.Length - 2] != '*')
                Consts.EXCEPTIONS.Throw(Consts.EXCEPTIONS.UseMaskNotPath, Consts.EXCEPTIONS.IO, Consts.EXCEPTIONS.EnumerateItems);
        }
        else if (dirMask[dirMask.Length - 2] != '\\' | dirMask[dirMask.Length - 1] != '*')
            Consts.EXCEPTIONS.Throw(Consts.EXCEPTIONS.UseMaskNotPath, Consts.EXCEPTIONS.IO, Consts.EXCEPTIONS.EnumerateItems);

        PInvoke.Kernel32.WIN32_FIND_DATAW findData;
        nuint count = 0;

        void* findHandle = PInvoke.Kernel32.FindFirstFileW(dirMask, &findData);
        error = findHandle == (void*)-1
            ? (ErrorType)PInvoke.Kernel32.GetLastError()
            : ErrorType.NoError;
        try
        {
            do
            {
                // Пропускаем "." и ".."
                if (findData.cFileName[0] == '.')
                {
                    if (findData.cFileName[1] == '\0' ||
                        (findData.cFileName[1] == '.' && findData.cFileName[2] == '\0'))
                        continue;
                }
                else if (findData.cFileName[0] == '\0')
                    continue;
                else
                {
                    count++;
                    if (!callback(new ItemInfo(&findData)))
                        break;
                }
            }
            while (PInvoke.Kernel32.FindNextFileW(findHandle, &findData));
        }
        finally
        {
            PInvoke.Kernel32.FindClose(findHandle);
        }

        return count;
    }

    public static nuint EnumerateAltStreams(StringW file, delegate* managed<StreamInfo, bool> callback, out ErrorType error)
    {
        PInvoke.Kernel32.WIN32_FIND_STREAM_DATA findData;
        nuint count = 0;

        void* findHandle = PInvoke.Kernel32.FindFirstStreamW(file, 0, &findData, 0);
        if (findHandle == (void*)-1)
            error = (ErrorType)PInvoke.Kernel32.GetLastError();
        else
        {
            error = ErrorType.NoError;

            try
            {
                do
                {
                    // Skip ::$DATA (normal data of file) and :ZoneIdentifier:$DATA
                    StringW findName = (StringW)findData.cStreamName;
                    if (findName.Length == 0 || findName == Consts.AltStream__DATA || (SkipZoneIdentifier && findName == Consts.AltStream__ZoneIdentifier))
                        continue;
                    else
                    {
                        count++;
                        if (!callback(new StreamInfo(&findData)))
                            break;
                    }
                }
                while (PInvoke.Kernel32.FindNextStreamW(findHandle, &findData));
            }
            finally
            {
                PInvoke.Kernel32.FindClose(findHandle);
            }
        }

        return count;
    }

    public static DataSize CalcAltStreamSize(StringW file, out ErrorType error)
    {
        PInvoke.Kernel32.WIN32_FIND_STREAM_DATA findData;
        DataSize size = default;

        void* findHandle = PInvoke.Kernel32.FindFirstStreamW(file, 0, &findData, 0);
        if (findHandle == (void*)-1)
            error = (ErrorType)PInvoke.Kernel32.GetLastError();
        else
        {
            error = ErrorType.NoError;

            try
            {
                do
                {
                    // Skip ::$DATA (normal data of file) and :ZoneIdentifier:$DATA
                    StringW findName = (StringW)findData.cStreamName;
                    if (findName.Length == 0 || findName == Consts.AltStream__DATA || (SkipZoneIdentifier && findName == Consts.AltStream__ZoneIdentifier))
                        continue;
                    else
                        size += new DataSize((ulong)findData.StreamSize);
                }
                while (PInvoke.Kernel32.FindNextStreamW(findHandle, &findData));
            }
            finally
            {
                PInvoke.Kernel32.FindClose(findHandle);
            }
        }

        return size;

    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct ItemInfo(PInvoke.Kernel32.WIN32_FIND_DATAW* ptr)
    {
        private readonly PInvoke.Kernel32.WIN32_FIND_DATAW* _ptr = ptr;

        public readonly DataSize Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(((ulong)_ptr->nFileSizeHigh << 32) | _ptr->nFileSizeLow);
        }

        public readonly bool IsDirectory
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (_ptr->dwFileAttributes & 0x10) != 0;
        }

        public readonly StringW Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (StringW)_ptr->cFileName;
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct StreamInfo(PInvoke.Kernel32.WIN32_FIND_STREAM_DATA* ptr)
    {
        private readonly PInvoke.Kernel32.WIN32_FIND_STREAM_DATA* _ptr = ptr;

        public readonly DataSize Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new((ulong)_ptr->StreamSize);
        }

        public readonly StringW RawName
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (StringW)_ptr->cStreamName;
        }
        public readonly StringW Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                StringW str = (StringW)_ptr->cStreamName;
                nuint last = str.LastIndexOf(':');
                return str.Slice(1, (last == Consts.NUIntMaxValue
                    ? str.Length
                    : last) - 1); //: in beginning + :... in ending
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StringW ToString(this ErrorType err) => err switch
    {
        ErrorType.NotFound => Consts.NotFound,
        ErrorType.PathNotFound => Consts.PathNotFound,
        ErrorType.AccessDenied => Consts.AccessDenied,
        ErrorType.InvalidHandle => Consts.InvalidHandle,
        ErrorType.SharingViolation => Consts.SharingViolation,
        _ => Consts.Unknown
    };

    public enum ErrorType : uint
    {
        NoError = 0,
        NotFound = 2,          // ERROR_FILE_NOT_FOUND
        PathNotFound = 3,      // ERROR_PATH_NOT_FOUND
        AccessDenied = 5,      // ERROR_ACCESS_DENIED
        InvalidHandle = 6,     // ERROR_INVALID_HANDLE
        NoMoreFiles = 18,      // ERROR_NO_MORE_FILES (важно для циклов)
        SharingViolation = 32, // ERROR_SHARING_VIOLATION
        Unknown = Consts.UIntMaxValue
    }
}