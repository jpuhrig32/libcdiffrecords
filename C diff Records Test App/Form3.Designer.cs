namespace C_diff_Records_Test_App
{
    partial class Form3
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
            System.Windows.Forms.Button survDataBrowse;
            this.survDataDialog = new System.Windows.Forms.OpenFileDialog();
            this.naatDataDialog = new System.Windows.Forms.OpenFileDialog();
            this.reportSaveDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.survDataText = new System.Windows.Forms.TextBox();
            this.saveToText = new System.Windows.Forms.TextBox();
            this.naatDataText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.naatDataBrowse = new System.Windows.Forms.Button();
            this.saveToBrowse = new System.Windows.Forms.Button();
            this.genReportButton = new System.Windows.Forms.Button();
            survDataBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // survDataDialog
            // 
            this.survDataDialog.DefaultExt = "txt";
            this.survDataDialog.FileName = "Surveillance Data";
            this.survDataDialog.Title = "Select Surveillance Data";
            // 
            // naatDataDialog
            // 
            this.naatDataDialog.DefaultExt = "txt";
            this.naatDataDialog.FileName = "openFileDialog2";
            this.naatDataDialog.Title = "Select NAAT Data";
            // 
            // reportSaveDialog
            // 
            this.reportSaveDialog.Description = "Select folder to save reports";
            // 
            // survDataText
            // 
            this.survDataText.Location = new System.Drawing.Point(41, 90);
            this.survDataText.Name = "survDataText";
            this.survDataText.Size = new System.Drawing.Size(262, 20);
            this.survDataText.TabIndex = 0;
            // 
            // saveToText
            // 
            this.saveToText.Location = new System.Drawing.Point(41, 212);
            this.saveToText.Name = "saveToText";
            this.saveToText.Size = new System.Drawing.Size(262, 20);
            this.saveToText.TabIndex = 2;
            // 
            // naatDataText
            // 
            this.naatDataText.Location = new System.Drawing.Point(41, 150);
            this.naatDataText.Name = "naatDataText";
            this.naatDataText.Size = new System.Drawing.Size(262, 20);
            this.naatDataText.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 196);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Folder To Save Reports";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "NAAT Datafile (Tab delimited)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Surveillance Data (Tab delimited)";
            // 
            // survDataBrowse
            // 
            survDataBrowse.Location = new System.Drawing.Point(319, 90);
            survDataBrowse.Name = "survDataBrowse";
            survDataBrowse.Size = new System.Drawing.Size(86, 20);
            survDataBrowse.TabIndex = 7;
            survDataBrowse.Text = "Browse...";
            survDataBrowse.UseVisualStyleBackColor = true;
            survDataBrowse.Click += new System.EventHandler(this.survDataBrowse_Click);
            // 
            // naatDataBrowse
            // 
            this.naatDataBrowse.Location = new System.Drawing.Point(319, 149);
            this.naatDataBrowse.Name = "naatDataBrowse";
            this.naatDataBrowse.Size = new System.Drawing.Size(86, 20);
            this.naatDataBrowse.TabIndex = 8;
            this.naatDataBrowse.Text = "Browse...";
            this.naatDataBrowse.UseVisualStyleBackColor = true;
            this.naatDataBrowse.Click += new System.EventHandler(this.naatDataBrowse_Click);
            // 
            // saveToBrowse
            // 
            this.saveToBrowse.Location = new System.Drawing.Point(319, 212);
            this.saveToBrowse.Name = "saveToBrowse";
            this.saveToBrowse.Size = new System.Drawing.Size(86, 20);
            this.saveToBrowse.TabIndex = 9;
            this.saveToBrowse.Text = "Browse...";
            this.saveToBrowse.UseVisualStyleBackColor = true;
            this.saveToBrowse.Click += new System.EventHandler(this.saveToBrowse_Click);
            // 
            // genReportButton
            // 
            this.genReportButton.Location = new System.Drawing.Point(41, 269);
            this.genReportButton.Name = "genReportButton";
            this.genReportButton.Size = new System.Drawing.Size(75, 23);
            this.genReportButton.TabIndex = 10;
            this.genReportButton.Text = "Submit";
            this.genReportButton.UseVisualStyleBackColor = true;
            this.genReportButton.Click += new System.EventHandler(this.genReportButton_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 350);
            this.Controls.Add(this.genReportButton);
            this.Controls.Add(this.saveToBrowse);
            this.Controls.Add(this.naatDataBrowse);
            this.Controls.Add(survDataBrowse);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.naatDataText);
            this.Controls.Add(this.saveToText);
            this.Controls.Add(this.survDataText);
            this.Name = "Form3";
            this.Text = "Surveillance NAAT comparison reports";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog survDataDialog;
        private System.Windows.Forms.OpenFileDialog naatDataDialog;
        private System.Windows.Forms.FolderBrowserDialog reportSaveDialog;
        private System.Windows.Forms.TextBox survDataText;
        private System.Windows.Forms.TextBox saveToText;
        private System.Windows.Forms.TextBox naatDataText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button naatDataBrowse;
        private System.Windows.Forms.Button saveToBrowse;
        private System.Windows.Forms.Button genReportButton;
    }
}