using System.Runtime.InteropServices;

namespace EnvDTE;

[ComImport]
[Guid("0BEAB46B-4C07-4F94-A8D7-1626020E4E53")]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
public interface Window
{
	[DispId(131)]
	void Activate();
}
