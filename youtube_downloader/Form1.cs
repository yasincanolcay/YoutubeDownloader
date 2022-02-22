using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoLibrary;
using MediaToolkit;
using MediaToolkit.Model;
using System.IO;
namespace youtube_downloader
{
    public partial class Form1 : Form
    {
        bool format;
        public Form1()
        {
            InitializeComponent();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = trackBar1.Value;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pictureBox3.Visible = false;
            pictureBox5.Visible = true;
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = 0;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            pictureBox3.Visible = true;
            pictureBox5.Visible = false;
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }

        private async void pictureBox2_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "İndirilecek klasör seçiniz" })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    progressBar1.Visible = true;
                    label3.Visible = true;
                    label3.Text = "Please Wait...";
                    label8.Text = textBox1.Text;
                    progressBar1.Value = 10;
                    label10.Text = progressBar1.Value.ToString();
                    progressBar1.Value = 20;
                    label10.Text = progressBar1.Value.ToString();
                    var yt = YouTube.Default;
                    var videos = await yt.GetVideoAsync(textBox1.Text);
                    File.WriteAllBytes(fbd.SelectedPath + @"\" + videos.FullName, await videos.GetBytesAsync());
                    var inputfile = new MediaToolkit.Model.MediaFile { Filename = fbd.SelectedPath + @"\" + videos.FullName };
                    var outputfile = new MediaToolkit.Model.MediaFile { Filename = $"{fbd.SelectedPath + @"\" + videos.FullName}.mp3" };
                    using (var engine = new Engine())
                    {
                        engine.GetMetadata(inputfile);
                        engine.Convert(inputfile, outputfile);
                    }
                    if (format == true)//mp3 indir
                    {
                        File.Delete(fbd.SelectedPath + @"\" + videos.FullName);
                        axWindowsMediaPlayer1.URL = $"{fbd.SelectedPath + @"\" + videos.FullName}.mp3";
                        label9.Text = $"{fbd.SelectedPath + @"\" + videos.FullName}.mp3";
                    }
                    else//mp4 indir
                    {
                        File.Delete($"{fbd.SelectedPath + @"\" + videos.FullName}.mp3");
                        axWindowsMediaPlayer1.URL = fbd.SelectedPath + @"\" + videos.FullName;
                        label9.Text = fbd.SelectedPath + @"\" + videos.FullName;
                    }
                    label3.Text = "SUCCESFULY✔";
                    progressBar1.Value = 50;
                    label10.Text = progressBar1.Value.ToString();
                    progressBar1.Value = 100;
                    label10.Text = "100% indirildi";
                    pictureBox3.Enabled = true;
                    label7.Text = "Müziği Oynat";
                    progressBar1.Visible = false;
                    label3.Visible = false;
                }
                else
                {
                    label3.Visible = true;
                    MessageBox.Show("Lütfen dosya yolu belirtiniz", "uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    label3.Text = "DOWNLOAD FAİLED!";
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            format = true;
            pictureBox2.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            format = false;
            pictureBox2.Enabled = true;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = 50;

        }
    }
}
