using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DeleteTemp_BFLAT.Native;

internal unsafe static class PInvoke
{
    public static class Kernel32
    {
        private const string dll = "kernel32.dll";

        [DllImport(dll)]
        public static extern uint GetLastError();
        [DllImport(dll)]
        public static extern BOOL CloseHandle(void* hObject);

        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern uint GetEnvironmentVariableW(char* lpName, char* lpBuffer, uint nSize);

        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern uint GetWindowsDirectoryW(char* lpBuffer, uint uSize);

        #region FS

        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern BOOL DeleteFileW(char* lpFileName);
        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern BOOL RemoveDirectoryW(char* lpDirectoryName);

        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern uint GetTempPathW(uint nBufferLength, char* lpBuffer);

        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern BOOL FindNextFileW(void* hFindFile, WIN32_FIND_DATAW* lpFindFileData);
        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern void* FindFirstFileW(char* hFindFile, WIN32_FIND_DATAW* lpFindFileData);

        [DllImport(dll)]
        public static extern BOOL FindClose(void* hFindFile);

        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern void* FindFirstStreamW(char* lpFileName, int InfoLevel, WIN32_FIND_STREAM_DATA* lpFindStreamData, uint dwFlags);

        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern BOOL FindNextStreamW(void* hFindStream, WIN32_FIND_STREAM_DATA* lpFindStreamData);

        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern BOOL GetFileSizeEx(void* hFile, long* lpFileSize);

        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern void* CreateFileW(
            char* lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            void* lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            void* hTemplateFile);

        [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 592)]
        public struct WIN32_FIND_DATAW
        {
            public uint dwFileAttributes;
            public uint ftCreationTimeLow; //ComTypes.FILETIME
            public uint ftCreationTimeHigh; //ComTypes.FILETIME
            public uint ftLastAccessTimeLow; //ComTypes.FILETIME
            public uint ftLastAccessTimeHigh; //ComTypes.FILETIME
            public uint ftLastWriteTimeLow; //ComTypes.FILETIME
            public uint ftLastWriteTimeHigh; //ComTypes.FILETIME
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;
            public fixed char cFileName[260];
            public fixed char cAlternateFileName[14];
        }

        [StructLayout(LayoutKind.Sequential, Size = 600)]
        public struct WIN32_FIND_STREAM_DATA
        {
            public long StreamSize;
            public fixed char cStreamName[260 + 36];
        }

        #endregion

        #region Console

        [DllImport(dll)]
        public static extern void* GetStdHandle(int nStdHandle);

        [DllImport(dll)]
        public static extern BOOL WriteConsoleW(
            void* hConsoleOutput,
            char* lpBuffer,
            uint nNumberOfCharsToWrite,
            out uint lpNumberOfCharsWritten,
            void* lpReserved);

        [DllImport(dll)]
        public static extern BOOL SetConsoleTextAttribute(void* hConsoleOutput, ushort wAttributes);
        [DllImport(dll)]
        public static extern BOOL GetConsoleScreenBufferInfo(void* hConsoleOutput, out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

        [DllImport(dll)]
        public static extern uint GetConsoleMode(void* hConsoleHandle, out uint lpMode);

        [DllImport(dll)]
        public static extern BOOL SetConsoleMode(void* hConsoleHandle, uint dwMode);

        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern BOOL ReadConsoleInputW(void* hConsoleInput, INPUT_RECORD* lpBuffer, uint nLength, out uint lpNumberOfEventsRead);

        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern uint GetConsoleTitleW(char* lpConsoleTitle, uint nSize);

        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern BOOL SetConsoleTitleW(char* lpConsoleTitle);

        [DllImport(dll, CharSet = CharSet.Unicode)]
        public static extern char* GetCommandLineW();

        public const int STD_OUTPUT_HANDLE = -11;
        public const int STD_INPUT_HANDLE = -10;
        public const uint ENABLE_ECHO_INPUT = 0x0004;
        public const uint ENABLE_LINE_INPUT = 0x0002;
        public const uint ENABLE_PROCESSED_INPUT = 0x0001;

        public enum ConsoleColor : ushort
        {
            Black = 0,
            Blue = 1,
            Green = 2,
            Cyan = 3,
            Red = 4,
            Magenta = 5,
            Yellow = 6,
            White = 7,
            Gray = 8,
            BrightBlue = 9,
            BrightGreen = 10,
            BrightCyan = 11,
            BrightRed = 12,
            BrightMagenta = 13,
            BrightYellow = 14,
            BrightWhite = 15
        }

        [StructLayout(LayoutKind.Sequential, Size = 22)]
        public struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public int dwSizeX; // Coord
            public int dwSizeY;
            public int dwCursorPositionX; // Coord
            public int dwCursorPositionY;
            public ushort wAttributes; // Attributes
            public short srWindowLeft; // SmallRect
            public short srWindowTop;
            public short srWindowRight;
            public short srWindowBottom;
            public int dwMaximumWindowSizeX; // Coord
            public int dwMaximumWindowSizeY;
        }

