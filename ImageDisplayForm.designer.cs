namespace GBRead
{
    partial class ImageDisplayForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.sizeBar = new System.Windows.Forms.TrackBar();
            this.scaleBar = new System.Windows.Forms.TrackBar();
            this.sizeLabel = new System.Windows.Forms.Label();
            this.scaleLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.color1RedBox = new System.Windows.Forms.ComboBox();
            this.color1BlueBox = new System.Windows.Forms.ComboBox();
            this.color1GreenBox = new System.Windows.Forms.ComboBox();
            this.color2GreenBox = new System.Windows.Forms.ComboBox();
            this.color2BlueBox = new System.Windows.Forms.ComboBox();
            this.color2RedBox = new System.Windows.Forms.ComboBox();
            this.color4GreenBox = new System.Windows.Forms.ComboBox();
            this.color4BlueBox = new System.Windows.Forms.ComboBox();
            this.color4RedBox = new System.Windows.Forms.ComboBox();
            this.color3GreenBox = new System.Windows.Forms.ComboBox();
            this.color3BlueBox = new System.Windows.Forms.ComboBox();
            this.color3RedBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.color1Panel = new System.Windows.Forms.Panel();
            this.color2Panel = new System.Windows.Forms.Panel();
            this.color3Panel = new System.Windows.Forms.Panel();
            this.color4Panel = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleBar)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(266, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 128);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // sizeBar
            // 
            this.sizeBar.LargeChange = 1;
            this.sizeBar.Location = new System.Drawing.Point(12, 25);
            this.sizeBar.Maximum = 20;
            this.sizeBar.Minimum = 1;
            this.sizeBar.Name = "sizeBar";
            this.sizeBar.Size = new System.Drawing.Size(248, 45);
            this.sizeBar.TabIndex = 3;
            this.sizeBar.Value = 16;
            this.sizeBar.Scroll += new System.EventHandler(this.sizeBar_Scroll);
            // 
            // scaleBar
            // 
            this.scaleBar.LargeChange = 1;
            this.scaleBar.Location = new System.Drawing.Point(15, 89);
            this.scaleBar.Maximum = 4;
            this.scaleBar.Minimum = 1;
            this.scaleBar.Name = "scaleBar";
            this.scaleBar.Size = new System.Drawing.Size(245, 45);
            this.scaleBar.TabIndex = 4;
            this.scaleBar.Value = 1;
            this.scaleBar.Scroll += new System.EventHandler(this.scaleBar_Scroll);
            // 
            // sizeLabel
            // 
            this.sizeLabel.AutoSize = true;
            this.sizeLabel.Location = new System.Drawing.Point(12, 9);
            this.sizeLabel.Name = "sizeLabel";
            this.sizeLabel.Size = new System.Drawing.Size(129, 13);
            this.sizeLabel.TabIndex = 9;
            this.sizeLabel.Text = "Image Preview Size: _ x _";
            // 
            // scaleLabel
            // 
            this.scaleLabel.AutoSize = true;
            this.scaleLabel.Location = new System.Drawing.Point(12, 73);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(86, 13);
            this.scaleLabel.TabIndex = 10;
            this.scaleLabel.Text = "Image Scale: _ x";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Color 1";
            // 
            // color1RedBox
            // 
            this.color1RedBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.color1RedBox.FormattingEnabled = true;
            this.color1RedBox.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "0A",
            "0B",
            "0C",
            "0D",
            "0E",
            "0F",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "1A",
            "1B",
            "1C",
            "1D",
            "1E",
            "1F"});
            this.color1RedBox.Location = new System.Drawing.Point(58, 136);
            this.color1RedBox.Name = "color1RedBox";
            this.color1RedBox.Size = new System.Drawing.Size(38, 19);
            this.color1RedBox.TabIndex = 5;
            this.color1RedBox.SelectedIndexChanged += new System.EventHandler(this.color1_SelectedIndexChanged);
            // 
            // color1BlueBox
            // 
            this.color1BlueBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.color1BlueBox.FormattingEnabled = true;
            this.color1BlueBox.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "0A",
            "0B",
            "0C",
            "0D",
            "0E",
            "0F",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "1A",
            "1B",
            "1C",
            "1D",
            "1E",
            "1F"});
            this.color1BlueBox.Location = new System.Drawing.Point(146, 136);
            this.color1BlueBox.Name = "color1BlueBox";
            this.color1BlueBox.Size = new System.Drawing.Size(38, 19);
            this.color1BlueBox.TabIndex = 7;
            this.color1BlueBox.SelectedIndexChanged += new System.EventHandler(this.color1_SelectedIndexChanged);
            // 
            // color1GreenBox
            // 
            this.color1GreenBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.color1GreenBox.FormattingEnabled = true;
            this.color1GreenBox.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "0A",
            "0B",
            "0C",
            "0D",
            "0E",
            "0F",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "1A",
            "1B",
            "1C",
            "1D",
            "1E",
            "1F"});
            this.color1GreenBox.Location = new System.Drawing.Point(102, 136);
            this.color1GreenBox.Name = "color1GreenBox";
            this.color1GreenBox.Size = new System.Drawing.Size(38, 19);
            this.color1GreenBox.TabIndex = 6;
            this.color1GreenBox.SelectedIndexChanged += new System.EventHandler(this.color1_SelectedIndexChanged);
            // 
            // color2GreenBox
            // 
            this.color2GreenBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.color2GreenBox.FormattingEnabled = true;
            this.color2GreenBox.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "0A",
            "0B",
            "0C",
            "0D",
            "0E",
            "0F",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "1A",
            "1B",
            "1C",
            "1D",
            "1E",
            "1F"});
            this.color2GreenBox.Location = new System.Drawing.Point(102, 161);
            this.color2GreenBox.Name = "color2GreenBox";
            this.color2GreenBox.Size = new System.Drawing.Size(38, 19);
            this.color2GreenBox.TabIndex = 9;
            this.color2GreenBox.SelectedIndexChanged += new System.EventHandler(this.color2_SelectedIndexChanged);
            // 
            // color2BlueBox
            // 
            this.color2BlueBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.color2BlueBox.FormattingEnabled = true;
            this.color2BlueBox.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "0A",
            "0B",
            "0C",
            "0D",
            "0E",
            "0F",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "1A",
            "1B",
            "1C",
            "1D",
            "1E",
            "1F"});
            this.color2BlueBox.Location = new System.Drawing.Point(146, 161);
            this.color2BlueBox.Name = "color2BlueBox";
            this.color2BlueBox.Size = new System.Drawing.Size(38, 19);
            this.color2BlueBox.TabIndex = 10;
            this.color2BlueBox.SelectedIndexChanged += new System.EventHandler(this.color2_SelectedIndexChanged);
            // 
            // color2RedBox
            // 
            this.color2RedBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.color2RedBox.FormattingEnabled = true;
            this.color2RedBox.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "0A",
            "0B",
            "0C",
            "0D",
            "0E",
            "0F",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "1A",
            "1B",
            "1C",
            "1D",
            "1E",
            "1F"});
            this.color2RedBox.Location = new System.Drawing.Point(58, 161);
            this.color2RedBox.Name = "color2RedBox";
            this.color2RedBox.Size = new System.Drawing.Size(38, 19);
            this.color2RedBox.TabIndex = 8;
            this.color2RedBox.SelectedIndexChanged += new System.EventHandler(this.color2_SelectedIndexChanged);
            // 
            // color4GreenBox
            // 
            this.color4GreenBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.color4GreenBox.FormattingEnabled = true;
            this.color4GreenBox.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "0A",
            "0B",
            "0C",
            "0D",
            "0E",
            "0F",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "1A",
            "1B",
            "1C",
            "1D",
            "1E",
            "1F"});
            this.color4GreenBox.Location = new System.Drawing.Point(102, 211);
            this.color4GreenBox.Name = "color4GreenBox";
            this.color4GreenBox.Size = new System.Drawing.Size(38, 19);
            this.color4GreenBox.TabIndex = 15;
            this.color4GreenBox.SelectedIndexChanged += new System.EventHandler(this.color4_SelectedIndexChanged);
            // 
            // color4BlueBox
            // 
            this.color4BlueBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.color4BlueBox.FormattingEnabled = true;
            this.color4BlueBox.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "0A",
            "0B",
            "0C",
            "0D",
            "0E",
            "0F",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "1A",
            "1B",
            "1C",
            "1D",
            "1E",
            "1F"});
            this.color4BlueBox.Location = new System.Drawing.Point(146, 211);
            this.color4BlueBox.Name = "color4BlueBox";
            this.color4BlueBox.Size = new System.Drawing.Size(38, 19);
            this.color4BlueBox.TabIndex = 16;
            this.color4BlueBox.SelectedIndexChanged += new System.EventHandler(this.color4_SelectedIndexChanged);
            // 
            // color4RedBox
            // 
            this.color4RedBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.color4RedBox.FormattingEnabled = true;
            this.color4RedBox.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "0A",
            "0B",
            "0C",
            "0D",
            "0E",
            "0F",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "1A",
            "1B",
            "1C",
            "1D",
            "1E",
            "1F"});
            this.color4RedBox.Location = new System.Drawing.Point(58, 211);
            this.color4RedBox.Name = "color4RedBox";
            this.color4RedBox.Size = new System.Drawing.Size(38, 19);
            this.color4RedBox.TabIndex = 14;
            this.color4RedBox.SelectedIndexChanged += new System.EventHandler(this.color4_SelectedIndexChanged);
            // 
            // color3GreenBox
            // 
            this.color3GreenBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.color3GreenBox.FormattingEnabled = true;
            this.color3GreenBox.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "0A",
            "0B",
            "0C",
            "0D",
            "0E",
            "0F",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "1A",
            "1B",
            "1C",
            "1D",
            "1E",
            "1F"});
            this.color3GreenBox.Location = new System.Drawing.Point(102, 186);
            this.color3GreenBox.Name = "color3GreenBox";
            this.color3GreenBox.Size = new System.Drawing.Size(38, 19);
            this.color3GreenBox.TabIndex = 12;
            this.color3GreenBox.SelectedIndexChanged += new System.EventHandler(this.color3_SelectedIndexChanged);
            // 
            // color3BlueBox
            // 
            this.color3BlueBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.color3BlueBox.FormattingEnabled = true;
            this.color3BlueBox.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "0A",
            "0B",
            "0C",
            "0D",
            "0E",
            "0F",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "1A",
            "1B",
            "1C",
            "1D",
            "1E",
            "1F"});
            this.color3BlueBox.Location = new System.Drawing.Point(146, 186);
            this.color3BlueBox.Name = "color3BlueBox";
            this.color3BlueBox.Size = new System.Drawing.Size(38, 19);
            this.color3BlueBox.TabIndex = 13;
            this.color3BlueBox.SelectedIndexChanged += new System.EventHandler(this.color3_SelectedIndexChanged);
            // 
            // color3RedBox
            // 
            this.color3RedBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.color3RedBox.FormattingEnabled = true;
            this.color3RedBox.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "0A",
            "0B",
            "0C",
            "0D",
            "0E",
            "0F",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "1A",
            "1B",
            "1C",
            "1D",
            "1E",
            "1F"});
            this.color3RedBox.Location = new System.Drawing.Point(58, 186);
            this.color3RedBox.Name = "color3RedBox";
            this.color3RedBox.Size = new System.Drawing.Size(38, 19);
            this.color3RedBox.TabIndex = 11;
            this.color3RedBox.SelectedIndexChanged += new System.EventHandler(this.color3_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Red";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(99, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Green";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(143, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Blue";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 162);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "Color 2";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 187);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "Color 3";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 212);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 32;
            this.label9.Text = "Color 4";
            // 
            // color1Panel
            // 
            this.color1Panel.Location = new System.Drawing.Point(190, 136);
            this.color1Panel.Name = "color1Panel";
            this.color1Panel.Size = new System.Drawing.Size(38, 19);
            this.color1Panel.TabIndex = 33;
            // 
            // color2Panel
            // 
            this.color2Panel.Location = new System.Drawing.Point(190, 161);
            this.color2Panel.Name = "color2Panel";
            this.color2Panel.Size = new System.Drawing.Size(38, 19);
            this.color2Panel.TabIndex = 34;
            // 
            // color3Panel
            // 
            this.color3Panel.Location = new System.Drawing.Point(190, 186);
            this.color3Panel.Name = "color3Panel";
            this.color3Panel.Size = new System.Drawing.Size(38, 19);
            this.color3Panel.TabIndex = 34;
            // 
            // color4Panel
            // 
            this.color4Panel.Location = new System.Drawing.Point(190, 211);
            this.color4Panel.Name = "color4Panel";
            this.color4Panel.Size = new System.Drawing.Size(38, 19);
            this.color4Panel.TabIndex = 34;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(12, 236);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 23);
            this.button1.TabIndex = 35;
            this.button1.Text = "Save Example Palette";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(139, 236);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 23);
            this.button2.TabIndex = 36;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // ImageDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(406, 272);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.color4Panel);
            this.Controls.Add(this.color3Panel);
            this.Controls.Add(this.color2Panel);
            this.Controls.Add(this.color1Panel);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.color4GreenBox);
            this.Controls.Add(this.color4BlueBox);
            this.Controls.Add(this.color4RedBox);
            this.Controls.Add(this.color3GreenBox);
            this.Controls.Add(this.color3BlueBox);
            this.Controls.Add(this.color3RedBox);
            this.Controls.Add(this.color2GreenBox);
            this.Controls.Add(this.color2BlueBox);
            this.Controls.Add(this.color2RedBox);
            this.Controls.Add(this.color1GreenBox);
            this.Controls.Add(this.color1BlueBox);
            this.Controls.Add(this.color1RedBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.scaleLabel);
            this.Controls.Add(this.sizeLabel);
            this.Controls.Add(this.scaleBar);
            this.Controls.Add(this.sizeBar);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ImageDisplayForm";
            this.Text = "Display Image...";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TrackBar sizeBar;
        private System.Windows.Forms.TrackBar scaleBar;
        private System.Windows.Forms.Label sizeLabel;
        private System.Windows.Forms.Label scaleLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox color1RedBox;
        private System.Windows.Forms.ComboBox color1BlueBox;
        private System.Windows.Forms.ComboBox color1GreenBox;
        private System.Windows.Forms.ComboBox color2GreenBox;
        private System.Windows.Forms.ComboBox color2BlueBox;
        private System.Windows.Forms.ComboBox color2RedBox;
        private System.Windows.Forms.ComboBox color4GreenBox;
        private System.Windows.Forms.ComboBox color4BlueBox;
        private System.Windows.Forms.ComboBox color4RedBox;
        private System.Windows.Forms.ComboBox color3GreenBox;
        private System.Windows.Forms.ComboBox color3BlueBox;
        private System.Windows.Forms.ComboBox color3RedBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel color1Panel;
        private System.Windows.Forms.Panel color2Panel;
        private System.Windows.Forms.Panel color3Panel;
        private System.Windows.Forms.Panel color4Panel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

