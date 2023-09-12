using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SocketTest
{
    public class CLog
    {
        private static Object m_Lock = new object();
        private Queue<string> m_MessageQueue = new Queue<string>();
        private Thread m_BackWriteThread;
        private bool m_bWrite = false;
        private string m_FilePath;
        private static readonly Lazy<CLog> m_instacne = new Lazy<CLog>(() => new CLog());
        public static CLog Instance { get => m_instacne.Value; }

        private CLog()
        {
            m_bWrite = true;
            m_BackWriteThread = new Thread(WriteThreadHandler);
            m_BackWriteThread.IsBackground = true;
            m_BackWriteThread.Name = "异步写入日志文件线程";
            m_BackWriteThread.Start();
        }

       ~CLog()
        {
            m_MessageQueue.Clear();
            m_bWrite = false;
        }
            
        public bool WriteRunTimeMessage(string message)
        {
            lock (m_Lock)
            {
                string logMessage = DateTime.Now.ToString("HH-mm-ss-fff") + "->" + message;
                m_MessageQueue.Enqueue(logMessage);
            }
            return true;
        }

        public bool SetPath(string path) 
        {
            m_FilePath = path+"\\Log\\";
            if(!System.IO.Directory.Exists(m_FilePath))
            {
                Directory.CreateDirectory(m_FilePath);
            }
            return true;
        }

        private void WriteThreadHandler()
        {
            Queue<string> tempQueue = new Queue<string>();
            while (m_bWrite)
            {
                StreamWriter sw = null;

                lock(m_MessageQueue)
                {
                    int nowConut = m_MessageQueue.Count;
                    if (nowConut == 0) 
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    for (int i = 0; i < nowConut; i++)
                    {
                        try
                        {
                            tempQueue.Enqueue(m_MessageQueue.Dequeue());
                        }
                        catch (System.InvalidOperationException exp)
                        {
                            CLog.Instance.WriteRunTimeMessage(exp.Message);
                            break;
                        }
                    }
                }

                try
                {
                    int nowTempConut = tempQueue.Count;
                    for (int i = 0;i < nowTempConut; i++)
                    {
                        string tempMessage = tempQueue.Dequeue();
                        DateTime dtNow = DateTime.Now;
                        string flieName = dtNow.ToString("yyyy-MM-dd-HH")+ ".log";
                        string fliePath = m_FilePath + flieName;
                        sw = new StreamWriter(fliePath, true, Encoding.UTF8, 1024);
                        sw.AutoFlush = true;
                        sw.WriteLine(tempMessage);                     
                        sw.Close();
                    }
                }
                catch(Exception exp)
                {
                    throw exp;
                }

                finally
                {
                    if (sw != null)
                    {
                        sw.Close();
                        sw.Dispose();
                    }
                }
                Thread.Sleep(1000);
            }

        }


    }

}
