using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Security;

namespace Steganography_with_Cycling_Chaos
{
    struct color
    {
        public int[,] red;
        public int[,] green;
        public int[,] blue;
    }
    struct photo
    {
        public Color[,] pixel;
        public Bitmap image;
    }
    public partial class Form1 : Form
    {
        steganograph steg;
        Color[,] pixel;
        Color[,] pixel2;
        string fileimagepath;
        string filesave1;
        int secheight = 0; int secwidth = 0;
        int coverheight = 0; int coverwidth = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "proposed method")
            {
                double lamda1 = double.Parse(textBox1.Text);
                double lamda2 = double.Parse(textBox2.Text);
                double lamda3 = double.Parse(textBox3.Text);
                double Gamma = double.Parse(textBox4.Text);
                double M = double.Parse(textBox5.Text);
                double Xzero = double.Parse(textBox6.Text);
                double Yzero = double.Parse(textBox7.Text);
                double Zzero = double.Parse(textBox8.Text);
                steg = new steganograph(lamda1, lamda2, lamda3, Xzero, Yzero, Zzero, Gamma, M, coverheight, coverwidth);
                //int[] input = new int[3 * secwidth * secheight + 2];
                int[] input = new int[secwidth * secheight + 2];
                int ind = 2;
                int capacity = coverheight * coverwidth / 8;
                input[0] = secheight;
                input[1] = secwidth;
                for (int ii = 0; ii < secwidth; ii++)
                    for (int jj = 0; jj < secheight; jj++)
                        input[ind++] = Convert.ToInt32(pixel2[ii, jj].R);
                int before = ind;
                //for (int ii = 0; ii < secwidth; ii++)
                //    for (int jj = 0; jj < secheight; jj++)
                //        input[ind++] = Convert.ToInt32(pixel2[ii, jj].G);
                //for (int ii = 0; ii < secwidth; ii++)
                //    for (int jj = 0; jj < secheight; jj++)
                //        input[ind++] = Convert.ToInt32(pixel2[ii, jj].B);
                //steg.save_pixels(input, 2, "im.txt", input.Length);
                int[] binary_data = steg.binary_maker(input);
                int count = 0;
                if (binary_data.Length > 9 * (coverwidth * coverheight))
                    throw new Exception("سايز عكس ورودي بزرگ است!");
                else
                {
                    color combine_color = convertor(pixel);
                    int[,] altered_red = steg.stegno(combine_color.red, binary_data, ref count, 1);
                    int[,] altered_green = (int[,])combine_color.green.Clone();
                    int[,] altered_blue = (int[,])combine_color.blue.Clone();
                    if (binary_data.Length > (coverwidth * coverheight))
                    {
                        altered_green = steg.stegno(combine_color.green, binary_data, ref count, 1);
                    }
                    if (binary_data.Length > 2 * (coverwidth * coverheight))
                    {
                        altered_blue = steg.stegno(combine_color.blue, binary_data, ref count, 1);
                    }
                    if (binary_data.Length > 3 * (coverwidth * coverheight))
                    {
                        altered_red = steg.stegno(altered_red, binary_data, ref count, 2);
                    }
                    if (binary_data.Length > 4 * (coverwidth * coverheight))
                    {
                        altered_green = steg.stegno(altered_green, binary_data, ref count, 2);
                    }
                    if (binary_data.Length > 5 * (coverwidth * coverheight))
                    {
                        altered_blue = steg.stegno(altered_blue, binary_data, ref count, 2);
                    }
                    if (binary_data.Length > 6 * (coverwidth * coverheight))
                    {
                        altered_red = steg.stegno(altered_red, binary_data, ref count, 4);
                    }
                    if (binary_data.Length > 7 * (coverwidth * coverheight))
                    {
                        altered_green = steg.stegno(altered_green, binary_data, ref count, 4);
                    }
                    if (binary_data.Length > 8 * (coverwidth * coverheight))
                    {
                        altered_blue = steg.stegno(altered_blue, binary_data, ref count, 4);
                    }
                    Bitmap image_save = new Bitmap(coverwidth, coverheight, PixelFormat.Format24bppRgb);
                    for (int ii = 0; ii < coverwidth; ii++)
                        for (int jj = 0; jj < coverheight; jj++)
                        {
                            Color temp = Color.FromArgb(altered_red[ii, jj], altered_green[ii, jj], altered_blue[ii, jj]);
                            image_save.SetPixel(ii, jj, temp);
                        }
                    SaveFileDialog save = new SaveFileDialog();
                    save.Filter = "Bitmap file(*.bmp)|*.bmp|Jpeg File(*.jpg)|*.jpg|PNG File(*.png)|*.png|TIFF File(*.tif)|*.tiff";
                    DialogResult dr = save.ShowDialog();
                    pictureBox1.Image = image_save;
                    if (dr.Equals(DialogResult.OK))
                        filesave1 = save.FileName;
                    ////ImageCodecInfo bmpCodec = FindEncoder(ImageFormat.Bmp);
                    //EncoderParameters parameters = new EncoderParameters();
                    //parameters.Param[0] = new EncoderParameter(Encoder.ColorDepth, 24);
                    image_save.Save(filesave1, ImageFormat.Bmp);
                }
            }
            else if (comboBox1.Text == "Yu method")
            {

            }
        }
        private color convertor(Color[,] pixel)
        {
            color results = new color();
            int[,] resultR = new int[coverwidth, coverheight];
            int[,] resultG = new int[coverwidth, coverheight];
            int[,] resultB = new int[coverwidth, coverheight];
            for (int ii = 0; ii < coverwidth; ii++)
                for (int jj = 0; jj < coverheight; jj++)
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
        }
        private photo GetPhoto()
        {
            Bitmap image = (Bitmap)Image.FromFile(fileimagepath);
            Color[,] pixel = new Color[image.Width, image.Height];
            for (int i = 0; i < image.Width; i++)
                for (int j = 0; j < image.Height; j++)
                {
                    pixel[i, j] = image.GetPixel(i, j);
                }
            photo result = new photo();
            result.image = (Bitmap)image.Clone();
            result.pixel = (Color[,])pixel.Clone();
            return result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "jpeg files(*.jpg)|*.jpg|Bitmap Files(*.bmp)|*.bmp|PNG Files(*.png)|*.png|Tif files(*.tif)|*.tif";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileimagepath = ofd.FileName;
                photo ph = GetPhoto();
                Bitmap image =(Bitmap) ph.image.Clone();
                pixel = (Color[,])ph.pixel.Clone();
                pictureBox1.Image = image;
                textBox9.Text = image.Width.ToString();
                textBox10.Text = image.Height.ToString();
                coverheight = image.Height;
                coverwidth = image.Width;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Tif files(*.tif)|*.tif|jpeg files(*.jpg)|*.jpg|Bitmap Files(*.bmp)|*.bmp|PNG Files(*.png)|*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileimagepath = ofd.FileName;
                photo ph = GetPhoto();
                pixel2 = (Color[,])ph.pixel.Clone();
                pictureBox2.Image = ph.image;
                textBox11.Text = ph.image.Width.ToString();
                textBox12.Text = ph.image.Height.ToString();
                secwidth = ph.image.Width;
                secheight = ph.image.Height;
            }
        }  
    }
}
