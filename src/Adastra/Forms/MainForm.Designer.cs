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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tutorialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.homepageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openVibesHomepageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.octaveDownloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxOpenVibeWorkingFolder = new System.Windows.Forms.TextBox();
            this.textBoxScenario = new System.Windows.Forms.TextBox();
            this.buttonSelectOpenVibeWorkingFolder = new System.Windows.Forms.Button();
            this.buttonSelectScenario = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonExit = new System.Windows.Forms.Button();
            this.comboBoxScenarioType = new System.Windows.Forms.ComboBox();
            this.buttonEditScenario = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rButtonRealtime = new System.Windows.Forms.RadioButton();
            this.rButtonRecordedSignal = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rbuttonOpenVibe = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbEmotivDspMethod = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonBrowseEmotivFile = new System.Windows.Forms.Button();
            this.textBoxEmotivFile = new System.Windows.Forms.TextBox();
            this.rbuttonEmotivFile = new System.Windows.Forms.RadioButton();
            this.rbuttonEmotivSignal = new System.Windows.Forms.RadioButton();
            this.cbEmotivDSP = new System.Windows.Forms.CheckBox();
            this.rbuttonEmotiv = new System.Windows.Forms.RadioButton();
            this.rbuttonWPFcharting = new System.Windows.Forms.RadioButton();
            this.rbuttonWindowsFormsCharting = new System.Windows.Forms.RadioButton();
            this.groupBoxCharting = new System.Windows.Forms.GroupBox();
            this.rbuttonExperimentator = new System.Windows.Forms.RadioButton();
            this.rbuttonFieldTrip = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbFieldTripDspMethod = new System.Windows.Forms.ComboBox();
            this.cbFieldTripDSP = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ndFieldTripPort = new System.Windows.Forms.NumericUpDown();
            this.tboxFieldTripHost = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxCharting.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndFieldTripPort)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStart.Location = new System.Drawing.Point(248, 355);
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
            this.label1.Location = new System.Drawing.Point(15, 39);
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
            this.menuStrip1.Size = new System.Drawing.Size(702, 24);
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
            this.tutorialToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.homepageToolStripMenuItem,
            this.openVibesHomepageToolStripMenuItem,
            this.octaveDownloadToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // tutorialToolStripMenuItem
            // 
            this.tutorialToolStripMenuItem.Name = "tutorialToolStripMenuItem";
            this.tutorialToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.tutorialToolStripMenuItem.Text = "Tutorial";
            this.tutorialToolStripMenuItem.Click += new System.EventHandler(this.tutorialToolStripMenuItem_Click);
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
            // octaveDownloadToolStripMenuItem
            // 
            this.octaveDownloadToolStripMenuItem.Name = "octaveDownloadToolStripMenuItem";
            this.octaveDownloadToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.octaveDownloadToolStripMenuItem.Text = "Octave download";
            this.octaveDownloadToolStripMenuItem.Click += new System.EventHandler(this.octaveDownloadToolStripMenuItem_Click);
            // 
            // textBoxOpenVibeWorkingFolder
            // 
            this.textBoxOpenVibeWorkingFolder.Location = new System.Drawing.Point(144, 79);
            this.textBoxOpenVibeWorkingFolder.Name = "textBoxOpenVibeWorkingFolder";
            this.textBoxOpenVibeWorkingFolder.Size = new System.Drawing.Size(242, 20);
            this.textBoxOpenVibeWorkingFolder.TabIndex = 4;
            this.textBoxOpenVibeWorkingFolder.Text = "D:\\e\\openvibe_src\\dist";
            // 
            // textBoxScenario
            // 
            this.textBoxScenario.Location = new System.Drawing.Point(94, 27);
            this.textBoxScenario.Name = "textBoxScenario";
            this.textBoxScenario.Size = new System.Drawing.Size(292, 20);
            this.textBoxScenario.TabIndex = 5;
            this.textBoxScenario.Text = "C:\\Users\\toncho\\Desktop\\Adastra\\scenarios\\signal-processing-VRPN-export.xml";
            // 
            // buttonSelectOpenVibeWorkingFolder
            // 
            this.buttonSelectOpenVibeWorkingFolder.Location = new System.Drawing.Point(392, 79);
            this.buttonSelectOpenVibeWorkingFolder.Name = "buttonSelectOpenVibeWorkingFolder";
            this.buttonSelectOpenVibeWorkingFolder.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectOpenVibeWorkingFolder.TabIndex = 6;
            this.buttonSelectOpenVibeWorkingFolder.Text = "Browse";
            this.buttonSelectOpenVibeWorkingFolder.UseVisualStyleBackColor = true;
            this.buttonSelectOpenVibeWorkingFolder.Click += new System.EventHandler(this.buttonSelectOpenVibeWorkingFolder_Click);
            // 
            // buttonSelectScenario
            // 
            this.buttonSelectScenario.Location = new System.Drawing.Point(392, 24);
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
            this.label2.Location = new System.Drawing.Point(12, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "OpenVibe working folder:";
            // 
            // buttonExit
            // 
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExit.Location = new System.Drawing.Point(572, 397);
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
            this.comboBoxScenarioType.Location = new System.Drawing.Point(96, 36);
            this.comboBoxScenarioType.Name = "comboBoxScenarioType";
            this.comboBoxScenarioType.Size = new System.Drawing.Size(597, 21);
            this.comboBoxScenarioType.TabIndex = 13;
            this.comboBoxScenarioType.SelectedIndexChanged += new System.EventHandler(this.comboBoxScenarioType_SelectedIndexChanged);
            // 
            // buttonEditScenario
            // 
            this.buttonEditScenario.Location = new System.Drawing.Point(473, 24);
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
            this.groupBox1.Location = new System.Drawing.Point(128, 88);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(566, 116);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "OpenVibe settings";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(183, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(243, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "You need to start OpenVibe Acquisition server first";
            this.label4.Visible = false;
            // 
            // rButtonRealtime
            // 
            this.rButtonRealtime.AutoSize = true;
            this.rButtonRealtime.Location = new System.Drawing.Point(108, 57);
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
            this.rButtonRecordedSignal.Location = new System.Drawing.Point(16, 57);
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
            this.label3.Location = new System.Drawing.Point(12, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Scenario path:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "(recommended)";
            // 
            // rbuttonOpenVibe
            // 
            this.rbuttonOpenVibe.AutoSize = true;
            this.rbuttonOpenVibe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbuttonOpenVibe.Location = new System.Drawing.Point(18, 88);
            this.rbuttonOpenVibe.Name = "rbuttonOpenVibe";
            this.rbuttonOpenVibe.Size = new System.Drawing.Size(104, 17);
            this.rbuttonOpenVibe.TabIndex = 20;
            this.rbuttonOpenVibe.Text = "use OpenVibe";
            this.rbuttonOpenVibe.UseVisualStyleBackColor = true;
            this.rbuttonOpenVibe.CheckedChanged += new System.EventHandler(this.rbuttonOpenVibe_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbEmotivDspMethod);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.buttonBrowseEmotivFile);
            this.groupBox2.Controls.Add(this.textBoxEmotivFile);
            this.groupBox2.Controls.Add(this.rbuttonEmotivFile);
            this.groupBox2.Controls.Add(this.rbuttonEmotivSignal);
            this.groupBox2.Controls.Add(this.cbEmotivDSP);
            this.groupBox2.Location = new System.Drawing.Point(128, 210);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(566, 103);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Emotiv EPOCH settings";
            // 
            // cbEmotivDspMethod
            // 
            this.cbEmotivDspMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmotivDspMethod.FormattingEnabled = true;
            this.cbEmotivDspMethod.Items.AddRange(new object[] {
            "Basic (ButterworthBandPass + Averaging)"});
            this.cbEmotivDspMethod.Location = new System.Drawing.Point(108, 73);
            this.cbEmotivDspMethod.Name = "cbEmotivDspMethod";
            this.cbEmotivDspMethod.Size = new System.Drawing.Size(221, 21);
            this.cbEmotivDspMethod.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(141, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(182, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Switch on and adjust Emotiv headset";
            this.label6.Visible = false;
            // 
            // buttonBrowseEmotivFile
            // 
            this.buttonBrowseEmotivFile.Location = new System.Drawing.Point(473, 19);
            this.buttonBrowseEmotivFile.Name = "buttonBrowseEmotivFile";
            this.buttonBrowseEmotivFile.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowseEmotivFile.TabIndex = 4;
            this.buttonBrowseEmotivFile.Text = "Browse";
            this.buttonBrowseEmotivFile.UseVisualStyleBackColor = true;
            this.buttonBrowseEmotivFile.Click += new System.EventHandler(this.buttonBrowseEmotivFile_Click);
            // 
            // textBoxEmotivFile
            // 
            this.textBoxEmotivFile.Location = new System.Drawing.Point(144, 21);
            this.textBoxEmotivFile.Name = "textBoxEmotivFile";
            this.textBoxEmotivFile.Size = new System.Drawing.Size(323, 20);
            this.textBoxEmotivFile.TabIndex = 3;
            // 
            // rbuttonEmotivFile
            // 
            this.rbuttonEmotivFile.AutoSize = true;
            this.rbuttonEmotivFile.Checked = true;
            this.rbuttonEmotivFile.Location = new System.Drawing.Point(16, 22);
            this.rbuttonEmotivFile.Name = "rbuttonEmotivFile";
            this.rbuttonEmotivFile.Size = new System.Drawing.Size(86, 17);
            this.rbuttonEmotivFile.TabIndex = 2;
            this.rbuttonEmotivFile.TabStop = true;
            this.rbuttonEmotivFile.Text = "Pre-recorded";
            this.rbuttonEmotivFile.UseVisualStyleBackColor = true;
            this.rbuttonEmotivFile.CheckedChanged += new System.EventHandler(this.rbuttonEmotivFile_CheckedChanged);
            // 
            // rbuttonEmotivSignal
            // 
            this.rbuttonEmotivSignal.AutoSize = true;
            this.rbuttonEmotivSignal.Location = new System.Drawing.Point(16, 45);
            this.rbuttonEmotivSignal.Name = "rbuttonEmotivSignal";
            this.rbuttonEmotivSignal.Size = new System.Drawing.Size(69, 17);
            this.rbuttonEmotivSignal.TabIndex = 1;
            this.rbuttonEmotivSignal.Text = "Real-time";
            this.rbuttonEmotivSignal.UseVisualStyleBackColor = true;
            this.rbuttonEmotivSignal.CheckedChanged += new System.EventHandler(this.rbuttonEmotivSignal_CheckedChanged);
            // 
            // cbEmotivDSP
            // 
            this.cbEmotivDSP.AutoSize = true;
            this.cbEmotivDSP.Location = new System.Drawing.Point(16, 75);
            this.cbEmotivDSP.Name = "cbEmotivDSP";
            this.cbEmotivDSP.Size = new System.Drawing.Size(77, 17);
            this.cbEmotivDSP.TabIndex = 0;
            this.cbEmotivDSP.Text = "Apply DSP";
            this.cbEmotivDSP.UseVisualStyleBackColor = true;
            // 
            // rbuttonEmotiv
            // 
            this.rbuttonEmotiv.AutoSize = true;
            this.rbuttonEmotiv.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbuttonEmotiv.Location = new System.Drawing.Point(18, 210);
            this.rbuttonEmotiv.Name = "rbuttonEmotiv";
            this.rbuttonEmotiv.Size = new System.Drawing.Size(87, 17);
            this.rbuttonEmotiv.TabIndex = 0;
            this.rbuttonEmotiv.Text = "use Emotiv";
            this.rbuttonEmotiv.UseVisualStyleBackColor = true;
            this.rbuttonEmotiv.CheckedChanged += new System.EventHandler(this.rbuttonEmotiv_CheckedChanged);
            // 
            // rbuttonWPFcharting
            // 
            this.rbuttonWPFcharting.AutoSize = true;
            this.rbuttonWPFcharting.Checked = true;
            this.rbuttonWPFcharting.Location = new System.Drawing.Point(15, 11);
            this.rbuttonWPFcharting.Name = "rbuttonWPFcharting";
            this.rbuttonWPFcharting.Size = new System.Drawing.Size(49, 17);
            this.rbuttonWPFcharting.TabIndex = 22;
            this.rbuttonWPFcharting.TabStop = true;
            this.rbuttonWPFcharting.Text = "WPF";
            this.rbuttonWPFcharting.UseVisualStyleBackColor = true;
            // 
            // rbuttonWindowsFormsCharting
            // 
            this.rbuttonWindowsFormsCharting.AutoSize = true;
            this.rbuttonWindowsFormsCharting.Location = new System.Drawing.Point(70, 11);
            this.rbuttonWindowsFormsCharting.Name = "rbuttonWindowsFormsCharting";
            this.rbuttonWindowsFormsCharting.Size = new System.Drawing.Size(100, 17);
            this.rbuttonWindowsFormsCharting.TabIndex = 23;
            this.rbuttonWindowsFormsCharting.Text = "Windows Forms";
            this.rbuttonWindowsFormsCharting.UseVisualStyleBackColor = true;
            // 
            // groupBoxCharting
            // 
            this.groupBoxCharting.Controls.Add(this.rbuttonWPFcharting);
            this.groupBoxCharting.Controls.Add(this.rbuttonWindowsFormsCharting);
            this.groupBoxCharting.Location = new System.Drawing.Point(517, 58);
            this.groupBoxCharting.Name = "groupBoxCharting";
            this.groupBoxCharting.Size = new System.Drawing.Size(176, 34);
            this.groupBoxCharting.TabIndex = 24;
            this.groupBoxCharting.TabStop = false;
            // 
            // rbuttonExperimentator
            // 
            this.rbuttonExperimentator.AutoSize = true;
            this.rbuttonExperimentator.Location = new System.Drawing.Point(18, 325);
            this.rbuttonExperimentator.Name = "rbuttonExperimentator";
            this.rbuttonExperimentator.Size = new System.Drawing.Size(77, 17);
            this.rbuttonExperimentator.TabIndex = 25;
            this.rbuttonExperimentator.Text = "Optimizator";
            this.rbuttonExperimentator.UseVisualStyleBackColor = true;
            this.rbuttonExperimentator.CheckedChanged += new System.EventHandler(this.rbuttonExperimentator_CheckedChanged);
            // 
            // rbuttonFieldTrip
            // 
            this.rbuttonFieldTrip.AutoSize = true;
            this.rbuttonFieldTrip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbuttonFieldTrip.Location = new System.Drawing.Point(18, 397);
            this.rbuttonFieldTrip.Name = "rbuttonFieldTrip";
            this.rbuttonFieldTrip.Size = new System.Drawing.Size(98, 17);
            this.rbuttonFieldTrip.TabIndex = 26;
            this.rbuttonFieldTrip.Text = "use FieldTrip";
            this.rbuttonFieldTrip.UseVisualStyleBackColor = true;
            this.rbuttonFieldTrip.Visible = false;
            this.rbuttonFieldTrip.CheckedChanged += new System.EventHandler(this.rbuttonFieldTrip_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbFieldTripDspMethod);
            this.groupBox3.Controls.Add(this.cbFieldTripDSP);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.ndFieldTripPort);
            this.groupBox3.Controls.Add(this.tboxFieldTripHost);
            this.groupBox3.Location = new System.Drawing.Point(0, 420);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(566, 81);
            this.groupBox3.TabIndex = 27;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "FieldTrip settings";
            this.groupBox3.Visible = false;
            // 
            // cbFieldTripDspMethod
            // 
            this.cbFieldTripDspMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFieldTripDspMethod.FormattingEnabled = true;
            this.cbFieldTripDspMethod.Items.AddRange(new object[] {
            "Basic (ButterworthBandPass + Averaging)"});
            this.cbFieldTripDspMethod.Location = new System.Drawing.Point(108, 46);
            this.cbFieldTripDspMethod.Name = "cbFieldTripDspMethod";
            this.cbFieldTripDspMethod.Size = new System.Drawing.Size(221, 21);
            this.cbFieldTripDspMethod.TabIndex = 8;
            // 
            // cbFieldTripDSP
            // 
            this.cbFieldTripDSP.AutoSize = true;
            this.cbFieldTripDSP.Location = new System.Drawing.Point(16, 48);
            this.cbFieldTripDSP.Name = "cbFieldTripDSP";
            this.cbFieldTripDSP.Size = new System.Drawing.Size(77, 17);
            this.cbFieldTripDSP.TabIndex = 7;
            this.cbFieldTripDSP.Text = "Apply DSP";
            this.cbFieldTripDSP.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(369, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Port:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Host:";
            // 
            // ndFieldTripPort
            // 
            this.ndFieldTripPort.Location = new System.Drawing.Point(404, 20);
            this.ndFieldTripPort.Maximum = new decimal(new int[] {
            65000,
            0,
            0,
            0});
            this.ndFieldTripPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ndFieldTripPort.Name = "ndFieldTripPort";
            this.ndFieldTripPort.Size = new System.Drawing.Size(63, 20);
            this.ndFieldTripPort.TabIndex = 1;
            this.ndFieldTripPort.Value = new decimal(new int[] {
            1979,
            0,
            0,
            0});
            // 
            // tboxFieldTripHost
            // 
            this.tboxFieldTripHost.Location = new System.Drawing.Point(53, 20);
            this.tboxFieldTripHost.Name = "tboxFieldTripHost";
            this.tboxFieldTripHost.Size = new System.Drawing.Size(276, 20);
            this.tboxFieldTripHost.TabIndex = 0;
            this.tboxFieldTripHost.Text = "localhost";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 436);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.rbuttonFieldTrip);
            this.Controls.Add(this.rbuttonExperimentator);
            this.Controls.Add(this.groupBoxCharting);
            this.Controls.Add(this.rbuttonOpenVibe);
            this.Controls.Add(this.rbuttonEmotiv);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBoxScenarioType);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Adastra - brain computer interface application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBoxCharting.ResumeLayout(false);
            this.groupBoxCharting.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ndFieldTripPort)).EndInit();
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
        private System.Windows.Forms.Button buttonEditScenario;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rButtonRealtime;
        private System.Windows.Forms.RadioButton rButtonRecordedSignal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rbuttonOpenVibe;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbuttonEmotiv;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem tutorialToolStripMenuItem;
        private System.Windows.Forms.RadioButton rbuttonWPFcharting;
        private System.Windows.Forms.RadioButton rbuttonWindowsFormsCharting;
        private System.Windows.Forms.GroupBox groupBoxCharting;
        private System.Windows.Forms.TextBox textBoxEmotivFile;
        private System.Windows.Forms.RadioButton rbuttonEmotivFile;
        private System.Windows.Forms.RadioButton rbuttonEmotivSignal;
        private System.Windows.Forms.CheckBox cbEmotivDSP;
        private System.Windows.Forms.Button buttonBrowseEmotivFile;
		private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbEmotivDspMethod;
        private System.Windows.Forms.RadioButton rbuttonExperimentator;
        private System.Windows.Forms.ToolStripMenuItem octaveDownloadToolStripMenuItem;
        private System.Windows.Forms.RadioButton rbuttonFieldTrip;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown ndFieldTripPort;
        private System.Windows.Forms.TextBox tboxFieldTripHost;
        private System.Windows.Forms.ComboBox cbFieldTripDspMethod;
        private System.Windows.Forms.CheckBox cbFieldTripDSP;
    }
}

