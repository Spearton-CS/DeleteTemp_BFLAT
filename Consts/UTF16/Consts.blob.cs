using System.Runtime.CompilerServices;

using UTF8_BLOB = System.ReadOnlySpan<byte>;

namespace DeleteTemp_BFLAT;

unsafe partial class Consts
{
    /*
    *UTF16-Display*
    offset(inChars),len(inChars) "utf8"u8
    */
    private static UTF8_BLOB blob =>
        /*De*/
        /*000,002*/ "D\0e\0"u8 +
        /*Init. It will take some microseconds... */
        /*002,040*/ "I\0n\0i\0t\0.\0 \0I\0t\0 \0w\0i\0l\0l\0 \0t\0a\0k\0e\0 \0s\0o\0m\0e\0 \0m\0i\0c\0r\0o\0s\0e\0c\0o\0n\0d\0s\0.\0.\0.\0 \0"u8 +
        /*If it stucked - utility died or your system dying*/
        /*042,049*/ "I\0f\0 \0i\0t\0 \0s\0t\0u\0c\0k\0e\0d\0 \0-\0 \0u\0t\0i\0l\0i\0t\0y\0 \0d\0i\0e\0d\0 \0o\0r\0 \0y\0o\0u\0r\0 \0s\0y\0s\0t\0e\0m\0 \0d\0y\0i\0n\0g\0"u8 +
        
        /*Can utility cleanup? */
        /*091,021*/ "C\0a\0n\0 \0u\0t\0i\0l\0i\0t\0y\0 \0c\0l\0e\0a\0n\0u\0p\0?\0 \0"u8 +
        /*(Y|Enter == yes, S == silent yes, O == options, *Anything other == NO*)*/
        /*112,071*/ "(\0Y\0|\0E\0n\0t\0e\0r\0 \0=\0=\0 \0y\0e\0s\0,\0 \0S\0 \0=\0=\0 \0s\0i\0l\0e\0n\0t\0 \0y\0e\0s\0,\0 \0O\0 \0=\0=\0 \0o\0p\0t\0i\0o\0n\0s\0,\0 \0*\0A\0n\0y\0t\0h\0i\0n\0g\0 \0o\0t\0h\0e\0r\0 \0=\0=\0 \0N\0O\0*\0)\0"u8 +
        
        /*SYSTEM */
        /*183,007*/ "S\0Y\0S\0T\0E\0M\0 \0"u8 +
        /*TEMP directory found: */
        /*190,022*/ "T\0E\0M\0P\0 \0d\0i\0r\0e\0c\0t\0o\0r\0y\0 \0f\0o\0u\0n\0d\0:\0 \0"u8 +

        /*DeleteTemp: BFLAT-tened edition*/
        /*212,031*/ "D\0e\0l\0e\0t\0e\0T\0e\0m\0p\0:\0 \0B\0F\0L\0A\0T\0-\0t\0e\0n\0e\0d\0 \0e\0d\0i\0t\0i\0o\0n\0"u8 +

        /*Init finished;*/
        /*243,014*/ "I\0n\0i\0t\0 \0f\0i\0n\0i\0s\0h\0e\0d\0;\0"u8 +

        /*Exception*/
        /*257,009*/ "E\0x\0c\0e\0p\0t\0i\0o\0n\0"u8 +
        /*IndexOutOfRange*/
        /*266,015*/ "I\0n\0d\0e\0x\0O\0u\0t\0O\0f\0R\0a\0n\0g\0e\0"u8 +
        /*OutOfMemory*/
        /*281,011*/ "O\0u\0t\0O\0f\0M\0e\0m\0o\0r\0y\0"u8 +
        /*NullReference*/
        /*292,013*/ "N\0u\0l\0l\0R\0e\0f\0e\0r\0e\0n\0c\0e\0"u8 +
        /*InvalidAlignment*/
        /*305,016*/ "I\0n\0v\0a\0l\0i\0d\0A\0l\0i\0g\0n\0m\0e\0n\0t\0"u8 +
        /*TempPathUnavailable*/
        /*321,019*/ "T\0e\0m\0p\0P\0a\0t\0h\0U\0n\0a\0v\0a\0i\0l\0a\0b\0l\0e\0"u8 +

        /*NativeMemory.*/
        /*340,013*/ "N\0a\0t\0i\0v\0e\0M\0e\0m\0o\0r\0y\0.\0"u8 +
        /*Alloc()*/
        /*353,007*/ "A\0l\0l\0o\0c\0(\0)\0"u8 +
        /*Realloc()*/
        /*360,009*/ "R\0e\0a\0l\0l\0o\0c\0(\0)\0"u8 +
        /*AllocAligned()*/
        /*369,014*/ "A\0l\0l\0o\0c\0A\0l\0i\0g\0n\0e\0d\0(\0)\0"u8 +
        /*ReallocAligned()*/
        /*383,016*/ "R\0e\0a\0l\0l\0o\0c\0A\0l\0i\0g\0n\0e\0d\0(\0)\0"u8 +

