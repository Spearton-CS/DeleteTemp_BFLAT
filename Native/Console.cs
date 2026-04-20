using System.Runtime.CompilerServices;

using Environment = System.Environment;

namespace DeleteTemp_BFLAT.Native;

using Wrappers;

internal unsafe static class Console
{
    private static readonly void* stdout;
    private static readonly void* stdin;
    private static TitleBuffer titleBuffer;
    private static PInvoke.Kernel32.CONSOLE_SCREEN_BUFFER_INFO currentScreenBufInfo;

    public static readonly StringW Args;

    public static bool Silent = false;
    public static bool NoErrors = false;

    [InlineArray(256)]
    struct TitleBuffer { private char _0; }
    static Console()
    {
        stdout = PInvoke.Kernel32.GetStdHandle(PInvoke.Kernel32.STD_OUTPUT_HANDLE);
        stdin = PInvoke.Kernel32.GetStdHandle(PInvoke.Kernel32.STD_INPUT_HANDLE);
        fixed (void* title = &titleBuffer)
        {
            NativeMemory.ClearBlock(title, 256);
            _ = PInvoke.Kernel32.GetConsoleTitleW((char*)title, 256);
        }
        if (!PInvoke.Kernel32.GetConsoleScreenBufferInfo(stdout, out currentScreenBufInfo))
            currentScreenBufInfo.wAttributes = 0x07;

        Args = (StringW)PInvoke.Kernel32.GetCommandLineW();
    }

