using System.Runtime.CompilerServices;

namespace DeleteTemp_BFLAT;

using Native;
using Wrappers;

internal unsafe static partial class Program
{
    private static EnvVar EnvVar;
    private static StringArena StringArena;

    private static void Main()
    {
        _ = Console.Args.Split(' ', &ParseArg);

        Init();

        Console.WriteLineWarning(Consts.CanUtilityCleanup);

        var key = Console.ReadKey();
        if (key is PInvoke.Kernel32.ConsoleKey.Y or PInvoke.Kernel32.ConsoleKey.Enter)
            Start();
        else if (key is PInvoke.Kernel32.ConsoleKey.S)
        {
            Console.Silent = true;
            Start();
        }
        else if (key is PInvoke.Kernel32.ConsoleKey.O)
        {
            Console.WriteLineInfo(Consts.OptionsHELP);

            while ((key = Console.ReadKey()) is not PInvoke.Kernel32.ConsoleKey.Escape
                and not PInvoke.Kernel32.ConsoleKey.Backspace
                and not PInvoke.Kernel32.ConsoleKey.Delete
                and not PInvoke.Kernel32.ConsoleKey.Q)
                switch (key)
                {
                    case PInvoke.Kernel32.ConsoleKey.Y:
                        Start();
                        break;
                    case PInvoke.Kernel32.ConsoleKey.D1:
                        Console.Silent = !Console.Silent;
                        break;
                    case PInvoke.Kernel32.ConsoleKey.D2:
                        NoCalc = !NoCalc;
                        break;
                    case PInvoke.Kernel32.ConsoleKey.D3:
                        NoCount = !NoCount;
                        break;
                    case PInvoke.Kernel32.ConsoleKey.D4:
                        Console.NoErrors = !Console.NoErrors;
                        break;
                }
        }

        Deinit();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool ParseArg(StringW arg)
    {
        if (arg == Consts.SilentArg)
            Console.Silent = true;
        else if (arg == Consts.NoCalcArg)
            NoCalc = true;
        else if (arg == Consts.NoCountArg)
            NoCount = true;
        else if (arg == Consts.NoErrorsArg)
            Console.NoErrors = true;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Init()
    {
        Console.ForegroundColor = PInvoke.Kernel32.ConsoleColor.White;
        Console.BackgroundColor = PInvoke.Kernel32.ConsoleColor.Black;

        Console.WriteLineInfo(Consts.InitWillTakeSomeMcSecs);

        EnvVar.Init(ref EnvVar);
        StringArena = new();

        Console.Title = new(Consts.ConsoleTitle, (nuint)Consts.ConsoleTitle.Length);

        Console.WriteLineSuccess(Consts.InitFinished);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Deinit()
    {
        Console.WriteLineInfo(Consts.DeInitWillTakeSomeMcSecs);

        Console.Title = EnvVar.OldConsoleTitle;
        Console.ResetColor();
        StringArena.Dispose();
    }
}