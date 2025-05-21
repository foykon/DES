using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DES
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PrintWithLibrary();
            Console.WriteLine("\n\n\tChanging ENC Path\n\n");
            PrintWitClass();
           
        }

        static void PrintWithLibrary()
        {
            #region Input

            Console.WriteLine(".NET Encryption Works\n");
            Console.WriteLine("using System.Security.Cryptography;\n");

            Console.Write("Enter the text to be encrypted:\t");
            string plainText = Console.ReadLine();

            Console.Write("Enter an 8-character key:\t");
            string keyText = Console.ReadLine();

            Console.Write("Enter 8 character IV:\t\t");
            string ivText = Console.ReadLine();

            if (keyText.Length != 8 || ivText.Length != 8)
            {
                Console.WriteLine("Error: Key and IV must be 8 characters.\t");
                return;
            }

            // ASCII -> byte[]
            byte[] key = Encoding.UTF8.GetBytes(keyText);
            byte[] iv = Encoding.UTF8.GetBytes(ivText);
            #endregion

            // Output
            string encrypted = EncryptDES(plainText, key, iv);
            Console.WriteLine("Encrypted output(Base64):\t\t" + encrypted);
            byte[] encryptedBytes = Convert.FromBase64String(encrypted);
            Console.WriteLine("Encrypted output(HEX):\t\t\t" + BitConverter.ToString(encryptedBytes).Replace("-", ""));

        }

        static string EncryptDES(string plainText, byte[] key, byte[] iv)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    cs.Write(plainBytes, 0, plainBytes.Length);
                    cs.FlushFinalBlock();

                    // Base64 string olarak çıktı ver
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        static void PrintWitClass()
        {
            #region Input
            Console.WriteLine("DES CBC Encryption Works\t");
            Console.WriteLine("using DES_CBC\t");

            Console.Write("Enter the text to be encrypted:\t");
            string plainText = Console.ReadLine();

            Console.Write("Enter an 8-character key:\t");
            string keyText = Console.ReadLine();

            Console.Write("Enter 8 character IV:\t\t");
            string ivText = Console.ReadLine();

            if (keyText.Length != 8 || ivText.Length != 8)
            {
                Console.WriteLine("❌ Error: Key and IV must be 8 characters.\t");
                return;
            }

            // ASCII -> bit[]
            bool[] plainBits = ConvertToBits(plainText);
            bool[] keyBits = ConvertToBits(keyText);
            bool[] ivBits = ConvertToBits(ivText);
            #endregion

            // Padding uygula
            plainBits = DES_CBC.ApplyPadding(plainBits);

            // CBC Şifreleme
            bool[] cipherBits = DES_CBC.EncryptCBC(plainBits, keyBits, ivBits);

            // Hex çıktı
            string cipherHex = ConvertToHex(cipherBits);

            // Output
            string base64Output = Convert.ToBase64String(BitsToBytes(cipherBits));
            Console.WriteLine($"Encrypted output with CBC (Base64):\t {base64Output}");
            Console.WriteLine($"Encrypted output with CBC (HEX):\t {cipherHex}");


        }
        // String'i bit dizisine çevirir
        static bool[] ConvertToBits(string text)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            bool[] bits = new bool[bytes.Length * 8];

            for (int i = 0; i < bytes.Length; i++)
            {
                for (int j = 7; j >= 0; j--)
                {
                    bits[i * 8 + (7 - j)] = (bytes[i] & (1 << j)) != 0;
                }
            }

            return bits;
        }

        static byte[] BitsToBytes(bool[] bits)
        {
            int numBytes = bits.Length / 8;
            byte[] bytes = new byte[numBytes];
            for (int i = 0; i < numBytes; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (bits[i * 8 + j])
                        bytes[i] |= (byte)(1 << (7 - j));
                }
            }
            return bytes;
        }

        // Bit dizisini hexadecimal string'e çevirir
        static string ConvertToHex(bool[] bits)
        {
            StringBuilder hex = new StringBuilder();

            for (int i = 0; i < bits.Length; i += 4)
            {
                int value = 0;
                for (int j = 0; j < 4; j++)
                {
                    if (bits[i + j])
                        value += 1 << (3 - j);
                }
                hex.Append(value.ToString("X"));
            }

            return hex.ToString();
        }

    }
}