    public static PInvoke.Kernel32.ConsoleColor ForegroundColor
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (PInvoke.Kernel32.ConsoleColor)(currentScreenBufInfo.wAttributes & 0x0F);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => PInvoke.Kernel32.SetConsoleTextAttribute(stdout, 
            currentScreenBufInfo.wAttributes = (ushort)((currentScreenBufInfo.wAttributes & 0xF0) | (ushort)value));
    }

    public static PInvoke.Kernel32.ConsoleColor BackgroundColor
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (PInvoke.Kernel32.ConsoleColor)((currentScreenBufInfo.wAttributes & 0xF0) >> 4);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => PInvoke.Kernel32.SetConsoleTextAttribute(stdout,
            currentScreenBufInfo.wAttributes = (ushort)((currentScreenBufInfo.wAttributes & 0x0F) | ((ushort)value << 4)));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ResetColor()
    {
        // ESC [ 0 m
        char* reset = stackalloc char[4];
        reset[0] = (char)0x1B; // ESC
        reset[1] = '['; reset[2] = '0'; reset[3] = 'm';

        PInvoke.Kernel32.WriteConsoleW(stdout, reset, 4, out _, null);
        currentScreenBufInfo.wAttributes = (ushort)((currentScreenBufInfo.wAttributes & 0xF0) | (ushort)PInvoke.Kernel32.ConsoleColor.Gray);
        currentScreenBufInfo.wAttributes = (ushort)((currentScreenBufInfo.wAttributes & 0x0F) | ((ushort)PInvoke.Kernel32.ConsoleColor.Black << 4));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(StringW str)
    {
        if (str.IsEmpty)
            Consts.EXCEPTIONS.Throw(Consts.EXCEPTIONS.NullReference, Consts.EXCEPTIONS.Console, Consts.EXCEPTIONS.Write);
        if (Silent)
            return;
        PInvoke.Kernel32.WriteConsoleW(stdout,
            str.Pointer, (uint)str.Length,
            out _, null);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLine()
    {
        if (Silent)
            return;
        char* nl = stackalloc char[2]; nl[0] = '\r'; nl[1] = '\n';
        PInvoke.Kernel32.WriteConsoleW(stdout,
            nl, 2,
            out _, null);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLine(StringW str)
    {
        if (str.IsEmpty)
            Consts.EXCEPTIONS.Throw(Consts.EXCEPTIONS.NullReference, Consts.EXCEPTIONS.Console, Consts.EXCEPTIONS.WriteLine);
        if (Silent)
            return;
        PInvoke.Kernel32.WriteConsoleW(stdout,
            str.Pointer, (uint)str.Length,
            out _, null);
        char* nl = stackalloc char[2]; nl[0] = '\r'; nl[1] = '\n';
        PInvoke.Kernel32.WriteConsoleW(stdout,
            nl, 2,
            out _, null);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PInvoke.Kernel32.ConsoleKey ReadKey()
    {
        // 1. Сохраняем текущий режим консоли
        _ = PInvoke.Kernel32.GetConsoleMode(stdin, out var oldMode);

        // 2. Отключаем Line Input (чтобы не ждать Enter) и Echo (чтобы не дублировать символ)
        PInvoke.Kernel32.SetConsoleMode(stdin,
            oldMode & ~(PInvoke.Kernel32.ENABLE_LINE_INPUT | PInvoke.Kernel32.ENABLE_ECHO_INPUT));

        while (true)
        {
            PInvoke.Kernel32.INPUT_RECORD record;
            // 3. Читаем одно событие. 
            // Если твоя сигнатура: ReadConsoleInputW(void* h, out INPUT_RECORD ir, uint len, out uint read)
            if (PInvoke.Kernel32.ReadConsoleInputW(stdin,
                &record, 1, out var read)
                && read > 0)
                // 4. Проверяем: это нажатие клавиши (1) и это событие KeyDown (true)
                if (record.EventType == 1 & record.KeyDown != 0)
                {
                    // 5. Запоминаем код клавиши
                    var vk = (PInvoke.Kernel32.ConsoleKey)record.VirtualKeyCode;

                    // 6. ОБЯЗАТЕЛЬНО возвращаем режим консоли назад, иначе потом ввод будет сломан
                    PInvoke.Kernel32.SetConsoleMode(stdin, oldMode);

                    return vk;
                }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(StringW str, PInvoke.Kernel32.ConsoleColor fore)
    {
        if (Silent)
            return;
        var old = ForegroundColor;
        ForegroundColor = fore;
        Write(str);
        ForegroundColor = old;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(StringW str, PInvoke.Kernel32.ConsoleColor fore, PInvoke.Kernel32.ConsoleColor back)
    {
        if (Silent)
            return;
        var oldFore = ForegroundColor;
        var oldBack = BackgroundColor;
        ForegroundColor = fore;
        BackgroundColor = back;
        Write(str);
        ForegroundColor = oldFore;
        BackgroundColor = oldBack;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLine(StringW str, PInvoke.Kernel32.ConsoleColor fore)
    {
        if (Silent)
            return;
        var old = ForegroundColor;
        ForegroundColor = fore;
        WriteLine(str);
        ForegroundColor = old;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLine(StringW str, PInvoke.Kernel32.ConsoleColor fore, PInvoke.Kernel32.ConsoleColor back)
    {
        if (Silent)
            return;
        var oldFore = ForegroundColor;
        var oldBack = BackgroundColor;
        ForegroundColor = fore;
        BackgroundColor = back;
        WriteLine(str);
        ForegroundColor = oldFore;
        BackgroundColor = oldBack;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteError(StringW str)
    {
        if (NoErrors | Silent)
            return;
        Write(str, PInvoke.Kernel32.ConsoleColor.Red);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteWarning(StringW str) => Write(str, PInvoke.Kernel32.ConsoleColor.Yellow);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteInfo(StringW str) => Write(str, PInvoke.Kernel32.ConsoleColor.Blue);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteSuccess(StringW str) => Write(str, PInvoke.Kernel32.ConsoleColor.Green);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLineError(StringW str)
    {
        if (NoErrors | Silent)
            return;
        WriteLine(str, PInvoke.Kernel32.ConsoleColor.Red);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLineWarning(StringW str) => WriteLine(str, PInvoke.Kernel32.ConsoleColor.Yellow);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLineInfo(StringW str) => WriteLine(str, PInvoke.Kernel32.ConsoleColor.Blue);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLineSuccess(StringW str) => WriteLine(str, PInvoke.Kernel32.ConsoleColor.Green);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(string str)
    {
        fixed (char* ptr = str)
            Write(new StringW(ptr, (nuint)str.Length));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLine(string str)
    {
        fixed (char* ptr = str)
            WriteLine(new StringW(ptr, (nuint)str.Length));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(string str, PInvoke.Kernel32.ConsoleColor fore)
    {
        fixed (char* ptr = str)
            Write(new StringW(ptr, (nuint)str.Length), fore);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLine(string str, PInvoke.Kernel32.ConsoleColor fore)
    {
        fixed (char* ptr = str)
            WriteLine(new StringW(ptr, (nuint)str.Length), fore);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Write(string str, PInvoke.Kernel32.ConsoleColor fore, PInvoke.Kernel32.ConsoleColor back)
    {
        fixed (char* ptr = str)
            Write(new StringW(ptr, (nuint)str.Length), fore, back);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLine(string str, PInvoke.Kernel32.ConsoleColor fore, PInvoke.Kernel32.ConsoleColor back)
    {
        fixed (char* ptr = str)
            WriteLine(new StringW(ptr, (nuint)str.Length), fore, back);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteError(string str)
    {
        if (NoErrors | Silent)
            return;
        fixed (char* ptr = str)
            WriteError(new StringW(ptr, (nuint)str.Length));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteWarning(string str)
    {
        fixed (char* ptr = str)
            WriteWarning(new StringW(ptr, (nuint)str.Length));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteInfo(string str)
    {
        fixed (char* ptr = str)
            WriteInfo(new StringW(ptr, (nuint)str.Length));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteSuccess(string str)
    {
        fixed (char* ptr = str)
            WriteSuccess(new StringW(ptr, (nuint)str.Length));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLineError(string str)
    {
        if (NoErrors | Silent)
            return;
        fixed (char* ptr = str)
            WriteLineError(new StringW(ptr, (nuint)str.Length));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLineWarning(string str)
    {
        fixed (char* ptr = str)
            WriteLineWarning(new StringW(ptr, (nuint)str.Length));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLineInfo(string str)
    {
        fixed (char* ptr = str)
            WriteLineInfo(new StringW(ptr, (nuint)str.Length));
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteLineSuccess(string str)
    {
        fixed (char* ptr = str)
            WriteLineSuccess(new StringW(ptr, (nuint)str.Length));
    }


    public static StringW Title
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            fixed (void* ptr = &titleBuffer)
                return (StringW)(char*)ptr;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if (PInvoke.Kernel32.SetConsoleTitleW(value))
            {
                fixed (void* ptr = &titleBuffer)
                    NativeMemory.Copy(value, ptr,
                        value.Length >= 255
                            ? 510
                            : value.Length * 2);
            }
        }
    }
}