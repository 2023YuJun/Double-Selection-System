namespace TeacherTerminal
{
    partial class TeacherSelectionTeam
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
            panel4 = new Panel();
            panel3 = new Panel();
            textBox2 = new TextBox();
            button6 = new Button();
            button5 = new Button();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            dataGridView = new DataGridView();
            panel2 = new Panel();
            button1 = new Button();
            textBox1 = new TextBox();
            comboBox = new ComboBox();
            label1 = new Label();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(211, 226, 244);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(dataGridView);
            panel1.Controls.Add(panel2);
            panel1.Location = new Point(20, 20);
            panel1.Name = "panel1";
            panel1.Size = new Size(840, 640);
            panel1.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.AutoScroll = true;
            panel4.Location = new Point(570, 76);
            panel4.Name = "panel4";
            panel4.Size = new Size(250, 466);
            panel4.TabIndex = 15;
            // 
            // panel3
            // 
            panel3.Controls.Add(textBox2);
            panel3.Controls.Add(button6);
            panel3.Controls.Add(button5);
            panel3.Controls.Add(button4);
            panel3.Controls.Add(button3);
            panel3.Location = new Point(46, 546);
            panel3.Name = "panel3";
            panel3.Size = new Size(489, 33);
            panel3.TabIndex = 14;
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Font = new Font("Microsoft YaHei UI", 12.5F, FontStyle.Regular, GraphicsUnit.Point);
            textBox2.Location = new Point(205, 2);
            textBox2.Margin = new Padding(0);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(81, 27);
            textBox2.TabIndex = 0;
            textBox2.Text = "1";
            textBox2.TextAlign = HorizontalAlignment.Center;
            textBox2.KeyPress += textBox2_KeyPress;
            // 
            // button6
            // 
            button6.BackColor = Color.White;
            button6.FlatAppearance.BorderSize = 0;
            button6.FlatStyle = FlatStyle.Flat;
            button6.Font = new Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button6.Location = new Point(368, 2);
            button6.Name = "button6";
            button6.Size = new Size(70, 27);
            button6.TabIndex = 11;
            button6.Text = ">>";
            button6.UseVisualStyleBackColor = false;
            button6.Click += button6_Click;
            // 
            // button5
            // 
            button5.BackColor = Color.White;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatStyle = FlatStyle.Flat;
            button5.Font = new Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button5.Location = new Point(292, 2);
            button5.Name = "button5";
            button5.Size = new Size(70, 27);
            button5.TabIndex = 10;
            button5.Text = ">";
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.White;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Font = new Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button4.Location = new Point(130, 2);
            button4.Name = "button4";
            button4.Size = new Size(70, 27);
            button4.TabIndex = 9;
            button4.Text = "<";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.White;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button3.Location = new Point(54, 2);
            button3.Name = "button3";
            button3.Size = new Size(70, 27);
            button3.TabIndex = 8;
            button3.Text = "<<";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.White;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Location = new Point(700, 587);
            button2.Name = "button2";
            button2.Size = new Size(120, 35);
            button2.TabIndex = 5;
            button2.Text = "带领确认";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.GridColor = Color.White;
            dataGridView.Location = new Point(20, 76);
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.RowHeadersWidth = 51;
            dataGridView.RowTemplate.Height = 29;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.Size = new Size(544, 466);
            dataGridView.TabIndex = 5;
            // 
            // panel2
            // 
            panel2.Controls.Add(button1);
            panel2.Controls.Add(textBox1);
            panel2.Controls.Add(comboBox);
            panel2.Controls.Add(label1);
            panel2.Location = new Point(20, 20);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 50);
            panel2.TabIndex = 4;
            // 
            // button1
            // 
            button1.BackColor = Color.White;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(675, 9);
            button1.Name = "button1";
            button1.Size = new Size(120, 35);
            button1.TabIndex = 4;
            button1.Text = "搜索";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.White;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("方正兰亭特黑_GBK", 15F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(325, 11);
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(330, 32);
            textBox1.TabIndex = 3;
            // 
            // comboBox
            // 
            comboBox.Font = new Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
            comboBox.FormattingEnabled = true;
            comboBox.Location = new Point(150, 10);
            comboBox.Name = "comboBox";
            comboBox.Size = new Size(160, 33);
            comboBox.TabIndex = 1;
            // 
            // label1
            // 
            label1.Font = new Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(5, 15);
            label1.Name = "label1";
            label1.Size = new Size(140, 25);
            label1.TabIndex = 0;
            label1.Text = "搜索学生队伍";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TeacherSelectionTeam
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(182, 211, 245);
            ClientSize = new Size(880, 680);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "TeacherSelectionTeam";
            Text = "TeacherSelectionTeam";
            Load += TeacherSelectionTeam_Load;
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private DataGridView dataGridView;
        private Panel panel2;
        private Button button1;
        private TextBox textBox1;
        private ComboBox comboBox;
        private Label label1;
        private Button button2;
        private Panel panel3;
        private TextBox textBox2;
        private Button button6;
        private Button button5;
        private Button button4;
        private Button button3;
        private Panel panel4;
    }
}