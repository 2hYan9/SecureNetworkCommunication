using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommunication
{
    class MessageDigestAlgorithm
    {
        public byte[] hash = new byte[16];

        private int S11 = 7;
        private int S12 = 12;
        private int S13 = 17;
        private int S14 = 22;
        private int S21 = 5;
        private int S22 = 9;
        private int S23 = 14;
        private int S24 = 20;
        private int S31 = 4;
        private int S32 = 11;
        private int S33 = 16;
        private int S34 = 23;
        private int S41 = 6;
        private int S42 = 10;
        private int S43 = 15;
        private int S44 = 21;

        private byte[] PADDING = {
            0x80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        };

        private uint[] state = new uint[4];

        private uint leftRotShift(uint x, int shift)
        {
            return (((x) << (shift)) | ((x) >> (32 - (shift))));
        }

        private uint fFunc(uint x, uint y, uint z)
        {
            return (((x) & (y)) | ((~x) & (z)));
        }

        private uint gFunc(uint x, uint y, uint z)
        {
            return (((x) & (z)) | ((y) & (~z)));
        }

        private uint hFunc(uint x, uint y, uint z)
        {
            return ((x) ^ (y) ^ (z));
        }

        private uint iFunc(uint x, uint y, uint z)
        {
            return ((y) ^ ((x) | (~z)));
        }

        private uint FF(uint a, uint b, uint c, uint d, uint x, int s, uint ac)
        {
            a += fFunc(b, c, d) + x + ac;
            a = leftRotShift(a, s);
            a += b;
            return a;
        }

        private uint GG(uint a, uint b, uint c, uint d, uint x, int s, uint ac)
        {
            a += gFunc(b, c, d) + x + ac;
            a = leftRotShift(a, s);
            a += b;
            return a;
        }

        private uint HH(uint a, uint b, uint c, uint d, uint x, int s, uint ac)
        {
            a += hFunc(b, c, d) + x + ac;
            a = leftRotShift(a, s);
            a += b;
            return a;
        }

        private uint II(uint a, uint b, uint c, uint d, uint x, int s, uint ac)
        {
            a += iFunc(b, c, d) + x + ac;
            a = leftRotShift(a, s);
            a += b;
            return a;
        }

        private void MD5Init()
        {
            state[0] = 0x67452301;
            state[1] = 0xefcdab89;
            state[2] = 0x98badcfe;
            state[3] = 0x10325476;
        }

        private void convertByteArraytoInt(byte[] inputBuffer, uint[] x)
        {
            for (int i = 0; i < 64; i++)
            {
                int idx = i / 4;
                int shift = i % 4;
                x[idx] += (uint)(inputBuffer[i] << ((3 - shift) * 8));
            }
        }

        private void MD5Transform(byte[] inputBuffer)
        {
            uint a = state[0], b = state[1], c = state[2], d = state[3];
            uint[] x = new uint[16];
            convertByteArraytoInt(inputBuffer, x);
            a = FF(a, b, c, d, x[0], S11, 0xd76aa478); /* 1  */
            d = FF(d, a, b, c, x[1], S12, 0xe8c7b756); /* 2  */
            c = FF(c, d, a, b, x[2], S13, 0x242070db); /* 3  */
            b = FF(b, c, d, a, x[3], S14, 0xc1bdceee); /* 4  */
            a = FF(a, b, c, d, x[4], S11, 0xf57c0faf); /* 5  */
            d = FF(d, a, b, c, x[5], S12, 0x4787c62a); /* 6  */
            c = FF(c, d, a, b, x[6], S13, 0xa8304613); /* 7  */
            b = FF(b, c, d, a, x[7], S14, 0xfd469501); /* 8  */
            a = FF(a, b, c, d, x[8], S11, 0x698098d8); /* 9  */
            d = FF(d, a, b, c, x[9], S12, 0x8b44f7af); /* 10 */
            c = FF(c, d, a, b, x[10], S13, 0xffff5bb1); /* 11 */
            b = FF(b, c, d, a, x[11], S14, 0x895cd7be); /* 12 */
            a = FF(a, b, c, d, x[12], S11, 0x6b901122); /* 13 */
            d = FF(d, a, b, c, x[13], S12, 0xfd987193); /* 14 */
            c = FF(c, d, a, b, x[14], S13, 0xa679438e); /* 15 */
            b = FF(b, c, d, a, x[15], S14, 0x49b40821); /* 16 */
            /* Round 2 */
            a = GG(a, b, c, d, x[1], S21, 0xf61e2562); /* 17 */
            d = GG(d, a, b, c, x[6], S22, 0xc040b340); /* 18 */
            c = GG(c, d, a, b, x[11], S23, 0x265e5a51); /* 19 */
            b = GG(b, c, d, a, x[0], S24, 0xe9b6c7aa); /* 20 */
            a = GG(a, b, c, d, x[5], S21, 0xd62f105d); /* 21 */
            d = GG(d, a, b, c, x[10], S22, 0x2441453);  /* 22 */
            c = GG(c, d, a, b, x[15], S23, 0xd8a1e681); /* 23 */
            b = GG(b, c, d, a, x[4], S24, 0xe7d3fbc8); /* 24 */
            a = GG(a, b, c, d, x[9], S21, 0x21e1cde6); /* 25 */
            d = GG(d, a, b, c, x[14], S22, 0xc33707d6); /* 26 */
            c = GG(c, d, a, b, x[3], S23, 0xf4d50d87); /* 27 */
            b = GG(b, c, d, a, x[8], S24, 0x455a14ed); /* 28 */
            a = GG(a, b, c, d, x[13], S21, 0xa9e3e905); /* 29 */
            d = GG(d, a, b, c, x[2], S22, 0xfcefa3f8); /* 30 */
            c = GG(c, d, a, b, x[7], S23, 0x676f02d9); /* 31 */
            b = GG(b, c, d, a, x[12], S24, 0x8d2a4c8a); /* 32 */
            /* Round 3 */
            a = HH(a, b, c, d, x[5], S31, 0xfffa3942); /* 33 */
            d = HH(d, a, b, c, x[8], S32, 0x8771f681); /* 34 */
            c = HH(c, d, a, b, x[11], S33, 0x6d9d6122); /* 35 */
            b = HH(b, c, d, a, x[14], S34, 0xfde5380c); /* 36 */
            a = HH(a, b, c, d, x[1], S31, 0xa4beea44); /* 37 */
            d = HH(d, a, b, c, x[4], S32, 0x4bdecfa9); /* 38 */
            c = HH(c, d, a, b, x[7], S33, 0xf6bb4b60); /* 39 */
            b = HH(b, c, d, a, x[10], S34, 0xbebfbc70); /* 40 */
            a = HH(a, b, c, d, x[13], S31, 0x289b7ec6); /* 41 */
            d = HH(d, a, b, c, x[0], S32, 0xeaa127fa); /* 42 */
            c = HH(c, d, a, b, x[3], S33, 0xd4ef3085); /* 43 */
            b = HH(b, c, d, a, x[6], S34, 0x4881d05);  /* 44 */
            a = HH(a, b, c, d, x[9], S31, 0xd9d4d039); /* 45 */
            d = HH(d, a, b, c, x[12], S32, 0xe6db99e5); /* 46 */
            c = HH(c, d, a, b, x[15], S33, 0x1fa27cf8); /* 47 */
            b = HH(b, c, d, a, x[2], S34, 0xc4ac5665); /* 48 */
            /* Round 4 */
            a = II(a, b, c, d, x[0], S41, 0xf4292244); /* 49 */
            d = II(d, a, b, c, x[7], S42, 0x432aff97); /* 50 */
            c = II(c, d, a, b, x[14], S43, 0xab9423a7); /* 51 */
            b = II(b, c, d, a, x[5], S44, 0xfc93a039); /* 52 */
            a = II(a, b, c, d, x[12], S41, 0x655b59c3); /* 53 */
            d = II(d, a, b, c, x[3], S42, 0x8f0ccc92); /* 54 */
            c = II(c, d, a, b, x[10], S43, 0xffeff47d); /* 55 */
            b = II(b, c, d, a, x[1], S44, 0x85845dd1); /* 56 */
            a = II(a, b, c, d, x[8], S41, 0x6fa87e4f); /* 57 */
            d = II(d, a, b, c, x[15], S42, 0xfe2ce6e0); /* 58 */
            c = II(c, d, a, b, x[6], S43, 0xa3014314); /* 59 */
            b = II(b, c, d, a, x[13], S44, 0x4e0811a1); /* 60 */
            a = II(a, b, c, d, x[4], S41, 0xf7537e82); /* 61 */
            d = II(d, a, b, c, x[11], S42, 0xbd3af235); /* 62 */
            c = II(c, d, a, b, x[2], S43, 0x2ad7d2bb); /* 63 */
            b = II(b, c, d, a, x[9], S44, 0xeb86d391); /* 64 */
            state[0] += a;
            state[1] += b;
            state[2] += c;
            state[3] += d;
        }

        public void MD5Compute(byte[] input)
        {
            byte[] buffer = new byte[64];
            int paddingCtr = 0;
            byte[] lengthByte = new byte[8];
            ulong len = (ulong)input.Length;
            lengthByte[0] = (byte)(len >> 56);
            lengthByte[1] = (byte)(len >> 48);
            lengthByte[2] = (byte)(len >> 40);
            lengthByte[3] = (byte)(len >> 32);
            lengthByte[4] = (byte)(len >> 24);
            lengthByte[5] = (byte)(len >> 16);
            lengthByte[6] = (byte)(len >> 8);
            lengthByte[7] = (byte)len;
            int lenCtr = 0;
            MD5Init();
            int complement = 56 - (input.Length % 64);
            complement = complement > 0 ? complement : 64 + complement;

            int totalLength = input.Length + complement;
            for (int i = 0; i < totalLength + 8; i++)
            {
                int idx = i % 64;
                if (i < input.Length)
                    buffer[idx] = input[i];
                else if (i < totalLength)
                    buffer[idx] = PADDING[paddingCtr++];
                else
                    buffer[idx] = lengthByte[lenCtr++];
                if (idx == 63)
                    MD5Transform(buffer);
            }
            for (int i = 0; i < 16; i++)
            {
                int idx = 3 - i / 4;
                int shift = (3 - i % 4) * 8;
                hash[i] = (byte)(state[idx] >> shift);
            }
        }
    }
}
