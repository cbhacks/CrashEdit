﻿using System.Runtime.InteropServices;

class BufferedTreeView : TreeView
{
    protected override void OnHandleCreated(EventArgs e)
    {
        SendMessage(Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)TVS_EX_DOUBLEBUFFER);
        base.OnHandleCreated(e);
    }
    private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
    private const int TVM_GETEXTENDEDSTYLE = 0x1100 + 45;
    private const int TVS_EX_DOUBLEBUFFER = 0x0004;
    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
}