namespace ATDChatClient.GUI
{
    partial class MemberListControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvMemberList = new System.Windows.Forms.DataGridView();
            this.ColumnCustomerId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnRemoveCustomer = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColumnAdmin = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMemberList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMemberList
            // 
            this.dgvMemberList.AllowUserToAddRows = false;
            this.dgvMemberList.AllowUserToDeleteRows = false;
            this.dgvMemberList.AllowUserToResizeColumns = false;
            this.dgvMemberList.AllowUserToResizeRows = false;
            this.dgvMemberList.BackgroundColor = System.Drawing.Color.White;
            this.dgvMemberList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvMemberList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMemberList.ColumnHeadersVisible = false;
            this.dgvMemberList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCustomerId,
            this.ColumnRemoveCustomer,
            this.ColumnAdmin});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMemberList.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMemberList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMemberList.Location = new System.Drawing.Point(0, 0);
            this.dgvMemberList.Name = "dgvMemberList";
            this.dgvMemberList.RowHeadersVisible = false;
            this.dgvMemberList.Size = new System.Drawing.Size(145, 100);
            this.dgvMemberList.TabIndex = 0;
            this.dgvMemberList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMemberList_CellClick);
            this.dgvMemberList.MouseLeave += new System.EventHandler(this.dgvMemberList_MouseLeave);
            // 
            // ColumnCustomerId
            // 
            this.ColumnCustomerId.HeaderText = "ColumnCustomerId";
            this.ColumnCustomerId.Name = "ColumnCustomerId";
            // 
            // ColumnRemoveCustomer
            // 
            this.ColumnRemoveCustomer.HeaderText = "ColumnRemoveCustomer";
            this.ColumnRemoveCustomer.Name = "ColumnRemoveCustomer";
            this.ColumnRemoveCustomer.Width = 20;
            // 
            // ColumnAdmin
            // 
            this.ColumnAdmin.HeaderText = "admin";
            this.ColumnAdmin.Name = "ColumnAdmin";
            this.ColumnAdmin.Width = 20;
            // 
            // MemberListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvMemberList);
            this.Name = "MemberListControl";
            this.Size = new System.Drawing.Size(145, 100);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMemberList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMemberList;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCustomerId;
        private System.Windows.Forms.DataGridViewImageColumn ColumnRemoveCustomer;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnAdmin;
    }
}
