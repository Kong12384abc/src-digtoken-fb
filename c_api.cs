using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace c_auth
{
  public class c_api
  {
    private static string api_link = "https://cauth.me/api/";
    private static string user_agent = "Mozilla cAuth";

    private static string program_key { get; set; }

    private static string enc_key { get; set; }

    private static string iv_key { get; set; }

    private static string session_id { get; set; }

    public static void c_init(string c_version, string c_program_key, string c_encryption_key)
    {
      try
      {
        using (WebClient webClient = new WebClient())
        {
          webClient.Proxy = (IWebProxy) null;
          webClient.Headers["User-Agent"] = c_api.user_agent;
          ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(c_encryption.pin_public_key);
          c_api.program_key = c_program_key;
          c_api.iv_key = c_encryption.iv_key();
          c_api.enc_key = c_encryption_key;
          NameValueCollection data = new NameValueCollection()
          {
            ["version"] = c_encryption.encrypt(c_version, c_api.enc_key, "default_iv"),
            ["session_iv"] = c_encryption.encrypt(c_api.iv_key, c_api.enc_key, "default_iv"),
            ["api_version"] = c_encryption.encrypt("3.4b", c_api.enc_key, "default_iv"),
            ["program_key"] = c_encryption.byte_arr_to_str(Encoding.UTF8.GetBytes(c_api.program_key))
          };
          string message = Encoding.UTF8.GetString(webClient.UploadValues(c_api.api_link + "handler.php?type=init", data));
          ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback) ((send, certificate, chain, sslPolicyErrors) => true);
          if (message != "program_doesnt_exist")
          {
            string str1 = c_encryption.decrypt(message, c_api.enc_key, "default_iv");
            string str2 = str1;
            if (str2 != null)
            {
              if (str2 == "killswitch_is_enabled")
              {
                c_messagebox.show("The killswitch of the program is enabled, contact the developer", c_messagebox.c_icons.stop);
                Environment.Exit(0);
                return;
              }
              if (!str2.Contains("wrong_version"))
              {
                if (str2 == "old_api_version")
                {
                  c_messagebox.show("Please download the newest API files on the auth's website", c_messagebox.c_icons.stop);
                  Environment.Exit(0);
                  return;
                }
              }
              else
              {
                c_messagebox.show("Wrong program version", c_messagebox.c_icons.stop);
                Process.Start(str1.Split('|')[1]);
                Environment.Exit(0);
                return;
              }
            }
            string[] strArray = str1.Split('|');
            c_api.iv_key += strArray[1];
            c_api.session_id = strArray[2];
          }
          else
          {
            c_messagebox.show("The program doesnt exist!!", c_messagebox.c_icons.stop);
            Environment.Exit(0);
          }
        }
      }
      catch (CryptographicException ex)
      {
        c_messagebox.show("Invalid API/Encryption key or the session expired", c_messagebox.c_icons.stop);
        Environment.Exit(0);
      }
      catch (Exception ex)
      {
        c_messagebox.show(ex.Message, c_messagebox.c_icons.stop);
        Environment.Exit(0);
      }
    }

    public static bool c_login(string c_username, string c_password, string c_hwid = "default")
    {
      if (c_hwid == "default")
        c_hwid = WindowsIdentity.GetCurrent().User.Value;
      try
      {
        using (WebClient webClient = new WebClient())
        {
          webClient.Proxy = (IWebProxy) null;
          webClient.Headers["User-Agent"] = c_api.user_agent;
          ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(c_encryption.pin_public_key);
          NameValueCollection data = new NameValueCollection()
          {
            ["username"] = c_encryption.encrypt(c_username, c_api.enc_key, c_api.iv_key),
            ["password"] = c_encryption.encrypt(c_password, c_api.enc_key, c_api.iv_key),
            ["hwid"] = c_encryption.encrypt(c_hwid, c_api.enc_key, c_api.iv_key),
            ["sessid"] = c_encryption.byte_arr_to_str(Encoding.UTF8.GetBytes(c_api.session_id))
          };
          string str1 = c_encryption.decrypt(Encoding.UTF8.GetString(webClient.UploadValues(c_api.api_link + "handler.php?type=login", data)), c_api.enc_key, c_api.iv_key);
          ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback) ((send, certificate, chain, sslPolicyErrors) => true);
          string str2 = str1;
          switch (str2)
          {
            case "invalid_hwid":
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Clear();
              Console.WriteLine("Invalid HWID #ERROR404");
              Console.ReadKey();
              Environment.Exit(0);
              return false;
            case "invalid_password":
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Clear();
              Console.WriteLine("Invalid password #ERROR404");
              Console.ReadKey();
              Environment.Exit(0);
              return false;
            case "invalid_username":
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Clear();
              Console.WriteLine("Invalid username #ERROR404");
              Console.ReadKey();
              Environment.Exit(0);
              return false;
            case "killswitch_is_enabled":
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Clear();
              Console.WriteLine("The killswitch of the program is enabled, contact the developer #ERROR404");
              Console.ReadKey();
              Environment.Exit(0);
              return false;
            case "no_sub":
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Clear();
              Console.WriteLine("Your subscription is over #ERROR404");
              Console.ReadKey();
              Environment.Exit(0);
              return false;
            case "user_is_banned":
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Clear();
              Console.WriteLine("The user is banned #ERROR404");
              Console.ReadKey();
              Environment.Exit(0);
              return false;
            case "user_is_paused":
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Clear();
              Console.WriteLine("The user's sub is paused #ERROR404");
              Console.ReadKey();
              Environment.Exit(0);
              return false;
            case null:
              c_messagebox.show("Invalid API/Encryption key or the session expired", c_messagebox.c_icons.information);
              return false;
            default:
              if (str2.Contains("logged_in"))
              {
                string[] strArray = str1.Split('|');
                c_userdata.username = strArray[1];
                c_userdata.email = strArray[2];
                c_userdata.expires = c_encryption.unix_to_date(Convert.ToDouble(strArray[3]));
                c_userdata.var = strArray[4];
                c_userdata.rank = Convert.ToInt32(strArray[5]);
                c_api.stored_pass = c_encryption.encrypt(c_password, c_api.enc_key, c_api.iv_key);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Logged in!!");
                return true;
              }
              goto case null;
          }
        }
      }
      catch (FormatException ex)
      {
        c_messagebox.show("The session expired!!", c_messagebox.c_icons.information);
        Environment.Exit(0);
        return false;
      }
      catch (Exception ex)
      {
        c_messagebox.show(ex.Message, c_messagebox.c_icons.stop);
        Environment.Exit(0);
        return false;
      }
    }

    public static bool c_register(
      string c_username,
      string c_email,
      string c_password,
      string c_token,
      string c_hwid = "default")
    {
      if (c_hwid == "default")
        c_hwid = WindowsIdentity.GetCurrent().User.Value;
      try
      {
        using (WebClient webClient = new WebClient())
        {
          webClient.Proxy = (IWebProxy) null;
          webClient.Headers["User-Agent"] = c_api.user_agent;
          ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(c_encryption.pin_public_key);
          NameValueCollection data = new NameValueCollection()
          {
            ["username"] = c_encryption.encrypt(c_username, c_api.enc_key, c_api.iv_key),
            ["email"] = c_encryption.encrypt(c_email, c_api.enc_key, c_api.iv_key),
            ["password"] = c_encryption.encrypt(c_password, c_api.enc_key, c_api.iv_key),
            ["token"] = c_encryption.encrypt(c_token, c_api.enc_key, c_api.iv_key),
            ["hwid"] = c_encryption.encrypt(c_hwid, c_api.enc_key, c_api.iv_key),
            ["sessid"] = c_encryption.byte_arr_to_str(Encoding.UTF8.GetBytes(c_api.session_id))
          };
          string str = c_encryption.decrypt(Encoding.UTF8.GetString(webClient.UploadValues(c_api.api_link + "handler.php?type=register", data)), c_api.enc_key, c_api.iv_key);
          ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback) ((send, certificate, chain, sslPolicyErrors) => true);
          switch (str)
          {
            case "email_already_exists":
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Clear();
              Console.WriteLine("Email already exists #ERROR404");
              Console.ReadKey();
              Environment.Exit(0);
              return false;
            case "invalid_email_format":
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Clear();
              Console.WriteLine("Invalid email format #ERROR404");
              Console.ReadKey();
              Environment.Exit(0);
              return false;
            case "invalid_token":
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Clear();
              Console.WriteLine("Invalid token #ERROR404");
              Console.ReadKey();
              Environment.Exit(0);
              return false;
            case "killswitch_is_enabled":
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Clear();
              Console.WriteLine("The killswitch of the program is enabled, contact the developer #ERROR404");
              Console.ReadKey();
              Environment.Exit(0);
              return false;
            case "maximum_users_reached":
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Clear();
              Console.WriteLine("Maximum users of the program was reached, please contact the program owner #ERROR404");
              Console.ReadKey();
              Environment.Exit(0);
              return false;
            case "success":
              Console.ForegroundColor = ConsoleColor.Green;
              Console.Clear();
              Console.WriteLine("Success!!");
              return true;
            case "used_token":
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Clear();
              Console.WriteLine("Already used token #ERROR404");
              Console.ReadKey();
              Environment.Exit(0);
              return false;
            case "user_already_exists":
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Clear();
              Console.WriteLine("User already exists #ERROR404");
              Console.ReadKey();
              Environment.Exit(0);
              return false;
            default:
              c_messagebox.show("Invalid API/Encryption key or the session expired", c_messagebox.c_icons.stop);
              Environment.Exit(0);
              return false;
          }
        }
      }
      catch (FormatException ex)
      {
        c_messagebox.show("The session expired!!", c_messagebox.c_icons.information);
        Environment.Exit(0);
        return false;
      }
      catch (Exception ex)
      {
        c_messagebox.show(ex.Message, c_messagebox.c_icons.stop);
        Environment.Exit(0);
        return false;
      }
    }

    public static bool c_activate(string c_username, string c_password, string c_token)
    {
      try
      {
        using (WebClient webClient = new WebClient())
        {
          webClient.Proxy = (IWebProxy) null;
          webClient.Headers["User-Agent"] = c_api.user_agent;
          ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(c_encryption.pin_public_key);
          NameValueCollection data = new NameValueCollection()
          {
            ["username"] = c_encryption.encrypt(c_username, c_api.enc_key, c_api.iv_key),
            ["password"] = c_encryption.encrypt(c_password, c_api.enc_key, c_api.iv_key),
            ["token"] = c_encryption.encrypt(c_token, c_api.enc_key, c_api.iv_key),
            ["sessid"] = c_encryption.byte_arr_to_str(Encoding.UTF8.GetBytes(c_api.session_id))
          };
          string str = c_encryption.decrypt(Encoding.UTF8.GetString(webClient.UploadValues(c_api.api_link + "handler.php?type=activate", data)), c_api.enc_key, c_api.iv_key);
          ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback) ((send, certificate, chain, sslPolicyErrors) => true);
          switch (str)
          {
            case "invalid_password":
              c_messagebox.show("Invalid password", c_messagebox.c_icons.stop);
              Environment.Exit(0);
              return false;
            case "invalid_token":
              c_messagebox.show("Invalid token", c_messagebox.c_icons.stop);
              Environment.Exit(0);
              return false;
            case "invalid_username":
              c_messagebox.show("Invalid username", c_messagebox.c_icons.stop);
              Environment.Exit(0);
              return false;
            case "killswitch_is_enabled":
              c_messagebox.show("The killswitch of the program is enabled, contact the developer", c_messagebox.c_icons.stop);
              return false;
            case "success":
              c_messagebox.show("Success!!", c_messagebox.c_icons.information);
              return true;
            case "used_token":
              c_messagebox.show("Already used token", c_messagebox.c_icons.stop);
              Environment.Exit(0);
              return false;
            case "user_is_banned":
              c_messagebox.show("The user is banned", c_messagebox.c_icons.stop);
              Environment.Exit(0);
              return false;
            case "user_is_paused":
              c_messagebox.show("The user's sub is paused", c_messagebox.c_icons.stop);
              Environment.Exit(0);
              return false;
            default:
              c_messagebox.show("Invalid API/Encryption key or the session expired", c_messagebox.c_icons.stop);
              Environment.Exit(0);
              return false;
          }
        }
      }
      catch (FormatException ex)
      {
        c_messagebox.show("The session expired!!", c_messagebox.c_icons.information);
        Environment.Exit(0);
        return false;
      }
      catch (Exception ex)
      {
        c_messagebox.show(ex.Message, c_messagebox.c_icons.stop);
        Environment.Exit(0);
        return false;
      }
    }

    public static bool c_all_in_one(string c_token, string c_hwid = "default")
    {
      if (c_hwid == "default")
        c_hwid = WindowsIdentity.GetCurrent().User.Value;
      if (c_api.c_login(c_token, c_token, c_hwid))
        return true;
      if (!c_api.c_register(c_token, c_token + "@email.com", c_token, c_token, c_hwid))
        return false;
      Environment.Exit(0);
      return true;
    }

    private static string stored_pass { get; set; }

    public static string c_var(string c_var_name, string c_hwid = "default")
    {
      if (c_hwid == "default")
        c_hwid = WindowsIdentity.GetCurrent().User.Value;
      try
      {
        using (WebClient webClient = new WebClient())
        {
          webClient.Proxy = (IWebProxy) null;
          webClient.Headers["User-Agent"] = c_api.user_agent;
          ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(c_encryption.pin_public_key);
          NameValueCollection data = new NameValueCollection()
          {
            ["var_name"] = c_encryption.encrypt(c_var_name, c_api.enc_key, c_api.iv_key),
            ["username"] = c_encryption.encrypt(c_userdata.username, c_api.enc_key, c_api.iv_key),
            ["password"] = c_api.stored_pass,
            ["hwid"] = c_encryption.encrypt(c_hwid, c_api.enc_key, c_api.iv_key),
            ["sessid"] = c_encryption.byte_arr_to_str(Encoding.UTF8.GetBytes(c_api.session_id))
          };
          string str = c_encryption.decrypt(Encoding.UTF8.GetString(webClient.UploadValues(c_api.api_link + "handler.php?type=var", data)), c_api.enc_key, c_api.iv_key);
          ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback) ((send, certificate, chain, sslPolicyErrors) => true);
          return str;
        }
      }
      catch (FormatException ex)
      {
        c_messagebox.show("The session expired!!", c_messagebox.c_icons.information);
        Environment.Exit(0);
        return "";
      }
      catch (Exception ex)
      {
        c_messagebox.show(ex.Message, c_messagebox.c_icons.stop);
        Environment.Exit(0);
        return "";
      }
    }

    public static void c_log(string c_message)
    {
      if (c_userdata.username == null)
        c_userdata.username = "NONE";
      try
      {
        using (WebClient webClient = new WebClient())
        {
          webClient.Proxy = (IWebProxy) null;
          webClient.Headers["User-Agent"] = c_api.user_agent;
          ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(c_encryption.pin_public_key);
          NameValueCollection data = new NameValueCollection()
          {
            ["username"] = c_encryption.encrypt(c_userdata.username, c_api.enc_key, c_api.iv_key),
            ["message"] = c_encryption.encrypt(c_message, c_api.enc_key, c_api.iv_key),
            ["sessid"] = c_encryption.byte_arr_to_str(Encoding.UTF8.GetBytes(c_api.session_id))
          };
          Encoding.UTF8.GetString(webClient.UploadValues(c_api.api_link + "handler.php?type=log", data));
          ServicePointManager.ServerCertificateValidationCallback += (RemoteCertificateValidationCallback) ((send, certificate, chain, sslPolicyErrors) => true);
        }
      }
      catch (FormatException ex)
      {
        c_messagebox.show("The session expired!!", c_messagebox.c_icons.information);
        Environment.Exit(0);
      }
      catch (Exception ex)
      {
        c_messagebox.show(ex.Message, c_messagebox.c_icons.stop);
        Environment.Exit(0);
      }
    }
  }
}
