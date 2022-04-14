using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp25
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        WebClient wc = new WebClient();


        private void button1_Click(object sender, EventArgs e)
        {
            string url = "https://codeload.github.com/" + textBox1.Text + "/" + textBox2.Text + "/zip/refs/heads/main";
            string save_path = textBox3.Text + "/" + textBox2.Text + ".zip";
            bool download = false;


            try
            {
                using (var stream = wc.OpenRead(url))
                {
                    download = true;
                }
            }
            catch
            {
                download = false;
                MessageBox.Show("找不到資源，請檢查網路狀態並重新再試!!");
            }

            if (download == true)
            {
                using (wc)
                {
                    try
                    {
                        if (textBox1.Text.Length != 0 && textBox2.Text.Length != 0 && textBox3.Text.Length != 0)
                        {

                            wc.DownloadFileTaskAsync(new Uri(url), save_path);

                        }
                        else
                        {
                            MessageBox.Show("請檢查是否已輸入!");
                        }
                    }
                    catch { }
                }
            }

        }

        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {

            if (!e.Cancelled && e.Error == null)
            {
                MessageBox.Show("下載完成");
            }
            else if (e.Cancelled)
            {
                Cancel();
            }

            if (progressBar1.Value == progressBar1.Maximum)
            {
                button1.Text = "開始下載";
                button1.Enabled = true;
                progressBar1.Value = 0;
                OpenFile(textBox3.Text + "\\" + textBox2.Text + ".zip");
            }



        }

        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Invoke(new MethodInvoker(delegate ()
            {
                progressBar1.Value = e.ProgressPercentage;
                button1.Enabled = false;
                button1.Text = "正在下載中...";
            }));
        }


        private void button2_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBox3.Text = fbd.SelectedPath.ToString();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            wc.DownloadProgressChanged += wc_DownloadProgressChanged;
            wc.DownloadFileCompleted += wc_DownloadFileCompleted;
            this.label4.Location = new Point(this.Width / 2 - label4.Width / 2, 30);
            label6.Text = "❗";
            label5.Text = "❗";


        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 0)
            {
                label6.Text = "❗";
            }
            else
            {
                label6.Text = "\u2713";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                label5.Text = "❗";
            }
            else
            {
                label5.Text = "\u2713";
            }
        }



        public void OpenFile(string filePath)
        {
            Process.Start("explorer.exe", filePath);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/zhengshunze");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void Cancel()
        {
            Invoke(new MethodInvoker(delegate ()
            {
                progressBar1.Value = progressBar1.Minimum;
                button1.Text = "開始下載";
                button1.Enabled = true;
                wc.CancelAsync();
            }));
        }
    }


}




