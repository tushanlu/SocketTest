namespace SocketTest
{
    partial class SetUpForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_SocketType = new System.Windows.Forms.Label();
            this.textBox_Port = new System.Windows.Forms.TextBox();
            this.label_Port = new System.Windows.Forms.Label();
            this.comboBox_ScketType = new System.Windows.Forms.ComboBox();
            this.label_Time = new System.Windows.Forms.Label();
            this.textBox_Time = new System.Windows.Forms.TextBox();
            this.button_Create = new System.Windows.Forms.Button();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_SocketType
            // 
            this.label_SocketType.AutoSize = true;
            this.label_SocketType.Location = new System.Drawing.Point(79, 50);
            this.label_SocketType.Name = "label_SocketType";
            this.label_SocketType.Size = new System.Drawing.Size(70, 14);
            this.label_SocketType.TabIndex = 0;
            this.label_SocketType.Text = "ScketType";
            // 
            // textBox_Port
            // 
            this.textBox_Port.Location = new System.Drawing.Point(178, 105);
            this.textBox_Port.Name = "textBox_Port";
            this.textBox_Port.Size = new System.Drawing.Size(121, 23);
            this.textBox_Port.TabIndex = 1;
            // 
            // label_Port
            // 
            this.label_Port.AutoSize = true;
            this.label_Port.Location = new System.Drawing.Point(79, 107);
            this.label_Port.Name = "label_Port";
            this.label_Port.Size = new System.Drawing.Size(35, 14);
            this.label_Port.TabIndex = 2;
            this.label_Port.Text = "Port";
            // 
            // comboBox_ScketType
            // 
            this.comboBox_ScketType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ScketType.FormattingEnabled = true;
            this.comboBox_ScketType.Location = new System.Drawing.Point(178, 50);
            this.comboBox_ScketType.Name = "comboBox_ScketType";
            this.comboBox_ScketType.Size = new System.Drawing.Size(121, 22);
            this.comboBox_ScketType.TabIndex = 3;
            // 
            // label_Time
            // 
            this.label_Time.AutoSize = true;
            this.label_Time.Location = new System.Drawing.Point(79, 164);
            this.label_Time.Name = "label_Time";
            this.label_Time.Size = new System.Drawing.Size(63, 14);
            this.label_Time.TabIndex = 4;
            this.label_Time.Text = "Time(ms)";
            // 
            // textBox_Time
            // 
            this.textBox_Time.Location = new System.Drawing.Point(178, 161);
            this.textBox_Time.Name = "textBox_Time";
            this.textBox_Time.Size = new System.Drawing.Size(121, 23);
            this.textBox_Time.TabIndex = 6;
            // 
            // button_Create
            // 
            this.button_Create.Location = new System.Drawing.Point(132, 278);
            this.button_Create.Name = "button_Create";
            this.button_Create.Size = new System.Drawing.Size(111, 59);
            this.button_Create.TabIndex = 7;
            this.button_Create.Text = "Create";
            this.button_Create.UseVisualStyleBackColor = true;
            this.button_Create.Click += new System.EventHandler(this.button_Create_Click);
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.button_Create);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(394, 434);
            this.groupBox.TabIndex = 8;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "SetUp";
            // 
            // SetUpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 434);
            this.Controls.Add(this.textBox_Time);
            this.Controls.Add(this.label_Time);
            this.Controls.Add(this.comboBox_ScketType);
            this.Controls.Add(this.label_Port);
            this.Controls.Add(this.textBox_Port);
            this.Controls.Add(this.label_SocketType);
            this.Controls.Add(this.groupBox);
            this.Name = "SetUpForm";
            this.Text = "SetUpForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SetUpForm_FormClosed);
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_SocketType;
        private System.Windows.Forms.TextBox textBox_Port;
        private System.Windows.Forms.Label label_Port;
        private System.Windows.Forms.ComboBox comboBox_ScketType;
        private System.Windows.Forms.Label label_Time;
        private System.Windows.Forms.TextBox textBox_Time;
        private System.Windows.Forms.Button button_Create;
        private System.Windows.Forms.GroupBox groupBox;
    }
}