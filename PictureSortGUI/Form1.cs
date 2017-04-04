using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureSortGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void nextButton_Click(object sender, EventArgs e)
        {
            List<int> flag = new List<int>();
            for (int i = 0; i < index.Length; i++)
            {
                if (isSeclet[i])
                {
                    flag.Add(i);
                    isSeclet[i] = false;
                }
            }
            SecletList(Keys.Enter);
            WriteFlag(flag);
            nowNum++;
            if (nowNum >= fileInfo.Length)
            {
                MessageBox.Show("没了", "", MessageBoxButtons.OK, MessageBoxIcon.None,
                    MessageBoxDefaultButton.Button1);
                this.Close();
                return;
            }
            var temp = fileInfo[nowNum].ToString();
            nowImage = Image.FromFile("pictures/" + temp);
            Bitmap map = (Bitmap)nowImage;
            nowImage = (Bitmap)map.GetThumbnailImage(pictureBox1.Height, pictureBox1.Width, null, IntPtr.Zero);
            if (sender == null)
            {
                OnPaint(null);
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryInfo theFolder = new DirectoryInfo("pictures");
            if (theFolder == null)
            {
                MessageBox.Show("没有找到pictuers文件夹", "", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);
                this.Close();
                return;
            }
            fileInfo = theFolder.GetFiles();
            isSeclet = new bool[index.Length];
            try
            {
                var file = new StreamReader("info.txt", Encoding.Default);
                nowNum = int.Parse(file.ReadLine().Split(' ')[0]);
            }
            catch (IOException)
            {
                nowNum = 0;
            }
            var temp = fileInfo[nowNum].ToString();
            nowImage = Image.FromFile("pictures/" + temp);
            Bitmap map = (Bitmap)nowImage;
            nowImage = (Bitmap)map.GetThumbnailImage(pictureBox1.Height, pictureBox1.Width, null, IntPtr.Zero);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            pictureBox1.Image = nowImage;
        }

        private void SecletList(Keys key)
        {
            for (int i = 0; i < index.Length; i++)
            {
                if (key == keyIndex[i])
                {
                    if (isSeclet[i] == false)
                    {
                        isSeclet[i] = true;
                    }
                    else
                    {
                        isSeclet[i] = false;
                    }
                }
            }
            listBox.Items.Clear();
            for (int i = 0; i < index.Length; i++)
            {
                if (isSeclet[i])
                {
                    listBox.Items.Add(("*选中*") + this.index[i]);
                }
                else
                {
                    listBox.Items.Add(index[i]);
                }
            }
        }



        private FileInfo[] fileInfo;
        private bool[] isSeclet;
        private String[] index =
        {
            "0","1","2","3","4","5","6","7","8","9"
        };

        private Keys[] keyIndex =
        {
            Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9
        };
        private Char[] keyCharIndex =
        {
            '0','1','2','3','4','5','6','7','8','9'
        };

        private int nowNum;
        private Image nowImage;

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                nextButton_Click(null, null);
            }
        }

        private void listBox_KeyDown(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode;
            if (e.KeyCode == Keys.Enter)
            {
                nextButton_Click(null, null);
                return;
            }
            SecletList(key);
        }

        private void listBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            var a = e.KeyChar;
        }

        private void WriteFlag(List<int> flag)
        {
            var fs = File.OpenWrite("flag.txt");
            fs.Position = fs.Length;
            String line = "";
            foreach (var x in flag)
            {
                line += Convert.ToString(x) + " ";
            }
            line += '\n';
            Encoding encoder = Encoding.ASCII;
            byte[] bytes = encoder.GetBytes(line);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
            FileStream file;
            try
            {
                file = File.OpenWrite("states.txt");
                byte[] Ibytes = encoder.GetBytes(Convert.ToString(nowNum) + '\n');
                file.Write(Ibytes, 0, Ibytes.Length);
                file.Close();
            }
            catch (Exception e)
            {

            }


        }
    }
}
