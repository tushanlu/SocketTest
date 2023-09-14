using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketTest
{
    public partial class SocketForm : Form
    {
        private List<TextBox> m_textBoxList;
        private StatusStrip m_statusStrip;
        private ToolStripButton m_buttonAdd;
        private ToolStripButton m_buttonRemove;
        private ToolStripButton m_buttonStart;
        private ToolStripButton m_buttonStop;
        private ToolStripButton m_buttonConnect;
        private ToolStripButton m_buttonmPause;
        private ToolStripButton m_buttonAddMessage;
        private ListBox m_listBox;
        private CSocket m_Socket;
        private Thread m_SendThread;
        private Thread m_ReceiveThread;
        private bool m_bThread;
        private bool m_bSend = false;
        private int m_Time = 500;
        private EventWaitHandle handler = new AutoResetEvent(false);

        public SocketForm(int port, bool socketType, int textBoxNum = 0, List<string> massageList = null)
        {
            InitializeComponent();
            m_textBoxList = new List<TextBox>();
            m_statusStrip = new StatusStrip();
            m_buttonAdd = new ToolStripButton();
            m_buttonRemove = new ToolStripButton();
            m_buttonStart = new ToolStripButton();
            m_buttonStop = new ToolStripButton();
            m_buttonConnect = new ToolStripButton();
            m_buttonmPause = new ToolStripButton();
            m_buttonAddMessage = new ToolStripButton();
            m_listBox = new ListBox();
            this.panel_Lefl.ControlAdded += panel_Lefl_ControlAdded;
            this.m_buttonAdd.Click += buttonAdd_Click;
            this.m_buttonRemove.Click += buttonReMove_Click;
            this.m_buttonStart.Click += buttonStart_Click;
            this.m_buttonStop.Click += buttonStop_Click;
            this.m_buttonmPause.Click += buttonPause_Click;
            this.m_buttonConnect.Click += buttonConnect_Click;
            this.m_buttonAddMessage.Click += buttonAddMessage_Click;
            InitFrom(textBoxNum, massageList);

            m_Socket = new CSocket();
            m_Socket.SetType(socketType);
            m_Socket.SetPort(port);
            m_Socket.Connect();
            m_ReceiveThread = new Thread(ReceiveThreadHandler);
            m_ReceiveThread.IsBackground = true;
            m_ReceiveThread.Name = "接收消息线程";
            m_ReceiveThread.Start();
        }

        [DllImport("user32.dll")]
        public static extern int GetFocus(); //获取当前获得焦点的控件

        private TextBox GetFocusTextBox()
        {
            TextBox textBoxTemp = null;
            //获取当前获得焦点的控件
            IntPtr handle = (IntPtr)GetFocus();
            if (handle == null)
            {
                this.FindForm().KeyPreview = true;
            }
            else
            {
                Control control = Control.FromHandle(handle);//这就是
                if (control is TextBox)
                {
                    textBoxTemp = (TextBox)control;
                }
            }
            return textBoxTemp;
        }


    public void SetTime(int time)
        {
            m_Time = time;
        }

        public int GetTime()
        {
            return m_Time;
        }

        public List<string> GetMessages() 
        {
            List<string> TempList = new List<string>();
            foreach (TextBox textBox in m_textBoxList)
            {
                TempList.Add(textBox.Text);
            }
            return TempList;
        }

        public bool GetSocketType()
        {
           return m_Socket.GetSocketType();
        }

        private void InitFrom(int num, List<string> massageList)
        {
            for (int i = 0; i < num; i++)
            {
                TextBox textBox = new TextBox();
                if (massageList != null && massageList.Count > 0)
                {
                    textBox.Text = massageList[i];
                }
                AddTextBox(ref textBox);
                textBox.Focus();
            }
            m_statusStrip.Parent = this;
            m_statusStrip.BringToFront();
            m_statusStrip.Dock = DockStyle.Bottom;
            m_buttonAdd.Text = "Add";
            m_buttonRemove.Text = "Remove";
            m_buttonStart.Text = "Start";
            m_buttonStop.Text = "Stop";
            m_buttonmPause.Text = "Pause";
            m_buttonConnect.Text = "Connect";
            m_buttonAddMessage.Text = "AddMessage";
            m_statusStrip.Items.Add(m_buttonAdd);
            m_statusStrip.Items.Add(m_buttonRemove);
            m_statusStrip.Items.Add(m_buttonStart);
            m_statusStrip.Items.Add(m_buttonStop);
            m_statusStrip.Items.Add(m_buttonmPause);
            m_statusStrip.Items.Add(m_buttonConnect);
            m_statusStrip.Items.Add(m_buttonAddMessage);
            List<ToolStripButton> templist = new List<ToolStripButton>
            {
                m_buttonStop,
                m_buttonmPause
            };
            EnabledHandoff(templist, true);

            m_listBox.HorizontalScrollbar = true;
            m_listBox.Dock = DockStyle.Fill;
            panel_Right.Controls.Add(m_listBox);
        }

        private void AddTextBox(ref TextBox textBox)
        {

            textBox.Width = panel_Lefl.Width - 10;
            Point point = new Point();
            point.X = 0;
            int ntextBoxListCount = m_textBoxList.Count;
            point.Y = ntextBoxListCount > 0 ? m_textBoxList[ntextBoxListCount - 1].Location.Y + textBox.Height + 3 : 0;
            textBox.Location = point;
            this.panel_Lefl.Controls.Add(textBox);
            textBox.TabIndex = ntextBoxListCount;
            m_textBoxList.Add(textBox);

        }

        private void EnabledHandoff(List<ToolStripButton> toolStripButtons, bool bHandoff)
        {
          
            foreach (ToolStripButton temp in m_statusStrip.Items)
            {       
               temp.Enabled = bHandoff;
            }
            foreach (ToolStripButton temp in toolStripButtons)
            {
                temp.Enabled = !bHandoff;
            }
            foreach (TextBox tempTextBox in m_textBoxList)
            {
                tempTextBox.Enabled = bHandoff;
            }
        }

        private void SendThreadHandler()
        {
            while (m_bThread)
            {
                if (m_textBoxList.Count == 0)
                {
                    Thread.Sleep(50);
                    continue;
                }
                try
                {
                    foreach (TextBox itemTextBox in m_textBoxList)
                    {
                        if (itemTextBox.Text == string.Empty) continue;
                        this.Invoke((EventHandler)delegate
                        {
                            itemTextBox.BackColor = Color.Gray;
                        });
                        bool bErr =  m_Socket.Send(itemTextBox.Text);
                        CLog.Instance.WriteRunTimeMessage(this.Text + " Send:" + itemTextBox.Text);
                        if (m_bSend)
                        {
                            handler.WaitOne();
                        }
                        Thread.Sleep(m_Time);
                        this.Invoke((EventHandler)delegate
                        {
                            itemTextBox.BackColor = Color.White;
                            //WriteListBox(this.Text + " Send:" + itemTextBox.Text);
                        });
                        if (!bErr)
                        {
                            m_bThread = false;
                            this.Invoke((EventHandler)delegate
                            {
                                List<ToolStripButton> templist = new List<ToolStripButton>
                                {
                                    m_buttonStop,
                                    m_buttonmPause
                                };
                                EnabledHandoff(templist, true);
                            });
                        }
                        if (!m_bThread)
                        {
                            break;
                        }

                    }

                }
                catch (Exception ex) 
                {
                    CLog.Instance.WriteRunTimeMessage("Error:" + ex.ToString());
                }

            }
        }

        private void ReceiveThreadHandler()
        {
            while (true)
            {
                string strTemp = m_Socket.GetMessage();
                if (strTemp == string.Empty)
                {
                    Thread.Sleep(50);
                    continue;
                }
                this.Invoke((EventHandler)delegate
                {
                    string strTime = DateTime.Now.ToString("HH-mm-ss-fff");
                    WriteListBox(strTime+" Receive: " + strTemp);
                });
               CLog.Instance.WriteRunTimeMessage(this.Text + " Receive: " + strTemp);
            }
        }

        private void WriteListBox(string strMesssage)
        {
            lock (m_listBox)
            {
                if (m_listBox.Items.Count > 1000)
                {
                    m_listBox.Items.RemoveAt(0);
                }
                m_listBox.Items.Add(strMesssage);
                m_listBox.SelectedIndex = m_listBox.Items.Count - 1;
            }
        }

        private void panel_Lefl_ControlAdded(object sender, ControlEventArgs e)
        {
          panel_Lefl.AutoScroll = true;
          panel_Lefl.VerticalScroll.Enabled = true;
          panel_Lefl.VerticalScroll.Visible = true;
          panel_Lefl.Scroll += panel_Lefl_Scroll;
        }

        private void panel_Lefl_Scroll(object sender, ScrollEventArgs e)
        {
            this.panel_Lefl.VerticalScroll.Value = e.NewValue;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            TextBox textBox = new TextBox();
            AddTextBox(ref textBox);
            textBox.Focus();
        }

        private void buttonReMove_Click(object sender, EventArgs e)
        {
            int nCount = m_textBoxList.Count;
            if(nCount==0) return;
            TextBox lastTextBox = m_textBoxList[nCount - 1];
            m_textBoxList.Remove(lastTextBox);
          panel_Lefl.Controls.Remove(lastTextBox);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!m_Socket.IsConnected()) return;
            m_bThread = true;
            m_SendThread = new Thread(SendThreadHandler);
            m_SendThread.IsBackground = true;
            m_SendThread.Name = "发送消息线程";
            m_SendThread.Start();
            List<ToolStripButton> templist = new List<ToolStripButton>
            {
                m_buttonStop,
                m_buttonmPause
            };
            EnabledHandoff(templist, false);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {

            m_bThread = false;
            List<ToolStripButton> templist = new List<ToolStripButton>
            {
                m_buttonStop,
                m_buttonmPause
            };
            EnabledHandoff(templist, true);
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            m_Socket.Close();
            m_Socket.Connect();
        }

        private void buttonAddMessage_Click(object sender, EventArgs e)
        {
            
            int textBoxCount = m_textBoxList.Count;
            int textBoxIndex = GetFocusTextBox() == null ? -1: GetFocusTextBox().TabIndex;
            if (textBoxCount == 0 || textBoxIndex == -1) return;
            string[] tempArr = m_textBoxList[textBoxIndex].Text.Split('{','}');
            bool bIntType =  int.TryParse(tempArr[tempArr.Length / 2], out int nTmpe);
            for (int i = textBoxIndex,add = 0; i < textBoxCount; i++,add++)
            {
                if (tempArr.Length ==0 || !bIntType) break;
                string strTemp = tempArr[0] +"{"+ (nTmpe + add) +"}" + tempArr[tempArr.Length-1];         
                m_textBoxList[i].Text = strTemp;
          
            }
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            if(!m_bSend)
            {
                m_buttonmPause.Text = "Resume";
            }
            else
            {
                m_buttonmPause.Text = "Pause";
                handler.Set();
            }
            m_bSend = !m_bSend;
        }

        private void SocketForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_bThread = false;
            m_ReceiveThread.Abort();
            m_Socket.Close();
        }

        private void SocketForm_Resize(object sender, EventArgs e)
        {
            this.panel_Right.Width = Convert.ToInt32(this.Width *0.45);
        }

        private void panel_Lefl_Resize(object sender, EventArgs e)
        {
            if (m_textBoxList == null) return;
            foreach (TextBox text in m_textBoxList)
            {
                text.Width = panel_Lefl.Width - 10;
            }
        }
    }
}
