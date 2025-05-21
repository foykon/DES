using System;
using System.Collections.Generic;
using System.Text;

public static class BitManipulator
{
    // string (metin) → bool[] (bit dizisi)
    public static bool[] StringToBits(string input)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(input);
        return BytesToBits(bytes);
    }

    // bool[] (bit dizisi) → string (metin)
    public static string BitsToString(bool[] bits)
    {
        byte[] bytes = BitsToBytes(bits);
        return Encoding.ASCII.GetString(bytes);
    }

    // byte[] → bool[] (bit dizisi)
    public static bool[] BytesToBits(byte[] bytes)
    {
        bool[] bits = new bool[bytes.Length * 8];

        for (int i = 0; i < bytes.Length; i++)
        {
            for (int bit = 0; bit < 8; bit++)
            {
                bits[i * 8 + bit] = (bytes[i] & (1 << (7 - bit))) != 0;
            }
        }

        return bits;
    }

    // bool[] → byte[]
    public static byte[] BitsToBytes(bool[] bits)
    {
        int byteLength = bits.Length / 8;
        byte[] bytes = new byte[byteLength];

        for (int i = 0; i < byteLength; i++)
        {
            for (int bit = 0; bit < 8; bit++)
            {
                if (bits[i * 8 + bit])
                    bytes[i] |= (byte)(1 << (7 - bit));
            }
        }

        return bytes;
    }

    // Bit dizisini XOR'la
    public static bool[] XOR(bool[] a, bool[] b)
    {
        if (a.Length != b.Length)
            throw new ArgumentException("XOR: Dizi uzunlukları aynı olmalı.");

        bool[] result = new bool[a.Length];

        for (int i = 0; i < a.Length; i++)
        {
            result[i] = a[i] ^ b[i];
        }

        return result;
    }

    // Sol rotasyon (dizi başını sona taşıma)
    public static bool[] LeftRotate(bool[] bits, int count)
    {
        bool[] result = new bool[bits.Length];

        for (int i = 0; i < bits.Length; i++)
        {
            result[i] = bits[(i + count) % bits.Length];
        }

        return result;
    }

    // Dizi parçalayıcı
    public static (bool[], bool[]) SplitInHalf(bool[] input)
    {
        int mid = input.Length / 2;
        bool[] left = new bool[mid];
        bool[] right = new bool[mid];
        Array.Copy(input, 0, left, 0, mid);
        Array.Copy(input, mid, right, 0, mid);
        return (left, right);
    }
}
