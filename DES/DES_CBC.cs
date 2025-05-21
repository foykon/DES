using System;
using System.Collections.Generic;
using System.Text;

public static class DES_CBC
{
    public static bool[] EncryptCBC(bool[] plaintextBits, bool[] keyBits, bool[] iv)
    {
        List<bool[]> subKeys = KeyGenerator.GenerateSubKeys(keyBits);

        //eski kod padding yapılmadan önce kullanılıyordu
        int blockSize = 64;
        int totalBlocks = plaintextBits.Length / blockSize;
        bool[] cipherBits = new bool[plaintextBits.Length];

        bool[] previousBlock = iv;

        for (int i = 0; i < totalBlocks; i++)
        {
            // 1. Blok al
            bool[] block = new bool[blockSize];
            Array.Copy(plaintextBits, i * blockSize, block, 0, blockSize);

            // 2. XOR with previous cipher block (or IV)
            bool[] xorBlock = new bool[blockSize];
            for (int j = 0; j < blockSize; j++)
                xorBlock[j] = block[j] ^ previousBlock[j];

            // 3. Encrypt this block
            bool[] encryptedBlock = DESEncryption.Encrypt(xorBlock, subKeys);

            // 4. Kopyala
            Array.Copy(encryptedBlock, 0, cipherBits, i * blockSize, blockSize);
            previousBlock = encryptedBlock; // for next CBC round
        }

        return cipherBits;
    }

    // PKCS7 tarzı padding: eksik kalan yerlere N tane N değeri (örnek: 5 byte eksikse \x05\x05\x05\x05\x05)
    public static bool[] ApplyPadding(bool[] inputBits)
    {
        int blockSize = 64;
        int extraBits = blockSize - (inputBits.Length % blockSize);
        if (extraBits == 0) extraBits = 64; // tam oturuyorsa tam bir blok padding ekle

        int newLength = inputBits.Length + extraBits;
        bool[] padded = new bool[newLength];
        Array.Copy(inputBits, padded, inputBits.Length);

        byte padValue = (byte)(extraBits / 8);
        bool[] padBits = ConvertByteToBits(padValue);

        for (int i = inputBits.Length; i < newLength; i += 8)
        {
            Array.Copy(padBits, 0, padded, i, 8);
        }

        return padded;
    }

    private static bool[] ConvertByteToBits(byte b)
    {
        bool[] bits = new bool[8];
        for (int i = 0; i < 8; i++)
            bits[i] = (b & (1 << (7 - i))) != 0;
        return bits;
    }
}
