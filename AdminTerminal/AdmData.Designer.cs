namespace AdminTerminal
{
    partial class AdmData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdmData));
            panel8 = new Panel();
            panel11 = new Panel();
            button10 = new Button();
            button7 = new Button();
            button6 = new Button();
            panel10 = new Panel();
            dataGridView1 = new DataGridView();
            panel9 = new Panel();
            button8 = new Button();
            button5 = new Button();
            textBox3 = new TextBox();
            comboBox1 = new ComboBox();
            label4 = new Label();
            panel2 = new Panel();
            dataGridView3 = new DataGridView();
            dataGridView2 = new DataGridView();
            panel3 = new Panel();
            button9 = new Button();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            panel1 = new Panel();
            panel8.SuspendLayout();
            panel11.SuspendLayout();
            panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel9.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            panel3.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel8
            // 
            panel8.Controls.Add(panel11);
            panel8.Controls.Add(panel10);
            panel8.Controls.Add(panel9);
            panel8.Location = new Point(0, 0);
            panel8.Name = "panel8";
            panel8.Size = new Size(840, 640);
            panel8.TabIndex = 5;
            panel8.Paint += panel8_Paint;
            // 
            // panel11
            // 
            panel11.Controls.Add(button10);
            panel11.Controls.Add(button7);
            panel11.Controls.Add(button6);
            panel11.Location = new Point(10, 600);
            panel11.Name = "panel11";
            panel11.Size = new Size(810, 40);
            panel11.TabIndex = 2;
            // 
            // button10
            // 
            button10.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button10.Location = new Point(600, 0);
            button10.Name = "button10";
            button10.Size = new Size(90, 40);
            button10.TabIndex = 2;
            button10.Text = "撤销";
            button10.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button7.Location = new Point(720, 0);
            button7.Name = "button7";
            button7.Size = new Size(90, 40);
            button7.TabIndex = 1;
            button7.Text = "保存";
            button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button6.Location = new Point(480, 0);
            button6.Name = "button6";
            button6.Size = new Size(90, 40);
            button6.TabIndex = 0;
            button6.Text = "删除";
            button6.UseVisualStyleBackColor = true;
            // 
            // panel10
            // 
            panel10.Controls.Add(dataGridView1);
            panel10.Location = new Point(10, 60);
            panel10.Name = "panel10";
            panel10.Size = new Size(810, 535);
            panel10.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(5, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(800, 520);
            dataGridView1.TabIndex = 0;
            // 
            // panel9
            // 
            panel9.Controls.Add(button8);
            panel9.Controls.Add(button5);
            panel9.Controls.Add(textBox3);
            panel9.Controls.Add(comboBox1);
            panel9.Controls.Add(label4);
            panel9.Location = new Point(10, 10);
            panel9.Name = "panel9";
            panel9.Size = new Size(810, 50);
            panel9.TabIndex = 0;
            // 
            // button8
            // 
            button8.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button8.Location = new Point(690, 0);
            button8.Name = "button8";
            button8.Size = new Size(110, 40);
            button8.TabIndex = 4;
            button8.Text = "导入数据";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button5
            // 
            button5.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button5.Location = new Point(600, 2);
            button5.Name = "button5";
            button5.Size = new Size(80, 35);
            button5.TabIndex = 3;
            button5.Text = "查询";
            button5.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(290, 5);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(290, 27);
            textBox3.TabIndex = 2;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(160, 5);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(120, 28);
            comboBox1.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(5, 5);
            label4.Name = "label4";
            label4.Size = new Size(152, 27);
            label4.TabIndex = 0;
            label4.Text = "信息查询条件：";
            // 
            // panel2
            // 
            panel2.Controls.Add(dataGridView3);
            panel2.Controls.Add(dataGridView2);
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(button3);
            panel2.Controls.Add(button2);
            panel2.Controls.Add(button1);
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(837, 622);
            panel2.TabIndex = 1;
            // 
            // dataGridView3
            // 
            dataGridView3.BackgroundColor = Color.White;
            dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView3.Location = new Point(450, 0);
            dataGridView3.Name = "dataGridView3";
            dataGridView3.RowHeadersWidth = 51;
            dataGridView3.RowTemplate.Height = 29;
            dataGridView3.Size = new Size(380, 560);
            dataGridView3.TabIndex = 7;
            // 
            // dataGridView2
            // 
            dataGridView2.BackgroundColor = Color.White;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(10, 0);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 51;
            dataGridView2.RowTemplate.Height = 29;
            dataGridView2.Size = new Size(380, 560);
            dataGridView2.TabIndex = 6;
            // 
            // panel3
            // 
            panel3.Controls.Add(button9);
            panel3.Controls.Add(button4);
            panel3.Location = new Point(3, 579);
            panel3.Name = "panel3";
            panel3.Size = new Size(820, 40);
            panel3.TabIndex = 5;
            // 
            // button9
            // 
            button9.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button9.Location = new Point(730, 0);
            button9.Name = "button9";
            button9.Size = new Size(90, 40);
            button9.TabIndex = 0;
            button9.Text = "保存";
            button9.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button4.Location = new Point(590, 0);
            button4.Name = "button4";
            button4.Size = new Size(110, 40);
            button4.TabIndex = 3;
            button4.Text = "导入文件";
            button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.BackgroundImage = (Image)resources.GetObject("button3.BackgroundImage");
            button3.BackgroundImageLayout = ImageLayout.Stretch;
            button3.Location = new Point(400, 277);
            button3.Name = "button3";
            button3.Size = new Size(40, 35);
            button3.TabIndex = 4;
            button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.BackgroundImage = (Image)resources.GetObject("button2.BackgroundImage");
            button2.BackgroundImageLayout = ImageLayout.Stretch;
            button2.Location = new Point(400, 217);
            button2.Name = "button2";
            button2.Size = new Size(40, 35);
            button2.TabIndex = 3;
            button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Stretch;
            button1.Location = new Point(400, 157);
            button1.Name = "button1";
            button1.Size = new Size(40, 35);
            button1.TabIndex = 2;
            button1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(211, 226, 244);
            panel1.Controls.Add(panel8);
            panel1.Controls.Add(panel2);
            panel1.Location = new Point(20, 20);
            panel1.Name = "panel1";
            panel1.Size = new Size(840, 640);
            panel1.TabIndex = 0;
            // 
            // AdmData
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(182, 211, 245);
            ClientSize = new Size(880, 680);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AdmData";
            Text = "AdmData";
            Load += AdmData_Load;
            panel8.ResumeLayout(false);
            panel11.ResumeLayout(false);
            panel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel9.ResumeLayout(false);
            panel9.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            panel3.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel panel2;
        private Button button3;
        private Button button2;
        private Button button1;
        private Button button4;
        private Panel panel8;
        private Panel panel9;
        private Button button5;
        private TextBox textBox3;
        private ComboBox comboBox1;
        private Label label4;
        private Panel panel11;
        private Panel panel10;
        private Button button7;
        private Button button6;
        private Button button8;
        private Panel panel3;
        private Button button9;
        private Button button10;
        private DataGridView dataGridView1;
        private DataGridView dataGridView3;
        private DataGridView dataGridView2;
        private Panel panel1;
    }
}