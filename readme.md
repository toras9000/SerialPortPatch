# SerialPortPatch

[![NugetShield]][NugetPackage]

[NugetPackage]: https://www.nuget.org/packages/SerialPortPatch
[NugetShield]: https://img.shields.io/nuget/v/SerialPortPatch

This library provides a small hack method for System.IO.Ports.SerialPort.  

This is intended to disable events corresponding to incoming characters so as not to impede high-speed communication.  
(The purpose is to avoid reception interruption due to reception of EOF (0x1A) character.)  
However, there should be a side effect that software flow control and parity error replacement will not work.  

Supported platforms are Windows only. Otherwise, it has no effect.

```csharp
var port = new SerialPort("COM1");
port.Open();
port.DisableCharEvents();
```
