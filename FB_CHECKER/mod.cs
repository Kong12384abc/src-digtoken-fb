using System;
using System.IO;
using System.Net;
using System.Threading;

namespace FB_CHECKER
{
  internal class mod
  {
    public static void Text()
    {
      string longTimeString = DateTime.Now.ToLongTimeString();
      Console.ForegroundColor = ConsoleColor.Blue;
      Console.Write(longTimeString);
      Console.ForegroundColor = ConsoleColor.White;
      Console.Write(" | ");
      Thread.Sleep(3500);
      Console.ForegroundColor = ConsoleColor.Green;
      Console.Write("DIG TOKEN  By Kong12384abc");
      Thread.Sleep(2500);
      Console.Clear();
    }

    public static void text1()
    {
      Console.ForegroundColor = ConsoleColor.Blue;
      Console.Write("FB : https://www.facebook.com/Kong12384abc");
      Thread.Sleep(2500);
      Console.Clear();
    }

    public static void dwacc()
    {
      string path1 = "C:\\Users\\" + Environment.UserName + "\\OneDrive\\Desktop\\file";
      string path2 = "C:\\Users\\" + Environment.UserName + "\\OneDrive\\Desktop\\file\\cat";
      if (!Directory.Exists(path1))
        Directory.CreateDirectory(path1);
      if (!Directory.Exists(path2))
        Directory.CreateDirectory(path2);
      WebClient webClient = new WebClient();
      webClient.DownloadFile("https://cdn.discordapp.com/attachments/709372966907150346/729046100023181432/Newtonsoft.Json.dll", "C:\\Users\\" + Environment.UserName + "\\OneDrive\\Desktop\\file\\cat\\combo.txt");
      webClient.DownloadFile("https://cdn.discordapp.com/attachments/709372966907150346/729046100023181432/Newtonsoft.Json.dll", "C:\\Users\\" + Environment.UserName + "\\OneDrive\\Desktop\\file\\cat\\proxy.txt");
      Console.Beep(500, 200);
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("Successfuly!");
      Thread.Sleep(3000);
      Console.Clear();
    }

    public static void dwnoacc()
    {
      string path1 = "C:\\Users\\" + Environment.UserName + "\\Desktop\\file";
      string path2 = "C:\\Users\\" + Environment.UserName + "\\Desktop\\file\\cat";
      if (!Directory.Exists(path1))
        Directory.CreateDirectory(path1);
      if (!Directory.Exists(path2))
        Directory.CreateDirectory(path2);
      WebClient webClient = new WebClient();
      webClient.DownloadFile("https://cdn.discordapp.com/attachments/709372966907150346/729046100023181432/Newtonsoft.Json.dll", "C:\\Users\\" + Environment.UserName + "\\Desktop\\file\\cat\\combo.txt");
      webClient.DownloadFile("https://cdn.discordapp.com/attachments/709372966907150346/729046100023181432/Newtonsoft.Json.dll", "C:\\Users\\" + Environment.UserName + "\\Desktop\\file\\cat\\proxy.txt");
      Console.Beep(500, 200);
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("Successfuly!");
      Thread.Sleep(3000);
      Console.Clear();
    }

    public static void dw()
    {
      WebClient webClient = new WebClient();
      webClient.DownloadFile("https://picatmin100563.000webhostapp.com/combo.txt", "combo.txt");
      webClient.DownloadFile("https://picatmin100563.000webhostapp.com/proxy.txt", "proxy.txt");
      Console.Beep(500, 200);
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("Successfuly!");
      Thread.Sleep(3000);
      Console.Clear();
    }
  }
}
