namespace ATDChatClient.GUI
{
    partial class SearchGroup
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
            this.dgvSearchGroup = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.GroupMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.joinGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchGroup)).BeginInit();
            this.GroupMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSearchGroup
            // 
            this.dgvSearchGroup.AllowUserToAddRows = false;
            this.dgvSearchGroup.AllowUserToDeleteRows = false;
            this.dgvSearchGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSearchGroup.BackgroundColor = System.Drawing.Color.White;
            this.dgvSearchGroup.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvSearchGroup.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvSearchGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearchGroup.ColumnHeadersVisible = false;
            this.dgvSearchGroup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dgvSearchGroup.Location = new System.Drawing.Point(12, 36);
            this.dgvSearchGroup.Name = "dgvSearchGroup";
            this.dgvSearchGroup.RowHeadersVisible = false;
            this.dgvSearchGroup.Size = new System.Drawing.Size(210, 216);
            this.dgvSearchGroup.TabIndex = 0;
            this.dgvSearchGroup.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvSearchGroup_MouseClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.Width = 80;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Column3";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.Width = 20;
            // 
            // tbSearch
            // 
            this.tbSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbSearch.Location = new System.Drawing.Point(12, 10);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(125, 20);
            this.tbSearch.TabIndex = 2;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(143, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 24);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // GroupMenuStrip
            // 
            this.GroupMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.joinGroupToolStripMenuItem});
            this.GroupMenuStrip.Name = "GroupMenuStrip";
            this.GroupMenuStrip.Size = new System.Drawing.Size(132, 26);
            // 
            // joinGroupToolStripMenuItem
            // 
            this.joinGroupToolStripMenuItem.Name = "joinGroupToolStripMenuItem";
            this.joinGroupToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.joinGroupToolStripMenuItem.Text = "Join Group";
            this.joinGroupToolStripMenuItem.Click += new System.EventHandler(this.joinGroupToolStripMenuItem_Click);
            // 
            // SearchGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 262);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.dgvSearchGroup);
            this.MinimumSize = new System.Drawing.Size(250, 300);
            this.Name = "SearchGroup";
            this.Text = "Search Group";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchGroup)).EndInit();
            this.GroupMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSearchGroup;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ContextMenuStrip GroupMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem joinGroupToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}