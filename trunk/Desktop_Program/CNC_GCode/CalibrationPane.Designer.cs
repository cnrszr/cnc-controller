namespace CNC_GCode
{
    partial class CalibrationPane
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
            this.groupBox_CNCControls = new System.Windows.Forms.GroupBox();
            this.button_StepOrRev = new System.Windows.Forms.Button();
            this.button_MachineZero = new System.Windows.Forms.Button();
            this.buttonZDown = new System.Windows.Forms.Button();
            this.button_ZUp = new System.Windows.Forms.Button();
            this.buttonYDown = new System.Windows.Forms.Button();
            this.button_YUp = new System.Windows.Forms.Button();
            this.button_XDown = new System.Windows.Forms.Button();
            this.button_XUp = new System.Windows.Forms.Button();
            this.button_Done = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox_CNCControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_CNCControls
            // 
            this.groupBox_CNCControls.Controls.Add(this.button_StepOrRev);
            this.groupBox_CNCControls.Controls.Add(this.button_MachineZero);
            this.groupBox_CNCControls.Controls.Add(this.buttonZDown);
            this.groupBox_CNCControls.Controls.Add(this.button_ZUp);
            this.groupBox_CNCControls.Controls.Add(this.buttonYDown);
            this.groupBox_CNCControls.Controls.Add(this.button_YUp);
            this.groupBox_CNCControls.Controls.Add(this.button_XDown);
            this.groupBox_CNCControls.Controls.Add(this.button_XUp);
            this.groupBox_CNCControls.Location = new System.Drawing.Point(12, 79);
            this.groupBox_CNCControls.Name = "groupBox_CNCControls";
            this.groupBox_CNCControls.Size = new System.Drawing.Size(191, 347);
            this.groupBox_CNCControls.TabIndex = 9;
            this.groupBox_CNCControls.TabStop = false;
            this.groupBox_CNCControls.Text = "CNC Controls";
            // 
            // button_StepOrRev
            // 
            this.button_StepOrRev.Location = new System.Drawing.Point(111, 279);
            this.button_StepOrRev.Name = "button_StepOrRev";
            this.button_StepOrRev.Size = new System.Drawing.Size(74, 49);
            this.button_StepOrRev.TabIndex = 9;
            this.button_StepOrRev.Text = "Set To Step";
            this.button_StepOrRev.UseVisualStyleBackColor = true;
            this.button_StepOrRev.Click += new System.EventHandler(this.button_StepOrRev_Click);
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
            // button_Done
            // 
            this.button_Done.Location = new System.Drawing.Point(57, 432);
            this.button_Done.Name = "button_Done";
            this.button_Done.Size = new System.Drawing.Size(75, 23);
            this.button_Done.TabIndex = 10;
            this.button_Done.Text = "Done";
            this.button_Done.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Location = new System.Drawing.Point(18, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(179, 56);
            this.richTextBox1.TabIndex = 11;
            this.richTextBox1.Text = "Please Calibrate the Spindle to the Start Position. Ensure the tip of the tool is" +
    " in contact with the Object";
            // 
            // CalibrationPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 466);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button_Done);
            this.Controls.Add(this.groupBox_CNCControls);
            this.Name = "CalibrationPane";
            this.Text = "CalibrationPane";
            this.groupBox_CNCControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_CNCControls;
        private System.Windows.Forms.Button button_StepOrRev;
        private System.Windows.Forms.Button button_MachineZero;
        private System.Windows.Forms.Button buttonZDown;
        private System.Windows.Forms.Button button_ZUp;
        private System.Windows.Forms.Button buttonYDown;
        private System.Windows.Forms.Button button_YUp;
        private System.Windows.Forms.Button button_XDown;
        private System.Windows.Forms.Button button_XUp;
        private System.Windows.Forms.Button button_Done;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}