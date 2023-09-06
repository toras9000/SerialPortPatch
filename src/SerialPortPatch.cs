using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using static SerialPortPatch.WindowsNatives;

namespace System.IO.Ports;

/// <summary>SerialPortクラスの拡張メソッド</summary>
public static class SerialPortPatch
{
    /// <summary>COMポートのキャラクタ受信イベントを無効化する</summary>
    /// <remarks>
    /// このメソッドは Windows のみをサポートする。
    /// その他のプラットフォームでは何も効果はない。
    /// </remarks>
    /// <param name="self">対象ポートインスタンス</param>
    /// <returns>設定の成否</returns>
    public static bool DisableCharEvents(this SerialPort self)
    {
        ArgumentNullException.ThrowIfNull(self);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (!self.IsOpen) throw new InvalidOperationException("Port is not open.");
            return DisableCharEventsWindows(self);
        }

        return false;
    }

    private static bool DisableCharEventsWindows(SerialPort self)
    {
        var streamInstance = self.BaseStream;
        var streamType = streamInstance.GetType();
        var privateBinding = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        var handleField = streamType.GetField("_handle", privateBinding);
        if (handleField == null) return false;

        var handleValue = handleField.GetValue(streamInstance) as SafeFileHandle;
        if (handleValue == null) return false;

        var getResult = GetCommMask(handleValue, out var mask);
        if (!getResult) return false;

        const UInt32 EV_RXFLAG = 0x0002;
        var newMask = mask & (~EV_RXFLAG);
        var setResult = SetCommMask(handleValue, newMask);
        if (!setResult) return false;

        return true;
    }
}

