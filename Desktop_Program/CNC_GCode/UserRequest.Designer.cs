namespace CNC_GCode
{
    partial class UserRequest
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
            this.richTextBox_Message = new System.Windows.Forms.RichTextBox();
            this.button_Okay = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox_Message
            // 
            this.richTextBox_Message.BackColor = System.Drawing.SystemColors.Menu;
            this.richTextBox_Message.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox_Message.Location = new System.Drawing.Point(13, 13);
            this.richTextBox_Message.Name = "richTextBox_Message";
            this.richTextBox_Message.ReadOnly = true;
            this.richTextBox_Message.Size = new System.Drawing.Size(181, 78);
            this.richTextBox_Message.TabIndex = 0;
            this.richTextBox_Message.Text = "";
            // 
            // button_Okay
            // 
            this.button_Okay.Location = new System.Drawing.Point(63, 97);
            this.button_Okay.Name = "button_Okay";
            this.button_Okay.Size = new System.Drawing.Size(75, 23);
            this.button_Okay.TabIndex = 1;
            this.button_Okay.Text = "Okay";
            this.button_Okay.UseVisualStyleBackColor = true;
            // 
            // UserRequest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(206, 137);
            this.Controls.Add(this.button_Okay);
            this.Controls.Add(this.richTextBox_Message);
            this.Name = "UserRequest";
            this.Text = "UserRequest";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_Message;
        private System.Windows.Forms.Button button_Okay;
    }
}