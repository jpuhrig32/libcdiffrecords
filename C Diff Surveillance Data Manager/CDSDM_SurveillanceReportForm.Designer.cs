namespace C_Diff_Surveillance_Data_Manager
{
    partial class CDSDM_SurveillanceReportForm
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
            this.startDatePicker = new System.Windows.Forms.DateTimePicker();
            this.endDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ignoreDateRangeCB = new System.Windows.Forms.CheckBox();
            this.reportButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveReportDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // startDatePicker
            // 
            this.startDatePicker.CustomFormat = "";
            this.startDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startDatePicker.Location = new System.Drawing.Point(12, 53);
            this.startDatePicker.MinDate = new System.DateTime(2016, 8, 1, 0, 0, 0, 0);
            this.startDatePicker.Name = "startDatePicker";
            this.startDatePicker.Size = new System.Drawing.Size(92, 20);
            this.startDatePicker.TabIndex = 0;
            this.startDatePicker.Value = new System.DateTime(2018, 1, 9, 0, 0, 0, 0);
            // 
            // endDatePicker
            // 
            this.endDatePicker.CustomFormat = "";
            this.endDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.endDatePicker.Location = new System.Drawing.Point(135, 53);
            this.endDatePicker.MinDate = new System.DateTime(2016, 8, 1, 0, 0, 0, 0);
            this.endDatePicker.Name = "endDatePicker";
            this.endDatePicker.Size = new System.Drawing.Size(92, 20);
            this.endDatePicker.TabIndex = 1;
            this.endDatePicker.Value = new System.DateTime(2018, 1, 9, 0, 0, 0, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Start Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(132, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "End Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Start and End Dates of Report (inclusive)";
            // 
            // ignoreDateRangeCB
            // 
            this.ignoreDateRangeCB.AutoSize = true;
            this.ignoreDateRangeCB.Location = new System.Drawing.Point(12, 91);
            this.ignoreDateRangeCB.Name = "ignoreDateRangeCB";
            this.ignoreDateRangeCB.Size = new System.Drawing.Size(117, 17);
            this.ignoreDateRangeCB.TabIndex = 5;
            this.ignoreDateRangeCB.Text = "Ignore Date Range";
            this.ignoreDateRangeCB.UseVisualStyleBackColor = true;
            // 
            // reportButton
            // 
            this.reportButton.Location = new System.Drawing.Point(12, 126);
            this.reportButton.Name = "reportButton";
            this.reportButton.Size = new System.Drawing.Size(92, 23);
            this.reportButton.TabIndex = 6;
            this.reportButton.Text = "Export Report";
            this.reportButton.UseVisualStyleBackColor = true;
            this.reportButton.Click += new System.EventHandler(this.reportButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(152, 126);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveReportDialog
            // 
            this.saveReportDialog.DefaultExt = "csv";
            this.saveReportDialog.Filter = "CSV Files|*.csv";
            this.saveReportDialog.Title = "Save Surveillance Report As...";
            // 
            // CDSDM_SurveillanceReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(241, 161);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.reportButton);
            this.Controls.Add(this.ignoreDateRangeCB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.endDatePicker);
            this.Controls.Add(this.startDatePicker);
            this.Name = "CDSDM_SurveillanceReportForm";
            this.Text = "Surveillance Report";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker startDatePicker;
        private System.Windows.Forms.DateTimePicker endDatePicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox ignoreDateRangeCB;
        private System.Windows.Forms.Button reportButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.SaveFileDialog saveReportDialog;
    }
}