namespace C_Diff_Surveillance_Data_Manager
{
    partial class CDSDM_IndexAdmitForm
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
            this.exportDataPointsRadioButton = new System.Windows.Forms.RadioButton();
            this.exportAdmDataRadioButton = new System.Windows.Forms.RadioButton();
            this.exportBothRadioButton = new System.Windows.Forms.RadioButton();
            this.exportFormatGroupBox = new System.Windows.Forms.GroupBox();
            this.exportDataButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveDataPointsDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveAdmissionsDialog = new System.Windows.Forms.SaveFileDialog();
            this.exportFormatGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // exportDataPointsRadioButton
            // 
            this.exportDataPointsRadioButton.AutoSize = true;
            this.exportDataPointsRadioButton.Checked = true;
            this.exportDataPointsRadioButton.Location = new System.Drawing.Point(6, 19);
            this.exportDataPointsRadioButton.Name = "exportDataPointsRadioButton";
            this.exportDataPointsRadioButton.Size = new System.Drawing.Size(128, 17);
            this.exportDataPointsRadioButton.TabIndex = 0;
            this.exportDataPointsRadioButton.TabStop = true;
            this.exportDataPointsRadioButton.Text = "Export As Data Points";
            this.exportDataPointsRadioButton.UseVisualStyleBackColor = true;
            // 
            // exportAdmDataRadioButton
            // 
            this.exportAdmDataRadioButton.AutoSize = true;
            this.exportAdmDataRadioButton.Location = new System.Drawing.Point(6, 42);
            this.exportAdmDataRadioButton.Name = "exportAdmDataRadioButton";
            this.exportAdmDataRadioButton.Size = new System.Drawing.Size(151, 17);
            this.exportAdmDataRadioButton.TabIndex = 1;
            this.exportAdmDataRadioButton.Text = "Export As Admissions Data";
            this.exportAdmDataRadioButton.UseVisualStyleBackColor = true;
            // 
            // exportBothRadioButton
            // 
            this.exportBothRadioButton.AutoSize = true;
            this.exportBothRadioButton.Location = new System.Drawing.Point(6, 65);
            this.exportBothRadioButton.Name = "exportBothRadioButton";
            this.exportBothRadioButton.Size = new System.Drawing.Size(80, 17);
            this.exportBothRadioButton.TabIndex = 2;
            this.exportBothRadioButton.Text = "Export Both";
            this.exportBothRadioButton.UseVisualStyleBackColor = true;
            // 
            // exportFormatGroupBox
            // 
            this.exportFormatGroupBox.Controls.Add(this.exportDataPointsRadioButton);
            this.exportFormatGroupBox.Controls.Add(this.exportAdmDataRadioButton);
            this.exportFormatGroupBox.Controls.Add(this.exportBothRadioButton);
            this.exportFormatGroupBox.Location = new System.Drawing.Point(12, 12);
            this.exportFormatGroupBox.Name = "exportFormatGroupBox";
            this.exportFormatGroupBox.Size = new System.Drawing.Size(221, 87);
            this.exportFormatGroupBox.TabIndex = 4;
            this.exportFormatGroupBox.TabStop = false;
            this.exportFormatGroupBox.Text = "Choose export format of index admissions";
            // 
            // exportDataButton
            // 
            this.exportDataButton.Location = new System.Drawing.Point(18, 130);
            this.exportDataButton.Name = "exportDataButton";
            this.exportDataButton.Size = new System.Drawing.Size(94, 23);
            this.exportDataButton.TabIndex = 5;
            this.exportDataButton.Text = "Export Data";
            this.exportDataButton.UseVisualStyleBackColor = true;
            this.exportDataButton.Click += new System.EventHandler(this.exportDataButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(156, 130);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveDataPointsDialog
            // 
            this.saveDataPointsDialog.DefaultExt = "csv";
            this.saveDataPointsDialog.Filter = "CSV Files|*.csv";
            this.saveDataPointsDialog.Title = "Save Index Admission Data Points As...";
            // 
            // saveAdmissionsDialog
            // 
            this.saveAdmissionsDialog.DefaultExt = "csv";
            this.saveAdmissionsDialog.Filter = "CSV Files|*.csv";
            this.saveAdmissionsDialog.Title = "Save Index Admissions Data As...";
            // 
            // CDSDM_IndexAdmitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 165);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.exportDataButton);
            this.Controls.Add(this.exportFormatGroupBox);
            this.Name = "CDSDM_IndexAdmitForm";
            this.Text = "Index Admissions";
            this.exportFormatGroupBox.ResumeLayout(false);
            this.exportFormatGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton exportDataPointsRadioButton;
        private System.Windows.Forms.RadioButton exportAdmDataRadioButton;
        private System.Windows.Forms.RadioButton exportBothRadioButton;
        private System.Windows.Forms.GroupBox exportFormatGroupBox;
        private System.Windows.Forms.Button exportDataButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.SaveFileDialog saveDataPointsDialog;
        private System.Windows.Forms.SaveFileDialog saveAdmissionsDialog;
    }
}