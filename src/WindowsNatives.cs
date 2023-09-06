using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace SerialPortPatch;

internal static partial class WindowsNatives
{
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool GetCommMask(SafeFileHandle hFile, out UInt32 lpEvtMask);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool SetCommMask(SafeFileHandle hFile, UInt32 dwEvtMask);
}