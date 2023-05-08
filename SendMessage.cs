using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Threading;
using System.Security.Cryptography;
using System.IO;


namespace SecureCommunication
{
    public partial class SendMessage : Form
    {
        public SendMessage()
        {
            InitializeComponent();
        }

        BigInteger P = 0;
        BigInteger Q = 0;
        BigInteger N = 0;
        BigInteger ServerP = 0;
        BigInteger ServerN = 0;
        bool isFile = false;

        // Connect to the server and initialization
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled= false;
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                IPAddress addr = IPAddress.Parse(IPAddr.Text);
                IPEndPoint ipe = new IPEndPoint(addr, 11000);
                s.Connect(ipe);
            }
            catch (ArgumentNullException ae)
            {
                Console.WriteLine("ArgumentNullException : {0}", ae.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : {0}", ex.ToString());
            }
            button2.Enabled = true;
            EncryptionScheme.Enabled = false;
            Key.Enabled = false;
            HashFunction.Enabled = false;
            textBox1.Enabled = false;
            
            string fileName = "../../publicKey.txt";
            string pkstr = "";
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(@fileName);
                pkstr += file.ReadLine();
                
                string[] pkBuffer = pkstr.Split(',');
                P = BigInteger.Parse(pkBuffer[0]);
                N = BigInteger.Parse(pkBuffer[1]);
                string prstr = file.ReadLine();
                string[] prBuffer = prstr.Split(',');
                Q = BigInteger.Parse(prBuffer[0]);

                file.Close();
            }
            catch (System.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                int config = 0;
                if (EncryptionScheme.Text == "AES")
                    config += 2;
                if (HashFunction.Text == "MD5")
                    config += 1;
                string configStr = pkstr + "," + config.ToString();
                byte[] pkBytes = Encoding.ASCII.GetBytes(configStr);
                int bytesSend = s.Send(pkBytes);

                byte[] buffer = new byte[1024];
                int bytesRec = s.Receive(buffer);
                string serverPK = Encoding.ASCII.GetString(buffer, 0, bytesRec);
                
                string[] strBuffer = serverPK.Split(',');
                ServerP = BigInteger.Parse(strBuffer[0]);
                ServerN = BigInteger.Parse(strBuffer[1]);
            }
            catch { }

            
            s.Close();
        }

        // To upload file, and the selected file will be convert to string to transport to the other side.
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Message.Text = ofd.FileName.ToString();

            }
        }
        

        // Send message
        private void button2_Click(object sender, EventArgs e)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                IPAddress addr = IPAddress.Parse(IPAddr.Text);
                IPEndPoint ipe = new IPEndPoint(addr, 11000);
                s.Connect(ipe);
            }
            catch(ArgumentNullException ae){
                Console.WriteLine("ArgumentNullException : {0}", ae.ToString());
            }catch(SocketException se){
                Console.WriteLine("SocketException : {0}", se.ToString());
            }catch(Exception ex){
                Console.WriteLine("Exception : {0}", ex.ToString());
            }

            string filePath = Message.Text;
            
            if (System.IO.File.Exists(filePath))
            {
                isFile = true;
                byte[] file = System.IO.File.ReadAllBytes(filePath);
                string line = Encoding.ASCII.GetString(file);
                byte[] pakage = toEncrypt(line);
                s.Send(pakage);
            }
            else
            {
                byte[] pakage = toEncrypt(Message.Text);
                s.Send(pakage);
            }
            
            s.Close();
        }

        private byte[] toEncrypt(string message)
        {
            // Convert string key to byte array
            byte[] kBytes = new byte[16];
            if (Key.Text == "Seed generates")
            {
                string seedStr = textBox1.Text;
                Console.WriteLine(seedStr);
                byte[] seed = new byte[4];
                for (int i = 0; i < seedStr.Length; i += 2)
                {
                    string buffer = "";
                    buffer += seedStr[i];
                    buffer += seedStr[i + 1];
                    try
                    {
                        kBytes[i / 2] = Convert.ToByte(buffer, 16);
                    }
                    catch { }
                }
                Random r = new Random();
                int[] hash = { 0, 0, 0, 0 };
                for (int i = 0; i < 16; i++)
                {
                    int index = r.Next();
                    index %= 4;
                    kBytes[i] = (byte)((seed[index] + hash[index]) % 256);
                    hash[index]++;
                }
            }
            else
            {
                string kStr = Key.Text;
                for (int i = 0; i < kStr.Length; i += 2)
                {
                    string buffer = "";
                    buffer += kStr[i];
                    buffer += kStr[i + 1];
                    try
                    {
                        kBytes[i / 2] = Convert.ToByte(buffer, 16);
                    }
                    catch { }
                }
            }
            
            // Enclapse the session key
            BigInteger kInt = new BigInteger(kBytes);
            BigInteger encryptedKey = BigInteger.ModPow(kInt, ServerP, ServerN);
            byte[] encryptedKeyBytes = encryptedKey.ToByteArray();
            byte keyLength = (byte)encryptedKeyBytes.Length;

            byte signatureLength;

            // Sign on this message
            if (HashFunction.Text == "MD5")
            {
                MessageDigestAlgorithm md5 = new MessageDigestAlgorithm();
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                md5.MD5Compute(buffer);
                BigInteger hashInt = new BigInteger(md5.hash);
                BigInteger sign = BigInteger.ModPow(hashInt, Q, N);
                Console.WriteLine(sign);
                message = sign.ToString() + message;
                signatureLength = (byte)(sign.ToString()).Length;
            }
            else
            {
                SecureHashAlgorithm sha = new SecureHashAlgorithm();
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                sha.SHACompute(buffer);
                BigInteger hashInt = new BigInteger(sha.hash);
                BigInteger sign = BigInteger.ModPow(hashInt, Q, N);
                message = sign.ToString() + message;
                signatureLength = (byte)(sign.ToString()).Length;
            }

            
            // Encrypt
            byte[] iv = { 0xad, 0xbe, 0xc5, 0x23, 0xa3, 0x34, 0x45, 0xac, 0xed, 0xfe, 0xca, 0xbe, 0xba, 0x12, 0x3d, 0x4f };
            byte[] msgBytes = Encoding.ASCII.GetBytes(message);
            if (EncryptionScheme.Text == "AES")
            {
                AdvancedEncryptionStandard aes = new AdvancedEncryptionStandard();
                aes.setKey(kBytes);
                aes.setIV(iv);
                byte[] encrypted = aes.encrypt(msgBytes);
                
                // Pakage the ciphertext, key and IV
                byte[] pakage = new Byte[1 + 1 + encryptedKeyBytes.Length + iv.Length + 1 + encrypted.Length];
                int idx = 0;
                pakage[idx++] = (byte)(isFile ? 1 : 0);
                pakage[idx++] = keyLength;
                for (int i = 0; i < encryptedKeyBytes.Length; i++)
                    pakage[idx++] = encryptedKeyBytes[i];
                for (int i = 0; i < iv.Length; i++)
                    pakage[idx++] = iv[i];
                pakage[idx++] = signatureLength;
                for (int i = 0; i < encrypted.Length; i++)
                    pakage[idx++] = encrypted[i];
                return pakage;
            }
            else
            {
                DataEncryptionStandard des = new DataEncryptionStandard();
                des.setKey(kBytes);
                des.setIV(iv);
                byte[] encrypted = des.encrypt(msgBytes);
                
                // Pakage the ciphertext, key and IV
                byte[] pakage = new Byte[1 + 1 + encryptedKeyBytes.Length + iv.Length + 1 + encrypted.Length];
                int idx = 0;
                pakage[idx++] = (byte)(isFile ? 1 : 0);
                pakage[idx++] = keyLength;
                for (int i = 0; i < encryptedKeyBytes.Length; i++)
                    pakage[idx++] = encryptedKeyBytes[i];
                for (int i = 0; i < iv.Length; i++)
                    pakage[idx++] = iv[i];
                pakage[idx++] = signatureLength;
                for (int i = 0; i < encrypted.Length; i++)
                    pakage[idx++] = encrypted[i];
                return pakage;
            }
        }

        private bool isCorrectIP(string ip)
        {
            string[] temp = ip.Split('.');
            if (temp.Length != 4)
                return false;
            for (int i = 0; i < temp.Length; i++)
            {
                string buffer = temp[i];
                if (buffer.Length > 3)
                    return false;
                if (buffer.Length == 0)
                    return false;
                for (int j = 0; j < buffer.Length; j++)
                    if (buffer[j] < '0' || buffer[j] > '9')
                        return false;
            }
            return true;
        }

        private void IPAddr_TextChanged(object sender, EventArgs e)
        {
            if (isCorrectIP(IPAddr.Text))
                button1.Enabled = true;
        }

        private void Key_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Key.Text == "Seed generates")
            {
                label9.Visible = true;
                textBox1.Enabled = true;
            }
                
        }
    }
}
