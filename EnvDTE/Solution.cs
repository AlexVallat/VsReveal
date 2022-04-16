using System.Runtime.InteropServices;

namespace EnvDTE;

[ComImport]
[Guid("26F6CC4B-7A48-4E4D-8AF5-9E960232E05F")]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
public interface Solution
{
	[DispId(42)]
	ProjectItem FindProjectItem([MarshalAs(UnmanagedType.BStr)] string FileName);
}
