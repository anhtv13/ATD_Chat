namespace ATDChatClient.GUI
{
    partial class ChatForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbChat = new System.Windows.Forms.TextBox();
            this.tbAddMember = new System.Windows.Forms.TextBox();
            this.lbMemberCount = new System.Windows.Forms.Label();
            this.dgvChat = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.tbGroupName = new System.Windows.Forms.TextBox();
            this.cbPrivateGroup = new System.Windows.Forms.CheckBox();
            this.pnTop = new System.Windows.Forms.Panel();
            this.ucMemberList = new ATDChatClient.GUI.MemberListControl();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChat)).BeginInit();
            this.pnTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(214, 213);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(54, 21);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // tbChat
            // 
            this.tbChat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbChat.Location = new System.Drawing.Point(0, 213);
            this.tbChat.Name = "tbChat";
            this.tbChat.Size = new System.Drawing.Size(210, 20);
            this.tbChat.TabIndex = 5;
            this.tbChat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbChat_KeyDown);
            // 
            // tbAddMember
            // 
            this.tbAddMember.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAddMember.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbAddMember.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbAddMember.Enabled = false;
            this.tbAddMember.Location = new System.Drawing.Point(164, 3);
            this.tbAddMember.Name = "tbAddMember";
            this.tbAddMember.Size = new System.Drawing.Size(100, 20);
            this.tbAddMember.TabIndex = 7;
            this.tbAddMember.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbAddMember_KeyDown);
            this.tbAddMember.MouseHover += new System.EventHandler(this.tbAddMember_MouseHover);
            // 
            // lbMemberCount
            // 
            this.lbMemberCount.AutoSize = true;
            this.lbMemberCount.Location = new System.Drawing.Point(3, 33);
            this.lbMemberCount.Name = "lbMemberCount";
            this.lbMemberCount.Size = new System.Drawing.Size(49, 13);
            this.lbMemberCount.TabIndex = 8;
            this.lbMemberCount.Text = "members";
            this.lbMemberCount.MouseHover += new System.EventHandler(this.lbMemberCount_MouseHover);
            // 
            // dgvChat
            // 
            this.dgvChat.AllowUserToAddRows = false;
            this.dgvChat.AllowUserToDeleteRows = false;
            this.dgvChat.AllowUserToResizeColumns = false;
            this.dgvChat.AllowUserToResizeRows = false;
            this.dgvChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvChat.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvChat.BackgroundColor = System.Drawing.Color.White;
            this.dgvChat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvChat.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvChat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChat.ColumnHeadersVisible = false;
            this.dgvChat.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvChat.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvChat.Location = new System.Drawing.Point(0, 57);
            this.dgvChat.Name = "dgvChat";
            this.dgvChat.ReadOnly = true;
            this.dgvChat.RowHeadersVisible = false;
            this.dgvChat.Size = new System.Drawing.Size(268, 152);
            this.dgvChat.TabIndex = 26;
            // 
            // Column1
            // 
            this.Column1.FillWeight = 76.14214F;
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 80;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.FillWeight = 111.9289F;
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.FillWeight = 111.9289F;
            this.Column3.HeaderText = "Column3";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 80;
            // 
            // tbGroupName
            // 
            this.tbGroupName.Enabled = false;
            this.tbGroupName.Location = new System.Drawing.Point(3, 3);
            this.tbGroupName.Name = "tbGroupName";
            this.tbGroupName.Size = new System.Drawing.Size(145, 20);
            this.tbGroupName.TabIndex = 27;
            this.tbGroupName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbGroupName_KeyDown);
            this.tbGroupName.MouseHover += new System.EventHandler(this.tbGroupName_MouseHover);
            // 
            // cbPrivateGroup
            // 
            this.cbPrivateGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPrivateGroup.AutoSize = true;
            this.cbPrivateGroup.Location = new System.Drawing.Point(167, 32);
            this.cbPrivateGroup.Name = "cbPrivateGroup";
            this.cbPrivateGroup.Size = new System.Drawing.Size(91, 17);
            this.cbPrivateGroup.TabIndex = 28;
            this.cbPrivateGroup.Text = "Private Group";
            this.cbPrivateGroup.UseVisualStyleBackColor = true;
            this.cbPrivateGroup.Visible = false;
            // 
            // pnTop
            // 
            this.pnTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnTop.Controls.Add(this.tbGroupName);
            this.pnTop.Controls.Add(this.cbPrivateGroup);
            this.pnTop.Controls.Add(this.tbAddMember);
            this.pnTop.Controls.Add(this.lbMemberCount);
            this.pnTop.Location = new System.Drawing.Point(1, 2);
            this.pnTop.Name = "pnTop";
            this.pnTop.Size = new System.Drawing.Size(267, 50);
            this.pnTop.TabIndex = 29;
            this.pnTop.Visible = false;
            // 
            // ucMemberList
            // 
            this.ucMemberList.Location = new System.Drawing.Point(1, 49);
            this.ucMemberList.Name = "ucMemberList";
            this.ucMemberList.Size = new System.Drawing.Size(145, 100);
            this.ucMemberList.TabIndex = 9;
            this.ucMemberList.Visible = false;
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(269, 236);
            this.Controls.Add(this.ucMemberList);
            this.Controls.Add(this.pnTop);
            this.Controls.Add(this.dgvChat);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbChat);
            this.MinimumSize = new System.Drawing.Size(285, 274);
            this.Name = "ChatForm";
            this.Text = "Chat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChat)).EndInit();
            this.pnTop.ResumeLayout(false);
            this.pnTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox tbChat;
        private System.Windows.Forms.TextBox tbAddMember;
        private System.Windows.Forms.Label lbMemberCount;
        private MemberListControl ucMemberList;
        private System.Windows.Forms.DataGridView dgvChat;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.TextBox tbGroupName;
        private System.Windows.Forms.CheckBox cbPrivateGroup;
        private System.Windows.Forms.Panel pnTop;
    }
}