        [StructLayout(LayoutKind.Sequential, Size = 20)]
        public struct INPUT_RECORD
        {
            public ushort EventType;
            private readonly ushort _padding; // Чтобы KeyDown начался с 4-го байта

            // KEY_EVENT_RECORD часть:
            public int KeyDown;         // 4
            public ushort RepeatCount;    // 2
            public ushort VirtualKeyCode; // 2
            public ushort VirtualScanCode;// 2
            public char UnicodeChar;      // 2
            public uint ControlKeyState;  // 4
        }

        public enum ConsoleKey : ushort
        {
            Backspace = 0x08,
            Tab = 0x09,
            Enter = 0x0D,
            Escape = 0x1B,
            Space = 0x20,
            Left = 0x25, Up = 0x26, Right = 0x27, Down = 0x28,
            Delete = 0x2E,
            D0 = 0x30, D1 = 0x31, D2 = 0x32, D3 = 0x33, D4 = 0x34,
            D5 = 0x35, D6 = 0x36, D7 = 0x37, D8 = 0x38, D9 = 0x39,
            A = 0x41, B = 0x42, C = 0x43, D = 0x44, E = 0x45, F = 0x46,
            G = 0x47, H = 0x48, I = 0x49, J = 0x4A, K = 0x4B, L = 0x4C,
            M = 0x4D, N = 0x4E, O = 0x4F, P = 0x50, Q = 0x51, R = 0x52,
            S = 0x53, T = 0x54, U = 0x55, V = 0x56, W = 0x57, X = 0x58,
            Y = 0x59, Z = 0x5A
        }

        #endregion

        [StructLayout(LayoutKind.Sequential, Size = 4)]
        public readonly struct BOOL(uint raw)
        {
            public readonly uint RAW = raw;

            public static BOOL True
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => new(1);
            }
            public static BOOL False
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => default;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator true(BOOL win32) => win32.RAW != 0;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator false(BOOL win32) => win32.RAW == 0;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool operator !(BOOL win32) => win32.RAW == 0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator bool(BOOL win32) => win32.RAW != 0;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static implicit operator BOOL(bool dotnet) => new(dotnet ? 1u : 0u);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator uint(BOOL win32) => win32.RAW;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator BOOL(uint raw) => new(raw);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator int(BOOL win32) => Unsafe.As<BOOL, int>(ref win32);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static explicit operator BOOL(int raw) => Unsafe.As<int, BOOL>(ref raw);
        }

    }

    public static class MSVCRT
    {
        private const string dll = "msvcrt.dll";

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void* malloc(nuint size);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void free(void* ptr);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void* realloc(void* ptr, nuint size);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void* _aligned_malloc(nuint size, nuint alignment);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void* _aligned_realloc(void* memblock, nuint size, nuint alignment);

        [DllImport(dll, CallingConvention = CallingConvention.Cdecl)]
        public static extern void _aligned_free(void* memblock);
    }
}