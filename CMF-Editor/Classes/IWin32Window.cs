using System;
using System.Windows;
using System.Windows.Interop;

namespace CMF_Editor.Classes
{
    class IWin32WindowWrapper : IWin32Window
    {
        public IntPtr Handle { get; }
        public IWin32WindowWrapper(Window window)
        {
            this.Handle = (new WindowInteropHelper(window)).Handle;
        }
    }
}
