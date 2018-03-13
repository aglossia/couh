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
            this.lstShow = new System.Windows.Forms.ListBox();
            this.lstHide = new System.Windows.Forms.ListBox();
            this.btnToHide = new System.Windows.Forms.Button();
            this.btnToShow = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstShow
            // 
            this.lstShow.BackColor = System.Drawing.SystemColors.Window;
            this.lstShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstShow.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstShow.FormattingEnabled = true;
            this.lstShow.HorizontalScrollbar = true;
            this.lstShow.ItemHeight = 12;
            this.lstShow.Location = new System.Drawing.Point(12, 12);
            this.lstShow.Name = "lstShow";
            this.lstShow.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstShow.Size = new System.Drawing.Size(462, 194);
            this.lstShow.TabIndex = 0;
            this.lstShow.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstShow_MouseDoubleClick);
            // 
            // lstHide
            // 
            this.lstHide.BackColor = System.Drawing.SystemColors.Window;
            this.lstHide.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstHide.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstHide.FormattingEnabled = true;
            this.lstHide.HorizontalScrollbar = true;
            this.lstHide.ItemHeight = 12;
            this.lstHide.Location = new System.Drawing.Point(12, 259);
            this.lstHide.Name = "lstHide";
            this.lstHide.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstHide.Size = new System.Drawing.Size(462, 86);
            this.lstHide.TabIndex = 1;
            this.lstHide.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstHide_MouseDoubleClick);
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
            this.btnToShow.Location = new System.Drawing.Point(314, 212);
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
            this.btnApply.Location = new System.Drawing.Point(80, 353);
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
            this.btnClose.Location = new System.Drawing.Point(314, 353);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // couh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 386);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnToShow);
            this.Controls.Add(this.btnToHide);
            this.Controls.Add(this.lstHide);
            this.Controls.Add(this.lstShow);
            this.Font = new System.Drawing.Font("MeiryoKe_Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "couh";
            this.Text = "couh";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstShow;
        private System.Windows.Forms.ListBox lstHide;
        private System.Windows.Forms.Button btnToHide;
        private System.Windows.Forms.Button btnToShow;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnClose;
    }
}

