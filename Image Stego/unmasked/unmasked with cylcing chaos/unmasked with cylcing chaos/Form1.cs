using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

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
                GetPhoto();
                pictureBox1.Image = image;
                textBox9.Text = image.Width.ToString();
                textBox10.Text = image.Height.ToString();
            }
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
            int[] binary_image = new int[1];
            steg = new steganograph(lamda1, lamda2, lamda3, Xzero, Yzero, Zzero, Gamma, M, image.Height, image.Width);
            color combine_color = convertor();
            int height=-1;int width=-1;
            int count = 0;
            steg.stegno2(combine_color.red, ref binary_image, ref height, ref width,ref count);
            //int sizeall=(3 * height * width);
            int sizeall=(3 * height * width);
            int before=count;
            if (count < binary_image.Length)
            {
                steg.stegno(combine_color.green,ref binary_image,1,ref count);
            }

            if (count < binary_image.Length)
            {
                steg.stegno(combine_color.blue, ref binary_image, 1, ref count);
            }
            if (count < binary_image.Length)
            {
                steg.stegno(combine_color.red, ref binary_image, 2, ref count);
            }
            if (count < binary_image.Length)
            {
                steg.stegno(combine_color.green, ref binary_image, 2, ref count);
            }
            if (count < binary_image.Length)
            {
                steg.stegno(combine_color.blue, ref binary_image, 2, ref count);
            }
            if (count < binary_image.Length)
            {
                steg.stegno(combine_color.red, ref binary_image, 4, ref count);
            }
            if (count < binary_image.Length)
            {
                steg.stegno(combine_color.green, ref binary_image, 4, ref count);
            }
            if (count < binary_image.Length)
            {
                steg.stegno(combine_color.blue, ref binary_image, 4, ref count);
            }
            ArrayList images = new ArrayList();
            int[] bits = new int[8];
            int index=0;
            for (int ii = 0; ii < binary_image.Length; ii++)
            {
                bits[index++] = binary_image[ii];
                if (index == 8)
                {
                    images.Add(steg.Convert_Binary_to_Int(bits));
                    index = 0;
                }
            }
            //steg.save_pixels(images, 0, "im.txt");
            Bitmap image_save = new Bitmap(width, height,PixelFormat.Format24bppRgb);
            count = 0;
            for (int ii = 0; ii < width; ii++)
                for (int jj = 0; jj < height; jj++)
                {
                    int r = Convert.ToInt32(images[count]);
                    //int g = Convert.ToInt32(images[height*width+count]);
                    //int b = Convert.ToInt32(images[2*height * width + count]);
                    count++;
                    Color temp = Color.FromArgb(r, r, r);
                    image_save.SetPixel(ii, jj, temp);
                }
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Jpeg File(*.jpg)|*.jpg|Bitmap file(*.bmp)|*.bmp|PNG File(*.png)|*.png";
            DialogResult dr = save.ShowDialog();
            pictureBox2.Image = image_save;
            if (dr.Equals(DialogResult.OK))
                filesave1 = save.FileName;
            image_save.Save(filesave1,ImageFormat.Bmp);

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
