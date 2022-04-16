using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VsReveal
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            var fileNames = new HashSet<string>();
            foreach (var arg in args)
            {
                string path;
                try
                {
                    path = Path.GetFullPath(arg);
                }
                catch (Exception)
                {
                    path = arg;
                }
                fileNames.Add(path);
            }

            foreach (var ide in NativeMethods.GetRunningVisualStudioInstances())
            {
                try
                {
                    var solution = ide.Solution;
                    if (solution != null)
                    {
                        var windowActivated = false;
                        foreach (var fileName in fileNames.ToArray())
                        {
                            try
                            {
                                var projectItem = solution.FindProjectItem(fileName);
                                if (projectItem != null)
                                {
                                    var window = projectItem.Open();
                                    if (window != null)
                                    {
                                        window.Activate();
                                        windowActivated = true;
                                        fileNames.Remove(fileName);
                                        Marshal.ReleaseComObject(window);
                                        window = null;
                                    }
                                    Marshal.ReleaseComObject(projectItem);
                                    projectItem = null;
                                }
                            }
                            catch (COMException e)
                            {
                                Debug.WriteLine(e.Message);
                            }
                        }

                        Marshal.ReleaseComObject(solution);
                        solution = null;

                        if (windowActivated)
                        {
                            var mainWindow = ide.MainWindow;
                            mainWindow.Activate();
                            Marshal.ReleaseComObject(mainWindow);
                        }

                        if (fileNames.Count == 0)
                        {
                            break; // No more files to open
                        }
                    }
                    Marshal.ReleaseComObject(ide);
                }
                catch (COMException e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            if (fileNames.Count > 0) // Some files couldn't be opened.
            {
                Application.EnableVisualStyles();

                var openNormally = new TaskDialogCommandLinkButton("&Open normally", "File will be opened with the default action");
                var openInNotepad = new TaskDialogCommandLinkButton("Open in &Notepad", "File will be opened with Notepad");

                var result = TaskDialog.ShowDialog(new TaskDialogPage
                {
                    Caption = "Reveal in Visual Studio",
                    Heading = "Could not reveal file",
                    Text = "File could not be found in any running instance of Visual Studio:\n    • " + string.Join("\n    • ", fileNames.Select(Path.GetFileName)),
                    Buttons = new TaskDialogButtonCollection
                    {
                        openNormally,
                        openInNotepad,
                        TaskDialogButton.Cancel
                    }
                });

                if (result == openNormally)
                {
                    foreach (var fileName in fileNames)
                    {
                        try
                        {
                            Process.Start(new ProcessStartInfo(fileName) { UseShellExecute = true });
                        }
                        catch (Win32Exception e)
                        {
                            Debug.WriteLine(e.Message);
                        }
                    }
                }
                else if (result == openInNotepad)
                {
                    foreach (var fileName in fileNames)
                    {
                        try
                        {
                            Process.Start(new ProcessStartInfo("notepad.exe", fileName) { UseShellExecute = true });
                        }
                        catch (Win32Exception e)
                        {
                            Debug.WriteLine(e.Message);
                        }
                    }
                }
            }
        }
    }
}