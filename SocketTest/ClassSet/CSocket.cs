using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Diagnostics.Eventing.Reader;

namespace SocketTest
{


    public class CSocket
    {
        private Socket m_Socket = null;
        private IPAddress m_iP = IPAddress.Parse("127.0.0.1");
        private int m_Port = 8080;
        private bool m_bConnected = false;
        private bool m_bType  = false; //是否为服务器模式
        private List<Socket> m_ClientList = new List<Socket>();
        private Queue<string> m_MessageQueue = new Queue<string>();
        private Thread m_ListenThread = null;
        private Thread m_ReceiveThread = null;
        public void SetPort(int num)
        {
            m_Port= num;
        }
    
        public void SetType(bool bType)
        {
            m_bType= bType;
        }

        public string GetMessage()
        {
            string strTemp = string.Empty;
            if(m_MessageQueue.Count > 0)
            {
                lock (m_MessageQueue)
                {
                    strTemp = m_MessageQueue.Dequeue();
                }
            }
            return strTemp;
        }

        public bool GetSocketType()
        {
            return m_bType; 
        }

        public bool Connect()
        {
            try
            {
                IPEndPoint iPEndPoint = new IPEndPoint(m_iP, m_Port);
                m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                if (m_bType)
                {
                    m_Socket.Bind(iPEndPoint);
                    m_Socket.Listen(99);
                    m_ListenThread = new Thread(ListenThread);
                    m_ListenThread.IsBackground = true;
                    m_ListenThread.Priority = ThreadPriority.Normal;
                    m_ListenThread.Start();
                }
                else
                {
                    m_Socket.Connect(iPEndPoint);
                    m_bConnected = true;
                }
                m_ReceiveThread = new Thread(ReceiveThread);
                m_ReceiveThread.IsBackground = true;
                m_ReceiveThread.Priority = ThreadPriority.Normal;
                m_ReceiveThread.Start();
                return true;
            }
            catch 
            {
                m_bConnected = false;
                return false;
            }
        }

        public bool IsConnected()
        {
            return m_bType? m_ClientList.Count > 0: m_bConnected;
        }

        public bool Close()
        {
            m_bConnected = false;
            m_ReceiveThread?.Abort();
            if (m_bType) { 
                m_ListenThread?.Abort(); 
                m_ClientList.Clear();
            }
            m_Socket?.Close();
            return true;
        }

        public bool Send(string message)
        {
            if (!IsConnected())
            {
                return false;
            }
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message + "\n");
                if (m_bType)
                {
                    SendAll(buffer);
                }
                else
                {
                    m_Socket.Send(buffer);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SendAll(byte[] buffer)
        {
            foreach (Socket clien in m_ClientList)
            {
                try
                {
                    clien.Send(buffer);
                }
                catch (Exception)
                {
                    lock (m_ClientList)
                    {
                        m_ClientList.Remove(clien);
                    }
                }
            }
        }

        private void ListenThread()
        {
            while (true)
            {
                Socket client = m_Socket.Accept();
                m_ClientList.Add(client);
            }
        }

        private void ReceiveThread()
        {
            while (true)
            {
                if (!IsConnected())
                {
                    Thread.Sleep(100);
                    continue;
                }
                Queue<string> tempQueue = new Queue<string>();
                try
                {
                    byte[] buffer = new byte[1024];
                    string strMessage = string.Empty;
                    int countReceive = 0;
                    if (m_bType)
                    {
                        foreach (Socket socket in m_ClientList)
                        {
                            countReceive = socket.Receive(buffer);
                            strMessage = Encoding.UTF8.GetString(buffer, 0, countReceive);
                            if (strMessage != string.Empty)
                            {
                                tempQueue.Enqueue(strMessage);
                            }
                        }
                    }
                    else
                    {
                        countReceive = m_Socket.Receive(buffer);
                        strMessage = Encoding.UTF8.GetString(buffer, 0, countReceive);
                        if (strMessage != string.Empty)
                        {
                            tempQueue.Enqueue(strMessage);
                        }
                    }
                }
                catch 
                {
                    CLog.Instance.WriteRunTimeMessage((GetSocketType() ? "Server:" : "Client:") + m_Port + " Close");
                    this.Close();
                }

                lock (m_MessageQueue)
                {
                    int Length = tempQueue.Count;
                    for (int i = 0; i < Length; i++)
                    {
                        string strTemp = tempQueue.Dequeue();
                        m_MessageQueue.Enqueue(strTemp);
                    }
                }

            }
        }
    }
}
