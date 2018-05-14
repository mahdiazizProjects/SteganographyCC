using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace Steganography_with_Cycling_Chaos
{
    class steganograph
    {
        int Height;
        int Width;
        double[] domains;
        double l1, l2, l3, m, gamma;
        double domain;
        double xzero; double yzero; double zzero;
        public steganograph(double L1, double L2, double L3, double Xzero, double Yzero, double Zzero, double Gamma, double M,int height,int width)
        {
            domains = new double[65536];
            l1 = L1;
            l2 = L2;
            l3 = L3;
            m = M;
            gamma = Gamma;
            xzero = Xzero;
            yzero = Yzero;
            zzero = Zzero;
            domain = 4.0 / (65536-1);
            Height = height;
            Width = width;

        }
        private int[] Convert_TO_Binary(int a)
        {
            int[] color = new int[8];
            int temp = -1;
            for (int i = 0; i < 8; i++)
            {
                temp = a / 2;

                if (a > 0)
                {
                    color[i] = a - 2 * temp;

                }
                else
                {
                    color[i] = temp;
                }
                a = a / 2;
            }
            return color;
        }
        public int[] binary_maker(int[] entry_data)
        {
            int[] output = new int[entry_data.Length * 8];
            int index = 0;
            for (int ii = 0; ii < entry_data.Length; ii++)
            {
                int jj=0;
                int[] binary = Convert_TO_Binary(entry_data[ii]);
                while (jj < binary.Length)
                {
                    output[index++] = binary[jj++];
                }
            }
            return output;
        }
        public int[,] stegno(int[,]image,int[] inputs,ref int count)
        {
            restrict();
            double Xnegative1 = xzero;
            double Ynegative1 = yzero;
            double Znegative1 = zzero;
            for (int kk = 0; kk < Height; kk++)
            {
                if (count >= inputs.Length)
                    break;

                int index = 0;
                double X = (l1 * Xnegative1) - (Math.Pow(Xnegative1, 3)) - (gamma * Math.Pow(Math.Abs(Ynegative1), m) * Xnegative1);
                double Y = (l2 * Ynegative1) - (Math.Pow(Ynegative1, 3)) - (gamma * Math.Pow(Math.Abs(Znegative1), m) * Ynegative1);
                double Z = (l3 * Znegative1) - (Math.Pow(Znegative1, 3)) - (gamma * Math.Pow(Math.Abs(Xnegative1), m) * Znegative1);
                double select = turning(Xnegative1, Ynegative1, Znegative1);
                for (int ii = 0; ii < domains.Length - 1; ii++)
                    if (domains[ii] <= select && domains[ii + 1] > select)
                    {
                        index = ii;
                        break;
                    }
                Random rr = new Random(index);
                int[] rand_sq = sq_rand_gen(Width, rr);
                for(int ii = 0; ii < rand_sq.Length&&count<inputs.Length; ii++)
                {
                    int index2 = rand_sq[ii];
                    if ((image[index2, kk] % 2) == 0)
                        image[index2, kk] += inputs[count];
                    else if (inputs[count] == 0)
                        image[index2, kk]--;
                    count++;
                }

                    Xnegative1 = X; Ynegative1 = Y; Znegative1 = Z;
            }
            count = count-(count % 8);
            return image;
        }
        public int[,] stegno_layer2(int[,] image, int[] inputs, ref int count)
        {
            restrict();
            double Xnegative1 = xzero;
            double Ynegative1 = yzero;
            double Znegative1 = zzero;
            for (int kk = 0; kk < Height; kk++)
            {
                if (count >= inputs.Length)
                    break;

                int index = 0;
                double X = (l1 * Xnegative1) - (Math.Pow(Xnegative1, 3)) - (gamma * Math.Pow(Math.Abs(Ynegative1), m) * Xnegative1);
                double Y = (l2 * Ynegative1) - (Math.Pow(Ynegative1, 3)) - (gamma * Math.Pow(Math.Abs(Znegative1), m) * Ynegative1);
                double Z = (l3 * Znegative1) - (Math.Pow(Znegative1, 3)) - (gamma * Math.Pow(Math.Abs(Xnegative1), m) * Znegative1);
                double select = turning(Xnegative1, Ynegative1, Znegative1);
                for (int ii = 0; ii < domains.Length - 1; ii++)
                    if (domains[ii] <= select && domains[ii + 1] > select)
                    {
                        index = ii;
                        break;
                    }
                Random rr = new Random(index);
                int[] rand_sq = sq_rand_gen(Width, rr);
                for (int ii = 0; ii < rand_sq.Length && count < inputs.Length; ii++)
                {
                    int index2 = rand_sq[ii];
                    if ((image[index2, kk] % 4) <2)
                        image[index2, kk] += inputs[count]*2;
                    else if (inputs[count] == 0)
                        image[index2, kk] -=2;
                    count++;
                }

                Xnegative1 = X; Ynegative1 = Y; Znegative1 = Z;
            }
            count = count - (count % 8);
            return image;
        }
        public int[,] stegno_layer3(int[,] image, int[] inputs, ref int count)
        {
            restrict();
            double Xnegative1 = xzero;
            double Ynegative1 = yzero;
            double Znegative1 = zzero;
            for (int kk = 0; kk < Height; kk++)
            {
                if (count >= inputs.Length)
                    break;
                int index = 0;
                double X = (l1 * Xnegative1) - (Math.Pow(Xnegative1, 3)) - (gamma * Math.Pow(Math.Abs(Ynegative1), m) * Xnegative1);
                double Y = (l2 * Ynegative1) - (Math.Pow(Ynegative1, 3)) - (gamma * Math.Pow(Math.Abs(Znegative1), m) * Ynegative1);
                double Z = (l3 * Znegative1) - (Math.Pow(Znegative1, 3)) - (gamma * Math.Pow(Math.Abs(Xnegative1), m) * Znegative1);
                double select = turning(Xnegative1, Ynegative1, Znegative1);
                for (int ii = 0; ii < domains.Length - 1; ii++)
                    if (domains[ii] <= select && domains[ii + 1] > select)
                    {
                        index = ii;
                        break;
                    }
                Random rr = new Random(index);
                int[] rand_sq = sq_rand_gen(Width, rr);
                for (int ii = 0; ii < rand_sq.Length && count < inputs.Length; ii++)
                {
                    int index2 = rand_sq[ii];
                    if ((image[index2, kk] % 8) < 4)
                        image[index2, kk] += inputs[count] * 4;
                    else if (inputs[count] == 0)
                        image[index2, kk] -= 4;
                    count++;
                }

                Xnegative1 = X; Ynegative1 = Y; Znegative1 = Z;
            }
            count = count - (count % 8);
            return image;
        }
        public int[] sq_rand_gen(int width, Random rr)
        {

            int[] sq_rand = new int[width];
            ArrayList squence = new ArrayList();
            for (int ii = 0; ii < width; ii++)
                squence.Add(ii);
            for (int ii = 0; ii < sq_rand.Length; ii++)
            {
                int count = squence.Count;
                int rand = rr.Next(count);
                int _fetch = Convert.ToInt32(squence[rand]);
                squence.Remove(_fetch);
                sq_rand[ii] = _fetch;
            }
            return sq_rand;
        }
        private double turning(double x, double y, double z)
        {
            double select;
            double temp1 = Math.Abs(x);
            double temp2 = Math.Abs(y);
            double temp3 = Math.Abs(z);
            if (temp1 > temp2 && temp1 > temp3)
                select = x;
            else if (temp2 > temp1 && temp2 > temp3)
                select = y;
            else select = z;
            return (select);
        }
        private void restrict()
        {
            domains[0] = -2.0;
            for (int i = 1; i < 65536; i++)
            {
                domains[i] = domains[i - 1] + domain;
            }
        }
    }
}
