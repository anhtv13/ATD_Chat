namespace ATDChatClient.GUI
{
    partial class Main
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnTop = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnMenuTrip = new System.Windows.Forms.Panel();
            this.chatMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tưVấnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nhómToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cáNhânToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.pnTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.chatMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnTop
            // 
            this.pnTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.pnTop.Controls.Add(this.pictureBox1);
            this.pnTop.Controls.Add(this.label1);
            this.pnTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTop.Location = new System.Drawing.Point(0, 0);
            this.pnTop.Name = "pnTop";
            this.pnTop.Size = new System.Drawing.Size(250, 30);
            this.pnTop.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.GreenYellow;
            this.pictureBox1.Location = new System.Drawing.Point(220, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 30);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "DIRECT CHAT";
            // 
            // pnMenuTrip
            // 
            this.pnMenuTrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnMenuTrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.pnMenuTrip.Location = new System.Drawing.Point(0, 30);
            this.pnMenuTrip.Name = "pnMenuTrip";
            this.pnMenuTrip.Size = new System.Drawing.Size(250, 30);
            this.pnMenuTrip.TabIndex = 1;
            // 
            // chatMenuStrip
            // 
            this.chatMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tưVấnToolStripMenuItem,
            this.nhómToolStripMenuItem,
            this.cáNhânToolStripMenuItem});
            this.chatMenuStrip.Name = "chatMenuStrip";
            this.chatMenuStrip.Size = new System.Drawing.Size(119, 70);
            // 
            // tưVấnToolStripMenuItem
            // 
            this.tưVấnToolStripMenuItem.Name = "tưVấnToolStripMenuItem";
            this.tưVấnToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.tưVấnToolStripMenuItem.Text = "Tư vấn";
            // 
            // nhómToolStripMenuItem
            // 
            this.nhómToolStripMenuItem.Name = "nhómToolStripMenuItem";
            this.nhómToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.nhómToolStripMenuItem.Text = "Nhóm";
            // 
            // cáNhânToolStripMenuItem
            // 
            this.cáNhânToolStripMenuItem.Name = "cáNhânToolStripMenuItem";
            this.cáNhânToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.cáNhânToolStripMenuItem.Text = "Cá nhân";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(0, 60);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(250, 181);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.Location = new System.Drawing.Point(3, 241);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(199, 27);
            this.textBox1.TabIndex = 4;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(204, 241);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(43, 27);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "Gửi";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnTop);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.pnMenuTrip);
            this.Name = "Main";
            this.Size = new System.Drawing.Size(250, 270);
            this.pnTop.ResumeLayout(false);
            this.pnTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.chatMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnTop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnMenuTrip;
        private System.Windows.Forms.ContextMenuStrip chatMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tưVấnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nhómToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cáNhânToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
