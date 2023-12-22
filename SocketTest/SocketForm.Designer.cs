namespace SocketTest
{
    partial class SocketForm
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
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel_Lefl = new System.Windows.Forms.Panel();
            this.panel_Right = new System.Windows.Forms.Panel();
            this.panel_Lefl.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 389);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel_Lefl
            // 
            this.panel_Lefl.Controls.Add(this.splitter1);
            this.panel_Lefl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Lefl.Location = new System.Drawing.Point(0, 0);
            this.panel_Lefl.Name = "panel_Lefl";
            this.panel_Lefl.Size = new System.Drawing.Size(401, 389);
            this.panel_Lefl.TabIndex = 0;
            this.panel_Lefl.Resize += new System.EventHandler(this.panel_Lefl_Resize);
            // 
            // panel_Right
            // 
            this.panel_Right.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_Right.Location = new System.Drawing.Point(401, 0);
            this.panel_Right.Name = "panel_Right";
            this.panel_Right.Size = new System.Drawing.Size(224, 389);
            this.panel_Right.TabIndex = 1;
            // 
            // SocketForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 389);
            this.Controls.Add(this.panel_Lefl);
            this.Controls.Add(this.panel_Right);
            this.Name = "SocketForm";
            this.Text = "SocketForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SocketForm_FormClosed);
            this.Resize += new System.EventHandler(this.SocketForm_Resize);
            this.panel_Lefl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel_Lefl;
        private System.Windows.Forms.Panel panel_Right;
    }
}