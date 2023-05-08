using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Cryptography;
using System.IO;


namespace SecureCommunication
{
    public partial class ReceiveMessage : Form
    {
        public ReceiveMessage()
        {
            InitializeComponent();
        }

        bool isConfiged = false;
        string encryptionScheme = "";
        string hashFunction = "";

        BigInteger P = 0;
        BigInteger Q = 0;
        BigInteger N = 0;
        BigInteger senderP = 0;
        BigInteger senderN = 0;
        int fileCnt = 0;

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            try
            {
                Socket watchSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(ReceiveFrom.Text);
                IPEndPoint ipe = new IPEndPoint(ip, 11000);
                watchSocket.Bind(ipe);
                watchSocket.Listen(100);
                Thread th = new Thread(listenMessage);
                th.IsBackground = true;
                th.Start(watchSocket);
            }
            catch { }
        }

        private void listenMessage(object obj)
        {
            Socket watchSocket = obj as Socket;
            
            try
            {
                // Start listening for connections.  
                while (true)
                {
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = watchSocket.Accept();
                    Thread gM = new Thread(getMessage);
                    gM.IsBackground = true;
                    gM.Start(handler);

                    if (!isConfiged)
                    {
                        Thread sTC = new Thread(sendToClient);
                        sTC.IsBackground = true;
                        sTC.Start(handler);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }  
        }

        private void sendToClient(object obj)
        {
            Socket sendToClient = obj as Socket;
            try
            {
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


                byte[] msg = Encoding.ASCII.GetBytes(pkstr);
                int bytesSend = sendToClient.Send(msg);
            }
            catch { }
        }

        private void getMessage(object obj)
        {
            Socket handler = obj as Socket;
            
            string data = null;
            try
            {
                if (isConfiged)
                {
                    while (true)
                    {
                        byte[] pakage = new Byte[1024];
                        int bytesRec = handler.Receive(pakage);
                        //data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        data += toDecrypt(pakage, bytesRec);

                        if (bytesRec == 0)
                        {
                            break;
                        }
                        ReceivedMessage.AppendText(data + "\r\n");
                    }
                }
                else
                {
                    ReceivedMessage.AppendText("Configing...\r\n");
                    byte[] bytes = new Byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    
                    string[] configStr = data.Split(',');
                    senderP = BigInteger.Parse(configStr[0]);
                    senderN = BigInteger.Parse(configStr[1]);
                    ReceivedMessage.AppendText("The public key of sender is:\r\n{" + configStr[0] + ", " + configStr[1] + "}\r\n");
                    
                    if (configStr[2][0] >= '2')
                        encryptionScheme = "AES";
                    else
                        encryptionScheme = "DES";
                    if (configStr[2][0] == '1' || configStr[2][0] == '3')
                        hashFunction = "MD5";
                    else
                        hashFunction = "SHA3";
                    ReceivedMessage.AppendText("Encryption scheme: " + encryptionScheme + "\r\nHash Function: " + hashFunction + "\r\n");
                    ReceivedMessage.AppendText("Configuration has done.\r\n\n");
                    
                    isConfiged = true;
                }
            }
            catch { }
            
        }

        private string toDecrypt(byte[] pakage, int bytesRec)
        {
            string data = "";
            string messageStr = "";
            byte signatureLength;
            byte isFile;
            if (encryptionScheme == "AES")
            {
                byte[] iv = new Byte[16];
                int idx = 0;
                isFile = pakage[idx++];
                byte keyLength = pakage[idx++];
                byte[] encryptedKey = new Byte[keyLength];
                byte[] ciphertext = new Byte[bytesRec - 16 - keyLength - 1 - 1 - 1];
                for (int i = 0; i < keyLength; i++)
                    encryptedKey[i] = pakage[idx++];
                BigInteger encryptedKeyInt = new BigInteger(encryptedKey);
                BigInteger keyInt = BigInteger.ModPow(encryptedKeyInt, Q, N);
                byte[] key = keyInt.ToByteArray();

                for (int i = 0; i < iv.Length; i++)
                    iv[i] = pakage[idx++];
                signatureLength = pakage[idx++];
                for (int i = 0; i < ciphertext.Length; i++)
                    ciphertext[i] = pakage[idx++];
                AdvancedEncryptionStandard aes = new AdvancedEncryptionStandard();
                aes.setKey(key);
                aes.setIV(iv);
                byte[] dec = aes.decrypt(ciphertext);
                data = Encoding.ASCII.GetString(dec);
            }
            else
            {
                byte[] iv = new Byte[8];
                int idx = 0;
                isFile = pakage[idx++];
                byte keyLength = pakage[idx++];
                byte[] encryptedKey = new Byte[keyLength];
                byte[] ciphertext = new Byte[bytesRec - 8 - keyLength - 1 - 1 - 1];
                for (int i = 0; i < keyLength; i++)
                    encryptedKey[i] = pakage[idx++];
                for (int i = 0; i < iv.Length; i++)
                    iv[i] = pakage[idx++];
                signatureLength = pakage[idx++]; 
                for (int i = 0; i < ciphertext.Length; i++)
                    ciphertext[i] = pakage[idx++];
                BigInteger encryptedKeyInt = new BigInteger(encryptedKey);
                BigInteger keyInt = BigInteger.ModPow(encryptedKeyInt, Q, N);
                Console.WriteLine(keyInt);
                byte[] key = keyInt.ToByteArray();


                DataEncryptionStandard des = new DataEncryptionStandard();
                des.setIV(iv);
                des.setKey(key);
                byte[] dec = des.decrypt(ciphertext);
                data += Encoding.ASCII.GetString(dec);
            }

            string signature = data.Substring(0, signatureLength);
            messageStr = data.Substring(signatureLength);

            BigInteger signInt = BigInteger.Parse(signature);
            BigInteger hashValue = BigInteger.ModPow(signInt, senderP, senderN);

            if (hashFunction == "MD5")
            {
                MessageDigestAlgorithm md5 = new MessageDigestAlgorithm();
                byte[] messageBytes = Encoding.ASCII.GetBytes(messageStr);
                md5.MD5Compute(messageBytes);
                BigInteger hashInt = new BigInteger(md5.hash);

                if (hashInt != hashValue)
                    messageStr = messageStr + "\nSystem:This message DID NOT pass the authentication!";
            }
            else
            {
                SecureHashAlgorithm sha = new SecureHashAlgorithm();
                byte[] messageBytes = Encoding.ASCII.GetBytes(messageStr);
                sha.SHACompute(messageBytes);
                BigInteger hashInt = new BigInteger(sha.hash);

                if (hashInt != hashValue)
                    messageStr = messageStr + "\nSystem:This message DID NOT pass the authentication!";
            }

            if (isFile == 1)
            {
                byte[] file = Encoding.ASCII.GetBytes(messageStr);
                string folder = "..\\FileRcvd";
                if(!System.IO.Directory.Exists(folder))
                    System.IO.Directory.CreateDirectory(folder);
                string fileName = "file" + fileCnt.ToString() + ".txt";
                string path = System.IO.Path.Combine(folder, fileName);
                System.IO.File.WriteAllBytes(path, file);
                fileCnt++;
                return "File was saved as " + path + "\n";
            }
            else
                return messageStr;
        }
    

        // Check whether the IP address is leagal or not
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

        private void ReceiveFrom_TextChanged(object sender, EventArgs e)
        {
            if (isCorrectIP(ReceiveFrom.Text))
                button4.Enabled = true;
        }

    }
}
