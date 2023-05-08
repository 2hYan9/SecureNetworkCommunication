using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Threading;
using System.IO;
using System.Security.Cryptography;

namespace SecureCommunication
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread th = new Thread(toConfig);
            th.IsBackground = true;
            th.Start();
            th.Join();
        }

        private void toConfig()
        {
            Console.WriteLine("Configing...");
            string fileName = "../../publicKey.txt";
            if (!System.IO.File.Exists(fileName))
            {
                Console.WriteLine("Generating a pair of public keys...");
                using (System.IO.FileStream fs = System.IO.File.Create(fileName))
                {
                    Random rand = new Random();
                    BigInteger P = generateBigInteger();
                    for (; !isPrime(P); P = generateBigInteger()) ;
                    BigInteger Q = generateBigInteger();
                    for (; !isPrime(Q); Q = generateBigInteger()) ;
                    BigInteger N = P * Q;
                    BigInteger Phi = (P - 1) * (Q - 1);

                    //Verify the primality of P and Q
                    BigInteger toVerify = rand.Next();
                    while (BigInteger.ModPow(toVerify, Phi, N) != 1)
                    {
                        P = generateBigInteger();
                        Q = generateBigInteger();
                        for (; !isPrime(P); P = generateBigInteger()) ;
                        for (; !isPrime(Q); Q = generateBigInteger()) ;
                        N = P * Q;
                        Phi = (P - 1) * (Q - 1);
                    }

                    //Generate a public key
                    BigInteger b = rand.Next() % Phi + 2;
                    for (; Euclid(b, Phi) != 1; b = rand.Next() % Phi + 2) ;

                    //Compute the private key
                    BigInteger temp = Phi;
                    for (; (temp + 1) % b != 0; temp += Phi) ;
                    BigInteger a = ((temp + 1) / b) % Phi;

                    //Record the public key and private key to a file
                    string publicKey = b.ToString() + "," + N.ToString();
                    string privateKey = a.ToString() + "," + P.ToString() + "," + Q.ToString();
                    string writeBuffer = publicKey + "\n" + privateKey;

                    byte[] bytesBuffer = Encoding.ASCII.GetBytes(writeBuffer);
                    foreach (byte by in bytesBuffer) fs.WriteByte(by);
                }
            }
            else
            {
                Console.WriteLine("File \"{0}\" already exists.", fileName);
                return;
            }
        }

        private BigInteger Euclid(BigInteger a, BigInteger b)
        {
            BigInteger r = b % a;
            if (r == 0)
                return a;
            else
                return Euclid(r, a);
        }

        private BigInteger generateBigInteger()
        {
            BigInteger res = new BigInteger(0);
            Random r = new Random();
            for (int i = 0; i < 256; i++)
            {
                int bit;
                if (i == 0)
                    bit = 1;
                else if (i == 255)
                    bit = 1;
                else
                {
                    int t = r.Next();
                    bit = t % 2;
                }
                res <<= 1;
                res += bit;
            }
            return res;
        }

        private bool isPrime(BigInteger integer)
        {
            BigInteger buffer = integer - 1;
            int t = 0;
            //2^t * buffer = integer - 1
            for (; buffer.IsEven; t++, buffer /= 2) ;
            //Console.WriteLine("{0}, {1}", buffer, t);
            Random rand = new Random();
            for (int i = 0; i < 16; i++)
            {
                int flag = 0;
                BigInteger bigRand = rand.Next();
                bigRand = bigRand % integer + 2;
                //Console.WriteLine(bigRand);
                BigInteger r = BigInteger.ModPow(bigRand, buffer, integer);
                for (int j = 0; j < t; j++)
                {
                    if (j == 0)
                    {
                        if (r == 1 || r == integer - 1)
                        {
                            flag = 1;
                            break;
                        }
                    }
                    else
                    {
                        if (r == integer - 1) ;
                        {
                            flag = 1;
                            break;
                        }
                    }
                    r = (r * r) % integer;
                }
                if (flag == 0)
                    return false;
            }
            return true;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage sm = new SendMessage();
            //sm.ShowDialog();
            sm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReceiveMessage rm = new ReceiveMessage();
            //rm.ShowDialog();
            rm.Show();
        }

    }
}
