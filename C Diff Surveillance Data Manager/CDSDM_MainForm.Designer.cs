namespace C_Diff_Surveillance_Data_Manager
{
    partial class CDSDM_MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDatabaseFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadStorageDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDatabaseDialog = new System.Windows.Forms.OpenFileDialog();
            this.openStorageFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.databaseStatusLabel = new System.Windows.Forms.Label();
            this.storageStatusLabel = new System.Windows.Forms.Label();
            this.exportAdmDataButton = new System.Windows.Forms.Button();
            this.saveAdmissionsDataFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.pickIndexAdmissionButton = new System.Windows.Forms.Button();
            this.summaryReportButton = new System.Windows.Forms.Button();
            this.surveillanceReportButton = new System.Windows.Forms.Button();
            this.filterSelectedButton = new System.Windows.Forms.Button();
            this.filterStoredButton = new System.Windows.Forms.Button();
            this.exportCurrentDatasetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCurrentDatabaseDialog = new System.Windows.Forms.SaveFileDialog();
            this.resetDatasetToOriginalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sampleFinderAllButton = new System.Windows.Forms.Button();
            this.sampleFinderByIDButton = new System.Windows.Forms.Button();
            this.saveStorageDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(496, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDatabaseFileToolStripMenuItem,
            this.loadStorageDataToolStripMenuItem,
            this.exportCurrentDatasetToolStripMenuItem,
            this.resetDatasetToOriginalToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadDatabaseFileToolStripMenuItem
            // 
            this.loadDatabaseFileToolStripMenuItem.Name = "loadDatabaseFileToolStripMenuItem";
            this.loadDatabaseFileToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.loadDatabaseFileToolStripMenuItem.Text = "Load Database File...";
            this.loadDatabaseFileToolStripMenuItem.Click += new System.EventHandler(this.loadDatabaseFileToolStripMenuItem_Click);
            // 
            // loadStorageDataToolStripMenuItem
            // 
            this.loadStorageDataToolStripMenuItem.Name = "loadStorageDataToolStripMenuItem";
            this.loadStorageDataToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.loadStorageDataToolStripMenuItem.Text = "Load Storage Data...";
            this.loadStorageDataToolStripMenuItem.Click += new System.EventHandler(this.loadStorageDataToolStripMenuItem_Click);
            // 
            // openDatabaseDialog
            // 
            this.openDatabaseDialog.DefaultExt = "csv";
            this.openDatabaseDialog.FileName = "Choose C Diff Database File";
            this.openDatabaseDialog.Filter = "Comma-separated value files|*.csv";
            this.openDatabaseDialog.Title = "Select C. Diff Database Data (CSV Format)";
            // 
            // openStorageFileDialog
            // 
            this.openStorageFileDialog.DefaultExt = "csv";
            this.openStorageFileDialog.FileName = "Choose Storage File CSV";
            this.openStorageFileDialog.Filter = "Comma-separated value files|*.csv";
            this.openStorageFileDialog.Title = "Choose Storage Database File CSV Format";
            // 
            // databaseStatusLabel
            // 
            this.databaseStatusLabel.AutoSize = true;
            this.databaseStatusLabel.Location = new System.Drawing.Point(12, 383);
            this.databaseStatusLabel.Name = "databaseStatusLabel";
            this.databaseStatusLabel.Size = new System.Drawing.Size(177, 13);
            this.databaseStatusLabel.TabIndex = 1;
            this.databaseStatusLabel.Text = "C Diff Database Status: Not Loaded";
            // 
            // storageStatusLabel
            // 
            this.storageStatusLabel.AutoSize = true;
            this.storageStatusLabel.Location = new System.Drawing.Point(12, 407);
            this.storageStatusLabel.Name = "storageStatusLabel";
            this.storageStatusLabel.Size = new System.Drawing.Size(188, 13);
            this.storageStatusLabel.TabIndex = 2;
            this.storageStatusLabel.Text = "Storage Database Status: Not Loaded";
            // 
            // exportAdmDataButton
            // 
            this.exportAdmDataButton.Location = new System.Drawing.Point(12, 40);
            this.exportAdmDataButton.Name = "exportAdmDataButton";
            this.exportAdmDataButton.Size = new System.Drawing.Size(154, 46);
            this.exportAdmDataButton.TabIndex = 3;
            this.exportAdmDataButton.Text = "Export Admission Data";
            this.exportAdmDataButton.UseVisualStyleBackColor = true;
            this.exportAdmDataButton.Click += new System.EventHandler(this.exportAdmDataButton_Click);
            // 
            // saveAdmissionsDataFileDialog
            // 
            this.saveAdmissionsDataFileDialog.DefaultExt = "csv";
            this.saveAdmissionsDataFileDialog.Filter = "CSV Files|*.csv";
            this.saveAdmissionsDataFileDialog.Title = "Save Admissions Data As..";
            // 
            // pickIndexAdmissionButton
            // 
            this.pickIndexAdmissionButton.Location = new System.Drawing.Point(172, 40);
            this.pickIndexAdmissionButton.Name = "pickIndexAdmissionButton";
            this.pickIndexAdmissionButton.Size = new System.Drawing.Size(154, 46);
            this.pickIndexAdmissionButton.TabIndex = 4;
            this.pickIndexAdmissionButton.Text = "Pick Index Admissions";
            this.pickIndexAdmissionButton.UseVisualStyleBackColor = true;
            this.pickIndexAdmissionButton.Click += new System.EventHandler(this.pickIndexAdmissionButton_Click);
            // 
            // summaryReportButton
            // 
            this.summaryReportButton.Location = new System.Drawing.Point(332, 40);
            this.summaryReportButton.Name = "summaryReportButton";
            this.summaryReportButton.Size = new System.Drawing.Size(154, 46);
            this.summaryReportButton.TabIndex = 5;
            this.summaryReportButton.Text = "Summary Stats Report";
            this.summaryReportButton.UseVisualStyleBackColor = true;
            this.summaryReportButton.Click += new System.EventHandler(this.masterReportButton_Click);
            // 
            // surveillanceReportButton
            // 
            this.surveillanceReportButton.Location = new System.Drawing.Point(12, 92);
            this.surveillanceReportButton.Name = "surveillanceReportButton";
            this.surveillanceReportButton.Size = new System.Drawing.Size(154, 46);
            this.surveillanceReportButton.TabIndex = 6;
            this.surveillanceReportButton.Text = "Surveillance Report";
            this.surveillanceReportButton.UseVisualStyleBackColor = true;
            this.surveillanceReportButton.Click += new System.EventHandler(this.surveillanceReportButton_Click);
            // 
            // filterSelectedButton
            // 
            this.filterSelectedButton.Location = new System.Drawing.Point(172, 92);
            this.filterSelectedButton.Name = "filterSelectedButton";
            this.filterSelectedButton.Size = new System.Drawing.Size(154, 46);
            this.filterSelectedButton.TabIndex = 7;
            this.filterSelectedButton.Text = "Filter By Selected Samples";
            this.filterSelectedButton.UseVisualStyleBackColor = true;
            this.filterSelectedButton.Click += new System.EventHandler(this.filterSelectedButton_Click);
            // 
            // filterStoredButton
            // 
            this.filterStoredButton.Location = new System.Drawing.Point(332, 92);
            this.filterStoredButton.Name = "filterStoredButton";
            this.filterStoredButton.Size = new System.Drawing.Size(154, 46);
            this.filterStoredButton.TabIndex = 8;
            this.filterStoredButton.Text = "Filter By Stored Samples";
            this.filterStoredButton.UseVisualStyleBackColor = true;
            this.filterStoredButton.Click += new System.EventHandler(this.filterStoredButton_Click);
            // 
            // exportCurrentDatasetToolStripMenuItem
            // 
            this.exportCurrentDatasetToolStripMenuItem.Name = "exportCurrentDatasetToolStripMenuItem";
            this.exportCurrentDatasetToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.exportCurrentDatasetToolStripMenuItem.Text = "Export Current Dataset...";
            this.exportCurrentDatasetToolStripMenuItem.Click += new System.EventHandler(this.exportCurrentDatasetToolStripMenuItem_Click);
            // 
            // saveCurrentDatabaseDialog
            // 
            this.saveCurrentDatabaseDialog.DefaultExt = "csv";
            this.saveCurrentDatabaseDialog.Filter = "CSV Files|*.csv";
            this.saveCurrentDatabaseDialog.Title = "Save Current Dataset As...";
            // 
            // resetDatasetToOriginalToolStripMenuItem
            // 
            this.resetDatasetToOriginalToolStripMenuItem.Name = "resetDatasetToOriginalToolStripMenuItem";
            this.resetDatasetToOriginalToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.resetDatasetToOriginalToolStripMenuItem.Text = "Reset Dataset to Original";
            this.resetDatasetToOriginalToolStripMenuItem.Click += new System.EventHandler(this.resetDatasetToOriginalToolStripMenuItem_Click);
            // 
            // sampleFinderAllButton
            // 
            this.sampleFinderAllButton.Location = new System.Drawing.Point(12, 144);
            this.sampleFinderAllButton.Name = "sampleFinderAllButton";
            this.sampleFinderAllButton.Size = new System.Drawing.Size(154, 46);
            this.sampleFinderAllButton.TabIndex = 9;
            this.sampleFinderAllButton.Text = "Find Samples in Dataset";
            this.sampleFinderAllButton.UseVisualStyleBackColor = true;
            this.sampleFinderAllButton.Click += new System.EventHandler(this.sampleFinderAllButton_Click);
            // 
            // sampleFinderByIDButton
            // 
            this.sampleFinderByIDButton.Location = new System.Drawing.Point(172, 144);
            this.sampleFinderByIDButton.Name = "sampleFinderByIDButton";
            this.sampleFinderByIDButton.Size = new System.Drawing.Size(154, 46);
            this.sampleFinderByIDButton.TabIndex = 10;
            this.sampleFinderByIDButton.Text = "Find Samples by SampleID";
            this.sampleFinderByIDButton.UseVisualStyleBackColor = true;
            this.sampleFinderByIDButton.Click += new System.EventHandler(this.sampleFinderByIDButton_Click);
            // 
            // saveStorageDialog
            // 
            this.saveStorageDialog.DefaultExt = "csv";
            this.saveStorageDialog.Filter = "CSV Files|*.csv";
            this.saveStorageDialog.Title = "Save Selected Tube Data As...";
            // 
            // CDSDM_MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 429);
            this.Controls.Add(this.sampleFinderByIDButton);
            this.Controls.Add(this.sampleFinderAllButton);
            this.Controls.Add(this.filterStoredButton);
            this.Controls.Add(this.filterSelectedButton);
            this.Controls.Add(this.surveillanceReportButton);
            this.Controls.Add(this.summaryReportButton);
            this.Controls.Add(this.pickIndexAdmissionButton);
            this.Controls.Add(this.exportAdmDataButton);
            this.Controls.Add(this.storageStatusLabel);
            this.Controls.Add(this.databaseStatusLabel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "CDSDM_MainForm";
            this.Text = "C Diff Surveillance Data Manager";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadDatabaseFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadStorageDataToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openDatabaseDialog;
        private System.Windows.Forms.OpenFileDialog openStorageFileDialog;
        private System.Windows.Forms.Label databaseStatusLabel;
        private System.Windows.Forms.Label storageStatusLabel;
        private System.Windows.Forms.Button exportAdmDataButton;
        private System.Windows.Forms.SaveFileDialog saveAdmissionsDataFileDialog;
        private System.Windows.Forms.Button pickIndexAdmissionButton;
        private System.Windows.Forms.Button summaryReportButton;
        private System.Windows.Forms.Button surveillanceReportButton;
        private System.Windows.Forms.Button filterSelectedButton;
        private System.Windows.Forms.Button filterStoredButton;
        private System.Windows.Forms.ToolStripMenuItem exportCurrentDatasetToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveCurrentDatabaseDialog;
        private System.Windows.Forms.ToolStripMenuItem resetDatasetToOriginalToolStripMenuItem;
        private System.Windows.Forms.Button sampleFinderAllButton;
        private System.Windows.Forms.Button sampleFinderByIDButton;
        private System.Windows.Forms.SaveFileDialog saveStorageDialog;
    }
}

