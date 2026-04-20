namespace DeleteTemp_BFLAT;

using Native;
using Wrappers;

unsafe static partial class Program
{
    private static DataSize totalSize = default, deletedSize = default;
    private static ulong totalFiles = 0, totalDirs = 0, deletedFiles = 0, deletedDirs = 0;
    private static bool NoCalc, NoCount;

    private static nuint pathOffset, pathLength;

    private static void Start()
    {
        Clean(EnvVar.TempPath);
        Console.WriteLine();
        Console.WriteLine();
        if (!EnvVar.SystemTempPath.IsEmpty)
        {
            Clean(EnvVar.SystemTempPath);
            Console.WriteLine();
            Console.WriteLine();
        }

        StringW free = EnvVar.Free;

        if (!NoCalc)
        {
            Console.WriteLine();
            Console.WriteSuccess(Consts.Total); Console.WriteSuccess(Consts.size); Console.WriteSuccess(Consts.ColonAndSpace);
            Console.WriteLineSuccess(totalSize.ToString(DataSize.StringFormat.Default, free));
            Console.WriteLine();
            Console.WriteSuccess(Consts.Deleted); Console.WriteSuccess(Consts.size); Console.WriteSuccess(Consts.ColonAndSpace);
            Console.WriteLineSuccess(deletedSize.ToString(DataSize.StringFormat.Default, free));
        }
        totalSize = deletedSize = default;
        if (!NoCount)
        {
            Console.WriteLine();
            Console.WriteSuccess(Consts.Total); Console.WriteSuccess(Consts.files); Console.WriteSuccess(Consts.ColonAndSpace);
            Console.WriteLineSuccess(totalFiles.ToString(free));
            Console.WriteLine();
            Console.WriteSuccess(Consts.Total); Console.WriteSuccess(Consts.directories); Console.WriteSuccess(Consts.ColonAndSpace);
            Console.WriteLineSuccess(totalDirs.ToString(free));
            Console.WriteLine();
            Console.WriteSuccess(Consts.Deleted); Console.WriteSuccess(Consts.files); Console.WriteSuccess(Consts.ColonAndSpace);
            Console.WriteLineSuccess(deletedFiles.ToString(free));
            Console.WriteLine();
            Console.WriteSuccess(Consts.Deleted); Console.WriteSuccess(Consts.directories); Console.WriteSuccess(Consts.ColonAndSpace);
            Console.WriteLineSuccess(deletedDirs.ToString(free));
        }
        totalFiles = totalDirs = deletedFiles = deletedDirs = default;

        Console.WriteLine();
        Console.WriteLineSuccess(Consts.CleaningCompleted);
        _ = Console.ReadKey();
    }

    private static void Clean(StringW directory)
    {
        StringW free = EnvVar.Free;
        Console.WriteInfo(Consts.StartingCleaning);
        Console.WriteLineInfo(directory);

        {
            StringArena strArena = StringArena;

            nuint offset = directory.Length - 1;
            NativeMemory.Copy(directory, free, offset * 2);
            strArena.Push(free.Slice(0, offset));

            while (strArena.Pop(out var subDir))
            {
                pathLength = (offset = subDir.Length) + 1;
                NativeMemory.Copy(subDir, free, subDir.Length * 2);
                free[offset++] = '\\'; free[offset++] = '*'; free[offset++] = '\0';
                StringW mask = new(free, offset), path = new(free, offset - 2);
                pathOffset = offset;
                NativeMemory.Copy(subDir, free.Pointer + offset, subDir.Length * 2);
                offset += subDir.Length;
                free[offset++] = '\\';

                Console.WriteInfo(Consts.Processing); Console.WriteInfo(Consts.directory); Console.WriteInfo(Consts.Space);
                offset = path.Slice(0, path.Length - 1).LastIndexOf('\\');
                Console.WriteLineInfo(path.Slice(offset + 1, path.Length - offset - 2));

                _ = IO.EnumerateItems(mask, &EnumerateItems, out var err);
                if (err is not IO.ErrorType.NoError
                    and IO.ErrorType.NotFound
                    or IO.ErrorType.PathNotFound
                    or IO.ErrorType.InvalidHandle
                    or IO.ErrorType.NoMoreFiles)
                {
                    Console.WriteError(Consts.FailedToDelete); Console.WriteError(Consts.directory); Console.Write(Consts.Space);
                    Console.WriteLineError(err.ToString());
                }
                else
                {
                    DataSize altStreams = IO.CalcAltStreamSize(path, out err);
                    totalSize += altStreams;

                    if (err is IO.ErrorType.NoError)
                        if (IO.DeleteDirectory(path, out err))
                        {
                            deletedDirs++;
                            deletedSize += altStreams;
                            Console.WriteLineSuccess(Consts.Deleted);
                        }
                        else
                        {
                            Console.WriteError(Consts.FailedToDelete); Console.WriteError(Consts.directory); Console.Write(Consts.Space);
                            if (err is IO.ErrorType.InvalidHandle
                                or IO.ErrorType.PathNotFound
                                or IO.ErrorType.NotFound
                                or IO.ErrorType.NoMoreFiles)
                                Console.WriteLineError(err.ToString());
                            else
                                Console.WriteLine();
                        }
                    else
                    {
                        Console.WriteError(Consts.FailedToDelete); Console.WriteError(Consts.directory); Console.Write(Consts.Space);
                        if (err is IO.ErrorType.InvalidHandle
                            or IO.ErrorType.PathNotFound
                            or IO.ErrorType.NotFound
                            or IO.ErrorType.NoMoreFiles)
                            Console.WriteLineError(err.ToString());
                        else
                            Console.WriteLine();
                    }
                }
            }

            static bool EnumerateItems(IO.ItemInfo item)
            {
                StringW path = EnvVar.Free.Slice(pathOffset, pathLength + item.Name.Length + 1);
                NativeMemory.Copy(item.Name, path.Pointer + pathLength, item.Name.Length * 2);
                path[path.Length - 1] = '\0';

                totalSize += item.Length;
                if (item.IsDirectory)
                {
                    totalDirs++;
                    StringArena.Push(path.Slice(0, path.Length - 1));
                }
                else
                {
                    totalFiles++;
                    Console.WriteInfo(Consts.Processing); Console.WriteInfo(Consts.file); Console.Write(Consts.Space);
                    Console.WriteLineInfo(item.Name);

                    DataSize altStreams = IO.CalcAltStreamSize(path, out var err);
                    totalSize += altStreams;

                    if (IO.DeleteFile(path, out err))
                    {
                        deletedSize += item.Length;
                        deletedSize += altStreams;
                        deletedFiles++;

                        Console.WriteLineSuccess(Consts.Deleted);
                    }
                    else
                    {
                        Console.WriteError(Consts.FailedToDelete); Console.WriteError(Consts.file); Console.Write(Consts.Space);
                        if (err is IO.ErrorType.InvalidHandle
                            or IO.ErrorType.PathNotFound
                            or IO.ErrorType.NotFound
                            or IO.ErrorType.NoMoreFiles)
                            Console.WriteLineError(err.ToString());
                        else
                            Console.WriteLine();
                    }
                }

                return true;
            }
        }

        Console.WriteLine();
        Console.WriteLineSuccess(Consts.DirectoryCleaned);

        NativeMemory.ClearBlock(free, free.Length * 2);
    }
}