        /*StringW.*/
        /*399,008*/ "S\0t\0r\0i\0n\0g\0W\0.\0"u8 +
        /*this[]*/
        /*407,006*/ "t\0h\0i\0s\0[\0]\0"u8 +
        /*Slice()*/
        /*413,007*/ "S\0l\0i\0c\0e\0(\0)\0"u8 +

        /*List<T>.*/
        /*420,008*/ "L\0i\0s\0t\0<\0T\0>\0.\0"u8 +
        /*this()*/
        /*428,006*/ "t\0h\0i\0s\0(\0)\0"u8 +
        /*RemoveAt()*/
        /*434,010*/ "R\0e\0m\0o\0v\0e\0A\0t\0(\0)\0"u8 +

        /*Console.*/
        /*444,008*/ "C\0o\0n\0s\0o\0l\0e\0.\0"u8 +
        /*Write()*/
        /*452,007*/ "W\0r\0i\0t\0e\0(\0)\0"u8 +
        /*WriteLine()*/
        /*459,011*/ "W\0r\0i\0t\0e\0L\0i\0n\0e\0(\0)\0"u8 +

        /*%SYSTEMTEMP% (in normal Windows is %WINDIR%\Temp) is unavailable (not found or have no rights for reading)*/
        /*470,106*/ "%\0S\0Y\0S\0T\0E\0M\0T\0E\0M\0P\0%\0 \0(\0i\0n\0 \0n\0o\0r\0m\0a\0l\0 \0W\0i\0n\0d\0o\0w\0s\0 \0i\0s\0 \0%\0W\0I\0N\0D\0I\0R\0%\0\\\0T\0e\0m\0p\0)\0 \0i\0s\0 \0u\0n\0a\0v\0a\0i\0l\0a\0b\0l\0e\0 \0(\0n\0o\0t\0 \0f\0o\0u\0n\0d\0 \0o\0r\0 \0h\0a\0v\0e\0 \0n\0o\0 \0r\0i\0g\0h\0t\0s\0 \0f\0o\0r\0 \0r\0e\0a\0d\0i\0n\0g\0)\0"u8 +
        /*Use mask, not path*/
        /*576,018*/ "U\0s\0e\0 \0m\0a\0s\0k\0,\0 \0n\0o\0t\0 \0p\0a\0t\0h\0"u8 +
        /*EnumerateItems()*/
        /*594,016*/ "E\0n\0u\0m\0e\0r\0a\0t\0e\0I\0t\0e\0m\0s\0(\0)\0"u8 +
        /*IO.*/
        /*610,003*/ "I\0O\0.\0"u8 +

        /*TEMP\0*/
        /*613,4|5*/ "T\0E\0M\0P\0\0\0"u8 +
        
        /*StringArena*/
        /*618,011*/ "S\0t\0r\0i\0n\0g\0A\0r\0e\0n\0a\0"u8 +
        /*Push*/
        /*629,004*/ "P\0u\0s\0h\0"u8 +
        
        /*TB\0GB\0MB\0KB\0*/
        /*633,012*/ "T\0B\0\0\0G\0B\0\0\0M\0B\0\0\0K\0B\0\0\0"u8 +
        
        /*ulong.MaxValue*/
        /*645,014*/ "u\0l\0o\0n\0g\0.\0M\0a\0x\0V\0a\0l\0u\0e\0"u8 +

        /*::$DATA*/
        /*659,007*/ ":\0:\0$\0D\0A\0T\0A\0"u8 +
        /*:Zone.Identifier:$DATA*/
        /*666,022*/ ":\0Z\0o\0n\0e\0.\0I\0d\0e\0n\0t\0i\0f\0i\0e\0r\0:\0$\0D\0A\0T\0A\0"u8 +

        /*Cleaning completed. Press key to close*/
        /*688,038*/ "C\0l\0e\0a\0n\0i\0n\0g\0 \0c\0o\0m\0p\0l\0e\0t\0e\0d\0.\0 \0P\0r\0e\0s\0s\0 \0k\0e\0y\0 \0t\0o\0 \0c\0l\0o\0s\0e\0"u8 +
        /*Starting cleaning */
        /*726,018*/ "S\0t\0a\0r\0t\0i\0n\0g\0 \0c\0l\0e\0a\0n\0i\0n\0g\0 \0"u8 +
        /*Processing */
        /*744,011*/ "P\0r\0o\0c\0e\0s\0s\0i\0n\0g\0 \0"u8 +
        /*Failed to delete: */
        /*755,018*/ "F\0a\0i\0l\0e\0d\0 \0t\0o\0 \0d\0e\0l\0e\0t\0e\0:\0 \0"u8 +
        /*Directory cleaned*/
        /*773,017*/ "D\0i\0r\0e\0c\0t\0o\0r\0y\0 \0c\0l\0e\0a\0n\0e\0d\0"u8 +

