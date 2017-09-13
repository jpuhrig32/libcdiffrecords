namespace C_diff_Records_Test_App
{
    partial class Form1
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
            this.inputFilePathTextBox = new System.Windows.Forms.TextBox();
            this.openFileButton = new System.Windows.Forms.Button();
            this.openFileLabel = new System.Windows.Forms.Label();
            this.loadFileButton = new System.Windows.Forms.Button();
            this.openInputFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openRecFileLabel = new System.Windows.Forms.Label();
            this.recFileTextBox = new System.Windows.Forms.TextBox();
            this.recFileBrowseButton = new System.Windows.Forms.Button();
            this.boxLocTextBox = new System.Windows.Forms.TextBox();
            this.boxLocBrowseButton = new System.Windows.Forms.Button();
            this.boxLocFileLabel = new System.Windows.Forms.Label();
            this.openBoxLocFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // inputFilePathTextBox
            // 
            this.inputFilePathTextBox.Location = new System.Drawing.Point(66, 111);
            this.inputFilePathTextBox.Name = "inputFilePathTextBox";
            this.inputFilePathTextBox.ReadOnly = true;
            this.inputFilePathTextBox.Size = new System.Drawing.Size(278, 20);
            this.inputFilePathTextBox.TabIndex = 0;
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(362, 110);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(76, 20);
            this.openFileButton.TabIndex = 1;
            this.openFileButton.Text = "Browse";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // openFileLabel
            // 
            this.openFileLabel.AutoSize = true;
            this.openFileLabel.Location = new System.Drawing.Point(63, 95);
            this.openFileLabel.Name = "openFileLabel";
            this.openFileLabel.Size = new System.Drawing.Size(110, 13);
            this.openFileLabel.TabIndex = 2;
            this.openFileLabel.Text = "Legacy Database File";
            this.openFileLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // loadFileButton
            // 
            this.loadFileButton.Location = new System.Drawing.Point(196, 310);
            this.loadFileButton.Name = "loadFileButton";
            this.loadFileButton.Size = new System.Drawing.Size(80, 34);
            this.loadFileButton.TabIndex = 3;
            this.loadFileButton.Text = "Load File";
            this.loadFileButton.UseVisualStyleBackColor = true;
            this.loadFileButton.Click += new System.EventHandler(this.loadFileButton_Click);
            // 
            // openInputFileDialog
            // 
            this.openInputFileDialog.FileName = "openFileDialog1";
            // 
            // openRecFileLabel
            // 
            this.openRecFileLabel.AutoSize = true;
            this.openRecFileLabel.Location = new System.Drawing.Point(66, 138);
            this.openRecFileLabel.Name = "openRecFileLabel";
            this.openRecFileLabel.Size = new System.Drawing.Size(88, 13);
            this.openRecFileLabel.TabIndex = 4;
            this.openRecFileLabel.Text = "Path To Save As";
            // 
            // recFileTextBox
            // 
            this.recFileTextBox.Location = new System.Drawing.Point(66, 154);
            this.recFileTextBox.Name = "recFileTextBox";
            this.recFileTextBox.ReadOnly = true;
            this.recFileTextBox.Size = new System.Drawing.Size(277, 20);
            this.recFileTextBox.TabIndex = 5;
            this.recFileTextBox.TextChanged += new System.EventHandler(this.recFileTextBox_TextChanged);
            // 
            // recFileBrowseButton
            // 
            this.recFileBrowseButton.Location = new System.Drawing.Point(362, 154);
            this.recFileBrowseButton.Name = "recFileBrowseButton";
            this.recFileBrowseButton.Size = new System.Drawing.Size(76, 20);
            this.recFileBrowseButton.TabIndex = 6;
            this.recFileBrowseButton.Text = "Browse...";
            this.recFileBrowseButton.UseVisualStyleBackColor = true;
            this.recFileBrowseButton.Click += new System.EventHandler(this.recFileBrowseButton_Click);
            // 
            // boxLocTextBox
            // 
            this.boxLocTextBox.Location = new System.Drawing.Point(66, 196);
            this.boxLocTextBox.Name = "boxLocTextBox";
            this.boxLocTextBox.ReadOnly = true;
            this.boxLocTextBox.Size = new System.Drawing.Size(276, 20);
            this.boxLocTextBox.TabIndex = 7;
            // 
            // boxLocBrowseButton
            // 
            this.boxLocBrowseButton.Location = new System.Drawing.Point(362, 196);
            this.boxLocBrowseButton.Name = "boxLocBrowseButton";
            this.boxLocBrowseButton.Size = new System.Drawing.Size(76, 20);
            this.boxLocBrowseButton.TabIndex = 8;
            this.boxLocBrowseButton.Text = "Browse...";
            this.boxLocBrowseButton.UseVisualStyleBackColor = true;
            this.boxLocBrowseButton.Click += new System.EventHandler(this.boxLocBrowseButton_Click);
            // 
            // boxLocFileLabel
            // 
            this.boxLocFileLabel.AutoSize = true;
            this.boxLocFileLabel.Location = new System.Drawing.Point(66, 180);
            this.boxLocFileLabel.Name = "boxLocFileLabel";
            this.boxLocFileLabel.Size = new System.Drawing.Size(93, 13);
            this.boxLocFileLabel.TabIndex = 9;
            this.boxLocFileLabel.Text = "Box Location Files";
            // 
            // openBoxLocFileDialog
            // 
            this.openBoxLocFileDialog.FileName = "openFileDialog1";
            this.openBoxLocFileDialog.Multiselect = true;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xlsx";
            this.saveFileDialog1.Filter = "\"Excel Files|*.xlsx|All Files|*.*\"";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(319, 310);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 33);
            this.button1.TabIndex = 11;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 356);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.boxLocFileLabel);
            this.Controls.Add(this.boxLocBrowseButton);
            this.Controls.Add(this.boxLocTextBox);
            this.Controls.Add(this.recFileBrowseButton);
            this.Controls.Add(this.recFileTextBox);
            this.Controls.Add(this.openRecFileLabel);
            this.Controls.Add(this.loadFileButton);
            this.Controls.Add(this.openFileLabel);
            this.Controls.Add(this.openFileButton);
            this.Controls.Add(this.inputFilePathTextBox);
            this.Name = "Form1";
            this.Text = "LibCdiffTestForm";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox inputFilePathTextBox;
        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.Label openFileLabel;
        private System.Windows.Forms.Button loadFileButton;
        private System.Windows.Forms.OpenFileDialog openInputFileDialog;
        private System.Windows.Forms.Label openRecFileLabel;
        private System.Windows.Forms.TextBox recFileTextBox;
        private System.Windows.Forms.Button recFileBrowseButton;
        private System.Windows.Forms.TextBox boxLocTextBox;
        private System.Windows.Forms.Button boxLocBrowseButton;
        private System.Windows.Forms.Label boxLocFileLabel;
        private System.Windows.Forms.OpenFileDialog openBoxLocFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button1;
    }
}

