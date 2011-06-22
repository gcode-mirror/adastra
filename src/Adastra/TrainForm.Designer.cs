namespace Adastra
{
    partial class TrainForm
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
            this.buttonRecordAction = new System.Windows.Forms.Button();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.comboBoxSelectedClass = new System.Windows.Forms.ComboBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFeatureVector = new System.Windows.Forms.TextBox();
            this.textBoxModelLocation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSelectModelLocation = new System.Windows.Forms.Button();
            this.buttonCloseForm = new System.Windows.Forms.Button();
            this.comboBoxRecordTime = new System.Windows.Forms.ComboBox();
            this.buttonSaveModel = new System.Windows.Forms.Button();
            this.textBoxLogger = new System.Windows.Forms.TextBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // buttonRecordAction
            // 
            this.buttonRecordAction.Location = new System.Drawing.Point(12, 51);
            this.buttonRecordAction.Name = "buttonRecordAction";
            this.buttonRecordAction.Size = new System.Drawing.Size(116, 23);
            this.buttonRecordAction.TabIndex = 1;
            this.buttonRecordAction.Text = "Record action";
            this.buttonRecordAction.UseVisualStyleBackColor = true;
            this.buttonRecordAction.Click += new System.EventHandler(this.buttonRecordAction_Click);
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCalculate.Location = new System.Drawing.Point(12, 129);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(530, 60);
            this.buttonCalculate.TabIndex = 2;
            this.buttonCalculate.Text = "Compute";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // comboBoxSelectedClass
            // 
            this.comboBoxSelectedClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSelectedClass.FormattingEnabled = true;
            this.comboBoxSelectedClass.Items.AddRange(new object[] {
            "Left",
            "Right",
            "Up",
            "Down",
            "Double-Click"});
            this.comboBoxSelectedClass.Location = new System.Drawing.Point(12, 12);
            this.comboBoxSelectedClass.Name = "comboBoxSelectedClass";
            this.comboBoxSelectedClass.Size = new System.Drawing.Size(172, 21);
            this.comboBoxSelectedClass.TabIndex = 3;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 89);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(686, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(224, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Incoming feature vector:";
            // 
            // textBoxFeatureVector
            // 
            this.textBoxFeatureVector.Location = new System.Drawing.Point(359, 53);
            this.textBoxFeatureVector.Name = "textBoxFeatureVector";
            this.textBoxFeatureVector.ReadOnly = true;
            this.textBoxFeatureVector.Size = new System.Drawing.Size(339, 20);
            this.textBoxFeatureVector.TabIndex = 6;
            // 
            // textBoxModelLocation
            // 
            this.textBoxModelLocation.Location = new System.Drawing.Point(62, 215);
            this.textBoxModelLocation.Name = "textBoxModelLocation";
            this.textBoxModelLocation.Size = new System.Drawing.Size(378, 20);
            this.textBoxModelLocation.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 215);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Save to:";
            // 
            // buttonSelectModelLocation
            // 
            this.buttonSelectModelLocation.Location = new System.Drawing.Point(446, 213);
            this.buttonSelectModelLocation.Name = "buttonSelectModelLocation";
            this.buttonSelectModelLocation.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectModelLocation.TabIndex = 9;
            this.buttonSelectModelLocation.Text = "Browse";
            this.buttonSelectModelLocation.UseVisualStyleBackColor = true;
            this.buttonSelectModelLocation.Click += new System.EventHandler(this.buttonSelectModelLocation_Click);
            // 
            // buttonCloseForm
            // 
            this.buttonCloseForm.Location = new System.Drawing.Point(608, 213);
            this.buttonCloseForm.Name = "buttonCloseForm";
            this.buttonCloseForm.Size = new System.Drawing.Size(90, 42);
            this.buttonCloseForm.TabIndex = 10;
            this.buttonCloseForm.Text = "Close";
            this.buttonCloseForm.UseVisualStyleBackColor = true;
            this.buttonCloseForm.Click += new System.EventHandler(this.buttonCloseForm_Click);
            // 
            // comboBoxRecordTime
            // 
            this.comboBoxRecordTime.FormattingEnabled = true;
            this.comboBoxRecordTime.Items.AddRange(new object[] {
            "3",
            "5",
            "10"});
            this.comboBoxRecordTime.Location = new System.Drawing.Point(155, 53);
            this.comboBoxRecordTime.Name = "comboBoxRecordTime";
            this.comboBoxRecordTime.Size = new System.Drawing.Size(43, 21);
            this.comboBoxRecordTime.TabIndex = 11;
            // 
            // buttonSaveModel
            // 
            this.buttonSaveModel.Location = new System.Drawing.Point(527, 213);
            this.buttonSaveModel.Name = "buttonSaveModel";
            this.buttonSaveModel.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveModel.TabIndex = 13;
            this.buttonSaveModel.Text = "Save";
            this.buttonSaveModel.UseVisualStyleBackColor = true;
            this.buttonSaveModel.Click += new System.EventHandler(this.buttonSaveModel_Click);
            // 
            // textBoxLogger
            // 
            this.textBoxLogger.Location = new System.Drawing.Point(10, 269);
            this.textBoxLogger.Multiline = true;
            this.textBoxLogger.Name = "textBoxLogger";
            this.textBoxLogger.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLogger.Size = new System.Drawing.Size(696, 78);
            this.textBoxLogger.TabIndex = 14;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(582, 129);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(74, 17);
            this.radioButton1.TabIndex = 15;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "LDA+MLP";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Enabled = false;
            this.radioButton2.Location = new System.Drawing.Point(582, 161);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(75, 17);
            this.radioButton2.TabIndex = 16;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "LDA+SVM";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // TrainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 365);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.textBoxLogger);
            this.Controls.Add(this.buttonSaveModel);
            this.Controls.Add(this.comboBoxRecordTime);
            this.Controls.Add(this.buttonCloseForm);
            this.Controls.Add(this.buttonSelectModelLocation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxModelLocation);
            this.Controls.Add(this.textBoxFeatureVector);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.comboBoxSelectedClass);
            this.Controls.Add(this.buttonCalculate);
            this.Controls.Add(this.buttonRecordAction);
            this.Name = "TrainForm";
            this.Text = "Train";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRecordAction;
        private System.Windows.Forms.Button buttonCalculate;
        private System.Windows.Forms.ComboBox comboBoxSelectedClass;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFeatureVector;
        private System.Windows.Forms.TextBox textBoxModelLocation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonSelectModelLocation;
        private System.Windows.Forms.Button buttonCloseForm;
        private System.Windows.Forms.ComboBox comboBoxRecordTime;
        private System.Windows.Forms.Button buttonSaveModel;
        private System.Windows.Forms.TextBox textBoxLogger;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;

    }
}