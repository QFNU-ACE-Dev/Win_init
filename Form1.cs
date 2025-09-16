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
            List<string> exeList = new List<string>();
            string baseDir = Application.StartupPath;
            // A部分
            switch (selectedA)
            {
                case 0: exeList.Add(Path.Combine(baseDir, "A1.exe")); break;
                case 1: exeList.Add(Path.Combine(baseDir, "A2.exe")); break;
                case 2: exeList.Add(Path.Combine(baseDir, "A3.exe")); break;
                case 3: break;
            }
            // B部分
            if (checkB1.Checked) exeList.Add(Path.Combine(baseDir, "B1.exe"));
            if (checkB2.Checked) exeList.Add(Path.Combine(baseDir, "B2.exe"));
            if (checkB3.Checked) exeList.Add(Path.Combine(baseDir, "B3.exe"));

            // 打开Form2并传递exeList
            Form2 form2 = new Form2(exeList);
            form2.Show();
            this.Hide();
        }
    }
}
