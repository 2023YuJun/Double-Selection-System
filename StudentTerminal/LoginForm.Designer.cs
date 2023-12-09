namespace StudentTerminal
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            panel1 = new Panel();
            Tiplab = new Label();
            Passwordtb = new TextBox();
            Accounttb = new TextBox();
            Fotgotlab = new Label();
            Loginbtn = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.None;
            panel1.BackColor = Color.Transparent;
            panel1.BackgroundImage = (Image)resources.GetObject("panel1.BackgroundImage");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.Controls.Add(Tiplab);
            panel1.Controls.Add(Passwordtb);
            panel1.Controls.Add(Accounttb);
            panel1.Controls.Add(Fotgotlab);
            panel1.Controls.Add(Loginbtn);
            panel1.Location = new Point(48, 47);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(991, 665);
            panel1.TabIndex = 0;
            // 
            // Tiplab
            // 
            Tiplab.Anchor = AnchorStyles.None;
            Tiplab.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            Tiplab.ForeColor = Color.Red;
            Tiplab.Location = new Point(545, 274);
            Tiplab.Name = "Tiplab";
            Tiplab.Size = new Size(413, 25);
            Tiplab.TabIndex = 9;
            Tiplab.Text = "The user name or password cannot be empty";
            Tiplab.TextAlign = ContentAlignment.MiddleLeft;
            Tiplab.Visible = false;
            // 
            // Passwordtb
            // 
            Passwordtb.Anchor = AnchorStyles.None;
            Passwordtb.BackColor = Color.White;
            Passwordtb.BorderStyle = BorderStyle.None;
            Passwordtb.Cursor = Cursors.IBeam;
            Passwordtb.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            Passwordtb.Location = new Point(670, 312);
            Passwordtb.Margin = new Padding(3, 4, 3, 4);
            Passwordtb.Name = "Passwordtb";
            Passwordtb.Size = new Size(262, 30);
            Passwordtb.TabIndex = 8;
            Passwordtb.UseSystemPasswordChar = true;
            Passwordtb.KeyPress += Passwordtb_KeyPress;
            // 
            // Accounttb
            // 
            Accounttb.Anchor = AnchorStyles.None;
            Accounttb.BackColor = Color.White;
            Accounttb.BorderStyle = BorderStyle.None;
            Accounttb.Cursor = Cursors.IBeam;
            Accounttb.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            Accounttb.Location = new Point(670, 232);
            Accounttb.Margin = new Padding(3, 4, 3, 4);
            Accounttb.Name = "Accounttb";
            Accounttb.Size = new Size(262, 30);
            Accounttb.TabIndex = 7;
            Accounttb.KeyPress += Accounttb_KeyPress;
            // 
            // Fotgotlab
            // 
            Fotgotlab.Anchor = AnchorStyles.None;
            Fotgotlab.Font = new Font("方正兰亭特黑_GBK", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Fotgotlab.ForeColor = Color.FromArgb(45, 90, 152);
            Fotgotlab.Location = new Point(545, 369);
            Fotgotlab.Name = "Fotgotlab";
            Fotgotlab.Padding = new Padding(10, 0, 0, 0);
            Fotgotlab.Size = new Size(202, 25);
            Fotgotlab.TabIndex = 6;
            Fotgotlab.Text = "Forgot Password?";
            Fotgotlab.TextAlign = ContentAlignment.MiddleLeft;
            Fotgotlab.Click += Fotgotlab_Click;
            // 
            // Loginbtn
            // 
            Loginbtn.Anchor = AnchorStyles.None;
            Loginbtn.BackColor = Color.FromArgb(45, 90, 152);
            Loginbtn.Font = new Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Loginbtn.ForeColor = Color.White;
            Loginbtn.Location = new Point(652, 420);
            Loginbtn.Margin = new Padding(0);
            Loginbtn.Name = "Loginbtn";
            Loginbtn.Size = new Size(145, 49);
            Loginbtn.TabIndex = 4;
            Loginbtn.Text = "Login";
            Loginbtn.UseVisualStyleBackColor = false;
            Loginbtn.Click += Loginbtn_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1083, 753);
            Controls.Add(panel1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LoginForm";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button Loginbtn;
        private Label Fotgotlab;
        private TextBox Passwordtb;
        private TextBox Accounttb;
        private Label Tiplab;
    }
}