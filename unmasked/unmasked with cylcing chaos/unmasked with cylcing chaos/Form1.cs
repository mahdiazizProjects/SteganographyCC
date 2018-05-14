using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace unmasked_with_cylcing_chaos
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
        string fileimagepath;
        string filesave1;
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "jpeg files(*.jpg)|*.jpg|Bitmap Files(*.bmp)|*.bmp|PNG Files(*.png)|*.png|Tif files(*.tif)|*.tif";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileimagepath = ofd.FileName;
            }
            GetPhoto();
            pictureBox1.Image = image;
            textBox9.Text = image.Width.ToString();
            textBox10.Text = image.Height.ToString();
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
            steg = new steganograph(lamda1, lamda2, lamda3, Xzero, Yzero, Zzero, Gamma, M, image.Height, image.Width);
            color combine_color = convertor();
            check_end data=steg.stegno(combine_color.red);
            ArrayList fetch_data =(ArrayList) data.output.Clone();
            string output="";
            for (int ii = 0; ii < fetch_data.Count; ii++)
                output += fetch_data[ii].ToString();
            if (data.checking < 4)
            {
                data = steg.stegno(combine_color.green);
                fetch_data = (ArrayList)data.output.Clone();
                for (int ii = 0; ii < fetch_data.Count; ii++)
                    output += fetch_data[ii].ToString();
            }
            if (data.checking < 4)
            {
                data = steg.stegno(combine_color.blue);
                fetch_data = (ArrayList)data.output.Clone();
                for (int ii = 0; ii < fetch_data.Count; ii++)
                    output += fetch_data[ii].ToString();
            }
            if (data.checking < 4)
            {
                data = steg.stegno_layer2(combine_color.red);
                fetch_data = (ArrayList)data.output.Clone();
                for (int ii = 0; ii < fetch_data.Count; ii++)
                    output += fetch_data[ii].ToString();
            }
            if (data.checking < 4)
            {
                data = steg.stegno_layer2(combine_color.green);
                fetch_data = (ArrayList)data.output.Clone();
                for (int ii = 0; ii < fetch_data.Count; ii++)
                    output += fetch_data[ii].ToString();
            }
            if (data.checking < 4)
            {
                data = steg.stegno_layer2(combine_color.blue);
                fetch_data = (ArrayList)data.output.Clone();
                for (int ii = 0; ii < fetch_data.Count; ii++)
                    output += fetch_data[ii].ToString();
            }
            if (data.checking < 4)
            {
                data = steg.stegno_layer3(combine_color.red);
                fetch_data = (ArrayList)data.output.Clone();
                for (int ii = 0; ii < fetch_data.Count; ii++)
                    output += fetch_data[ii].ToString();
            }
            if (data.checking < 4)
            {
                data = steg.stegno_layer3(combine_color.green);
                fetch_data = (ArrayList)data.output.Clone();
                for (int ii = 0; ii < fetch_data.Count; ii++)
                    output += fetch_data[ii].ToString();
            }
            if (data.checking < 4)
            {
                data = steg.stegno_layer3(combine_color.blue);
                fetch_data = (ArrayList)data.output.Clone();
                for (int ii = 0; ii < fetch_data.Count; ii++)
                    output += fetch_data[ii].ToString();
            }
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "text files|*.txt";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                filesave1 = sf.FileName;
            }
            textBox11.Text = output;
            steg.save(filesave1, output);

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

    }
}
