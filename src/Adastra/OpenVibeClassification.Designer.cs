namespace Adastra
{
    partial class OpenVibeClassification
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.buttonClose = new System.Windows.Forms.Button();
            this.chartClassification = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartClassification)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(684, 238);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(116, 33);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // chartClassification
            // 
            chartArea4.Name = "ChartArea1";
            this.chartClassification.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chartClassification.Legends.Add(legend4);
            this.chartClassification.Location = new System.Drawing.Point(12, 22);
            this.chartClassification.Name = "chartClassification";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chartClassification.Series.Add(series4);
            this.chartClassification.Size = new System.Drawing.Size(788, 188);
            this.chartClassification.TabIndex = 3;
            this.chartClassification.Text = "chart1";
            // 
            // OpenVibeClassification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 285);
            this.Controls.Add(this.chartClassification);
            this.Controls.Add(this.buttonClose);
            this.Name = "OpenVibeClassification";
            this.Text = "OpenVibeClassification";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OpenVibeClassification_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.chartClassification)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartClassification;
    }
}