namespace LetterAmazer.Tools.CountryNameAdder
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
            this.sourceTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.translatedTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.languageTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // sourceTextBox
            // 
            this.sourceTextBox.Location = new System.Drawing.Point(12, 29);
            this.sourceTextBox.Multiline = true;
            this.sourceTextBox.Name = "sourceTextBox";
            this.sourceTextBox.Size = new System.Drawing.Size(236, 552);
            this.sourceTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Countries:";
            // 
            // translatedTextBox
            // 
            this.translatedTextBox.Location = new System.Drawing.Point(279, 66);
            this.translatedTextBox.Multiline = true;
            this.translatedTextBox.Name = "translatedTextBox";
            this.translatedTextBox.Size = new System.Drawing.Size(236, 515);
            this.translatedTextBox.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(522, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "add names";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(276, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Translated version";
            // 
            // languageTextBox
            // 
            this.languageTextBox.Location = new System.Drawing.Point(279, 13);
            this.languageTextBox.Name = "languageTextBox";
            this.languageTextBox.Size = new System.Drawing.Size(100, 20);
            this.languageTextBox.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 609);
            this.Controls.Add(this.languageTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.translatedTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sourceTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sourceTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox translatedTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox languageTextBox;

    }
}

