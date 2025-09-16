using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Win_init
{
    public partial class Form1 : Form
    {
        // 保存A、B选择
        private int selectedA = -1; // 0:A1, 1:A2, 2:A3, 3:跳过
        private List<int> selectedBs = new List<int>();

        public Form1()
        {
            InitializeComponent();

            // 事件绑定
            radioA1.CheckedChanged += RadioA_CheckedChanged;
            radioA2.CheckedChanged += RadioA_CheckedChanged;
            radioA3.CheckedChanged += RadioA_CheckedChanged;
            radioSkip.CheckedChanged += RadioA_CheckedChanged;

            btnNext.Click += BtnNext_Click;
            btnPrev.Click += BtnPrev_Click;
            btnFinish.Click += BtnFinish_Click;

            btnNext.Enabled = false;
            panelPage2.Visible = false;
            panelPage1.Visible = true;
        }

        private void RadioA_CheckedChanged(object sender, EventArgs e)
        {
            btnNext.Enabled = radioA1.Checked || radioA2.Checked || radioA3.Checked || radioSkip.Checked;
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            // 保存A选择
            if (radioA1.Checked) selectedA = 0;
            else if (radioA2.Checked) selectedA = 1;
            else if (radioA3.Checked) selectedA = 2;
            else if (radioSkip.Checked) selectedA = 3;

            panelPage1.Visible = false;
            panelPage2.Visible = true;
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            panelPage2.Visible = false;
            panelPage1.Visible = true;
        }

        private void BtnFinish_Click(object sender, EventArgs e)
        {
            selectedBs.Clear();
            if (checkB1.Checked) selectedBs.Add(0);
            if (checkB2.Checked) selectedBs.Add(1);
            if (checkB3.Checked) selectedBs.Add(2);

            string baseDir = Application.StartupPath;
            try
            {
                // 运行A
                switch (selectedA)
                {
                    case 0: Process.Start(Path.Combine(baseDir, "A1.exe")); break;
                    case 1: Process.Start(Path.Combine(baseDir, "A2.exe")); break;
                    case 2: Process.Start(Path.Combine(baseDir, "A3.exe")); break;
                    case 3: /* 跳过 */ break;
                }
                // 运行B
                foreach (var b in selectedBs)
                {
                    Process.Start(Path.Combine(baseDir, $"B{b + 1}.exe"));
                }
                MessageBox.Show("配置完成！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("运行程序时出错: " + ex.Message);
            }
        }
    }
}
