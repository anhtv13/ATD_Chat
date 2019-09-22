namespace ATDChatClient
{
    partial class MainForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSearchFriend = new System.Windows.Forms.Button();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.dgvFriend = new System.Windows.Forms.DataGridView();
            this.StatusColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.CustomerIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnSearchGroup = new System.Windows.Forms.Button();
            this.tbCreateGroup = new System.Windows.Forms.TextBox();
            this.dgvGroup = new System.Windows.Forms.DataGridView();
            this.GroupIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MembersColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnBroadcast = new System.Windows.Forms.Button();
            this.cbBroadcast = new System.Windows.Forms.ComboBox();
            this.GroupMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.chatGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leaveGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FriendMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.chatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFriend)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroup)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.GroupMenuStrip.SuspendLayout();
            this.FriendMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.ItemSize = new System.Drawing.Size(42, 30);
            this.tabControl.Location = new System.Drawing.Point(3, 1);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(279, 261);
            this.tabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSearchFriend);
            this.tabPage1.Controls.Add(this.cbStatus);
            this.tabPage1.Controls.Add(this.dgvFriend);
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(271, 223);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Friend";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSearchFriend
            // 
            this.btnSearchFriend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchFriend.Location = new System.Drawing.Point(213, 194);
            this.btnSearchFriend.Name = "btnSearchFriend";
            this.btnSearchFriend.Size = new System.Drawing.Size(52, 23);
            this.btnSearchFriend.TabIndex = 4;
            this.btnSearchFriend.Text = "Search";
            this.btnSearchFriend.UseVisualStyleBackColor = true;
            this.btnSearchFriend.Click += new System.EventHandler(this.btnSearchFriend_Click);
            // 
            // cbStatus
            // 
            this.cbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Items.AddRange(new object[] {
            "Available",
            "Busy",
            "Away",
            "Invisible"});
            this.cbStatus.Location = new System.Drawing.Point(6, 196);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(121, 21);
            this.cbStatus.TabIndex = 2;
            // 
            // dgvFriend
            // 
            this.dgvFriend.AllowUserToAddRows = false;
            this.dgvFriend.AllowUserToDeleteRows = false;
            this.dgvFriend.AllowUserToResizeColumns = false;
            this.dgvFriend.AllowUserToResizeRows = false;
            this.dgvFriend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvFriend.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvFriend.BackgroundColor = System.Drawing.Color.White;
            this.dgvFriend.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvFriend.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvFriend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFriend.ColumnHeadersVisible = false;
            this.dgvFriend.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StatusColumn,
            this.CustomerIdColumn});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFriend.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvFriend.GridColor = System.Drawing.Color.Black;
            this.dgvFriend.Location = new System.Drawing.Point(3, 4);
            this.dgvFriend.MultiSelect = false;
            this.dgvFriend.Name = "dgvFriend";
            this.dgvFriend.ReadOnly = true;
            this.dgvFriend.RowHeadersVisible = false;
            this.dgvFriend.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFriend.ShowCellToolTips = false;
            this.dgvFriend.Size = new System.Drawing.Size(265, 187);
            this.dgvFriend.TabIndex = 0;
            this.dgvFriend.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvFriendList_MouseClick);
            this.dgvFriend.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvFriendList_MouseDoubleClick);
            // 
            // StatusColumn
            // 
            this.StatusColumn.HeaderText = "Status";
            this.StatusColumn.Name = "StatusColumn";
            this.StatusColumn.ReadOnly = true;
            this.StatusColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.StatusColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.StatusColumn.Width = 25;
            // 
            // CustomerIdColumn
            // 
            this.CustomerIdColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CustomerIdColumn.HeaderText = "ID";
            this.CustomerIdColumn.Name = "CustomerIdColumn";
            this.CustomerIdColumn.ReadOnly = true;
            this.CustomerIdColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnSearchGroup);
            this.tabPage2.Controls.Add(this.tbCreateGroup);
            this.tabPage2.Controls.Add(this.dgvGroup);
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(271, 223);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Group";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnSearchGroup
            // 
            this.btnSearchGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchGroup.Location = new System.Drawing.Point(213, 194);
            this.btnSearchGroup.Name = "btnSearchGroup";
            this.btnSearchGroup.Size = new System.Drawing.Size(52, 23);
            this.btnSearchGroup.TabIndex = 3;
            this.btnSearchGroup.Text = "Search";
            this.btnSearchGroup.UseVisualStyleBackColor = true;
            this.btnSearchGroup.Click += new System.EventHandler(this.btnSearchGroup_Click);
            // 
            // tbCreateGroup
            // 
            this.tbCreateGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tbCreateGroup.Location = new System.Drawing.Point(6, 198);
            this.tbCreateGroup.Name = "tbCreateGroup";
            this.tbCreateGroup.Size = new System.Drawing.Size(89, 20);
            this.tbCreateGroup.TabIndex = 2;
            this.tbCreateGroup.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbCreateGroup_KeyDown);
            // 
            // dgvGroup
            // 
            this.dgvGroup.AllowUserToAddRows = false;
            this.dgvGroup.AllowUserToDeleteRows = false;
            this.dgvGroup.AllowUserToOrderColumns = true;
            this.dgvGroup.AllowUserToResizeColumns = false;
            this.dgvGroup.AllowUserToResizeRows = false;
            this.dgvGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvGroup.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvGroup.BackgroundColor = System.Drawing.Color.White;
            this.dgvGroup.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGroup.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGroup.ColumnHeadersVisible = false;
            this.dgvGroup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GroupIdColumn,
            this.GroupNameColumn,
            this.MembersColumn});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGroup.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvGroup.Location = new System.Drawing.Point(3, 4);
            this.dgvGroup.MultiSelect = false;
            this.dgvGroup.Name = "dgvGroup";
            this.dgvGroup.ReadOnly = true;
            this.dgvGroup.RowHeadersVisible = false;
            this.dgvGroup.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGroup.Size = new System.Drawing.Size(265, 187);
            this.dgvGroup.TabIndex = 0;
            this.dgvGroup.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvGroup_CellBeginEdit);
            this.dgvGroup.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGroup_CellEndEdit);
            this.dgvGroup.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvGroup_MouseClick);
            this.dgvGroup.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvGroupList_MouseDoubleClick);
            // 
            // GroupIdColumn
            // 
            this.GroupIdColumn.HeaderText = "GroupId";
            this.GroupIdColumn.Name = "GroupIdColumn";
            this.GroupIdColumn.ReadOnly = true;
            this.GroupIdColumn.Width = 80;
            // 
            // GroupNameColumn
            // 
            this.GroupNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.GroupNameColumn.FillWeight = 153.8462F;
            this.GroupNameColumn.HeaderText = "GroupName";
            this.GroupNameColumn.Name = "GroupNameColumn";
            this.GroupNameColumn.ReadOnly = true;
            // 
            // MembersColumn
            // 
            this.MembersColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MembersColumn.FillWeight = 46.15384F;
            this.MembersColumn.HeaderText = "Members";
            this.MembersColumn.Name = "MembersColumn";
            this.MembersColumn.ReadOnly = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnBroadcast);
            this.tabPage3.Controls.Add(this.cbBroadcast);
            this.tabPage3.Location = new System.Drawing.Point(4, 34);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(271, 223);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Admin";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnBroadcast
            // 
            this.btnBroadcast.Location = new System.Drawing.Point(153, 2);
            this.btnBroadcast.Name = "btnBroadcast";
            this.btnBroadcast.Size = new System.Drawing.Size(75, 22);
            this.btnBroadcast.TabIndex = 1;
            this.btnBroadcast.Text = "Broadcast";
            this.btnBroadcast.UseVisualStyleBackColor = true;
            this.btnBroadcast.Click += new System.EventHandler(this.btnBroadcast_Click);
            // 
            // cbBroadcast
            // 
            this.cbBroadcast.FormattingEnabled = true;
            this.cbBroadcast.Items.AddRange(new object[] {
            "Admin",
            "User",
            "Vip"});
            this.cbBroadcast.Location = new System.Drawing.Point(5, 3);
            this.cbBroadcast.Name = "cbBroadcast";
            this.cbBroadcast.Size = new System.Drawing.Size(142, 21);
            this.cbBroadcast.TabIndex = 0;
            this.cbBroadcast.Text = "User";
            // 
            // GroupMenuStrip
            // 
            this.GroupMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chatGroupToolStripMenuItem,
            this.deleteGroupToolStripMenuItem,
            this.leaveGroupToolStripMenuItem});
            this.GroupMenuStrip.Name = "GroupMenuStrip";
            this.GroupMenuStrip.Size = new System.Drawing.Size(143, 70);
            // 
            // chatGroupToolStripMenuItem
            // 
            this.chatGroupToolStripMenuItem.Name = "chatGroupToolStripMenuItem";
            this.chatGroupToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.chatGroupToolStripMenuItem.Text = "Chat group";
            this.chatGroupToolStripMenuItem.Click += new System.EventHandler(this.chatGroupToolStripMenuItem_Click);
            // 
            // deleteGroupToolStripMenuItem
            // 
            this.deleteGroupToolStripMenuItem.Name = "deleteGroupToolStripMenuItem";
            this.deleteGroupToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.deleteGroupToolStripMenuItem.Text = "Delete group";
            this.deleteGroupToolStripMenuItem.Visible = false;
            this.deleteGroupToolStripMenuItem.Click += new System.EventHandler(this.deleteGroupToolStripMenuItem_Click);
            // 
            // leaveGroupToolStripMenuItem
            // 
            this.leaveGroupToolStripMenuItem.Name = "leaveGroupToolStripMenuItem";
            this.leaveGroupToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.leaveGroupToolStripMenuItem.Text = "Leave Group";
            this.leaveGroupToolStripMenuItem.Click += new System.EventHandler(this.leaveGroupToolStripMenuItem_Click);
            // 
            // FriendMenuStrip
            // 
            this.FriendMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chatToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.FriendMenuStrip.Name = "menuStrip";
            this.FriendMenuStrip.Size = new System.Drawing.Size(108, 48);
            // 
            // chatToolStripMenuItem
            // 
            this.chatToolStripMenuItem.Name = "chatToolStripMenuItem";
            this.chatToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.chatToolStripMenuItem.Text = "Chat";
            this.chatToolStripMenuItem.Click += new System.EventHandler(this.chatToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.tabControl);
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFriend)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroup)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.GroupMenuStrip.ResumeLayout(false);
            this.FriendMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvFriend;
        private System.Windows.Forms.ContextMenuStrip FriendMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem chatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.DataGridView dgvGroup;
        private System.Windows.Forms.ContextMenuStrip GroupMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem chatGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leaveGroupToolStripMenuItem;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.ToolStripMenuItem deleteGroupToolStripMenuItem;
        private System.Windows.Forms.Button btnSearchFriend;
        private System.Windows.Forms.TextBox tbCreateGroup;
        private System.Windows.Forms.Button btnSearchGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MembersColumn;
        private System.Windows.Forms.DataGridViewImageColumn StatusColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerIdColumn;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnBroadcast;
        private System.Windows.Forms.ComboBox cbBroadcast;
    }
}