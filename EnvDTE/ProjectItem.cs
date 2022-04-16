using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EnvDTE;

[ComImport]
[Guid("0B48100A-473E-433C-AB8F-66B9739AB620")]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
public interface ProjectItem
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	[DispId(205)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Window Open([In][MarshalAs(UnmanagedType.BStr)] string ViewKind = "{00000000-0000-0000-0000-000000000000}");
}
