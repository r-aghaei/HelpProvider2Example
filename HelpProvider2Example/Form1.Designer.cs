
namespace HelpProvider2Example
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
            this.button1 = new System.Windows.Forms.Button();
            this.helpProvider21 = new HelpProvider2Example.HelpProvider2();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.helpProvider21.SetHelpString(this.button1, "متن آزمایشی");
            this.button1.Location = new System.Drawing.Point(9, 10);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.helpProvider21.SetShowHelp(this.button1, true);
            this.button1.Size = new System.Drawing.Size(262, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "Click on help button, then click on me!";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // helpProvider21
            // 
            this.helpProvider21.BackColor = System.Drawing.SystemColors.Window;
            this.helpProvider21.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpProvider21.ForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 184);
            this.Controls.Add(this.button1);
            this.HelpButton = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private HelpProvider2 helpProvider21;
    }
}

