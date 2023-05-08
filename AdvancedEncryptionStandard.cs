using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommunication
{
    class AdvancedEncryptionStandard
    {
        private byte[] keyValue = new Byte[16];
        private byte[] ivValue = new Byte[16];

        public static byte[,] Sbox = {
		    {(byte)0x63, (byte)0x7C, (byte)0x77, (byte)0x7B, (byte)0xF2, (byte)0x6B, (byte)0x6F, (byte)0xC5, (byte)0x30, (byte)0x01, (byte)0x67, (byte)0x2B, (byte)0xFE, (byte)0xD7, (byte)0xAB, (byte)0x76}, 
			{(byte)0xCA, (byte)0x82, (byte)0xC9, (byte)0x7D, (byte)0xFA, (byte)0x59, (byte)0x47, (byte)0xF0, (byte)0xAD, (byte)0xD4, (byte)0xA2, (byte)0xAF, (byte)0x9C, (byte)0xA4, (byte)0x72, (byte)0xC0}, 
			{(byte)0xB7, (byte)0xFD, (byte)0x93, (byte)0x26, (byte)0x36, (byte)0x3F, (byte)0xF7, (byte)0xCC, (byte)0x34, (byte)0xA5, (byte)0xE5, (byte)0xF1, (byte)0x71, (byte)0xD8, (byte)0x31, (byte)0x15}, 
			{(byte)0x04, (byte)0xC7, (byte)0x23, (byte)0xC3, (byte)0x18, (byte)0x96, (byte)0x05, (byte)0x9A, (byte)0x07, (byte)0x12, (byte)0x80, (byte)0xE2, (byte)0xEB, (byte)0x27, (byte)0xB2, (byte)0x75}, 
			{(byte)0x09, (byte)0x83, (byte)0x2C, (byte)0x1A, (byte)0x1B, (byte)0x6E, (byte)0x5A, (byte)0xA0, (byte)0x52, (byte)0x3B, (byte)0xD6, (byte)0xB3, (byte)0x29, (byte)0xE3, (byte)0x2F, (byte)0x84}, 
			{(byte)0x53, (byte)0xD1, (byte)0x00, (byte)0xED, (byte)0x20, (byte)0xFC, (byte)0xB1, (byte)0x5B, (byte)0x6A, (byte)0xCB, (byte)0xBE, (byte)0x39, (byte)0x4A, (byte)0x4C, (byte)0x58, (byte)0xCF}, 
			{(byte)0xD0, (byte)0xEF, (byte)0xAA, (byte)0xFB, (byte)0x43, (byte)0x4D, (byte)0x33, (byte)0x85, (byte)0x45, (byte)0xF9, (byte)0x02, (byte)0x7F, (byte)0x50, (byte)0x3C, (byte)0x9F, (byte)0xA8}, 
			{(byte)0x51, (byte)0xA3, (byte)0x40, (byte)0x8F, (byte)0x92, (byte)0x9D, (byte)0x38, (byte)0xF5, (byte)0xBC, (byte)0xB6, (byte)0xDA, (byte)0x21, (byte)0x10, (byte)0xFF, (byte)0xF3, (byte)0xD2}, 
			{(byte)0xCD, (byte)0x0C, (byte)0x13, (byte)0xEC, (byte)0x5F, (byte)0x97, (byte)0x44, (byte)0x17, (byte)0xC4, (byte)0xA7, (byte)0x7E, (byte)0x3D, (byte)0x64, (byte)0x5D, (byte)0x19, (byte)0x73}, 
			{(byte)0x60, (byte)0x81, (byte)0x4F, (byte)0xDC, (byte)0x22, (byte)0x2A, (byte)0x90, (byte)0x88, (byte)0x46, (byte)0xEE, (byte)0xB8, (byte)0x14, (byte)0xDE, (byte)0x5E, (byte)0x0B, (byte)0xDB}, 
			{(byte)0xE0, (byte)0x32, (byte)0x3A, (byte)0x0A, (byte)0x49, (byte)0x06, (byte)0x24, (byte)0x5C, (byte)0xC2, (byte)0xD3, (byte)0xAC, (byte)0x62, (byte)0x91, (byte)0x95, (byte)0xE4, (byte)0x79}, 
			{(byte)0xE7, (byte)0xC8, (byte)0x37, (byte)0x6D, (byte)0x8D, (byte)0xD5, (byte)0x4E, (byte)0xA9, (byte)0x6C, (byte)0x56, (byte)0xF4, (byte)0xEA, (byte)0x65, (byte)0x7A, (byte)0xAE, (byte)0x08}, 
			{(byte)0xBA, (byte)0x78, (byte)0x25, (byte)0x2E, (byte)0x1C, (byte)0xA6, (byte)0xB4, (byte)0xC6, (byte)0xE8, (byte)0xDD, (byte)0x74, (byte)0x1F, (byte)0x4B, (byte)0xBD, (byte)0x8B, (byte)0x8A}, 
			{(byte)0x70, (byte)0x3E, (byte)0xB5, (byte)0x66, (byte)0x48, (byte)0x03, (byte)0xF6, (byte)0x0E, (byte)0x61, (byte)0x35, (byte)0x57, (byte)0xB9, (byte)0x86, (byte)0xC1, (byte)0x1D, (byte)0x9E}, 
			{(byte)0xE1, (byte)0xF8, (byte)0x98, (byte)0x11, (byte)0x69, (byte)0xD9, (byte)0x8E, (byte)0x94, (byte)0x9B, (byte)0x1E, (byte)0x87, (byte)0xE9, (byte)0xCE, (byte)0x55, (byte)0x28, (byte)0xDF}, 
			{(byte)0x8C, (byte)0xA1, (byte)0x89, (byte)0x0D, (byte)0xBF, (byte)0xE6, (byte)0x42, (byte)0x68, (byte)0x41, (byte)0x99, (byte)0x2D, (byte)0x0F, (byte)0xB0, (byte)0x54, (byte)0xBB, (byte)0x16}
        };

        public static byte[,] invSbox = {
            {(byte)0x52, (byte)0x09, (byte)0x6A, (byte)0xD5, (byte)0x30, (byte)0x36, (byte)0xA5, (byte)0x38, (byte)0xBF, (byte)0x40, (byte)0xA3, (byte)0x9E, (byte)0x81, (byte)0xF3, (byte)0xD7, (byte)0xFB}, 
			{(byte)0x7C, (byte)0xE3, (byte)0x39, (byte)0x82, (byte)0x9B, (byte)0x2F, (byte)0xFF, (byte)0x87, (byte)0x34, (byte)0x8E, (byte)0x43, (byte)0x44, (byte)0xC4, (byte)0xDE, (byte)0xE9, (byte)0xCB}, 
			{(byte)0x54, (byte)0x7B, (byte)0x94, (byte)0x32, (byte)0xA6, (byte)0xC2, (byte)0x23, (byte)0x3D, (byte)0xEE, (byte)0x4C, (byte)0x95, (byte)0x0B, (byte)0x42, (byte)0xFA, (byte)0xC3, (byte)0x4E}, 
			{(byte)0x08, (byte)0x2E, (byte)0xA1, (byte)0x66, (byte)0x28, (byte)0xD9, (byte)0x24, (byte)0xB2, (byte)0x76, (byte)0x5B, (byte)0xA2, (byte)0x49, (byte)0x6D, (byte)0x8B, (byte)0xD1, (byte)0x25}, 
			{(byte)0x72, (byte)0xF8, (byte)0xF6, (byte)0x64, (byte)0x86, (byte)0x68, (byte)0x98, (byte)0x16, (byte)0xD4, (byte)0xA4, (byte)0x5C, (byte)0xCC, (byte)0x5D, (byte)0x65, (byte)0xB6, (byte)0x92}, 
			{(byte)0x6C, (byte)0x70, (byte)0x48, (byte)0x50, (byte)0xFD, (byte)0xED, (byte)0xB9, (byte)0xDA, (byte)0x5E, (byte)0x15, (byte)0x46, (byte)0x57, (byte)0xA7, (byte)0x8D, (byte)0x9D, (byte)0x84}, 
			{(byte)0x90, (byte)0xD8, (byte)0xAB, (byte)0x00, (byte)0x8C, (byte)0xBC, (byte)0xD3, (byte)0x0A, (byte)0xF7, (byte)0xE4, (byte)0x58, (byte)0x05, (byte)0xB8, (byte)0xB3, (byte)0x45, (byte)0x06}, 
			{(byte)0xD0, (byte)0x2C, (byte)0x1E, (byte)0x8F, (byte)0xCA, (byte)0x3F, (byte)0x0F, (byte)0x02, (byte)0xC1, (byte)0xAF, (byte)0xBD, (byte)0x03, (byte)0x01, (byte)0x13, (byte)0x8A, (byte)0x6B}, 
			{(byte)0x3A, (byte)0x91, (byte)0x11, (byte)0x41, (byte)0x4F, (byte)0x67, (byte)0xDC, (byte)0xEA, (byte)0x97, (byte)0xF2, (byte)0xCF, (byte)0xCE, (byte)0xF0, (byte)0xB4, (byte)0xE6, (byte)0x73}, 
			{(byte)0x96, (byte)0xAC, (byte)0x74, (byte)0x22, (byte)0xE7, (byte)0xAD, (byte)0x35, (byte)0x85, (byte)0xE2, (byte)0xF9, (byte)0x37, (byte)0xE8, (byte)0x1C, (byte)0x75, (byte)0xDF, (byte)0x6E}, 
			{(byte)0x47, (byte)0xF1, (byte)0x1A, (byte)0x71, (byte)0x1D, (byte)0x29, (byte)0xC5, (byte)0x89, (byte)0x6F, (byte)0xB7, (byte)0x62, (byte)0x0E, (byte)0xAA, (byte)0x18, (byte)0xBE, (byte)0x1B}, 
			{(byte)0xFC, (byte)0x56, (byte)0x3E, (byte)0x4B, (byte)0xC6, (byte)0xD2, (byte)0x79, (byte)0x20, (byte)0x9A, (byte)0xDB, (byte)0xC0, (byte)0xFE, (byte)0x78, (byte)0xCD, (byte)0x5A, (byte)0xF4}, 
			{(byte)0x1F, (byte)0xDD, (byte)0xA8, (byte)0x33, (byte)0x88, (byte)0x07, (byte)0xC7, (byte)0x31, (byte)0xB1, (byte)0x12, (byte)0x10, (byte)0x59, (byte)0x27, (byte)0x80, (byte)0xEC, (byte)0x5F}, 
			{(byte)0x60, (byte)0x51, (byte)0x7F, (byte)0xA9, (byte)0x19, (byte)0xB5, (byte)0x4A, (byte)0x0D, (byte)0x2D, (byte)0xE5, (byte)0x7A, (byte)0x9F, (byte)0x93, (byte)0xC9, (byte)0x9C, (byte)0xEF}, 
			{(byte)0xA0, (byte)0xE0, (byte)0x3B, (byte)0x4D, (byte)0xAE, (byte)0x2A, (byte)0xF5, (byte)0xB0, (byte)0xC8, (byte)0xEB, (byte)0xBB, (byte)0x3C, (byte)0x83, (byte)0x53, (byte)0x99, (byte)0x61}, 
			{(byte)0x17, (byte)0x2B, (byte)0x04, (byte)0x7E, (byte)0xBA, (byte)0x77, (byte)0xD6, (byte)0x26, (byte)0xE1, (byte)0x69, (byte)0x14, (byte)0x63, (byte)0x55, (byte)0x21, (byte)0x0C, (byte)0x7D} 
        };

        public static byte[,] cx = {
	        {0x02, 0x03, 0x01, 0x01}, 
	        {0x01, 0x02, 0x03, 0x01}, 
	        {0x01, 0x01, 0x02, 0x03}, 
	        {0x03, 0x01, 0x01, 0x02}
        };

        public static byte[,] invcx = {
			{0x0e, 0x0b, 0x0d, 0x09}, 
	        {0x09, 0x0e, 0x0b, 0x0d}, 
	        {0x0d, 0x09, 0x0e, 0x0b}, 
	        {0x0b, 0x0d, 0x09, 0x0e}
        };

        public void setKey(byte[] key)
        {
            for (int i = 0; i < 16; i++) keyValue[i] = key[i];
        }

        public void setIV(byte[] iv)
        {
            for (int i = 0; i < 16; i++) ivValue[i] = iv[i];
        }

        public static byte[] Rcon = { 0x00, 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36, 0x6c, 0xd8, 0xab, 0x4d, 0x9a, 0x2f, 0x5e, 0xbc, 0x63, 0xc6, 0x97, 0x35, 0x6a, 0xd4, 0xb3, 0x7d, 0xfa, 0xef, 0xc5, 0x91 };

        public static void AddRoundKey(int[,] a, int[,] rk, int round)
        {
            int i, j;
            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    a[i, j] ^= rk[i, j + round * 4];
                }
            }
        }

        public static void SubBytes(int[,] a)
        {
            int i, j;
            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    a[i, j] = (Sbox[a[i, j] >> 4, a[i, j] & 0xF]) & 0xff;
                }
            }
        }

        public static void InvSubBytes(int[,] a)
        {
            int i, j;
            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    a[i, j] = (invSbox[a[i, j] >> 4, a[i, j] & 0xF]) & 0xff;
                }
            }
        }

        public static void KeySubBytes(int[] a)
        {
            int i;
            for (i = 0; i < 4; i++)
            {
                a[i] = (Sbox[a[i] >> 4, a[i] & 0xF]) & 0xff;
            }
        }

        public static void ShiftRows(int[,] a)
        {
            int i, j;
            int[] tmp = new int[4];
            for (int k = 0; k < 4; k++)
            {
                tmp[k] = 0;
            }
            for (i = 1; i < 4; i++)
            {					//shift from the second row
                for (j = 0; j < 4; j++)
                {
                    tmp[(j - i + 4) % 4] = a[i, j];
                }
                for (j = 0; j < 4; j++)
                {
                    a[i, j] = tmp[j];
                }
            }
        }

        public static void InvShiftRows(int[,] a)
        {
            int i, j;
            int[] tmp = new int[4];
            for (int k = 0; k < 4; k++)
            {
                tmp[k] = 0;
            }
            for (i = 1; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    tmp[(j + i + 4) % 4] = a[i, j];
                }
                for (j = 0; j < 4; j++)
                {
                    a[i, j] = tmp[j];
                }
            }
        }

        public static int ifover(int temp)
        {		//multiply 0x2
            if (1 > (temp >> 7))
            {
                temp = temp << 1;
            }
            else
            {									// if temp is bigger than 0x80
                temp = temp << 1;
                temp = (temp ^ 0x1b) & 0xff;
            }
            return temp;
        }

        public static int ifover(int temp, int t)
        {
            int k = 0;

            if (0x1 == t)
            {						//temp * 0x01 = temp
                k = temp;
            }

            if (0x2 == t)
            {
                k = ifover(temp);					//temp * 0x02
            }

            if (0x3 == t)
            {						//temp * 0x03 = temp * 0x02 + temp
                k = ifover(temp) ^ temp;
            }

            if (0xe == t)
            {						//temp * 0x0e = temp * 0x02 * 0x02 * 0x02 + temp * 0x02 * 0x02 + temp * 0x02
                k = ifover(ifover(ifover(temp))) ^ ifover(ifover(temp)) ^ ifover(temp);
            }

            if (0xb == t)
            {						//temp * 0x0b = temp * 0x02 * 0x02 * 0x02 + temp * 0x02 +temp
                k = ifover(ifover(ifover(temp))) ^ ifover(temp) ^ temp;
            }
            if (0xd == t)
            {						//temp * 0x0d = temp * 0x02 * 0x02 * 0x02 + temp * 0x02 * 0x02 + temp
                k = ifover(ifover(ifover(temp))) ^ ifover(ifover(temp)) ^ temp;
            }
            if (0x9 == t)
            {						//temp * 0x09 = temp * 0x02 * 0x02 * 0x02 + temp
                k = ifover(ifover(ifover(temp))) ^ temp;
            }
            return k;
        }

        public static void InvMixColumns(int[,] a)
        {
            int[,] b = new int[4, 4];
            for (int m = 0; m < 4; m++)
            {
                for (int n = 0; n < 4; n++)
                {
                    b[m, n] = 0;
                }
            }
            int i, j;
            for (j = 0; j < 4; j++)
            {
                for (i = 0; i < 4; i++)
                {
                    if (0 == i)
                    {
                        b[i, j] = ifover(a[0, j], 0xe) ^ ifover(a[1, j], 0xb) ^ ifover(a[2, j], 0xd) ^ ifover(a[3, j], 0x9);
                    }
                    if (1 == i)
                    {
                        b[i, j] = ifover(a[0, j], 0x9) ^ ifover(a[1, j], 0xe) ^ ifover(a[2, j], 0xb) ^ ifover(a[3, j], 0xd);
                    }
                    if (2 == i)
                    {
                        b[i, j] = ifover(a[0, j], 0xd) ^ ifover(a[1, j], 0x9) ^ ifover(a[2, j], 0xe) ^ ifover(a[3, j], 0xb);
                    }
                    if (3 == i)
                    {
                        b[i, j] = ifover(a[0, j], 0xb) ^ ifover(a[1, j], 0xd) ^ ifover(a[2, j], 0x9) ^ ifover(a[3, j], 0xe);
                    }
                }
            }
            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                    a[i, j] = b[i, j];
            }
        }

        public static void MixColumns(int[,] a)
        {
            int[,] b = new int[4, 4];
            for (int m = 0; m < 4; m++)
            {
                for (int n = 0; n < 4; n++)
                {
                    b[m, n] = 0;
                }
            }
            int i, j;
            for (j = 0; j < 4; j++)
            {
                for (i = 0; i < 4; i++)
                {
                    if (0 == i)
                    {
                        b[i, j] = ifover(a[0, j], 2) ^ ifover(a[1, j], 3) ^ ifover(a[2, j], 1) ^ ifover(a[3, j], 1);
                    }
                    if (1 == i)
                    {
                        b[i, j] = ifover(a[0, j], 1) ^ ifover(a[1, j], 2) ^ ifover(a[2, j], 3) ^ ifover(a[3, j], 1);
                    }
                    if (2 == i)
                    {
                        b[i, j] = ifover(a[0, j], 1) ^ ifover(a[1, j], 1) ^ ifover(a[2, j], 2) ^ ifover(a[3, j], 3);
                    }
                    if (3 == i)
                    {
                        b[i, j] = ifover(a[0, j], 3) ^ ifover(a[1, j], 1) ^ ifover(a[2, j], 1) ^ ifover(a[3, j], 2);
                    }
                }
            }
            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                    a[i, j] = b[i, j];
            }
        }

        public static void RotWord(int[] a)
        {
            int j;
            int[] tmp = new int[4];
            for (int k = 0; k < 4; k++)
            {
                tmp[k] = 0;
            }
            for (j = 0; j < 4; j++)
            {
                tmp[(j + 3) % 4] = a[j];
            }
            for (j = 0; j < 4; j++)
            {
                a[j] = tmp[j];
            }
        }
        public static String plainbit(String plaintext)
        {
            if (plaintext.Length < 32)
            {
                for (int i = 0; i < 32 - plaintext.Length; i++)
                {
                    plaintext = "0" + plaintext;
                }
            }
            if (plaintext.Length > 32)
            {
                for (int i = 0; i < 32 - plaintext.Length; i++)
                {
                    plaintext = plaintext.Substring(0, 32);
                }
            }
            return plaintext;
        }

        public static String Keybit(String Key)
        {
            if (Key.Length < 32)
            {
                for (int i = 0; i < 32 - Key.Length; i++)
                {
                    Key = "0" + Key;
                }
            }
            if (Key.Length > 32)
            {
                for (int i = 0; i < 32 - Key.Length; i++)
                {
                    Key = Key.Substring(0, 32);
                }
            }
            return Key;
        }

        // Round key generation
        public static int[,] KeyExpansion(String Key, int Nk, int Nr, int ed)
        {
            Key = Keybit(Key);
            int Clength = 0;
            String tempKey = null;
            int[,] RoundKey = new int[4, 4 * (Nr + 1)];
            for (int m = 0; m < 4; m++)
            {
                for (int n = 0; n < 4 * (Nr + 1); n++)
                {
                    RoundKey[m, n] = 0;
                }
            }
            for (Clength = 0; Clength < Nk; Clength++)
            {
                for (int m = 0, n = 0; n < 8; n = n + 2, m++)
                {
                    tempKey = Key.Substring(n + 8 * Clength, 2);
                    try
                    {
                        RoundKey[m, Clength] = int.Parse(tempKey, System.Globalization.NumberStyles.HexNumber);
                    }
                    catch (OverflowException ex)
                    {
                        Console.WriteLine(tempKey);
                        Console.WriteLine(ex.ToString());
                    }

                }
            }
            int[] temp = new int[4];

            while (Clength < 4 * (Nr + 1))
            {
                for (int i = 0; i < 4; i++)
                {
                    temp[i] = RoundKey[i, Clength - 1];
                }
                if (0 == (Clength % Nk))
                {
                    RotWord(temp);
                    KeySubBytes(temp);

                    temp[0] = (temp[0] ^ (int)Rcon[Clength / Nk]) & 0xff;
                }
                else if ((6 < Nk) && (4 == Clength % Nk))
                {
                    KeySubBytes(temp);
                }
                for (int i = 0; i < 4; i++)
                {
                    RoundKey[i, Clength] = RoundKey[i, Clength - Nk] ^ temp[i];
                }
                Clength = Clength + 1;
            }
            int tempkey = 0;
            if (1 == ed)
            {
                for (int n = 0; n < (Nr + 1) / 2; n++)
                {
                    for (int m = 0; m < 4; m++)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            tempkey = RoundKey[m, 4 * (Nr - n) + i];
                            RoundKey[m, 4 * (Nr - n) + i] = RoundKey[m, 4 * n + i];
                            RoundKey[m, 4 * n + i] = tempkey;
                        }
                    }
                }
            }
            String tempstring = null;
            for (int i = 0; i < 4; i++)
            {
                //System.out.print("RoundKey["+i+"]");
                for (int j = 0; j < Clength; j++)
                {
                    tempstring = Convert.ToString(RoundKey[i, j], 16);
                    if (1 == tempstring.Length)
                    {
                        tempstring = "0" + tempstring;
                    }
                    //System.out.print(" "+tempstring);
                }
                //System.out.println();
            }
            return RoundKey;
        }

        //Encryption
        public static String AESencrypt(String plaintext, String Key)
        {
            String tempplain = null;
            plaintext = plainbit(plaintext);
            int Nk = 4;
            int Nr = 10;
            int[,] RoundKey = new int[4, Nk * (Nr + 1)];
            for (int m = 0; m < 4; m++)
            {
                for (int n = 0; n < Nk * (Nr + 1); n++)
                {
                    RoundKey[m, n] = 0;
                }
            }
            RoundKey = KeyExpansion(Key, Nk, Nr, 0);	// generate the round key
            int[,] data = new int[4, 4];			// the state martrix
            for (int m = 0; m < 4; m++)
            {
                for (int n = 0; n < 4; n++)
                {
                    data[m, n] = 0;
                }
            }
            for (int Clength = 0; Clength < 4; Clength++)
            {
                for (int m = 0, n = 0; n < 8; n = n + 2, m++)
                {
                    tempplain = plaintext.Substring(n + 8 * Clength, 2);
                    data[m, Clength] = Int32.Parse(tempplain, System.Globalization.NumberStyles.HexNumber);
                }
            }
            int round = 0;
            AddRoundKey(data, RoundKey, round);
            for (round = 1; round < Nr; round++)
            {
                SubBytes(data);
                ShiftRows(data);
                MixColumns(data);
                AddRoundKey(data, RoundKey, round);
            }
            SubBytes(data);
            ShiftRows(data);
            AddRoundKey(data, RoundKey, round);
            String tempcipher = "";
            String ciphertext = "";
            for (int m = 0; m < 4; m++)
            {
                for (int n = 0; n < 4; n++)
                {
                    tempcipher = Convert.ToString(data[n, m], 16);
                    if (1 == tempcipher.Length)
                    {
                        tempcipher = "0" + tempcipher;
                    }
                    ciphertext = ciphertext + tempcipher;
                }
            }
            return ciphertext;
        }

        //Decryption
        public static String AESdecrypt(String plaintext, String Key)
        {
            String tempplain = null;
            plaintext = plainbit(plaintext);
            int Nk = 4;
            int Nr = 10;
            int[,] RoundKey = new int[4, Nk * (Nr + 1)];
            for (int m = 0; m < 4; m++)
            {
                for (int n = 0; n < Nk * (Nr + 1); n++)
                {
                    RoundKey[m, n] = 0;
                }
            }
            RoundKey = KeyExpansion(Key, Nk, Nr, 1);
            int[,] data = new int[4, 4];
            for (int m = 0; m < 4; m++)
            {
                for (int n = 0; n < 4; n++)
                {
                    data[m, n] = 0;
                }
            }
            for (int Clength = 0; Clength < 4; Clength++)
            {
                for (int m = 0, n = 0; n < 8; n = n + 2, m++)
                {
                    tempplain = plaintext.Substring(n + 8 * Clength, 2);
                    data[m, Clength] = Int32.Parse(tempplain, System.Globalization.NumberStyles.HexNumber);
                }
            }
            int round = 0;
            AddRoundKey(data, RoundKey, round);
            for (round = 1; round < Nr; round++)
            {
                InvShiftRows(data);
                InvSubBytes(data);
                AddRoundKey(data, RoundKey, round);

                InvMixColumns(data);
            }
            InvShiftRows(data);
            InvSubBytes(data);
            AddRoundKey(data, RoundKey, round);
            String tempcipher = "";
            String ciphertext = "";
            for (int m = 0; m < 4; m++)
            {
                for (int n = 0; n < 4; n++)
                {
                    tempcipher = Convert.ToString(data[n, m], 16);
                    if (1 == tempcipher.Length)
                    {
                        tempcipher = "0" + tempcipher;
                    }
                    ciphertext = ciphertext + tempcipher;
                }
            }
            return ciphertext;
        }

        public static string convertByteArraytoString(byte[] data)
        {
            string result = "";
            for (int i = 0; i < data.Length; i++)
            {
                string buf = Convert.ToString(data[i], 16);
                if (buf.Length == 1)
                    buf = "0" + buf;
                result += buf;
            }
            return result;
        }

        public static byte[] convertStringtoByteArray(string data)
        {
            byte[] result = new byte[16];
            for (int i = 0; i < data.Length; i += 2)
            {
                string buf = "";
                buf += data[i];
                buf += data[i + 1];
                result[i / 2] = Convert.ToByte(buf, 16);
            }
            return result;
        }

        public byte[] encrypt(byte[] plaintext)
        {
            // Padding
            byte paddingCnt = (byte)(16 - plaintext.Length % 16);
            byte[] padding = new byte[plaintext.Length + paddingCnt];
            for (int i = 0; i < padding.Length; i++)
            {
                if (i < plaintext.Length)
                    padding[i] = plaintext[i];
                else
                    padding[i] = paddingCnt;
            }

            // Encrypt in CBC mode
            byte[] result = new byte[plaintext.Length + paddingCnt];
            int ctr = 0;
            byte[] block = new byte[16];
            for (int i = 0; i < padding.Length; i++)
            {
                int idx = i % 16;
                block[idx] = padding[i];
                if (idx == 15)
                {

                    for (int j = 0; j < 16; j++)
                        block[j] = (byte)(block[j] ^ ivValue[j]);

                    string key = convertByteArraytoString(keyValue);
                    string data = convertByteArraytoString(block);
                    string enc = AESencrypt(data, key);
                    // Console.WriteLine(enc + " "  + enc.Length);
                    byte[] blockResult = convertStringtoByteArray(enc);


                    for (int j = 0; j < 16; j++)
                    {
                        ivValue[j] = blockResult[j];
                        result[ctr++] = blockResult[j];
                    }
                }
            }
            return result;
        }

        public byte[] decrypt(byte[] ciphertext)
        {
            byte[] padding = new byte[ciphertext.Length];
            int ctr = 0;
            byte[] block = new byte[16];
            for (int i = 0; i < ciphertext.Length; i++)
            {
                int idx = i % 16;
                block[idx] = ciphertext[i];
                if (idx == 15)
                {
                    string key = convertByteArraytoString(keyValue);
                    string data = convertByteArraytoString(block);
                    string dec = AESdecrypt(data, key);
                    // Console.WriteLine(dec);

                    byte[] blockResult = convertStringtoByteArray(dec);

                    for (int j = 0; j < 16; j++)
                    {
                        // padding[ctr++] = blockResult[j];
                        padding[ctr++] = (byte)(blockResult[j] ^ ivValue[j]);
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
