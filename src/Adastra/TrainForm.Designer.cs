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
            this.progressBarRecord = new System.Windows.Forms.ProgressBar();
            this.textBoxModelName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCloseForm = new System.Windows.Forms.Button();
            this.comboBoxRecordTime = new System.Windows.Forms.ComboBox();
            this.buttonSaveModel = new System.Windows.Forms.Button();
            this.radioBtnLdaMLP = new System.Windows.Forms.RadioButton();
            this.radioBtnLdaSVM = new System.Windows.Forms.RadioButton();
            this.progressBarModelCalculation = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listBoxLogger = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // buttonRecordAction
            // 
            this.buttonRecordAction.Location = new System.Drawing.Point(148, 57);
            this.buttonRecordAction.Name = "buttonRecordAction";
            this.buttonRecordAction.Size = new System.Drawing.Size(130, 23);
            this.buttonRecordAction.TabIndex = 1;
            this.buttonRecordAction.Text = "Record action";
            this.buttonRecordAction.UseVisualStyleBackColor = true;
            this.buttonRecordAction.Click += new System.EventHandler(this.buttonRecordAction_Click);
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCalculate.Location = new System.Drawing.Point(12, 92);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(542, 60);
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
            this.comboBoxSelectedClass.Location = new System.Drawing.Point(97, 17);
            this.comboBoxSelectedClass.Name = "comboBoxSelectedClass";
            this.comboBoxSelectedClass.Size = new System.Drawing.Size(181, 21);
            this.comboBoxSelectedClass.TabIndex = 3;
            // 
            // progressBarRecord
            // 
            this.progressBarRecord.Location = new System.Drawing.Point(284, 57);
            this.progressBarRecord.Name = "progressBarRecord";
            this.progressBarRecord.Size = new System.Drawing.Size(270, 21);
            this.progressBarRecord.TabIndex = 4;
            // 
            // textBoxModelName
            // 
            this.textBoxModelName.Location = new System.Drawing.Point(85, 213);
            this.textBoxModelName.Name = "textBoxModelName";
            this.textBoxModelName.Size = new System.Drawing.Size(390, 20);
            this.textBoxModelName.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 216);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Model name:";
            // 
            // buttonCloseForm
            // 
            this.buttonCloseForm.Location = new System.Drawing.Point(584, 211);
            this.buttonCloseForm.Name = "buttonCloseForm";
            this.buttonCloseForm.Size = new System.Drawing.Size(116, 42);
            this.buttonCloseForm.TabIndex = 10;
            this.buttonCloseForm.Text = "Close";
            this.buttonCloseForm.UseVisualStyleBackColor = true;
            this.buttonCloseForm.Click += new System.EventHandler(this.buttonCloseForm_Click);
            // 
            // comboBoxRecordTime
            // 
            this.comboBoxRecordTime.FormattingEnabled = true;
            this.comboBoxRecordTime.Items.AddRange(new object[] {
            "5",
            "10"});
            this.comboBoxRecordTime.Location = new System.Drawing.Point(97, 59);
            this.comboBoxRecordTime.Name = "comboBoxRecordTime";
            this.comboBoxRecordTime.Size = new System.Drawing.Size(45, 21);
            this.comboBoxRecordTime.TabIndex = 11;
            // 
            // buttonSaveModel
            // 
            this.buttonSaveModel.Location = new System.Drawing.Point(493, 213);
            this.buttonSaveModel.Name = "buttonSaveModel";
            this.buttonSaveModel.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveModel.TabIndex = 13;
            this.buttonSaveModel.Text = "Save";
            this.buttonSaveModel.UseVisualStyleBackColor = true;
            this.buttonSaveModel.Click += new System.EventHandler(this.buttonSaveModel_Click);
            // 
            // radioBtnLdaMLP
            // 
            this.radioBtnLdaMLP.AutoSize = true;
            this.radioBtnLdaMLP.Checked = true;
            this.radioBtnLdaMLP.Location = new System.Drawing.Point(598, 92);
            this.radioBtnLdaMLP.Name = "radioBtnLdaMLP";
            this.radioBtnLdaMLP.Size = new System.Drawing.Size(74, 17);
            this.radioBtnLdaMLP.TabIndex = 15;
            this.radioBtnLdaMLP.TabStop = true;
            this.radioBtnLdaMLP.Text = "LDA+MLP";
            this.radioBtnLdaMLP.UseVisualStyleBackColor = true;
            // 
            // radioBtnLdaSVM
            // 
            this.radioBtnLdaSVM.AutoSize = true;
            this.radioBtnLdaSVM.Location = new System.Drawing.Point(598, 124);
            this.radioBtnLdaSVM.Name = "radioBtnLdaSVM";
            this.radioBtnLdaSVM.Size = new System.Drawing.Size(75, 17);
            this.radioBtnLdaSVM.TabIndex = 16;
            this.radioBtnLdaSVM.Text = "LDA+SVM";
            this.radioBtnLdaSVM.UseVisualStyleBackColor = true;
            // 
            // progressBarModelCalculation
            // 
            this.progressBarModelCalculation.Location = new System.Drawing.Point(13, 172);
            this.progressBarModelCalculation.Name = "progressBarModelCalculation";
            this.progressBarModelCalculation.Size = new System.Drawing.Size(685, 23);
            this.progressBarModelCalculation.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Action:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Record time (s):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 247);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Log:";
            // 
            // listBoxLogger
            // 
            this.listBoxLogger.FormattingEnabled = true;
            this.listBoxLogger.Location = new System.Drawing.Point(13, 263);
            this.listBoxLogger.Name = "listBoxLogger";
            this.listBoxLogger.Size = new System.Drawing.Size(685, 160);
            this.listBoxLogger.TabIndex = 21;
            // 
            // TrainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 431);
            this.Controls.Add(this.listBoxLogger);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBarModelCalculation);
            this.Controls.Add(this.radioBtnLdaSVM);
            this.Controls.Add(this.radioBtnLdaMLP);
            this.Controls.Add(this.buttonSaveModel);
            this.Controls.Add(this.comboBoxRecordTime);
            this.Controls.Add(this.buttonCloseForm);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxModelName);
            this.Controls.Add(this.progressBarRecord);
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
        private System.Windows.Forms.ProgressBar progressBarRecord;
        private System.Windows.Forms.TextBox textBoxModelName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonCloseForm;
        private System.Windows.Forms.ComboBox comboBoxRecordTime;
        private System.Windows.Forms.Button buttonSaveModel;
        private System.Windows.Forms.RadioButton radioBtnLdaMLP;
        private System.Windows.Forms.RadioButton radioBtnLdaSVM;
        private System.Windows.Forms.ProgressBar progressBarModelCalculation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBoxLogger;

    }
}