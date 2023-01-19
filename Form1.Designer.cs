namespace SwimSys
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
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
            this.components = new System.ComponentModel.Container();
            this.btnUpdateGamerecord = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBxFolder = new System.Windows.Forms.TextBox();
            this.btnFolderSelect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbxDbContents = new System.Windows.Forms.ListBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnQuit = new System.Windows.Forms.Button();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.btnWriteRecord = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnUpdateGamerecord
            // 
            this.btnUpdateGamerecord.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnUpdateGamerecord.Location = new System.Drawing.Point(873, 215);
            this.btnUpdateGamerecord.Name = "btnUpdateGamerecord";
            this.btnUpdateGamerecord.Size = new System.Drawing.Size(315, 85);
            this.btnUpdateGamerecord.TabIndex = 0;
            this.btnUpdateGamerecord.Text = "大会記録更新";
            this.btnUpdateGamerecord.UseVisualStyleBackColor = true;
            this.btnUpdateGamerecord.Click += new System.EventHandler(this.btnUpdateGamerecord_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(537, 267);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 60);
            this.label1.TabIndex = 1;
            // 
            // txtBxFolder
            // 
            this.txtBxFolder.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBxFolder.Location = new System.Drawing.Point(367, 88);
            this.txtBxFolder.Name = "txtBxFolder";
            this.txtBxFolder.Size = new System.Drawing.Size(1165, 43);
            this.txtBxFolder.TabIndex = 2;
            // 
            // btnFolderSelect
            // 
            this.btnFolderSelect.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnFolderSelect.Location = new System.Drawing.Point(442, 215);
            this.btnFolderSelect.Name = "btnFolderSelect";
            this.btnFolderSelect.Size = new System.Drawing.Size(283, 85);
            this.btnFolderSelect.TabIndex = 3;
            this.btnFolderSelect.Text = "パス設定";
            this.btnFolderSelect.UseVisualStyleBackColor = true;
            this.btnFolderSelect.Click += new System.EventHandler(this.btnFolderSelect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(29, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(282, 42);
            this.label2.TabIndex = 4;
            this.label2.Text = "データベースパス";
            // 
            // lbxDbContents
            // 
            this.lbxDbContents.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbxDbContents.FormattingEnabled = true;
            this.lbxDbContents.ItemHeight = 30;
            this.lbxDbContents.Location = new System.Drawing.Point(12, 334);
            this.lbxDbContents.Name = "lbxDbContents";
            this.lbxDbContents.Size = new System.Drawing.Size(2100, 844);
            this.lbxDbContents.TabIndex = 6;
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(1935, 49);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(121, 61);
            this.btnQuit.TabIndex = 8;
            this.btnQuit.Text = "終了";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnWriteRecord
            // 
            this.btnWriteRecord.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnWriteRecord.Location = new System.Drawing.Point(1304, 221);
            this.btnWriteRecord.Name = "btnWriteRecord";
            this.btnWriteRecord.Size = new System.Drawing.Size(374, 78);
            this.btnWriteRecord.TabIndex = 9;
            this.btnWriteRecord.Text = "大会記録書き出し";
            this.btnWriteRecord.UseVisualStyleBackColor = true;
            this.btnWriteRecord.Click += new System.EventHandler(this.btnWriteRecord_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2116, 1195);
            this.Controls.Add(this.btnWriteRecord);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.lbxDbContents);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnFolderSelect);
            this.Controls.Add(this.txtBxFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnUpdateGamerecord);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpdateGamerecord;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBxFolder;
        private System.Windows.Forms.Button btnFolderSelect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lbxDbContents;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.Button btnWriteRecord;
    }
}

