using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_init
{
    public partial class Form2 : Form
    {
        private readonly List<string> exeList;
        private int currentIndex = 0;
        private int retryCount = 0;
        private const int maxRetry = 3;

        public Form2(List<string> exeList)
        {
            InitializeComponent();
            this.exeList = exeList ?? new List<string>();
        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = exeList.Count;
            progressBar1.Value = 0;
            lblStatus.Text = "准备安装...";

            await RunAllExe();
        }

        private async Task RunAllExe()
        {
            for (currentIndex = 0; currentIndex < exeList.Count; currentIndex++)
            {
                string exePath = exeList[currentIndex];
                string exeName = Path.GetFileName(exePath);
                retryCount = 0;
                bool finished = false;

                while (!finished)
                {
                    lblStatus.Text = $"正在安装：{exeName}";
                    bool success = await RunSingleExe(exePath);

                    if (success)
                    {
                        finished = true;
                    }
                    else
                    {
                        if (retryCount < maxRetry)
                        {
                            var dr = ShowRetryDialog(exeName, retryCount + 1);
                            if (dr == DialogResult.Retry)
                            {
                                retryCount++;
                                continue;
                            }
                        }
                        // 超过最大重试次数或取消
                        finished = true;
                    }
                }
                // 每次无论成功或失败都前进
                progressBar1.Value = currentIndex + 1;
            }

            // 强制100%
            progressBar1.Value = progressBar1.Maximum;
            lblStatus.Text = "所有程序已执行完毕";
            ShowFinishDialog();
        }

        private async Task<bool> RunSingleExe(string exePath)
        {
            try
            {
                var tcs = new TaskCompletionSource<bool>();
                using (Process proc = new Process())
                {
                    proc.StartInfo.FileName = exePath;
                    proc.EnableRaisingEvents = true;
                    proc.Exited += (s, e) =>
                    {
                        // 0为正常退出
                        tcs.TrySetResult(proc.ExitCode == 0);
                    };
                    bool started = proc.Start();
                    if (!started)
                        return false;
                    await Task.Run(() => proc.WaitForExit());
                    return await tcs.Task;
                }
            }
            catch
            {
                return false;
            }
        }

        private DialogResult ShowRetryDialog(string exeName, int retryTimes)
        {
            string msg = $"应用程序（{exeName}）执行失败，请重试。";
            string caption = "执行失败";
            if (retryTimes < maxRetry)
            {
                return MessageBox.Show(msg, caption, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            else
            {
                // 超过最大重试次数，只显示“确定”按钮
                return MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        private void ShowFinishDialog()
        {
            var result = MessageBox.Show("安装完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                Application.Exit();
            }
        }
    }
}