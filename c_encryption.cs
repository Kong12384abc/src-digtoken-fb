using System;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace c_auth
{
  public class c_encryption
  {
    public static string byte_arr_to_str(byte[] ba)
    {
      StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
      foreach (byte num in ba)
        stringBuilder.AppendFormat("{0:x2}", (object) num);
      return stringBuilder.ToString();
    }

    public static byte[] str_to_byte_arr(string hex)
    {
      int length = hex.Length;
      byte[] numArray = new byte[length / 2];
      for (int startIndex = 0; startIndex < length; startIndex += 2)
        numArray[startIndex / 2] = Convert.ToByte(hex.Substring(startIndex, 2), 16);
      return numArray;
    }

    public static string EncryptString(string plainText, byte[] key, byte[] iv)
    {
      Aes aes = Aes.Create();
      aes.Mode = CipherMode.CBC;
      aes.Key = key;
      aes.IV = iv;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (ICryptoTransform encryptor = aes.CreateEncryptor())
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
          {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            return c_encryption.byte_arr_to_str(memoryStream.ToArray());
          }
        }
      }
    }

    public static string DecryptString(string cipherText, byte[] key, byte[] iv)
    {
      Aes aes = Aes.Create();
      aes.Mode = CipherMode.CBC;
      aes.Key = key;
      aes.IV = iv;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (ICryptoTransform decryptor = aes.CreateDecryptor())
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Write))
          {
            byte[] byteArr = c_encryption.str_to_byte_arr(cipherText);
            cryptoStream.Write(byteArr, 0, byteArr.Length);
            cryptoStream.FlushFinalBlock();
            byte[] array = memoryStream.ToArray();
            return Encoding.UTF8.GetString(array, 0, array.Length);
          }
        }
      }
    }

    public static string iv_key()
    {
      Guid guid = Guid.NewGuid();
      string str = guid.ToString();
      guid = Guid.NewGuid();
      int length = guid.ToString().IndexOf("-", StringComparison.Ordinal);
      return str.Substring(0, length);
    }

    public static string sha256(string randomString)
    {
      return c_encryption.byte_arr_to_str(new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(randomString)));
    }

    public static string encrypt(string message, string enc_key, string iv = "default_iv")
    {
      byte[] bytes = Encoding.UTF8.GetBytes(c_encryption.sha256(enc_key).Substring(0, 32));
      return iv == "default_iv" ? c_encryption.EncryptString(message, bytes, Encoding.UTF8.GetBytes("1514834626578394")) : c_encryption.EncryptString(message, bytes, Encoding.UTF8.GetBytes(c_encryption.sha256(iv).Substring(0, 16)));
    }

    public static string decrypt(string message, string enc_key, string iv = "default_iv")
    {
      byte[] bytes = Encoding.UTF8.GetBytes(c_encryption.sha256(enc_key).Substring(0, 32));
      return iv == "default_iv" ? c_encryption.DecryptString(message, bytes, Encoding.UTF8.GetBytes("1514834626578394")) : c_encryption.DecryptString(message, bytes, Encoding.UTF8.GetBytes(c_encryption.sha256(iv).Substring(0, 16)));
    }

    public static DateTime unix_to_date(double unixTimeStamp)
    {
      DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
      dateTime = dateTime.AddSeconds(unixTimeStamp);
      return dateTime.ToLocalTime();
    }

    public static bool pin_public_key(
      object sender,
      X509Certificate certificate,
      X509Chain chain,
      SslPolicyErrors sslPolicyErrors)
    {
      return certificate != null && certificate.GetPublicKeyString().Equals("3082010A0282010100C7429D4B4591E50FE4B3ABDA72DB3F3EA578E12B9CD4E228E4EDFAC3F9681F354C913386A13E88181D1B14D91723FB50770C5DC94FCA59D4DEE4F6632041EFE76C3B6BCFF6B8F5B38AF92547D04BD08AF71087B094F5DFE8760C8CD09A3771836807588B02282BEC7C4CD73EE7C650C0A7C7F36F2FA56DA17E892B2760C4C75950EA5C90CD4EA301EC0CBC36B8372FE8515A7131CC6DF13A97D95B94C6A92AC4E5BFF217FCB20B3C01DB085229E919555D426D919E9A9F0D4C599FE7473FA7DBDE9B33279E2FC29F6CE09FA1269409E4A82175C8E0B65723DB6F856A53E3FD11363ADD63D1346790A3E4D1E454D1714ECED9815A0F85C5019C0D4DC3D58234C10203010001");
    }
  }
}
