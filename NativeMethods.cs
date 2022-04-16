using EnvDTE;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace VsReveal
{
    internal static class NativeMethods
    {
        [DllImport("ole32.dll")]
        private static extern int CreateBindCtx(uint reserved, out IBindCtx ppbc);

        public static IEnumerable<DTE> GetRunningVisualStudioInstances()
        {
            IRunningObjectTable? runningObjectTable = null;
            IEnumMoniker? monikerEnumerator = null;

            try
            {
                Marshal.ThrowExceptionForHR(CreateBindCtx(0, out var ctx));
                ctx.GetRunningObjectTable(out runningObjectTable);
                if (runningObjectTable == null)
                {
                    throw new InvalidComObjectException("Unable to get running object table");
                }

                runningObjectTable.EnumRunning(out monikerEnumerator);
                monikerEnumerator.Reset();
                IMoniker[] moniker = new IMoniker[1];
                IntPtr pointerFetchedMonikers = IntPtr.Zero;
                while (monikerEnumerator.Next(1, moniker, pointerFetchedMonikers) == 0)
                { 
                    moniker[0].GetDisplayName(ctx, null, out var runningObjectName);
                    if (runningObjectName.StartsWith("!VisualStudio.DTE.", StringComparison.Ordinal))
                    {
                        Console.WriteLine(runningObjectName);
                        runningObjectTable.GetObject(moniker[0], out var dteObject);
                        if (dteObject is DTE dte)
                        {
                            yield return dte;
                        }
                    }
                }
            }
            finally
            {
                if (runningObjectTable != null)
                {
                    Marshal.ReleaseComObject(runningObjectTable);
                }

                if (monikerEnumerator != null)
                {
                    Marshal.ReleaseComObject(monikerEnumerator);
                }
            }
        }
    }
}
