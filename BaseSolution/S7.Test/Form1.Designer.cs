namespace S7.Test
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cbPlcType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nudRack = new System.Windows.Forms.NumericUpDown();
            this.nudSlot = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.nudVarLength = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.nudDB = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.cbVarType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.cbDataType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lbVarValue = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudRack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSlot)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVarLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDB)).BeginInit();
            this.SuspendLayout();
            // 
            // cbPlcType
            // 
            this.cbPlcType.FormattingEnabled = true;
            this.cbPlcType.Items.AddRange(new object[] {
            "S7-200",
            "S7-300",
            "S7-400",
            "S7-1200",
            "S7-1500"});
            this.cbPlcType.Location = new System.Drawing.Point(76, 15);
            this.cbPlcType.Name = "cbPlcType";
            this.cbPlcType.Size = new System.Drawing.Size(98, 20);
            this.cbPlcType.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "PLC类型：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(181, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "IP地址：";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(231, 15);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(108, 21);
            this.txtIP.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(343, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Rack：";
            // 
            // nudRack
            // 
            this.nudRack.Location = new System.Drawing.Point(380, 15);
            this.nudRack.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudRack.Name = "nudRack";
            this.nudRack.Size = new System.Drawing.Size(60, 21);
            this.nudRack.TabIndex = 5;
            // 
            // nudSlot
            // 
            this.nudSlot.Location = new System.Drawing.Point(480, 15);
            this.nudSlot.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudSlot.Name = "nudSlot";
            this.nudSlot.Size = new System.Drawing.Size(69, 21);
            this.nudSlot.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(446, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "Slot：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "状态：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(74, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "未连接";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(565, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "连接";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(646, 15);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "断开";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.lbVarValue);
            this.panel1.Controls.Add(this.nudVarLength);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.nudDB);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.cbVarType);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.cbDataType);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Location = new System.Drawing.Point(18, 74);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(770, 364);
            this.panel1.TabIndex = 12;
            // 
            // nudVarLength
            // 
            this.nudVarLength.Enabled = false;
            this.nudVarLength.Location = new System.Drawing.Point(480, 15);
            this.nudVarLength.Name = "nudVarLength";
            this.nudVarLength.Size = new System.Drawing.Size(42, 21);
            this.nudVarLength.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(397, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 7;
            this.label10.Text = "变量值长度：";
            // 
            // nudDB
            // 
            this.nudDB.Enabled = false;
            this.nudDB.Location = new System.Drawing.Point(349, 15);
            this.nudDB.Name = "nudDB";
            this.nudDB.Size = new System.Drawing.Size(42, 21);
            this.nudDB.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(319, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 12);
            this.label9.TabIndex = 5;
            this.label9.Text = "DB:";
            // 
            // cbVarType
            // 
            this.cbVarType.Enabled = false;
            this.cbVarType.FormattingEnabled = true;
            this.cbVarType.Items.AddRange(new object[] {
            "Bit",
            "Byte",
            "Word",
            "DWord",
            "Int",
            "DInt",
            "Real",
            "String",
            "StringEx",
            "Timer",
            "Counter"});
            this.cbVarType.Location = new System.Drawing.Point(223, 15);
            this.cbVarType.Name = "cbVarType";
            this.cbVarType.Size = new System.Drawing.Size(83, 20);
            this.cbVarType.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(165, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 3;
            this.label8.Text = "变量类型：";
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(547, 15);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "读取";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // cbDataType
            // 
            this.cbDataType.Enabled = false;
            this.cbDataType.FormattingEnabled = true;
            this.cbDataType.Items.AddRange(new object[] {
            "Input",
            "Output",
            "Memory",
            "DataBlock",
            "Timer",
            "Counter"});
            this.cbDataType.Location = new System.Drawing.Point(73, 15);
            this.cbDataType.Name = "cbDataType";
            this.cbDataType.Size = new System.Drawing.Size(83, 20);
            this.cbDataType.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "DataType:";
            // 
            // lbVarValue
            // 
            this.lbVarValue.FormattingEnabled = true;
            this.lbVarValue.ItemHeight = 12;
            this.lbVarValue.Location = new System.Drawing.Point(10, 51);
            this.lbVarValue.Name = "lbVarValue";
            this.lbVarValue.Size = new System.Drawing.Size(744, 304);
            this.lbVarValue.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudSlot);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudRack);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbPlcType);
            this.Name = "Form1";
            this.Text = "PLC连通测试工具";
            ((System.ComponentModel.ISupportInitialize)(this.nudRack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSlot)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVarLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbPlcType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudRack;
        private System.Windows.Forms.NumericUpDown nudSlot;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbDataType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox cbVarType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudDB;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudVarLength;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ListBox lbVarValue;
    }
}

