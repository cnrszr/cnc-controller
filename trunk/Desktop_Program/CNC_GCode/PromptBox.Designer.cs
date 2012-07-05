namespace CNC_GCode
{
    partial class PromptBox
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
            this.label_text = new System.Windows.Forms.Label();
            this.textBox_Input = new System.Windows.Forms.TextBox();
            this.button_OK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_text
            // 
            this.label_text.AutoSize = true;
            this.label_text.Location = new System.Drawing.Point(13, 18);
            this.label_text.Name = "label_text";
            this.label_text.Size = new System.Drawing.Size(62, 13);
            this.label_text.TabIndex = 0;
            this.label_text.Text = "Enter Value";
            // 
            // textBox_Input
            // 
            this.textBox_Input.AcceptsReturn = true;
            this.textBox_Input.Location = new System.Drawing.Point(16, 46);
            this.textBox_Input.Name = "textBox_Input";
            this.textBox_Input.Size = new System.Drawing.Size(180, 20);
            this.textBox_Input.TabIndex = 1;
            this.textBox_Input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_Input_KeyDown);
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(67, 72);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 2;
            this.button_OK.Text = "OK";
            this.button_OK.UseVisualStyleBackColor = true;
            // 
            // PromptBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(208, 106);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.textBox_Input);
            this.Controls.Add(this.label_text);
            this.Name = "PromptBox";
            this.Text = "PromptBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label label_text;
        public System.Windows.Forms.TextBox textBox_Input;
        public System.Windows.Forms.Button button_OK;


    }
}