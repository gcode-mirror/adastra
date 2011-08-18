namespace Adastra
{
    partial class MainForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("EEG signal");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Claffication output from OpenVibe");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Display", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Using OpenVibe\'s feature aggegator");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Using Adastra\'s feature aggregator");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Train", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Using OpenVibe\'s feature aggegator");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Using Adastra\'s feature aggregator");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Classify", new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode8});
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Classification", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Mouse cursor");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Device control", new System.Windows.Forms.TreeNode[] {
            treeNode11});
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Scenario", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode10,
            treeNode12});
            this.buttonStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.homepageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openVibesHomepageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxOpenVibeWorkingFolder = new System.Windows.Forms.TextBox();
            this.textBoxScenario = new System.Windows.Forms.TextBox();
            this.buttonSelectOpenVibeWorkingFolder = new System.Windows.Forms.Button();
            this.buttonSelectScenario = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonExit = new System.Windows.Forms.Button();
            this.comboBoxScenarioType = new System.Windows.Forms.ComboBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.buttonEditScenario = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rButtonRealtime = new System.Windows.Forms.RadioButton();
            this.rButtonRecordedSignal = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rbuttonOpenVibe = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.rbuttonEmotiv = new System.Windows.Forms.RadioButton();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStart.Location = new System.Drawing.Point(276, 285);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(209, 34);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Scenario type:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(720, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
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
            this.aboutToolStripMenuItem,
            this.homepageToolStripMenuItem,
            this.openVibesHomepageToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // homepageToolStripMenuItem
            // 
            this.homepageToolStripMenuItem.Name = "homepageToolStripMenuItem";
            this.homepageToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.homepageToolStripMenuItem.Text = "Adastra\'s homepage";
            this.homepageToolStripMenuItem.Click += new System.EventHandler(this.homepageToolStripMenuItem_Click);
            // 
            // openVibesHomepageToolStripMenuItem
            // 
            this.openVibesHomepageToolStripMenuItem.Name = "openVibesHomepageToolStripMenuItem";
            this.openVibesHomepageToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.openVibesHomepageToolStripMenuItem.Text = "OpenVibe\'s homepage";
            this.openVibesHomepageToolStripMenuItem.Click += new System.EventHandler(this.openVibesHomepageToolStripMenuItem_Click);
            // 
            // textBoxOpenVibeWorkingFolder
            // 
            this.textBoxOpenVibeWorkingFolder.Location = new System.Drawing.Point(162, 79);
            this.textBoxOpenVibeWorkingFolder.Name = "textBoxOpenVibeWorkingFolder";
            this.textBoxOpenVibeWorkingFolder.Size = new System.Drawing.Size(242, 20);
            this.textBoxOpenVibeWorkingFolder.TabIndex = 4;
            this.textBoxOpenVibeWorkingFolder.Text = "D:\\e\\openvibe_src\\dist";
            // 
            // textBoxScenario
            // 
            this.textBoxScenario.Location = new System.Drawing.Point(112, 27);
            this.textBoxScenario.Name = "textBoxScenario";
            this.textBoxScenario.Size = new System.Drawing.Size(292, 20);
            this.textBoxScenario.TabIndex = 5;
            this.textBoxScenario.Text = "C:\\Users\\toncho\\Desktop\\Adastra\\scenarios\\signal-processing-VRPN-export.xml";
            // 
            // buttonSelectOpenVibeWorkingFolder
            // 
            this.buttonSelectOpenVibeWorkingFolder.Location = new System.Drawing.Point(410, 79);
            this.buttonSelectOpenVibeWorkingFolder.Name = "buttonSelectOpenVibeWorkingFolder";
            this.buttonSelectOpenVibeWorkingFolder.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectOpenVibeWorkingFolder.TabIndex = 6;
            this.buttonSelectOpenVibeWorkingFolder.Text = "Browse";
            this.buttonSelectOpenVibeWorkingFolder.UseVisualStyleBackColor = true;
            this.buttonSelectOpenVibeWorkingFolder.Click += new System.EventHandler(this.buttonSelectOpenVibeWorkingFolder_Click);
            // 
            // buttonSelectScenario
            // 
            this.buttonSelectScenario.Location = new System.Drawing.Point(410, 24);
            this.buttonSelectScenario.Name = "buttonSelectScenario";
            this.buttonSelectScenario.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectScenario.TabIndex = 7;
            this.buttonSelectScenario.Text = "Browse";
            this.buttonSelectScenario.UseVisualStyleBackColor = true;
            this.buttonSelectScenario.Click += new System.EventHandler(this.buttonSelectScenario_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "OpenVibe working folder:";
            // 
            // buttonExit
            // 
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExit.Location = new System.Drawing.Point(591, 317);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(114, 33);
            this.buttonExit.TabIndex = 12;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // comboBoxScenarioType
            // 
            this.comboBoxScenarioType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxScenarioType.FormattingEnabled = true;
            this.comboBoxScenarioType.Location = new System.Drawing.Point(96, 41);
            this.comboBoxScenarioType.Name = "comboBoxScenarioType";
            this.comboBoxScenarioType.Size = new System.Drawing.Size(609, 21);
            this.comboBoxScenarioType.TabIndex = 13;
            this.comboBoxScenarioType.SelectedIndexChanged += new System.EventHandler(this.comboBoxScenarioType_SelectedIndexChanged);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(0, 325);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Node2";
            treeNode1.Text = "EEG signal";
            treeNode2.Name = "Node6";
            treeNode2.Text = "Claffication output from OpenVibe";
            treeNode3.Name = "Node1";
            treeNode3.Text = "Display";
            treeNode4.Name = "Node7";
            treeNode4.Text = "Using OpenVibe\'s feature aggegator";
            treeNode5.Name = "Node8";
            treeNode5.Text = "Using Adastra\'s feature aggregator";
            treeNode6.Name = "Node4";
            treeNode6.Text = "Train";
            treeNode7.Name = "Node11";
            treeNode7.Text = "Using OpenVibe\'s feature aggegator";
            treeNode8.Name = "Node12";
            treeNode8.Text = "Using Adastra\'s feature aggregator";
            treeNode9.Name = "Node5";
            treeNode9.Text = "Classify";
            treeNode10.Name = "Node3";
            treeNode10.Text = "Classification";
            treeNode11.Name = "Node10";
            treeNode11.Text = "Mouse cursor";
            treeNode12.Name = "Node9";
            treeNode12.Text = "Device control";
            treeNode13.Name = "Node0";
            treeNode13.Text = "Scenario";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode13});
            this.treeView1.Size = new System.Drawing.Size(295, 242);
            this.treeView1.TabIndex = 14;
            this.treeView1.Visible = false;
            // 
            // buttonEditScenario
            // 
            this.buttonEditScenario.Location = new System.Drawing.Point(491, 25);
            this.buttonEditScenario.Name = "buttonEditScenario";
            this.buttonEditScenario.Size = new System.Drawing.Size(75, 23);
            this.buttonEditScenario.TabIndex = 15;
            this.buttonEditScenario.Text = "Edit";
            this.buttonEditScenario.UseVisualStyleBackColor = true;
            this.buttonEditScenario.Click += new System.EventHandler(this.buttonEditScenario_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.rButtonRealtime);
            this.groupBox1.Controls.Add(this.rButtonRecordedSignal);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxScenario);
            this.groupBox1.Controls.Add(this.buttonEditScenario);
            this.groupBox1.Controls.Add(this.buttonSelectScenario);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.buttonSelectOpenVibeWorkingFolder);
            this.groupBox1.Controls.Add(this.textBoxOpenVibeWorkingFolder);
            this.groupBox1.Location = new System.Drawing.Point(128, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(580, 116);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "OpenVibe settings";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(201, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(243, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "You need to start OpenVibe Acquisition server first";
            this.label4.Visible = false;
            // 
            // rButtonRealtime
            // 
            this.rButtonRealtime.AutoSize = true;
            this.rButtonRealtime.Location = new System.Drawing.Point(126, 57);
            this.rButtonRealtime.Name = "rButtonRealtime";
            this.rButtonRealtime.Size = new System.Drawing.Size(69, 17);
            this.rButtonRealtime.TabIndex = 18;
            this.rButtonRealtime.Text = "Real-time";
            this.rButtonRealtime.UseVisualStyleBackColor = true;
            this.rButtonRealtime.CheckedChanged += new System.EventHandler(this.rButtonRealtime_CheckedChanged);
            // 
            // rButtonRecordedSignal
            // 
            this.rButtonRecordedSignal.AutoSize = true;
            this.rButtonRecordedSignal.Checked = true;
            this.rButtonRecordedSignal.Location = new System.Drawing.Point(34, 57);
            this.rButtonRecordedSignal.Name = "rButtonRecordedSignal";
            this.rButtonRecordedSignal.Size = new System.Drawing.Size(86, 17);
            this.rButtonRecordedSignal.TabIndex = 17;
            this.rButtonRecordedSignal.TabStop = true;
            this.rButtonRecordedSignal.Text = "Pre-recorded";
            this.rButtonRecordedSignal.UseVisualStyleBackColor = true;
            this.rButtonRecordedSignal.CheckedChanged += new System.EventHandler(this.rButtonRecordedSignal_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Scenario path:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "(recommended)";
            // 
            // rbuttonOpenVibe
            // 
            this.rbuttonOpenVibe.AutoSize = true;
            this.rbuttonOpenVibe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbuttonOpenVibe.Location = new System.Drawing.Point(18, 103);
            this.rbuttonOpenVibe.Name = "rbuttonOpenVibe";
            this.rbuttonOpenVibe.Size = new System.Drawing.Size(104, 17);
            this.rbuttonOpenVibe.TabIndex = 20;
            this.rbuttonOpenVibe.Text = "use OpenVibe";
            this.rbuttonOpenVibe.UseVisualStyleBackColor = true;
            this.rbuttonOpenVibe.CheckedChanged += new System.EventHandler(this.rbuttonOpenVibe_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Location = new System.Drawing.Point(128, 202);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(580, 67);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Emotiv EPOCH settings";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(17, 19);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(545, 41);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "Basic support for Emotiv is implemented. Ssignal is being feeded directly to the " +
                "machine learning algorithms. Currently no digital signal processing is applied.";
            // 
            // rbuttonEmotiv
            // 
            this.rbuttonEmotiv.AutoSize = true;
            this.rbuttonEmotiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbuttonEmotiv.Location = new System.Drawing.Point(18, 234);
            this.rbuttonEmotiv.Name = "rbuttonEmotiv";
            this.rbuttonEmotiv.Size = new System.Drawing.Size(87, 17);
            this.rbuttonEmotiv.TabIndex = 0;
            this.rbuttonEmotiv.TabStop = true;
            this.rbuttonEmotiv.Text = "use Emotiv";
            this.rbuttonEmotiv.UseVisualStyleBackColor = true;
            this.rbuttonEmotiv.CheckedChanged += new System.EventHandler(this.rbuttonEmotiv_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 358);
            this.Controls.Add(this.rbuttonOpenVibe);
            this.Controls.Add(this.rbuttonEmotiv);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.comboBoxScenarioType);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MainForm";
            this.Text = "Adastra";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem openVibesHomepageToolStripMenuItem;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBoxScenarioType;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button buttonEditScenario;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rButtonRealtime;
        private System.Windows.Forms.RadioButton rButtonRecordedSignal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rbuttonOpenVibe;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbuttonEmotiv;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
    }
}

