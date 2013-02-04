namespace WindowsApplication1
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.portNameCheckBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.modenCount = new System.Windows.Forms.Label();
            this.logBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.speedLabel = new System.Windows.Forms.Label();
            this.errorModenCount = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.portNameDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ringStatusGridView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portNameDataGridView)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ringStatusGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 78);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(71, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "开始";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "设备端口号：";
            // 
            // portNameCheckBox
            // 
            this.portNameCheckBox.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10",
            "COM11",
            "COM12",
            "COM13",
            "COM14",
            "COM15",
            "COM16",
            "COM17",
            "COM18",
            "COM19",
            "COM20",
            "COM21",
            "COM22",
            "COM23",
            "COM24",
            "COM25",
            "COM26",
            "COM27",
            "COM28",
            "COM29",
            "COM30",
            "COM31",
            "COM32",
            "COM33",
            "COM34",
            "COM35",
            "COM36",
            "COM37",
            "COM38",
            "COM39",
            "COM40"});
            this.portNameCheckBox.Location = new System.Drawing.Point(91, 34);
            this.portNameCheckBox.Name = "portNameCheckBox";
            this.portNameCheckBox.Size = new System.Drawing.Size(80, 20);
            this.portNameCheckBox.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(12, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 20);
            this.label5.TabIndex = 12;
            this.label5.Text = "设备总数：";
            // 
            // modenCount
            // 
            this.modenCount.AutoSize = true;
            this.modenCount.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.modenCount.Location = new System.Drawing.Point(102, 131);
            this.modenCount.Name = "modenCount";
            this.modenCount.Size = new System.Drawing.Size(19, 20);
            this.modenCount.TabIndex = 13;
            this.modenCount.Text = "0";
            // 
            // logBox
            // 
            this.logBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Column;
            this.logBox.Location = new System.Drawing.Point(10, 302);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logBox.Size = new System.Drawing.Size(358, 299);
            this.logBox.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Crimson;
            this.label2.Location = new System.Drawing.Point(172, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 20);
            this.label2.TabIndex = 19;
            this.label2.Text = "异常设备：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(12, 178);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(159, 20);
            this.label6.TabIndex = 20;
            this.label6.Text = "最近500条速度：";
            // 
            // speedLabel
            // 
            this.speedLabel.AutoSize = true;
            this.speedLabel.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.speedLabel.Location = new System.Drawing.Point(160, 178);
            this.speedLabel.Name = "speedLabel";
            this.speedLabel.Size = new System.Drawing.Size(69, 20);
            this.speedLabel.TabIndex = 21;
            this.speedLabel.Text = "0条/分";
            // 
            // errorModenCount
            // 
            this.errorModenCount.AutoSize = true;
            this.errorModenCount.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.errorModenCount.ForeColor = System.Drawing.Color.Crimson;
            this.errorModenCount.Location = new System.Drawing.Point(270, 131);
            this.errorModenCount.Name = "errorModenCount";
            this.errorModenCount.Size = new System.Drawing.Size(19, 20);
            this.errorModenCount.TabIndex = 23;
            this.errorModenCount.Text = "0";
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(101, 78);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(70, 23);
            this.button4.TabIndex = 27;
            this.button4.Text = "停止";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(206, 32);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 29;
            this.button3.Text = "添加设备";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(385, 22);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(349, 579);
            this.tabControl1.TabIndex = 32;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.portNameDataGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(341, 553);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "设备状态";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // portNameDataGridView
            // 
            this.portNameDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.portNameDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.portNameDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.portNameDataGridView.Location = new System.Drawing.Point(0, 0);
            this.portNameDataGridView.Name = "portNameDataGridView";
            this.portNameDataGridView.ReadOnly = true;
            this.portNameDataGridView.RowTemplate.Height = 23;
            this.portNameDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.portNameDataGridView.Size = new System.Drawing.Size(345, 553);
            this.portNameDataGridView.TabIndex = 32;
            this.portNameDataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.portNameDataGridView_RowsAdded_1);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.FillWeight = 30F;
            this.dataGridViewTextBoxColumn1.HeaderText = "端口";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 70F;
            this.dataGridViewTextBoxColumn2.HeaderText = "状态";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ringStatusGridView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(341, 553);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "测试记录";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ringStatusGridView
            // 
            this.ringStatusGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ringStatusGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ringStatusGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.ringStatusGridView.Location = new System.Drawing.Point(1, 3);
            this.ringStatusGridView.Name = "ringStatusGridView";
            this.ringStatusGridView.RowTemplate.Height = 23;
            this.ringStatusGridView.Size = new System.Drawing.Size(345, 554);
            this.ringStatusGridView.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.FillWeight = 50F;
            this.Column1.HeaderText = "手机号";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "状态";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.FillWeight = 1F;
            this.Column3.HeaderText = "测试时间";
            this.Column3.Name = "Column3";
            this.Column3.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 613);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.errorModenCount);
            this.Controls.Add(this.speedLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.modenCount);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.portNameCheckBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "赞成科技反刷卡测试工具V1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.portNameDataGridView)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ringStatusGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.ComboBox portNameCheckBox;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label modenCount;
        private System.Windows.Forms.TextBox logBox;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label speedLabel;
        private System.Windows.Forms.Label errorModenCount;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.DataGridView portNameDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView ringStatusGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        
    }
}

