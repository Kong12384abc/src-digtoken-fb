using System;
using System.Runtime.InteropServices;

namespace c_auth
{
  public class c_messagebox
  {
    [DllImport("User32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBox(IntPtr h, string m, string c, c_messagebox.c_icons icon);

    public static void show(string text, c_messagebox.c_icons ico)
    {
      c_messagebox.MessageBox((IntPtr) 0, text, "SpecX#9484", ico);
    }

    public enum c_icons
    {
      error = 16, 
      hand = 16, 
      stop = 16, 
      question = 32,  
      exclamation = 48, 
      warning = 48, 
      asterisk = 64, 
      information = 64, 
    }
  }
}
