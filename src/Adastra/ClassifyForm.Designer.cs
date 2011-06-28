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
            this.label1 = new System.Windows.Forms.Label();
            this.buttonStartProcessing = new System.Windows.Forms.Button();
            this.listBoxResult = new System.Windows.Forms.ListBox();
            this.listBoxClasses = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxModels = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Trained classifier model:";
            // 
            // buttonStartProcessing
            // 
            this.buttonStartProcessing.Location = new System.Drawing.Point(161, 184);
            this.buttonStartProcessing.Name = "buttonStartProcessing";
            this.buttonStartProcessing.Size = new System.Drawing.Size(298, 40);
            this.buttonStartProcessing.TabIndex = 4;
            this.buttonStartProcessing.Text = "Process";
            this.buttonStartProcessing.UseVisualStyleBackColor = true;
            this.buttonStartProcessing.Click += new System.EventHandler(this.buttonStartProcessing_Click);
            // 
            // listBoxResult
            // 
            this.listBoxResult.FormattingEnabled = true;
            this.listBoxResult.Location = new System.Drawing.Point(15, 246);
            this.listBoxResult.Name = "listBoxResult";
            this.listBoxResult.Size = new System.Drawing.Size(585, 95);
            this.listBoxResult.TabIndex = 5;
            // 
            // listBoxClasses
            // 
            this.listBoxClasses.FormattingEnabled = true;
            this.listBoxClasses.Location = new System.Drawing.Point(302, 55);
            this.listBoxClasses.Name = "listBoxClasses";
            this.listBoxClasses.Size = new System.Drawing.Size(279, 108);
            this.listBoxClasses.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(299, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Available classes:";
            // 
            // listBoxModels
            // 
            this.listBoxModels.FormattingEnabled = true;
            this.listBoxModels.Location = new System.Drawing.Point(15, 55);
            this.listBoxModels.Name = "listBoxModels";
            this.listBoxModels.Size = new System.Drawing.Size(265, 108);
            this.listBoxModels.TabIndex = 9;
            this.listBoxModels.SelectedIndexChanged += new System.EventHandler(this.listBoxModels_SelectedIndexChanged);
            // 
            // ClassifyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 361);
            this.Controls.Add(this.listBoxModels);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBoxClasses);
            this.Controls.Add(this.listBoxResult);
            this.Controls.Add(this.buttonStartProcessing);
            this.Controls.Add(this.label1);
            this.Name = "ClassifyForm";
            this.Text = "Classify EEG signal";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonStartProcessing;
        private System.Windows.Forms.ListBox listBoxResult;
        private System.Windows.Forms.ListBox listBoxClasses;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxModels;
    }
}