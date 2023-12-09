namespace WinForm
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
            panel1.Location = new Point(37, 40);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(771, 565);
            panel1.TabIndex = 0;
            // 
            // Tiplab
            // 
            Tiplab.Anchor = AnchorStyles.None;
            Tiplab.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            Tiplab.ForeColor = Color.Red;
            Tiplab.Location = new Point(424, 233);
            Tiplab.Margin = new Padding(2, 0, 2, 0);
            Tiplab.Name = "Tiplab";
            Tiplab.Size = new Size(321, 21);
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
            Passwordtb.Location = new Point(521, 265);
            Passwordtb.Margin = new Padding(2, 3, 2, 3);
            Passwordtb.Name = "Passwordtb";
            Passwordtb.Size = new Size(204, 24);
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
            Accounttb.Location = new Point(521, 197);
            Accounttb.Margin = new Padding(2, 3, 2, 3);
            Accounttb.Name = "Accounttb";
            Accounttb.Size = new Size(204, 24);
            Accounttb.TabIndex = 7;
            Accounttb.KeyPress += Accounttb_KeyPress;
            // 
            // Fotgotlab
            // 
            Fotgotlab.Anchor = AnchorStyles.None;
            Fotgotlab.Font = new Font("方正兰亭特黑_GBK", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Fotgotlab.ForeColor = Color.FromArgb(45, 90, 152);
            Fotgotlab.Location = new Point(424, 314);
            Fotgotlab.Margin = new Padding(2, 0, 2, 0);
            Fotgotlab.Name = "Fotgotlab";
            Fotgotlab.Padding = new Padding(8, 0, 0, 0);
            Fotgotlab.Size = new Size(157, 21);
            Fotgotlab.TabIndex = 6;
            Fotgotlab.Text = "Forgot Password?";
            Fotgotlab.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Loginbtn
            // 
            Loginbtn.Anchor = AnchorStyles.None;
            Loginbtn.BackColor = Color.FromArgb(45, 90, 152);
            Loginbtn.Font = new Font("方正兰亭特黑_GBK", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Loginbtn.ForeColor = Color.White;
            Loginbtn.Location = new Point(507, 357);
            Loginbtn.Margin = new Padding(0);
            Loginbtn.Name = "Loginbtn";
            Loginbtn.Size = new Size(113, 42);
            Loginbtn.TabIndex = 4;
            Loginbtn.Text = "Login";
            Loginbtn.UseVisualStyleBackColor = false;
            Loginbtn.Click += Loginbtn_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(842, 640);
            Controls.Add(panel1);
            Margin = new Padding(2, 3, 2, 3);
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