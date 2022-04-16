# VsReveal
This utility is convenient when working with multiple solutions in Visual Studio. When you open a file with vs-reveal.exe,
instead of opening it in the most recent Visual Studio instance, or a new instance, it will be opened in the instance of
Visual Studio which already has open the solution containing that file.

For example, if you have two instances of Visual Studio open, one with Frontend.sln open and one with Backend.sln open, and
you open `devenv.exe /Edit scripts.js` then it might open with the Frontend instance, or it might open with the Backend instance
depending on the order they were opened. If, instead, you open `vs-reveal.exe scripts.js` then it will detect that scripts.js
is included in Frontend.sln, and open or reveal the file in that instance.

This is only useful if you typically work with multiple instances of Visual Studio open at the same time, and also work outside
of Visual Studio, for example:
 * Another IDE like VS Code
 * A separate UI for source control
 * Windows Explorer
 * Terminal

For VS Code, [Open in External App](https://marketplace.visualstudio.com/items?itemName=YuTengjing.open-in-external-app) is a useful
extension which allows you to open any file in a defined external editor. If you define vs-reveal.exe as that editor then this
lets you jump to the same file in an open instance of Visual Studio.

If a file passed to vs-reveal is not part of any of the currently open solutions, a prompt appears offering to open the file
normally, or with notepad. If opened normally, it will open with the default Open verb, as if double clicked in Explorer.

## Compatibility
This has only been tested with Visual Studio 2022 on Windows 10. It probably works with other versions too.