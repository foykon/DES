using System;
using System.Collections.Generic;

public static class KeyGenerator
{
    // PC-1 (Permuted Choice 1) → 64-bit key → 56-bit key
    private static readonly int[] PC1 = new int[]
    {
        57, 49, 41, 33, 25, 17,  9,
         1, 58, 50, 42, 34, 26, 18,
        10,  2, 59, 51, 43, 35, 27,
        19, 11,  3, 60, 52, 44, 36,

        63, 55, 47, 39, 31, 23, 15,
         7, 62, 54, 46, 38, 30, 22,
        14,  6, 61, 53, 45, 37, 29,
        21, 13,  5, 28, 20, 12,  4
    };

    // PC-2 (Permuted Choice 2) → 56-bit → 48-bit alt anahtar
    private static readonly int[] PC2 = new int[]
    {
        14, 17, 11, 24,  1,  5,
         3, 28, 15,  6, 21, 10,
        23, 19, 12,  4, 26,  8,
        16,  7, 27, 20, 13,  2,
        41, 52, 31, 37, 47, 55,
        30, 40, 51, 45, 33, 48,
        44, 49, 39, 56, 34, 53,
        46, 42, 50, 36, 29, 32
    };

    // Her tur için kaydırma sayısı (1 veya 2)
    private static readonly int[] ShiftsPerRound = new int[]
    {
        1, 1, 2, 2, 2, 2, 2, 2,
        1, 2, 2, 2, 2, 2, 2, 1
    };

    // Anahtarı alıp 16 adet 48-bit alt anahtar üretir
    public static List<bool[]> GenerateSubKeys(bool[] originalKey64)
    {
        List<bool[]> subKeys = new List<bool[]>();

        // 1. PC-1 uygula (64-bit → 56-bit)
        bool[] key56 = Permutations.ApplyPermutation(originalKey64, PC1);

        // 2. İkiye ayır (C ve D)
        var (C, D) = BitManipulator.SplitInHalf(key56); // her biri 28 bit

        // 3. 16 tur için alt anahtar üret
        for (int i = 0; i < 16; i++)
        {
            // Sol kaydır
            C = BitManipulator.LeftRotate(C, ShiftsPerRound[i]);
            D = BitManipulator.LeftRotate(D, ShiftsPerRound[i]);

            // Birleştir
            bool[] combined = new bool[C.Length + D.Length];
            C.CopyTo(combined, 0);
            D.CopyTo(combined, C.Length);

            // PC-2 uygula (56-bit → 48-bit)
            bool[] subKey48 = Permutations.ApplyPermutation(combined, PC2);
            subKeys.Add(subKey48);
        }

        return subKeys;
    }
}
