using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace Steganography_with_Cycling_Chaos
{
    struct color
    {
        public int[,] red;
        public int[,] green;
        public int[,] blue;
    }
    public partial class Form1 : Form
    {
        steganograph steg;
        Bitmap image;
        Color[,] pixel;
        string filename;
        string fileimagepath;
        string filesave1;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double lamda1 = double.Parse(textBox1.Text);
            double lamda2 = double.Parse(textBox2.Text);
            double lamda3 = double.Parse(textBox3.Text);
            double Gamma = double.Parse(textBox4.Text);
            double M = double.Parse(textBox5.Text);
            double Xzero = double.Parse(textBox6.Text);
            double Yzero = double.Parse(textBox7.Text);
            double Zzero = double.Parse(textBox8.Text);
            steg=new steganograph(lamda1,lamda2,lamda3,Xzero,Yzero,Zzero,Gamma,M,image.Height,image.Width);

            StreamReader rw = new StreamReader(filename);
            string read = rw.ReadToEnd();
            char[] ins = read.ToCharArray();
            char[] input = new char[ins.Length + 3];
            for(int ii=0;ii<ins.Length;ii++)
            input[ii] = ins[ii];
            input[ins.Length] = (char)21;
            input[ins.Length+1] = (char)21;
            input[ins.Length + 2] = (char)21;
            int[] a = new int[input.Length];
            for (int ii = 0; ii < input.Length; ii++)
                a[ii] = (int)input[ii];
            int[] binary_data = steg.binary_maker(a);
            int count = 0;
            color combine_color=convertor();
            int[,]altered_red=steg.stegno(combine_color.red, binary_data,ref count);
            int[,]altered_green=(int[,])combine_color.green.Clone();
            int[,] altered_blue = (int[,])combine_color.blue.Clone();
            if (binary_data.Length > (image.Width * image.Height))
            {
                altered_green= steg.stegno(combine_color.green, binary_data, ref count);
            }
            if (binary_data.Length > 2*(image.Width * image.Height))
            {
                altered_blue = steg.stegno(combine_color.blue, binary_data, ref count);
            }
            if (binary_data.Length > 3 * (image.Width * image.Height))
            {
               altered_red = steg.stegno_layer2(altered_red, binary_data, ref count);
            }
            if (binary_data.Length > 4*(image.Width * image.Height))
            {
                altered_green = steg.stegno_layer2(altered_green, binary_data, ref count);
            }
            if (binary_data.Length > 5 * (image.Width * image.Height))
            {
                altered_blue = steg.stegno_layer2(altered_blue, binary_data, ref count);
            }
            if (binary_data.Length > 6 * (image.Width * image.Height))
            {
                altered_red = steg.stegno_layer3(altered_red, binary_data, ref count);
            }
            if (binary_data.Length > 7 * (image.Width * image.Height))
            {
                altered_green = steg.stegno_layer3(altered_green, binary_data, ref count);
            }
            if (binary_data.Length > 8 * (image.Width * image.Height))
            {
                altered_blue = steg.stegno_layer3(altered_blue, binary_data, ref count);
            }
            Bitmap image_save=new Bitmap(image.Width,image.Height);
            for(int ii=0;ii<image.Width;ii++)
                for (int jj = 0; jj < image.Height; jj++)
                {
                    Color temp = Color.FromArgb(altered_red[ii, jj], altered_green[ii, jj], altered_blue[ii, jj]);
                    image_save.SetPixel(ii, jj, temp);
                }
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Tif File(*.tif)|*.tif|Bitmap file(*.bmp)|*.bmp|Jpeg File(*.jpg)|*.jpg|PNG File(*.png)|*.png";
            DialogResult dr = save.ShowDialog();
            pictureBox1.Image = image_save;
            if (dr.Equals(DialogResult.OK))
                filesave1 = save.FileName;
            image_save.Save(filesave1);
        }
        private color convertor()
        {
            color results = new color();
            int[,] resultR = new int[image.Width, image.Height];
            int[,] resultG = new int[image.Width, image.Height];
            int[,] resultB = new int[image.Width, image.Height];
            for (int ii = 0; ii < image.Width; ii++)
                for (int jj = 0; jj < image.Height; jj++)
                {
                    resultR[ii, jj] = (int)pixel[ii, jj].R;
                    resultG[ii, jj] = (int)pixel[ii, jj].G;
                    resultB[ii, jj] = (int)pixel[ii, jj].B;
                }
            results.red = (int[,])resultR.Clone();
            results.green = (int[,])resultG.Clone();
            results.blue = (int[,])resultB.Clone();
            return results;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "3.0";
            textBox2.Text = "2.98";
            textBox3.Text = "2.87";
            textBox4.Text = "3.05";
            textBox5.Text = "0.25";
            textBox6.Text = "-0.01";
            textBox7.Text = "0.03";
            textBox8.Text = "0.02";
            Iscolor.Checked = true;
        }
        private void GetPhoto()
        {
            image = (Bitmap)Image.FromFile(fileimagepath);
            pixel = new Color[image.Width, image.Height];
            for (int i = 0; i < image.Width; i++)
                for (int j = 0; j < image.Height; j++)
                {
                    pixel[i, j] = image.GetPixel(i, j);
                }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Tif File(*.tif)|*.tif|Bitmap file(*.bmp)|*.bmp|Jpeg File(*.jpg)|*.jpg|PNG File(*.png)|*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileimagepath = ofd.FileName;
            }
            GetPhoto();
            pictureBox1.Image = image;
            textBox9.Text = image.Width.ToString();
            textBox10.Text = image.Height.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "text files|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filename = ofd.FileName;
            }
        }  
    }
}
