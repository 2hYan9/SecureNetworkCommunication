using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommunication
{
    class DataEncryptionStandard
    {
        public static byte[] IP = { 58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8, 57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3, 61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7 };
        // Expansion permute table
        public static byte[] E = { 32, 1, 2, 3, 4, 5, 4, 5, 6, 7, 8, 9, 8, 9, 10, 11, 12, 13, 12, 13, 14, 15, 16, 17, 16, 17, 18, 19, 20, 21, 20, 21, 22, 23, 24, 25, 24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1 };
        // P-box permute
        public static byte[] P = { 16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31, 10, 2, 8, 24, 14, 32, 27, 3, 9, 19, 13, 30, 6, 22, 11, 4, 25 };
        // Key's PC2 permute
        public static byte[] PC2 = { 14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32 };
        // Key's PC1 permute
        public static byte[] PC1 = { 57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36, 63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4 };
        // S-box permute
        public static byte[,] S = { { 0xe, 0x0, 0x4, 0xf, 0xd, 0x7, 0x1, 0x4, 0x2, 0xe, 0xf, 0x2, 0xb, 0xd, 0x8, 0x1, 0x3, 0xa, 0xa, 0x6, 0x6, 0xc, 0xc, 0xb, 0x5, 0x9, 0x9, 0x5, 0x0, 0x3, 0x7, 0x8, 0x4, 0xf, 0x1, 0xc, 0xe, 0x8, 0x8, 0x2, 0xd, 0x4, 0x6, 0x9, 0x2, 0x1, 0xb, 0x7, 0xf, 0x5, 0xc, 0xb, 0x9, 0x3, 0x7, 0xe, 0x3, 0xa, 0xa, 0x0, 0x5, 0x6, 0x0, 0xd }, { 0xf, 0x3, 0x1, 0xd, 0x8, 0x4, 0xe, 0x7, 0x6, 0xf, 0xb, 0x2, 0x3, 0x8, 0x4, 0xe, 0x9, 0xc, 0x7, 0x0, 0x2, 0x1, 0xd, 0xa, 0xc, 0x6, 0x0, 0x9, 0x5, 0xb, 0xa, 0x5, 0x0, 0xd, 0xe, 0x8, 0x7, 0xa, 0xb, 0x1, 0xa, 0x3, 0x4, 0xf, 0xd, 0x4, 0x1, 0x2, 0x5, 0xb, 0x8, 0x6, 0xc, 0x7, 0x6, 0xc, 0x9, 0x0, 0x3, 0x5, 0x2, 0xe, 0xf, 0x9 }, { 0xa, 0xd, 0x0, 0x7, 0x9, 0x0, 0xe, 0x9, 0x6, 0x3, 0x3, 0x4, 0xf, 0x6, 0x5, 0xa, 0x1, 0x2, 0xd, 0x8, 0xc, 0x5, 0x7, 0xe, 0xb, 0xc, 0x4, 0xb, 0x2, 0xf, 0x8, 0x1, 0xd, 0x1, 0x6, 0xa, 0x4, 0xd, 0x9, 0x0, 0x8, 0x6, 0xf, 0x9, 0x3, 0x8, 0x0, 0x7, 0xb, 0x4, 0x1, 0xf, 0x2, 0xe, 0xc, 0x3, 0x5, 0xb, 0xa, 0x5, 0xe, 0x2, 0x7, 0xc }, { 0x7, 0xd, 0xd, 0x8, 0xe, 0xb, 0x3, 0x5, 0x0, 0x6, 0x6, 0xf, 0x9, 0x0, 0xa, 0x3, 0x1, 0x4, 0x2, 0x7, 0x8, 0x2, 0x5, 0xc, 0xb, 0x1, 0xc, 0xa, 0x4, 0xe, 0xf, 0x9, 0xa, 0x3, 0x6, 0xf, 0x9, 0x0, 0x0, 0x6, 0xc, 0xa, 0xb, 0x1, 0x7, 0xd, 0xd, 0x8, 0xf, 0x9, 0x1, 0x4, 0x3, 0x5, 0xe, 0xb, 0x5, 0xc, 0x2, 0x7, 0x8, 0x2, 0x4, 0xe }, { 0x2, 0xe, 0xc, 0xb, 0x4, 0x2, 0x1, 0xc, 0x7, 0x4, 0xa, 0x7, 0xb, 0xd, 0x6, 0x1, 0x8, 0x5, 0x5, 0x0, 0x3, 0xf, 0xf, 0xa, 0xd, 0x3, 0x0, 0x9, 0xe, 0x8, 0x9, 0x6, 0x4, 0xb, 0x2, 0x8, 0x1, 0xc, 0xb, 0x7, 0xa, 0x1, 0xd, 0xe, 0x7, 0x2, 0x8, 0xd, 0xf, 0x6, 0x9, 0xf, 0xc, 0x0, 0x5, 0x9, 0x6, 0xa, 0x3, 0x4, 0x0, 0x5, 0xe, 0x3 }, { 0xc, 0xa, 0x1, 0xf, 0xa, 0x4, 0xf, 0x2, 0x9, 0x7, 0x2, 0xc, 0x6, 0x9, 0x8, 0x5, 0x0, 0x6, 0xd, 0x1, 0x3, 0xd, 0x4, 0xe, 0xe, 0x0, 0x7, 0xb, 0x5, 0x3, 0xb, 0x8, 0x9, 0x4, 0xe, 0x3, 0xf, 0x2, 0x5, 0xc, 0x2, 0x9, 0x8, 0x5, 0xc, 0xf, 0x3, 0xa, 0x7, 0xb, 0x0, 0xe, 0x4, 0x1, 0xa, 0x7, 0x1, 0x6, 0xd, 0x0, 0xb, 0x8, 0x6, 0xd }, { 0x4, 0xd, 0xb, 0x0, 0x2, 0xb, 0xe, 0x7, 0xf, 0x4, 0x0, 0x9, 0x8, 0x1, 0xd, 0xa, 0x3, 0xe, 0xc, 0x3, 0x9, 0x5, 0x7, 0xc, 0x5, 0x2, 0xa, 0xf, 0x6, 0x8, 0x1, 0x6, 0x1, 0x6, 0x4, 0xb, 0xb, 0xd, 0xd, 0x8, 0xc, 0x1, 0x3, 0x4, 0x7, 0xa, 0xe, 0x7, 0xa, 0x9, 0xf, 0x5, 0x6, 0x0, 0x8, 0xf, 0x0, 0xe, 0x5, 0x2, 0x9, 0x3, 0x2, 0xc }, { 0xd, 0x1, 0x2, 0xf, 0x8, 0xd, 0x4, 0x8, 0x6, 0xa, 0xf, 0x3, 0xb, 0x7, 0x1, 0x4, 0xa, 0xc, 0x9, 0x5, 0x3, 0x6, 0xe, 0xb, 0x5, 0x0, 0x0, 0xe, 0xc, 0x9, 0x7, 0x2, 0x7, 0x2, 0xb, 0x1, 0x4, 0xe, 0x1, 0x7, 0x9, 0x4, 0xc, 0xa, 0xe, 0x8, 0x2, 0xd, 0x0, 0xf, 0x6, 0xc, 0xa, 0x9, 0xd, 0x0, 0xf, 0x3, 0x3, 0x5, 0x5, 0x6, 0x8, 0xb } };
        // Initial inverse permute
        public static byte[] IP2 = { 40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31, 38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25 };
        // Left shift numbers
        public static byte[] LS = { 1, 2, 4, 6, 8, 10, 12, 14, 15, 17, 19, 21, 23, 25, 27, 0 };

        private byte[] keyValue1 = new byte[8];
        private byte[] keyValue2 = new byte[8];
        private byte[] ivValue = new byte[8];

        public void setKey(byte[] key)
        {
            int ctr = 0;
            for (int i = 0; i < 8; i++) keyValue1[i] = key[ctr++];
            for (int i = 0; i < 8; i++) keyValue2[i] = key[ctr++];
        }
        public void setIV(byte[] iv)
        {
            for (int i = 0; i < 8; i++) ivValue[i] = iv[i];
        }

        // Permute function
        public static ulong permute(ulong input, byte[] table, int tlength)
        {
            ulong result = 0;
            int i = 0;
            do
                result = ((result << 1) | ((input >> (tlength - table[i++])) & 1));
            while (i < table.Length);
            return result;
        }

        public static ulong permute(ulong input, byte[] temp)
        {
            return permute(input, temp, temp.Length);
        }

        // S-box permute
        public static int sBox(ulong input)
        {
            int result = 0;
            for (int i = 0; i < 8; i++)
                result = ((result << 4) + S[i, (int)((input >> (6 * (7 - i))) & 0x3F)]);
            return result;
        }

        // Key rotate shift
        public static ulong rotate(ulong key, int round)
        {
            int left = (int)(key >> 28);
            int right = (int)(key & 0xFFFFFFFL);
            left = (int)(((left << LS[round]) + (left >> (28 - LS[round]))) & 0xFFFFFFFL);
            right = (int)(((right << LS[round]) + (right >> (28 - LS[round]))) & 0xFFFFFFFL);
            return (((ulong)left << 28) + (ulong)right);
        }

        // Round function
        public static int roundFunc(int rin, ulong k, int round)
        {
            ulong rk = 0;
            rk = rotate(k, round);
            rk = permute(rk, PC2, 56);
            ulong longRin = (ulong)rin;
            return (int)permute((ulong)sBox(permute(longRin, E, 32) ^ rk), P);
        }

        public static ulong des(ulong key, ulong data, bool enc)
        {
            // Initial Permute
            data = permute(data, IP);

            // Do PC1 permute on the key
            key = permute(key, PC1, 64);

            // Divided data into left and right parts
            int L = (int)(data >> 32);
            int R = (int)(data);
            int temp = 0;
            // Encryption in 16 rounds
            if (enc)
                for (int i = 0; i < 16; i++)
                {
                    temp = L;
                    L = R;
                    R = roundFunc(R, key, i) ^ temp;
                }
            // Decryption in  16 rounds
            else
                for (int i = 15; i >= 0; i--)
                {
                    temp = L;
                    L = R;
                    R = roundFunc(R, key, i) ^ temp;
                }

            data = (((ulong)R & 0xFFFFFFFFL) << 32) + ((ulong)L & 0xFFFFFFFFL);
            data = permute(data, IP2);

            return data;
        }

        static public ulong convertByteArrayToLong(byte[] data)
        {
            ulong result = 0;
            for (int i = 0; i < data.Length; i++)
            {
                int shift = (7 - i) * 8;

                ulong buf = (ulong)data[i];
                buf = buf << shift;
                result = result | buf;
            }
            return result;
        }

        static public byte[] convertLongToByteArray(ulong data)
        {
            byte[] result = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                int shift = (7 - i) * 8;
                byte buf = (byte)(data >> shift);
                result[i] = buf;
            }
            return result;
        }


        public byte[] encrypt(byte[] plaintext)
        {
            int paddingCnt = 8 - (plaintext.Length % 8);
            int lenghth = plaintext.Length + paddingCnt;
            byte[] padding = new byte[lenghth];
            byte[] result = new byte[lenghth];
            int ctr = 0;

            // Padding
            for (int i = 0; i < lenghth; i++)
            {
                if (i < plaintext.Length)
                    padding[i] = plaintext[i];
                else
                    padding[i] = (byte)paddingCnt;
            }

            // Encrypt in CBC mode
            byte[] block = new byte[8];
            for (int i = 0; i < lenghth; i++)
            {
                int idx = i % 8;
                block[idx] = padding[i];
                if (idx == 7)
                {

                    for (int j = 0; j < 8; j++)
                        block[j] = (byte)(block[j] ^ ivValue[j]);
                    ulong buf = convertByteArrayToLong(block);
                    ulong key1 = convertByteArrayToLong(keyValue1);
                    ulong key2 = convertByteArrayToLong(keyValue2);
                    ulong firstRond = des(key1, buf, true);
                    ulong secondRound = des(key2, firstRond, false);
                    ulong blockResult = des(key1, secondRound, true);
                    byte[] bytesResult = convertLongToByteArray(blockResult);
                    for (int j = 0; j < 8; j++)
                    {
                        ivValue[j] = bytesResult[j];
                        result[ctr++] = bytesResult[j];
                    }
                }
            }
            return result;
        }

        public byte[] decrypt(byte[] ciphertext)
        {
            byte[] block = new byte[8];
            byte[] padding = new byte[ciphertext.Length];
            int ctr = 0;
            for (int i = 0; i < ciphertext.Length; i++)
            {
                int idx = i % 8;
                block[idx] = ciphertext[i];
                if (idx == 7)
                {
                    ulong buf = convertByteArrayToLong(block);
                    ulong key1 = convertByteArrayToLong(keyValue1);
                    ulong key2 = convertByteArrayToLong(keyValue2);
                    ulong firstRound = des(key1, buf, false);
                    ulong secondRound = des(key2, firstRound, true);
                    ulong blockResult = des(key1, secondRound, false);
                    byte[] bytesResult = convertLongToByteArray(blockResult);
                    for (int j = 0; j < 8; j++)
                    {
                        padding[ctr++] = (byte)(bytesResult[j] ^ ivValue[j]);
                        ivValue[j] = block[j];
                    }
                }
            }
            int paddingCnt = 0;
            for (int i = 0; i < padding.Length; i++)
            {
                if (padding[i] == padding.Length - i)
                {
                    byte buf = padding[i];
                    bool flag = true;
                    for (int j = i; j < padding.Length; j++)
                    {
                        if (padding[j] != buf)
                            flag = false;
                    }
                    if (flag)
                        paddingCnt = buf;
                }
            }
            byte[] result = new byte[padding.Length - paddingCnt];
            for (int i = 0; i < result.Length; i++) result[i] = padding[i];
            return result;
        }
    }
}
