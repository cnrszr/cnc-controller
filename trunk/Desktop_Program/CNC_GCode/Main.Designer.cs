namespace CNC_GCode
{
    partial class Main
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
            this.openFileDialog_GCode = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gCodeToolStripMenuItem_GetCodes = new System.Windows.Forms.ToolStripMenuItem();
            this.getCodesInUseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeCommentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getCodeInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cNCMachineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.millCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBox_Commands = new System.Windows.Forms.RichTextBox();
            this.button_XUp = new System.Windows.Forms.Button();
            this.button_XDown = new System.Windows.Forms.Button();
            this.button_YUp = new System.Windows.Forms.Button();
            this.buttonYDown = new System.Windows.Forms.Button();
            this.button_ZUp = new System.Windows.Forms.Button();
            this.buttonZDown = new System.Windows.Forms.Button();
            this.groupBox_CNCControls = new System.Windows.Forms.GroupBox();
            this.button_Calibrated = new System.Windows.Forms.Button();
            this.button_StepOrRev = new System.Windows.Forms.Button();
            this.button_MachineZero = new System.Windows.Forms.Button();
            this.groupBox_Commands = new System.Windows.Forms.GroupBox();
            this.progressBar_Milling = new System.Windows.Forms.ProgressBar();
            this.generateMachineCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.groupBox_CNCControls.SuspendLayout();
            this.groupBox_Commands.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog_GCode
            // 
            this.openFileDialog_GCode.FileName = "openFileDialog_GCode";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.gCodeToolStripMenuItem_GetCodes,
            this.cNCMachineToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(938, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.closeToolStripMenuItem.Text = "Close File";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // gCodeToolStripMenuItem_GetCodes
            // 
            this.gCodeToolStripMenuItem_GetCodes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getCodesInUseToolStripMenuItem,
            this.removeCommentsToolStripMenuItem,
            this.getCodeInfoToolStripMenuItem,
            this.generateMachineCodeToolStripMenuItem});
            this.gCodeToolStripMenuItem_GetCodes.Name = "gCodeToolStripMenuItem_GetCodes";
            this.gCodeToolStripMenuItem_GetCodes.Size = new System.Drawing.Size(58, 20);
            this.gCodeToolStripMenuItem_GetCodes.Text = "G Code";
            // 
            // getCodesInUseToolStripMenuItem
            // 
            this.getCodesInUseToolStripMenuItem.Name = "getCodesInUseToolStripMenuItem";
            this.getCodesInUseToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.getCodesInUseToolStripMenuItem.Text = "Get Codes In Use";
            this.getCodesInUseToolStripMenuItem.Click += new System.EventHandler(this.getCodesInUseToolStripMenuItem_Click);
            // 
            // removeCommentsToolStripMenuItem
            // 
            this.removeCommentsToolStripMenuItem.Name = "removeCommentsToolStripMenuItem";
            this.removeCommentsToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.removeCommentsToolStripMenuItem.Text = "Remove Comments";
            this.removeCommentsToolStripMenuItem.Click += new System.EventHandler(this.removeCommentsToolStripMenuItem_Click);
            // 
            // getCodeInfoToolStripMenuItem
            // 
            this.getCodeInfoToolStripMenuItem.Name = "getCodeInfoToolStripMenuItem";
            this.getCodeInfoToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.getCodeInfoToolStripMenuItem.Text = "Get Code Info";
            this.getCodeInfoToolStripMenuItem.Click += new System.EventHandler(this.getCodeInfoToolStripMenuItem_Click);
            // 
            // cNCMachineToolStripMenuItem
            // 
            this.cNCMachineToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.millCodeToolStripMenuItem});
            this.cNCMachineToolStripMenuItem.Name = "cNCMachineToolStripMenuItem";
            this.cNCMachineToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.cNCMachineToolStripMenuItem.Text = "CNC Machine";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // millCodeToolStripMenuItem
            // 
            this.millCodeToolStripMenuItem.Name = "millCodeToolStripMenuItem";
            this.millCodeToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.millCodeToolStripMenuItem.Text = "Mill Code";
            this.millCodeToolStripMenuItem.Click += new System.EventHandler(this.millCodeToolStripMenuItem_Click);
            // 
            // richTextBox_Commands
            // 
            this.richTextBox_Commands.Location = new System.Drawing.Point(14, 20);
            this.richTextBox_Commands.Name = "richTextBox_Commands";
            this.richTextBox_Commands.Size = new System.Drawing.Size(241, 517);
            this.richTextBox_Commands.TabIndex = 1;
            this.richTextBox_Commands.Text = "";
            // 
            // button_XUp
            // 
            this.button_XUp.Location = new System.Drawing.Point(118, 74);
            this.button_XUp.Name = "button_XUp";
            this.button_XUp.Size = new System.Drawing.Size(50, 50);
            this.button_XUp.TabIndex = 2;
            this.button_XUp.Text = "→";
            this.button_XUp.UseVisualStyleBackColor = true;
            this.button_XUp.Click += new System.EventHandler(this.button_XUp_Click);
            // 
            // button_XDown
            // 
            this.button_XDown.Location = new System.Drawing.Point(21, 74);
            this.button_XDown.Name = "button_XDown";
            this.button_XDown.Size = new System.Drawing.Size(50, 50);
            this.button_XDown.TabIndex = 3;
            this.button_XDown.Text = "←";
            this.button_XDown.UseVisualStyleBackColor = true;
            this.button_XDown.Click += new System.EventHandler(this.button_XDown_Click);
            // 
            // button_YUp
            // 
            this.button_YUp.Location = new System.Drawing.Point(70, 18);
            this.button_YUp.Name = "button_YUp";
            this.button_YUp.Size = new System.Drawing.Size(50, 50);
            this.button_YUp.TabIndex = 4;
            this.button_YUp.Text = "↑";
            this.button_YUp.UseVisualStyleBackColor = true;
            this.button_YUp.Click += new System.EventHandler(this.button_YUp_Click);
            // 
            // buttonYDown
            // 
            this.buttonYDown.Location = new System.Drawing.Point(70, 130);
            this.buttonYDown.Name = "buttonYDown";
            this.buttonYDown.Size = new System.Drawing.Size(50, 50);
            this.buttonYDown.TabIndex = 5;
            this.buttonYDown.Text = "↓";
            this.buttonYDown.UseVisualStyleBackColor = true;
            this.buttonYDown.Click += new System.EventHandler(this.buttonYDown_Click);
            // 
            // button_ZUp
            // 
            this.button_ZUp.Location = new System.Drawing.Point(118, 209);
            this.button_ZUp.Name = "button_ZUp";
            this.button_ZUp.Size = new System.Drawing.Size(50, 50);
            this.button_ZUp.TabIndex = 6;
            this.button_ZUp.Text = "+";
            this.button_ZUp.UseVisualStyleBackColor = true;
            this.button_ZUp.Click += new System.EventHandler(this.button_ZUp_Click);
            // 
            // buttonZDown
            // 
            this.buttonZDown.Location = new System.Drawing.Point(21, 209);
            this.buttonZDown.Name = "buttonZDown";
            this.buttonZDown.Size = new System.Drawing.Size(50, 50);
            this.buttonZDown.TabIndex = 7;
            this.buttonZDown.Text = "-";
            this.buttonZDown.UseVisualStyleBackColor = true;
            this.buttonZDown.Click += new System.EventHandler(this.buttonZDown_Click);
            // 
            // groupBox_CNCControls
            // 
            this.groupBox_CNCControls.Controls.Add(this.button_Calibrated);
            this.groupBox_CNCControls.Controls.Add(this.button_StepOrRev);
            this.groupBox_CNCControls.Controls.Add(this.button_MachineZero);
            this.groupBox_CNCControls.Controls.Add(this.buttonZDown);
            this.groupBox_CNCControls.Controls.Add(this.button_ZUp);
            this.groupBox_CNCControls.Controls.Add(this.buttonYDown);
            this.groupBox_CNCControls.Controls.Add(this.button_YUp);
            this.groupBox_CNCControls.Controls.Add(this.button_XDown);
            this.groupBox_CNCControls.Controls.Add(this.button_XUp);
            this.groupBox_CNCControls.Location = new System.Drawing.Point(472, 27);
            this.groupBox_CNCControls.Name = "groupBox_CNCControls";
            this.groupBox_CNCControls.Size = new System.Drawing.Size(191, 373);
            this.groupBox_CNCControls.TabIndex = 8;
            this.groupBox_CNCControls.TabStop = false;
            this.groupBox_CNCControls.Text = "CNC Controls";
            this.groupBox_CNCControls.Visible = false;
            // 
            // button_Calibrated
            // 
            this.button_Calibrated.Location = new System.Drawing.Point(21, 334);
            this.button_Calibrated.Name = "button_Calibrated";
            this.button_Calibrated.Size = new System.Drawing.Size(164, 28);
            this.button_Calibrated.TabIndex = 10;
            this.button_Calibrated.Text = "Calibrated To Start";
            this.button_Calibrated.UseVisualStyleBackColor = true;
            this.button_Calibrated.Click += new System.EventHandler(this.button_Calibrated_Click);
            // 
            // button_StepOrRev
            // 
            this.button_StepOrRev.Location = new System.Drawing.Point(111, 279);
            this.button_StepOrRev.Name = "button_StepOrRev";
            this.button_StepOrRev.Size = new System.Drawing.Size(74, 49);
            this.button_StepOrRev.TabIndex = 9;
            this.button_StepOrRev.Text = "Set To Step";
            this.button_StepOrRev.UseVisualStyleBackColor = true;
            this.button_StepOrRev.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_MachineZero
            // 
            this.button_MachineZero.Location = new System.Drawing.Point(21, 279);
            this.button_MachineZero.Name = "button_MachineZero";
            this.button_MachineZero.Size = new System.Drawing.Size(74, 49);
            this.button_MachineZero.TabIndex = 8;
            this.button_MachineZero.Text = "Go to Machine Zero";
            this.button_MachineZero.UseVisualStyleBackColor = true;
            this.button_MachineZero.Click += new System.EventHandler(this.button_MachineZero_Click);
            // 
            // groupBox_Commands
            // 
            this.groupBox_Commands.Controls.Add(this.richTextBox_Commands);
            this.groupBox_Commands.Location = new System.Drawing.Point(669, 27);
            this.groupBox_Commands.Name = "groupBox_Commands";
            this.groupBox_Commands.Size = new System.Drawing.Size(269, 553);
            this.groupBox_Commands.TabIndex = 9;
            this.groupBox_Commands.TabStop = false;
            this.groupBox_Commands.Text = "Command Box";
            // 
            // progressBar_Milling
            // 
            this.progressBar_Milling.Location = new System.Drawing.Point(12, 560);
            this.progressBar_Milling.Name = "progressBar_Milling";
            this.progressBar_Milling.Size = new System.Drawing.Size(645, 20);
            this.progressBar_Milling.TabIndex = 10;
            // 
            // generateMachineCodeToolStripMenuItem
            // 
            this.generateMachineCodeToolStripMenuItem.Name = "generateMachineCodeToolStripMenuItem";
            this.generateMachineCodeToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.generateMachineCodeToolStripMenuItem.Text = "Generate Machine Code";
            this.generateMachineCodeToolStripMenuItem.Click += new System.EventHandler(this.generateMachineCodeToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 592);
            this.Controls.Add(this.progressBar_Milling);
            this.Controls.Add(this.groupBox_Commands);
            this.Controls.Add(this.groupBox_CNCControls);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "CNC";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox_CNCControls.ResumeLayout(false);
            this.groupBox_Commands.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog_GCode;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gCodeToolStripMenuItem_GetCodes;
        private System.Windows.Forms.ToolStripMenuItem getCodesInUseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richTextBox_Commands;
        private System.Windows.Forms.ToolStripMenuItem removeCommentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getCodeInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cNCMachineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button button_XUp;
        private System.Windows.Forms.Button button_XDown;
        private System.Windows.Forms.Button button_YUp;
        private System.Windows.Forms.Button buttonYDown;
        private System.Windows.Forms.Button button_ZUp;
        private System.Windows.Forms.Button buttonZDown;
        private System.Windows.Forms.GroupBox groupBox_CNCControls;
        private System.Windows.Forms.GroupBox groupBox_Commands;
        private System.Windows.Forms.Button button_MachineZero;
        private System.Windows.Forms.Button button_StepOrRev;
        private System.Windows.Forms.ToolStripMenuItem millCodeToolStripMenuItem;
        private System.Windows.Forms.Button button_Calibrated;
        public System.Windows.Forms.ProgressBar progressBar_Milling;
        private System.Windows.Forms.ToolStripMenuItem generateMachineCodeToolStripMenuItem;
    }
}

