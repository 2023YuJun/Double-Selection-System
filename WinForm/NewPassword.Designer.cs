namespace WinForm
{
    partial class NewPassword
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
            panel1 = new Panel();
            textBox1 = new TextBox();
            label1 = new Label();
            button1 = new Button();
            button2 = new Button();
            panel3 = new Panel();
            textBox3 = new TextBox();
            label3 = new Label();
            TipLabel = new Label();
            label2 = new Label();
            textBox2 = new TextBox();
            panel2 = new Panel();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(5, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(400, 60);
            panel1.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(90, 18);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(300, 29);
            textBox1.TabIndex = 1;
            textBox1.KeyPress += textBox1_KeyPress;
            // 
            // label1
            // 
            label1.Font = new Font("方正兰亭特黑_GBK", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(7, 20);
            label1.Name = "label1";
            label1.Size = new Size(80, 25);
            label1.TabIndex = 0;
            label1.Text = "设置密钥";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            button1.BackColor = Color.White;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(65, 330);
            button1.Name = "button1";
            button1.Size = new Size(120, 35);
            button1.TabIndex = 5;
            button1.Text = "确认";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.White;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Location = new Point(230, 330);
            button2.Name = "button2";
            button2.Size = new Size(120, 35);
            button2.TabIndex = 6;
            button2.Text = "取消";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(textBox3);
            panel3.Controls.Add(label3);
            panel3.Location = new Point(5, 144);
            panel3.Name = "panel3";
            panel3.Size = new Size(400, 60);
            panel3.TabIndex = 2;
            // 
            // textBox3
            // 
            textBox3.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            textBox3.Location = new Point(90, 18);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(300, 29);
            textBox3.TabIndex = 1;
            textBox3.KeyPress += textBox3_KeyPress;
            // 
            // label3
            // 
            label3.Font = new Font("方正兰亭特黑_GBK", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(7, 20);
            label3.Name = "label3";
            label3.Size = new Size(80, 25);
            label3.TabIndex = 0;
            label3.Text = "密码确认";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TipLabel
            // 
            TipLabel.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            TipLabel.ForeColor = Color.Red;
            TipLabel.Location = new Point(5, 290);
            TipLabel.Name = "TipLabel";
            TipLabel.Size = new Size(400, 25);
            TipLabel.TabIndex = 7;
            TipLabel.Text = "提示：";
            TipLabel.TextAlign = ContentAlignment.MiddleLeft;
            TipLabel.Visible = false;
            // 
            // label2
            // 
            label2.Font = new Font("方正兰亭特黑_GBK", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(7, 20);
            label2.Name = "label2";
            label2.Size = new Size(80, 25);
            label2.TabIndex = 0;
            label2.Text = "新密码";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            textBox2.Location = new Point(90, 18);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(300, 29);
            textBox2.TabIndex = 1;
            textBox2.KeyPress += textBox2_KeyPress;
            // 
            // panel2
            // 
            panel2.Controls.Add(textBox2);
            panel2.Controls.Add(label2);
            panel2.Location = new Point(5, 78);
            panel2.Name = "panel2";
            panel2.Size = new Size(400, 60);
            panel2.TabIndex = 2;
            // 
            // NewPassword
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(211, 226, 244);
            ClientSize = new Size(410, 380);
            Controls.Add(TipLabel);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "NewPassword";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NewPassword";
            Load += NewPassword_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private TextBox textBox1;
        private Label label1;
        private Button button1;
        private Button button2;
        private Panel panel3;
        private TextBox textBox3;
        private Label label3;
        private Label TipLabel;
        private Label label2;
        private TextBox textBox2;
        private Panel panel2;
    }
}