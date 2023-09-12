using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace SocketTest
{
    

    public partial class MainForm : Form
    {
        private Dictionary<TabPage, SocketForm> m_SoketFormDict = new Dictionary<TabPage, SocketForm>();
        private TabControl m_tabControl = new TabControl();
        private ToolStrip m_toolStrip = new ToolStrip();
        private ToolStripButton m_buttonOpen = new ToolStripButton();
        private ToolStripButton m_buttonSave = new ToolStripButton();
        private ToolStripButton m_buttonAdd = new ToolStripButton();
        private ToolStripButton m_buttonDel = new ToolStripButton();
        private ToolStripButton m_buttonSetTime = new ToolStripButton();

        public MainForm()
        {
            InitializeComponent();
            CLog.Instance.SetPath(System.Windows.Forms.Application.StartupPath);
            m_toolStrip.Parent = panelTop;
            m_toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            panelTop.Height = m_toolStrip.Height;
            this.m_buttonOpen.Click += buttonOpen_Click;
            this.m_buttonSave.Click += buttonSave_Click;
            this.m_buttonAdd.Click += buttonAdd_Click;
            this.m_buttonDel.Click += buttonDel_Click;
            this.m_buttonSetTime.Click += buttonSetTime_Click;
            panelFill.Controls.Add(m_tabControl);
            m_tabControl.ControlRemoved += tabControl_ControlRemoved;
            m_tabControl.Dock = DockStyle.Fill;
            m_buttonOpen.Text = "Open";
            m_buttonSave.Text = "Save";
            m_buttonAdd.Text = "AddTab";
            m_buttonDel.Text = "DelTab";
            m_buttonSetTime.Text = "SetTime";
            m_toolStrip.Items.Add(m_buttonOpen);
            m_toolStrip.Items.Add(m_buttonSave);
            m_toolStrip.Items.Add(m_buttonAdd);
            m_toolStrip.Items.Add(m_buttonDel);
            m_toolStrip.Items.Add(m_buttonSetTime);
           
        }

        private void AddTadPage(int port, bool socketType,int Time, int num = 0, List<string>messageList = null)
        {
            TabPage TabPageTmpe = new TabPage();
            TabPageTmpe.Text = (socketType ? "Server:": "Client:" )+ port;
            m_tabControl.Controls.Add(TabPageTmpe);
            SocketForm socketForm = new SocketForm(port, socketType, num, messageList);
            socketForm.Text = TabPageTmpe.Text;
            socketForm.FormBorderStyle = FormBorderStyle.None;
            socketForm.TopLevel = false;
            TabPageTmpe.Controls.Add(socketForm);
            socketForm.Dock = DockStyle.Fill;
            socketForm.Show();
            socketForm.SetTime(Time);
            m_SoketFormDict.Add(TabPageTmpe, socketForm);
   
        }

        private void Save(string filename)
        {
            
            CProject project = new CProject();
            foreach (TabPage tabPage in m_SoketFormDict.Keys)
            {
                string[] tabPageS = tabPage.Text.Split(':');
                project.m_TabPageList.Add(tabPageS[tabPageS.Length-1]);
                project.m_SocketTypeList.Add(m_SoketFormDict[tabPage].GetSocketType());
                project.m_TimeList.Add(m_SoketFormDict[tabPage].GetTime());
                project.m_MessageList.Add(m_SoketFormDict[tabPage].GetMessages());
            }
            try
            {
                string output = JsonConvert.SerializeObject(project);
                StreamWriter sw = new StreamWriter(filename);
                sw.Write(output);
                sw.Close();
            }
            catch
            {
                CLog.Instance.WriteRunTimeMessage("Save File Error:" + filename);
            }
        }

        private void Read(string filename) 
        {
            Clear();
            string readTemp = File.ReadAllText(filename);
            CProject project = null;
            try
            {
                project = JsonConvert.DeserializeObject<CProject>(readTemp);

            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                CLog.Instance.WriteRunTimeMessage("Read File Error:" + filename);
                return;
            }
            if (project == null) return;
            for (int i = 0; i < project.m_TabPageList.Count; i++)
            {
                AddTadPage(int.Parse(project.m_TabPageList[i]), project.m_SocketTypeList[i] , project.m_TimeList[i],project.m_MessageList[i].Count, project.m_MessageList[i]);
            }

        }

        private void Clear()
        {
            m_SoketFormDict.Clear();
            m_tabControl.Controls.Clear();
        }

        private string Input()
        {
            string setTemp = string.Empty;
            Form inputForm = new Form();
            inputForm.Width = 180;
            inputForm.Height = 100;
            inputForm.ControlBox = false;
            inputForm.ShowIcon = false;
            inputForm.ShowInTaskbar = false;
            inputForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            inputForm.Text = "Input";
            inputForm.StartPosition = FormStartPosition.CenterParent;
            TextBox textBox = new TextBox();
            textBox.Parent = inputForm;
            textBox.Location = new Point(0, 2);
            textBox.Width = inputForm.Width - 2;
            Button button = new Button();
            button.Parent = inputForm;
            button.Text = "OK";
            button.Location = new Point(inputForm.Width - button.Width - 20, inputForm.Height - button.Height - 50);
            button.Click += new EventHandler((object obj, EventArgs er) =>
            {
                setTemp = textBox.Text;
                inputForm.Close();

            });
            inputForm.ShowDialog();
            return setTemp;
        }

        private void Del() 
        {
            TabPage tabPage = m_tabControl.SelectedTab;
            if (m_SoketFormDict.ContainsKey(tabPage))
            {
                m_SoketFormDict[tabPage].Close();
            }
            m_SoketFormDict.Remove(tabPage);
            m_tabControl.Controls.Remove(tabPage);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if(m_SoketFormDict.Count == 0)
            {
                return;
            }
            System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.Filter = "文件|*";
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string newFilepath = dialog.FileName;
                Save(newFilepath);
            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "文件|*";
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string filepath = dialog.FileName;
                Read(filepath);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {

            TabPage TabPageTmpe = new TabPage();
            TabPageTmpe.Text = "Config";
            m_tabControl.Controls.Add(TabPageTmpe);
            SetUpForm setUpForm = new SetUpForm();
            setUpForm.addDelegate += new AddDelegate(AddTadPage);
            setUpForm.dleDelegate += new DleDelegate(Del);
            setUpForm.FormBorderStyle = FormBorderStyle.None;
            setUpForm.TopLevel = false;
            TabPageTmpe.Controls.Add(setUpForm);
            setUpForm.Width = this.Width/2;
            setUpForm.Dock = DockStyle.Left;
            setUpForm.Show();
            m_tabControl.SelectedTab = TabPageTmpe;

        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (m_tabControl.Controls.Count == 0) return;    
            Del();

        }

        private void buttonSetTime_Click(object sender, EventArgs e)
        {
            if (m_tabControl.Controls.Count == 0) return;
            string strTemp = Input();
            if (strTemp == string.Empty) return;
            int nTime = int.Parse(strTemp);
            TabPage tabPage = m_tabControl.SelectedTab;
            m_SoketFormDict[tabPage].SetTime(nTime);
        }

        private void tabControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            m_tabControl.SelectedIndex = m_tabControl.TabCount - 1;
        }
    }

}
