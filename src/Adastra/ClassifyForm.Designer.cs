namespace Adastra
{
    partial class ClassifyForm
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
            this.textBoxModelLocation = new System.Windows.Forms.TextBox();
            this.buttonLoadModel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonStartProcessing = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonSelectModel = new System.Windows.Forms.Button();
            this.listBoxClasses = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxModelLocation
            // 
            this.textBoxModelLocation.Location = new System.Drawing.Point(135, 11);
            this.textBoxModelLocation.Name = "textBoxModelLocation";
            this.textBoxModelLocation.Size = new System.Drawing.Size(300, 20);
            this.textBoxModelLocation.TabIndex = 0;
            // 
            // buttonLoadModel
            // 
            this.buttonLoadModel.Location = new System.Drawing.Point(525, 9);
            this.buttonLoadModel.Name = "buttonLoadModel";
            this.buttonLoadModel.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadModel.TabIndex = 1;
            this.buttonLoadModel.Text = "Load";
            this.buttonLoadModel.UseVisualStyleBackColor = true;
            this.buttonLoadModel.Click += new System.EventHandler(this.buttonModel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Trained classifier model:";
            // 
            // buttonStartProcessing
            // 
            this.buttonStartProcessing.Location = new System.Drawing.Point(268, 73);
            this.buttonStartProcessing.Name = "buttonStartProcessing";
            this.buttonStartProcessing.Size = new System.Drawing.Size(301, 40);
            this.buttonStartProcessing.TabIndex = 4;
            this.buttonStartProcessing.Text = "Process";
            this.buttonStartProcessing.UseVisualStyleBackColor = true;
            this.buttonStartProcessing.Click += new System.EventHandler(this.buttonStartProcessing_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 145);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(588, 95);
            this.listBox1.TabIndex = 5;
            // 
            // buttonSelectModel
            // 
            this.buttonSelectModel.Location = new System.Drawing.Point(444, 9);
            this.buttonSelectModel.Name = "buttonSelectModel";
            this.buttonSelectModel.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectModel.TabIndex = 6;
            this.buttonSelectModel.Text = "Browse";
            this.buttonSelectModel.UseVisualStyleBackColor = true;
            this.buttonSelectModel.Click += new System.EventHandler(this.buttonSelectModel_Click);
            // 
            // listBoxClasses
            // 
            this.listBoxClasses.FormattingEnabled = true;
            this.listBoxClasses.Location = new System.Drawing.Point(12, 63);
            this.listBoxClasses.Name = "listBoxClasses";
            this.listBoxClasses.Size = new System.Drawing.Size(217, 69);
            this.listBoxClasses.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Available classes:";
            // 
            // ClassifyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 252);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBoxClasses);
            this.Controls.Add(this.buttonSelectModel);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.buttonStartProcessing);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonLoadModel);
            this.Controls.Add(this.textBoxModelLocation);
            this.Name = "ClassifyForm";
            this.Text = "Classify EEG signal";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxModelLocation;
        private System.Windows.Forms.Button buttonLoadModel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonStartProcessing;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonSelectModel;
        private System.Windows.Forms.ListBox listBoxClasses;
        private System.Windows.Forms.Label label2;
    }
}