        /*Total */
        /*790,006*/ "T\0o\0t\0a\0l\0 \0"u8 +
        /*Deleted */
        /*796,008*/ "D\0e\0l\0e\0t\0e\0d\0 \0"u8 +
        /*size*/
        /*804,004*/ "s\0i\0z\0e\0"u8 +
        /*files*/
        /*808,005*/ "f\0i\0l\0e\0s\0"u8 +
        /*directories*/
        /*813,011*/ "d\0i\0r\0e\0c\0t\0o\0r\0i\0e\0s\0"u8 +
        /*directory*/
        /*824,009*/ "d\0i\0r\0e\0c\0t\0o\0r\0y\0"u8 +

        /*PathNotFound*/
        /*830,012*/ "P\0a\0t\0h\0N\0o\0t\0F\0o\0u\0n\0d\0"u8 +
        /*AccessDenied*/
        /*842,012*/ "A\0c\0c\0e\0s\0s\0D\0e\0n\0i\0e\0d\0"u8 +
        /*InvalidHandle*/
        /*854,013*/ "I\0n\0v\0a\0l\0i\0d\0H\0a\0n\0d\0l\0e\0"u8 +
        /*SharingViolation*/
        /*867,016*/ "S\0h\0a\0r\0i\0n\0g\0V\0i\0o\0l\0a\0t\0i\0o\0n\0"u8 +
        /*Unknown*/
        /*883,007*/ "U\0n\0k\0n\0o\0w\0n\0"u8;

    private static UTF8_BLOB optionsHelpBlob =>
        /**To escape settings (and utility) press ESC or DELETE or BACKSPACE or Q**/
        /*000,74*/ "*\0T\0o\0 \0e\0s\0c\0a\0p\0e\0 \0s\0e\0t\0t\0i\0n\0g\0s\0 \0(\0a\0n\0d\0 \0u\0t\0i\0l\0i\0t\0y\0)\0 \0p\0r\0e\0s\0s\0 \0E\0S\0C\0 \0o\0r\0 \0D\0E\0L\0E\0T\0E\0 \0o\0r\0 \0B\0A\0C\0K\0S\0P\0A\0C\0E\0 \0o\0r\0 \0Q\0*\0\r\0\n\0"u8 +
        /*074,02*/ "\r\0\n\0"u8 +
        /*Startup parameters is:*/
        /*076,24*/ "S\0t\0a\0r\0t\0u\0p\0 \0p\0a\0r\0a\0m\0e\0t\0e\0r\0s\0 \0i\0s\0:\0\r\0\n\0"u8 +
        /*--silent (No console output, means there's --no-calc and --no-count)*/
        /*100,70*/ "-\0-\0s\0i\0l\0e\0n\0t\0 \0(\0N\0o\0 \0c\0o\0n\0s\0o\0l\0e\0 \0o\0u\0t\0p\0u\0t\0,\0 \0m\0e\0a\0n\0s\0 \0t\0h\0e\0r\0e\0'\0s\0 \0-\0-\0n\0o\0-\0c\0a\0l\0c\0 \0a\0n\0d\0 \0-\0-\0n\0o\0-\0c\0o\0u\0n\0t\0)\0\r\0\n\0"u8 +
        /*--no-calc (No calculation of sizes)*/
        /*170,37*/ "-\0-\0n\0o\0-\0c\0a\0l\0c\0 \0(\0N\0o\0 \0c\0a\0l\0c\0u\0l\0a\0t\0i\0o\0n\0 \0o\0f\0 \0s\0i\0z\0e\0s\0)\0\r\0\n\0"u8 +
        /*--no-count (No counting of files and dirs)*/
        /*207,44*/ "-\0-\0n\0o\0-\0c\0o\0u\0n\0t\0 \0(\0N\0o\0 \0c\0o\0u\0n\0t\0i\0n\0g\0 \0o\0f\0 \0f\0i\0l\0e\0s\0 \0a\0n\0d\0 \0d\0i\0r\0s\0)\0\r\0\n\0"u8 +
        /*--no-errors (No console output of errors)*/
        /*251,43*/ "-\0-\0n\0o\0-\0e\0r\0r\0o\0r\0s\0 \0(\0N\0o\0 \0c\0o\0n\0s\0o\0l\0e\0 \0o\0u\0t\0p\0u\0t\0 \0o\0f\0 \0e\0r\0r\0o\0r\0s\0)\0\r\0\n\0"u8;

    private static char* blobptr
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            fixed (byte* ptr = &blob[0])
                return (char*)ptr;
        }
    }
    private static char* optionsHelpBlobptr
    {
        get
        {
            fixed (byte* ptr = &optionsHelpBlob[0])
                return (char*)ptr;
        }
    }
}