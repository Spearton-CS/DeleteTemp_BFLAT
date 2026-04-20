using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Environment = System.Environment;

namespace DeleteTemp_BFLAT.Wrappers;

using Native;

[StructLayout(LayoutKind.Sequential)]
internal unsafe readonly struct EnvVar
{
    public const int BufferSizeChars = 4096;

    private readonly Buffer buffer;
    public readonly StringW TempPath;
    public readonly StringW SystemTempPath;
    public readonly StringW OldConsoleTitle;
    public readonly StringW Free;

    public static void Init(ref EnvVar init)
    {
        fixed (char* data = init.buffer.data)
        {
            char* ptr = data;
            uint len = BufferSizeChars, written;

            written = PInvoke.Kernel32.GetEnvironmentVariableW(Consts.TEMP_CSTR, ptr, len);
            if (written == 0)
                Consts.EXCEPTIONS.Throw(Consts.EXCEPTIONS.TempPathUnavailable, default, default);

            ptr[written++] = '\\';
            ptr[written] = '\0';
            fixed (StringW* str = &init.TempPath) //Unsafe.AsRef<T>(in T); doesn't exist in zerolib... lol
            {
                Console.WriteInfo(Consts.TempFound);
                Console.WriteLineInfo(*str = new(ptr, written));
            }

            written++; //'\0'
            ptr += written; len -= written;

            written = PInvoke.Kernel32.GetWindowsDirectoryW(ptr, len);
            if (written != 0)
            {
                ptr[written++] = '\\';
                ptr[written++] = 'T';
                ptr[written++] = 'e';
                ptr[written++] = 'm';
                ptr[written++] = 'p';
                ptr[written++] = '\\';
                ptr[written] = '\0';

                fixed (StringW* str = &init.SystemTempPath)
                {
                    Console.WriteInfo(Consts.SystemTempFound);
                    Console.WriteLineInfo(*str = new(ptr, written));
                }
            }
            else
            {
                fixed (StringW* str = &init.SystemTempPath)
                    *str = default;
                Console.WriteLineWarning(Consts.WARNINGS.SystemTempUnavailable);
            }

            written++; //'\0'
            ptr += written; len -= written;

            StringW title = Console.Title;
            NativeMemory.Copy(title.Pointer, ptr, (written = (uint)title.Length) * 2);
            ptr[written] = '\0';
            fixed (StringW* str = &init.OldConsoleTitle)
                *str = new(ptr, written);

            written++;
            ptr += written; len -= written;

            NativeMemory.ClearBlock(ptr, len * 2);
            fixed (StringW* str = &init.Free)
                *str = new(ptr, len);
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = BufferSizeChars * 2)]
    private struct Buffer { public fixed char data[BufferSizeChars]; }
}