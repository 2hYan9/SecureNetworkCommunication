using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureCommunication
{
    class SecureHashAlgorithm
    {
        public byte[] hash = new byte[32];

        private uint[] state = new uint[8];

        private uint[] K = { 0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
                             0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
                             0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
                             0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967, 
                             0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85, 
                             0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070, 
                             0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3, 
                             0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
                           };

        private byte[] PADDING = {
            0x80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        };

        private uint rotR(uint x, int shift)
        {
            return (((x) >> (shift)) | ((x) << (32 - (shift))));
        }

        private uint Ch(uint x, uint y, uint z)
        {
            return (x & y) ^ ((~x) & z);
        }

        private uint Maj(uint x, uint y, uint z)
        {
            return (x & y) ^ (x & z) ^ (y & z);
        }

        private uint Sigma1(uint x)
        {
            return rotR(x, 2) ^ rotR(x, 13) ^ rotR(x, 22);
        }

        private uint Sigma2(uint x)
        {
            return rotR(x, 6) ^ rotR(x, 11) ^ rotR(x, 25);
        }

        private uint Sigma3(uint x)
        {
            return rotR(x, 7) ^ rotR(x, 18) ^ (x >> 3);
        }

        private uint Sigma4(uint x)
        {
            return rotR(x, 17) ^ rotR(x, 19) ^ (x >> 10);
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

        private void SHAInit()
        {
            state[0] = 0x6a09e667;
            state[1] = 0xbb67ae85;
            state[2] = 0x3c6ef372;
            state[3] = 0xa54ff53a;
            state[4] = 0x510e527f;
            state[5] = 0x9b05688c;
            state[6] = 0x1f83d9ab;
            state[7] = 0x5be0cd19;
        }

        private void SHATransform(byte[] input)
        {
            uint a = state[0], b = state[1], c = state[2], d = state[3], e = state[4], f = state[5], g = state[6], h = state[7];
            uint[] W = new uint[64];
            convertByteArraytoInt(input, W);
            for (int i = 16; i < 64; i++)
                W[i] = Sigma4(W[i - 2]) + W[i - 7] + Sigma3(W[i - 15]) + W[i - 16];
            for (int i = 0; i < 64; i++)
            {
                uint t1 = h + Sigma2(e) + Ch(e, f, g) + K[i] + W[i];
                uint t2 = Sigma1(a) + Maj(a, b, c);
                h = g;
                g = f;
                f = e;
                e = d + t1;
                d = c;
                c = b;
                b = a;
                a = t1 + t2;
            }
            state[0] += a;
            state[1] += b;
            state[2] += c;
            state[3] += d;
            state[4] += e;
            state[5] += f;
            state[6] += g;
            state[7] += h;
        }

        public void SHACompute(byte[] input)
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
            SHAInit();
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
                    SHATransform(buffer);
            }
            for (int i = 0; i < 32; i++)
            {
                int idx = i / 4;
                int shift = (3 - i % 4) * 8;
                hash[i] = (byte)(state[idx] >> shift);
            }
        }
    }
}
