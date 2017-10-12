namespace ThinkDiff
{
    partial class ThinkDiffMainForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDatafileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadStorageBoxesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createSurveillanceReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNAATComparisonReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDatabaseFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openBoxFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusLabel = new System.Windows.Forms.Label();
            this.thinkDiffProgressBar = new System.Windows.Forms.ProgressBar();
            this.filterForToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFromDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.admissionsWithAdmissionSamplesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexAdmissionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.exportPatientDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportBinDataDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 27);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(876, 471);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.DataSourceChanged += new System.EventHandler(this.dataGridView1_DataSourceChanged);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(900, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadDatafileToolStripMenuItem,
            this.exportPatientDataToolStripMenuItem,
            this.loadStorageBoxesToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadDatafileToolStripMenuItem
            // 
            this.loadDatafileToolStripMenuItem.Name = "loadDatafileToolStripMenuItem";
            this.loadDatafileToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.loadDatafileToolStripMenuItem.Text = "Import Patient Data...";
            this.loadDatafileToolStripMenuItem.Click += new System.EventHandler(this.loadDatafileToolStripMenuItem_Click);
            // 
            // loadStorageBoxesToolStripMenuItem
            // 
            this.loadStorageBoxesToolStripMenuItem.Name = "loadStorageBoxesToolStripMenuItem";
            this.loadStorageBoxesToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.loadStorageBoxesToolStripMenuItem.Text = "Load Storage Boxes...";
            this.loadStorageBoxesToolStripMenuItem.Click += new System.EventHandler(this.loadStorageBoxesToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filterForToolStripMenuItem,
            this.removeFromDataToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.editToolStripMenuItem.Text = "Filter";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filterDataToolStripMenuItem,
            this.createSurveillanceReportToolStripMenuItem,
            this.createNAATComparisonReportToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // filterDataToolStripMenuItem
            // 
            this.filterDataToolStripMenuItem.Name = "filterDataToolStripMenuItem";
            this.filterDataToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.filterDataToolStripMenuItem.Text = "Filter Data...";
            // 
            // createSurveillanceReportToolStripMenuItem
            // 
            this.createSurveillanceReportToolStripMenuItem.Name = "createSurveillanceReportToolStripMenuItem";
            this.createSurveillanceReportToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.createSurveillanceReportToolStripMenuItem.Text = "Create Surveillance Report";
            // 
            // createNAATComparisonReportToolStripMenuItem
            // 
            this.createNAATComparisonReportToolStripMenuItem.Name = "createNAATComparisonReportToolStripMenuItem";
            this.createNAATComparisonReportToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.createNAATComparisonReportToolStripMenuItem.Text = "Create NAAT Comparison Report";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // openDatabaseFileDialog
            // 
            this.openDatabaseFileDialog.DefaultExt = "\".csv\"";
            this.openDatabaseFileDialog.Title = "Select Patient Data File...";
            this.openDatabaseFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openDatabaseFileDialog_FileOk);
            // 
            // openBoxFileDialog
            // 
            this.openBoxFileDialog.DefaultExt = "csv";
            this.openBoxFileDialog.FileName = "openBoxFileDialog";
            this.openBoxFileDialog.Filter = "\"Storage File | *.csv\"";
            this.openBoxFileDialog.Title = "Select Storage Data File";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(147, 514);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(108, 18);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "No data loaded";
            // 
            // thinkDiffProgressBar
            // 
            this.thinkDiffProgressBar.Location = new System.Drawing.Point(12, 512);
            this.thinkDiffProgressBar.Name = "thinkDiffProgressBar";
            this.thinkDiffProgressBar.Size = new System.Drawing.Size(129, 24);
            this.thinkDiffProgressBar.TabIndex = 3;
            // 
            // filterForToolStripMenuItem
            // 
            this.filterForToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.admissionsWithAdmissionSamplesToolStripMenuItem,
            this.indexAdmissionToolStripMenuItem});
            this.filterForToolStripMenuItem.Name = "filterForToolStripMenuItem";
            this.filterForToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.filterForToolStripMenuItem.Text = "Filter For";
            // 
            // removeFromDataToolStripMenuItem
            // 
            this.removeFromDataToolStripMenuItem.Name = "removeFromDataToolStripMenuItem";
            this.removeFromDataToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.removeFromDataToolStripMenuItem.Text = "Remove From Data";
            // 
            // admissionsWithAdmissionSamplesToolStripMenuItem
            // 
            this.admissionsWithAdmissionSamplesToolStripMenuItem.Name = "admissionsWithAdmissionSamplesToolStripMenuItem";
            this.admissionsWithAdmissionSamplesToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.admissionsWithAdmissionSamplesToolStripMenuItem.Text = "Admissions With Admission Samples";
            this.admissionsWithAdmissionSamplesToolStripMenuItem.Click += new System.EventHandler(this.admissionsWithAdmissionSamplesToolStripMenuItem_Click);
            // 
            // indexAdmissionToolStripMenuItem
            // 
            this.indexAdmissionToolStripMenuItem.Name = "indexAdmissionToolStripMenuItem";
            this.indexAdmissionToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.indexAdmissionToolStripMenuItem.Text = "Index Admission";
            this.indexAdmissionToolStripMenuItem.Click += new System.EventHandler(this.indexAdmissionToolStripMenuItem_Click);
            // 
            // filterBackgroundWorker
            // 
            this.filterBackgroundWorker.WorkerReportsProgress = true;
            this.filterBackgroundWorker.WorkerSupportsCancellation = true;
            this.filterBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.filterBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.filterBackgroundWorker_RunWorkerCompleted);
            // 
            // exportPatientDataToolStripMenuItem
            // 
            this.exportPatientDataToolStripMenuItem.Name = "exportPatientDataToolStripMenuItem";
            this.exportPatientDataToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.exportPatientDataToolStripMenuItem.Text = "Export Patient Data...";
            this.exportPatientDataToolStripMenuItem.Click += new System.EventHandler(this.exportPatientDataToolStripMenuItem_Click);
            // 
            // exportBinDataDialog
            // 
            this.exportBinDataDialog.DefaultExt = "csv";
            this.exportBinDataDialog.Filter = "\"Comma Separated Values |*.csv\"";
            this.exportBinDataDialog.ShowHelp = true;
            this.exportBinDataDialog.SupportMultiDottedExtensions = true;
            this.exportBinDataDialog.Title = "Save Patient Data As....";
            // 
            // ThinkDiffMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 541);
            this.Controls.Add(this.thinkDiffProgressBar);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ThinkDiffMainForm";
            this.Text = "ThinkDiff";
            this.Load += new System.EventHandler(this.ThinkDiffMainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadDatafileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openDatabaseFileDialog;
        private System.Windows.Forms.ToolStripMenuItem loadStorageBoxesToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openBoxFileDialog;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ProgressBar thinkDiffProgressBar;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createSurveillanceReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createNAATComparisonReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterForToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem admissionsWithAdmissionSamplesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexAdmissionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFromDataToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker filterBackgroundWorker;
        private System.Windows.Forms.ToolStripMenuItem exportPatientDataToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog exportBinDataDialog;
    }
}

