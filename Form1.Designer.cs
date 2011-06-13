namespace Adastra
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.homepageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openVibesHomepageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxOpenVibeWorkingFolder = new System.Windows.Forms.TextBox();
            this.textBoxScenario = new System.Windows.Forms.TextBox();
            this.buttonSelectOpenVibeWorkingFolder = new System.Windows.Forms.Button();
            this.buttonSelectScenario = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonExit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(619, 156);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(109, 34);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 167);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // chart1
            // 
            chartArea3.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chart1.Legends.Add(legend3);
            this.chart1.Location = new System.Drawing.Point(11, 211);
            this.chart1.Name = "chart1";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(749, 192);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(783, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.homepageToolStripMenuItem,
            this.openVibesHomepageToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // homepageToolStripMenuItem
            // 
            this.homepageToolStripMenuItem.Name = "homepageToolStripMenuItem";
            this.homepageToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.homepageToolStripMenuItem.Text = "Adastra\'s homepage";
            this.homepageToolStripMenuItem.Click += new System.EventHandler(this.homepageToolStripMenuItem_Click);
            // 
            // openVibesHomepageToolStripMenuItem
            // 
            this.openVibesHomepageToolStripMenuItem.Name = "openVibesHomepageToolStripMenuItem";
            this.openVibesHomepageToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openVibesHomepageToolStripMenuItem.Text = "OpenVibe\'s homepage";
            this.openVibesHomepageToolStripMenuItem.Click += new System.EventHandler(this.openVibesHomepageToolStripMenuItem_Click);
            // 
            // textBoxOpenVibeWorkingFolder
            // 
            this.textBoxOpenVibeWorkingFolder.Location = new System.Drawing.Point(141, 83);
            this.textBoxOpenVibeWorkingFolder.Name = "textBoxOpenVibeWorkingFolder";
            this.textBoxOpenVibeWorkingFolder.Size = new System.Drawing.Size(506, 20);
            this.textBoxOpenVibeWorkingFolder.TabIndex = 4;
            this.textBoxOpenVibeWorkingFolder.Text = "D:\\e\\openvibe_src\\dist\\";
            // 
            // textBoxScenario
            // 
            this.textBoxScenario.Location = new System.Drawing.Point(141, 118);
            this.textBoxScenario.Name = "textBoxScenario";
            this.textBoxScenario.Size = new System.Drawing.Size(506, 20);
            this.textBoxScenario.TabIndex = 5;
            this.textBoxScenario.Text = "C:\\Users\\toncho\\Desktop\\Adastra\\scenarios\\motor-imagery-bci-4-replay-VRPN-export." +
                "xml";
            // 
            // buttonSelectOpenVibeWorkingFolder
            // 
            this.buttonSelectOpenVibeWorkingFolder.Location = new System.Drawing.Point(653, 83);
            this.buttonSelectOpenVibeWorkingFolder.Name = "buttonSelectOpenVibeWorkingFolder";
            this.buttonSelectOpenVibeWorkingFolder.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectOpenVibeWorkingFolder.TabIndex = 6;
            this.buttonSelectOpenVibeWorkingFolder.Text = "Open";
            this.buttonSelectOpenVibeWorkingFolder.UseVisualStyleBackColor = true;
            this.buttonSelectOpenVibeWorkingFolder.Click += new System.EventHandler(this.buttonSelectOpenVibeWorkingFolder_Click);
            // 
            // buttonSelectScenario
            // 
            this.buttonSelectScenario.Location = new System.Drawing.Point(653, 118);
            this.buttonSelectScenario.Name = "buttonSelectScenario";
            this.buttonSelectScenario.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectScenario.TabIndex = 7;
            this.buttonSelectScenario.Text = "Open";
            this.buttonSelectScenario.UseVisualStyleBackColor = true;
            this.buttonSelectScenario.Click += new System.EventHandler(this.buttonSelectScenario_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(12, 28);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(717, 42);
            this.textBox3.TabIndex = 8;
            this.textBox3.Text = resources.GetString("textBox3.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "OpenVibe working folder:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(83, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Scenario:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 410);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(298, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Displays values streamed by OpenVibe\'s Analog VRPN server";
            // 
            // buttonExit
            // 
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExit.Location = new System.Drawing.Point(619, 424);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(141, 40);
            this.buttonExit.TabIndex = 12;
            this.buttonExit.Text = "Close";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 475);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.buttonSelectScenario);
            this.Controls.Add(this.buttonSelectOpenVibeWorkingFolder);
            this.Controls.Add(this.textBoxScenario);
            this.Controls.Add(this.textBoxOpenVibeWorkingFolder);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "Form1";
            this.Text = "Adastra";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem homepageToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxOpenVibeWorkingFolder;
        private System.Windows.Forms.TextBox textBoxScenario;
        private System.Windows.Forms.Button buttonSelectOpenVibeWorkingFolder;
        private System.Windows.Forms.Button buttonSelectScenario;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem openVibesHomepageToolStripMenuItem;
        private System.Windows.Forms.Button buttonExit;
    }
}

