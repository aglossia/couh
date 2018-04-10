namespace couh
{
    partial class couh
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(couh));
            this.btnToHide = new System.Windows.Forms.Button();
            this.btnToShow = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvShow = new System.Windows.Forms.DataGridView();
            this.columnShowName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnShowDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvHide = new System.Windows.Forms.DataGridView();
            this.columnHideName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHideDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelVer = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHide)).BeginInit();
            this.SuspendLayout();
            // 
            // btnToHide
            // 
            this.btnToHide.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnToHide.Location = new System.Drawing.Point(80, 212);
            this.btnToHide.Name = "btnToHide";
            this.btnToHide.Size = new System.Drawing.Size(109, 39);
            this.btnToHide.TabIndex = 2;
            this.btnToHide.Text = "↓";
            this.btnToHide.UseVisualStyleBackColor = true;
            this.btnToHide.Click += new System.EventHandler(this.btnToHide_Click);
            // 
            // btnToShow
            // 
            this.btnToShow.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnToShow.Location = new System.Drawing.Point(534, 214);
            this.btnToShow.Name = "btnToShow";
            this.btnToShow.Size = new System.Drawing.Size(109, 39);
            this.btnToShow.TabIndex = 3;
            this.btnToShow.Text = "↑";
            this.btnToShow.UseVisualStyleBackColor = true;
            this.btnToShow.Click += new System.EventHandler(this.btnToShow_Click);
            // 
            // btnApply
            // 
            this.btnApply.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnApply.Location = new System.Drawing.Point(281, 383);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnClose.Location = new System.Drawing.Point(405, 383);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgvShow
            // 
            this.dgvShow.AllowUserToAddRows = false;
            this.dgvShow.AllowUserToDeleteRows = false;
            this.dgvShow.AllowUserToResizeColumns = false;
            this.dgvShow.AllowUserToResizeRows = false;
            this.dgvShow.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvShow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvShow.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnShowName,
            this.columnShowDate});
            this.dgvShow.Location = new System.Drawing.Point(12, 12);
            this.dgvShow.Name = "dgvShow";
            this.dgvShow.ReadOnly = true;
            this.dgvShow.RowHeadersVisible = false;
            this.dgvShow.RowHeadersWidth = 25;
            this.dgvShow.RowTemplate.Height = 21;
            this.dgvShow.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvShow.Size = new System.Drawing.Size(731, 194);
            this.dgvShow.TabIndex = 7;
            this.dgvShow.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvShow_CellDoubleClick);
            this.dgvShow.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvShow_ColumnHeaderMouseClick);
            // 
            // columnShowName
            // 
            this.columnShowName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnShowName.HeaderText = "Name";
            this.columnShowName.Name = "columnShowName";
            this.columnShowName.ReadOnly = true;
            this.columnShowName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // columnShowDate
            // 
            this.columnShowDate.HeaderText = "Date";
            this.columnShowDate.Name = "columnShowDate";
            this.columnShowDate.ReadOnly = true;
            // 
            // dgvHide
            // 
            this.dgvHide.AllowUserToAddRows = false;
            this.dgvHide.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHide.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnHideName,
            this.columnHideDate});
            this.dgvHide.Location = new System.Drawing.Point(12, 259);
            this.dgvHide.Name = "dgvHide";
            this.dgvHide.ReadOnly = true;
            this.dgvHide.RowHeadersVisible = false;
            this.dgvHide.RowHeadersWidth = 25;
            this.dgvHide.RowTemplate.Height = 21;
            this.dgvHide.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHide.Size = new System.Drawing.Size(731, 118);
            this.dgvHide.TabIndex = 8;
            // 
            // columnHideName
            // 
            this.columnHideName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnHideName.HeaderText = "Name";
            this.columnHideName.Name = "columnHideName";
            this.columnHideName.ReadOnly = true;
            // 
            // columnHideDate
            // 
            this.columnHideDate.HeaderText = "Date";
            this.columnHideDate.Name = "columnHideDate";
            this.columnHideDate.ReadOnly = true;
            // 
            // labelVer
            // 
            this.labelVer.AutoSize = true;
            this.labelVer.Location = new System.Drawing.Point(698, 394);
            this.labelVer.Name = "labelVer";
            this.labelVer.Size = new System.Drawing.Size(47, 12);
            this.labelVer.TabIndex = 9;
            this.labelVer.Text = "Ver -.-";
            // 
            // couh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 418);
            this.Controls.Add(this.labelVer);
            this.Controls.Add(this.dgvHide);
            this.Controls.Add(this.dgvShow);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnToShow);
            this.Controls.Add(this.btnToHide);
            this.Font = new System.Drawing.Font("MeiryoKe_Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "couh";
            this.Text = "couh";
            this.Shown += new System.EventHandler(this.couh_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvShow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHide)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnToHide;
        private System.Windows.Forms.Button btnToShow;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvShow;
        private System.Windows.Forms.DataGridView dgvHide;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnHideName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnHideDate;
        private System.Windows.Forms.Label labelVer;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnShowName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnShowDate;
    }
}

