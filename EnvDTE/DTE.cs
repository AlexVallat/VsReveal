using System.Runtime.InteropServices;

namespace EnvDTE;

[ComImport]
[Guid("04A72314-32E9-48E2-9B87-A63603454F3E")]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
public interface DTE
{
    [DispId(204)]
    Window MainWindow { get; }

    [DispId(209)]
    Solution Solution { get; }
}
