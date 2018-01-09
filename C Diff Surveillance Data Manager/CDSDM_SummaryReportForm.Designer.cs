namespace C_Diff_Surveillance_Data_Manager
{
    partial class CDSDM_SummaryReportForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RBIncludeAll = new System.Windows.Forms.RadioButton();
            this.RBCommonOnly = new System.Windows.Forms.RadioButton();
            this.exportButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveReportDialog = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RBCommonOnly);
            this.groupBox1.Controls.Add(this.RBIncludeAll);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 68);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Report Formatting";
            // 
            // RBIncludeAll
            // 
            this.RBIncludeAll.AutoSize = true;
            this.RBIncludeAll.Checked = true;
            this.RBIncludeAll.Location = new System.Drawing.Point(6, 19);
            this.RBIncludeAll.Name = "RBIncludeAll";
            this.RBIncludeAll.Size = new System.Drawing.Size(101, 17);
            this.RBIncludeAll.TabIndex = 1;
            this.RBIncludeAll.TabStop = true;
            this.RBIncludeAll.Text = "Include All Units";
            this.RBIncludeAll.UseVisualStyleBackColor = true;
            // 
            // RBCommonOnly
            // 
            this.RBCommonOnly.AutoSize = true;
            this.RBCommonOnly.Location = new System.Drawing.Point(6, 42);
            this.RBCommonOnly.Name = "RBCommonOnly";
            this.RBCommonOnly.Size = new System.Drawing.Size(184, 17);
            this.RBCommonOnly.TabIndex = 2;
            this.RBCommonOnly.Text = "Common Units Only (>50 patients)";
            this.RBCommonOnly.UseVisualStyleBackColor = true;
            // 
            // exportButton
            // 
            this.exportButton.Location = new System.Drawing.Point(12, 86);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(90, 23);
            this.exportButton.TabIndex = 1;
            this.exportButton.Text = "Export Report";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(114, 86);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(90, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveReportDialog
            // 
            this.saveReportDialog.DefaultExt = "csv";
            this.saveReportDialog.Filter = "CSV Files|*.csv";
            this.saveReportDialog.Title = "Save Summary Report As...";
            // 
            // CDSDM_MasterReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(216, 126);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.groupBox1);
            this.Name = "CDSDM_MasterReportForm";
            this.Text = "Summary Report";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RBCommonOnly;
        private System.Windows.Forms.RadioButton RBIncludeAll;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.SaveFileDialog saveReportDialog;
    }
}