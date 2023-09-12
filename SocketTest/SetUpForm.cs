using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SocketTest.MainForm;

namespace SocketTest
{
    public delegate void AddDelegate(int port, bool socketType,  int Time,int num = 1, List<string> messageList = null);
    public delegate void DleDelegate();
    public partial class SetUpForm : Form
    {
        public event AddDelegate addDelegate;
        public event DleDelegate dleDelegate;

        public SetUpForm()
        {
            InitializeComponent();
            comboBox_ScketType.Items.Add("Client");
            comboBox_ScketType.Items.Add("Server");
            comboBox_ScketType.SelectedIndex = 0;
            textBox_Port.Text = "9000";
            textBox_Time.Text = "500";
        }

        private void SetUpForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            dleDelegate();
        }

        private bool IsScketType()
        {
           return comboBox_ScketType.SelectedIndex == 1;
        }

        private void button_Create_Click(object sender, EventArgs e)
        {
            int nPort,nTime ;
            bool bPortEmpty =  int.TryParse(textBox_Port.Text, out nPort);
            bool bTimeEmpty =  int.TryParse(textBox_Time.Text, out nTime);
            if (!bPortEmpty || !bTimeEmpty) return;
            addDelegate(nPort, IsScketType(), nTime);
            this.Close();
        }

    }
}
