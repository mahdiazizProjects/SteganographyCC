using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
namespace unmasked_with_cylcing_chaos
{
    class steganograph
    {
        int Height;
        int Width;
        double[] domains;
        double l1, l2, l3, m, gamma;
        double domain;
        double xzero; double yzero; double zzero;
        public steganograph(double L1, double L2, double L3, double Xzero, double Yzero, double Zzero, double Gamma, double M, int height, int width)
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
            domain = 4.0 / (65536 - 1);
            Height = height;
            Width = width;

        }
        private int Convert_Binary_to_Int(int[] temp)
        {
            int col = 0; ;
            for (int j = 0; j < temp.Length; j++)
            {

                col += temp[j] * (int)Math.Pow(2, j);

            }
            return col;
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
        public void stegno2(int[,] image,ref ArrayList output)
        {
            restrict();
            int count = 0;
            int[] bits = new int[8];
            double Xnegative1 = xzero;
            double Ynegative1 = yzero;
            double Znegative1 = zzero;
            int index = 0;
            double X = (l1 * Xnegative1) - (Math.Pow(Xnegative1, 3)) - (gamma * Math.Pow(Math.Abs(Ynegative1), m) * Xnegative1);
            double Y = (l2 * Ynegative1) - (Math.Pow(Ynegative1, 3)) - (gamma * Math.Pow(Math.Abs(Znegative1), m) * Ynegative1);
            double Z = (l3 * Znegative1) - (Math.Pow(Znegative1, 3)) - (gamma * Math.Pow(Math.Abs(Xnegative1), m) * Znegative1);
            double select = turning(Xnegative1, Ynegative1, Znegative1);
            Xnegative1 = X; Ynegative1 = Y; Znegative1 = Z;
            for (int ii = 0; ii < domains.Length - 1; ii++)
                if (domains[ii] <= select && domains[ii + 1] > select)
                {
                    index = ii;
                    break;
                }
            Random rr = new Random(index);
            int[] rand_sq2 = sq_rand_gen(Width, rr);
            for (int kk = 0; kk < rand_sq2.Length; kk++)
            {
                index = 0;
                X = (l1 * Xnegative1) - (Math.Pow(Xnegative1, 3)) - (gamma * Math.Pow(Math.Abs(Ynegative1), m) * Xnegative1);
                Y = (l2 * Ynegative1) - (Math.Pow(Ynegative1, 3)) - (gamma * Math.Pow(Math.Abs(Znegative1), m) * Ynegative1);
                Z = (l3 * Znegative1) - (Math.Pow(Znegative1, 3)) - (gamma * Math.Pow(Math.Abs(Xnegative1), m) * Znegative1);
                select = turning(Xnegative1, Ynegative1, Znegative1);
                for (int ii = 0; ii < domains.Length - 1; ii++)
                    if (domains[ii] <= select && domains[ii + 1] > select)
                    {
                        index = ii;
                        break;
                    }
                rr = new Random(index);
                int[] rand_sq = sq_rand_gen(Height, rr);
                if (kk == 0)
                {
                    int[] lengthbit = new int[16];
                    for (int ii = 0; ii < 33; ii++)
                    {
                        int index2 = rand_sq[ii];
                        int context = image[rand_sq2[kk],index2];
                        if (count == 16)
                        {
                            int temp = Convert_Binary_to_Int(lengthbit);
                            output.Add(temp);
                            count = 0;
                        }
                        if (context % 2 == 0)
                            lengthbit[count++] = 0;
                        else
                            lengthbit[count++] = 1;
                    }
                    count = 0;
                    for (int ii = 32; ii < rand_sq.Length; ii++)
                    {
                        int index2 = rand_sq[ii];
                        int context = image[rand_sq2[kk], index2];
                        if (count == 8)
                        {
                            int temp = Convert_Binary_to_Int(bits);
                            output.Add(temp);
                            count = 0;
                        }
                        if (context % 2 == 0)
                            bits[count++] = 0;
                        else
                            bits[count++] = 1;
                    }
                }
                else
                {
                    for (int ii = 0; ii < rand_sq.Length; ii++)
                    {
                        int index2 = rand_sq[ii];
                        int context = image[rand_sq2[kk],index2];
                        if (count == 8)
                        {
                            int temp = Convert_Binary_to_Int(bits);
                            output.Add(temp);
                            count = 0;
                        }
                        if (context % 2 == 0)
                            bits[count++] = 0;
                        else
                            bits[count++] = 1;
                    }
                }

                Xnegative1 = X; Ynegative1 = Y; Znegative1 = Z;
            }
            if (((Width * Height) % 8) != 0 )
                output.RemoveAt(output.Count - 1);
        }
        public void stegno(int[,] image,ref ArrayList output,int dominator,int sizeall)
        {
            restrict();
            int count = 0;
            int[] bits = new int[8];
            double Xnegative1 = xzero;
            double Ynegative1 = yzero;
            double Znegative1 = zzero;
            int index = 0;
            double X = (l1 * Xnegative1) - (Math.Pow(Xnegative1, 3)) - (gamma * Math.Pow(Math.Abs(Ynegative1), m) * Xnegative1);
            double Y = (l2 * Ynegative1) - (Math.Pow(Ynegative1, 3)) - (gamma * Math.Pow(Math.Abs(Znegative1), m) * Ynegative1);
            double Z = (l3 * Znegative1) - (Math.Pow(Znegative1, 3)) - (gamma * Math.Pow(Math.Abs(Xnegative1), m) * Znegative1);
            double select = turning(Xnegative1, Ynegative1, Znegative1);
            Xnegative1 = X; Ynegative1 = Y; Znegative1 = Z;
            for (int ii = 0; ii < domains.Length - 1; ii++)
                if (domains[ii] <= select && domains[ii + 1] > select)
                {
                    index = ii;
                    break;
                }
            Random rr = new Random(index);
            int[] rand_sq1 = sq_rand_gen(Width, rr);
            for (int kk = 0; kk < rand_sq1.Length; kk++)
            {
                index = 0;
                X = (l1 * Xnegative1) - (Math.Pow(Xnegative1, 3)) - (gamma * Math.Pow(Math.Abs(Ynegative1), m) * Xnegative1);
                Y = (l2 * Ynegative1) - (Math.Pow(Ynegative1, 3)) - (gamma * Math.Pow(Math.Abs(Znegative1), m) * Ynegative1);
                Z = (l3 * Znegative1) - (Math.Pow(Znegative1, 3)) - (gamma * Math.Pow(Math.Abs(Xnegative1), m) * Znegative1);
                select = turning(Xnegative1, Ynegative1, Znegative1);
                for (int ii = 0; ii < domains.Length - 1; ii++)
                    if (domains[ii] <= select && domains[ii + 1] > select)
                    {
                        index = ii;
                        break;
                    }
                rr = new Random(index);
                int[] rand_sq = sq_rand_gen(Height, rr);
                for (int ii = 0; ii < rand_sq.Length&&output.Count<=sizeall; ii++)
                {
                    int index2 = rand_sq[ii];
                    int context = image[rand_sq1[kk],index2];
                    if (count == 8)
                    {
                        int temp = Convert_Binary_to_Int(bits);
                        output.Add(temp);
                        count = 0;
                    }
                    if (context % (2*dominator )< dominator)
                        bits[count++] = 0;
                    else
                        bits[count++] = 1;
                }

                Xnegative1 = X; Ynegative1 = Y; Znegative1 = Z;
            }
            if (((Width * Height) % 8) != 0)
                output.RemoveAt(output.Count - 1);
        }
        public void save_pixels(ArrayList p,int start,string filename)
        {
            StreamWriter sr = new StreamWriter(filename);
            for (int i = start; i < p.Count; i++)
            {
                sr.Write(p[i]+ " ");
            }
            sr.Close();
        }
        //public void stegno_layer2(int[,] image, ref ArrayList output)
        //{
        //    restrict();
        //    int count = 0;
        //    int[] bits = new int[8];
        //    double Xnegative1 = xzero;
        //    double Ynegative1 = yzero;
        //    double Znegative1 = zzero;
        //    for (int kk = 0; kk < Width; kk++)
        //    {
        //        int index = 0;
        //        double X = (l1 * Xnegative1) - (Math.Pow(Xnegative1, 3)) - (gamma * Math.Pow(Math.Abs(Ynegative1), m) * Xnegative1);
        //        double Y = (l2 * Ynegative1) - (Math.Pow(Ynegative1, 3)) - (gamma * Math.Pow(Math.Abs(Znegative1), m) * Ynegative1);
        //        double Z = (l3 * Znegative1) - (Math.Pow(Znegative1, 3)) - (gamma * Math.Pow(Math.Abs(Xnegative1), m) * Znegative1);
        //        double select = turning(Xnegative1, Ynegative1, Znegative1);
        //        for (int ii = 0; ii < domains.Length - 1; ii++)
        //            if (domains[ii] <= select && domains[ii + 1] > select)
        //            {
        //                index = ii;
        //                break;
        //            }
        //        Random rr = new Random(index);
        //        int[] rand_sq = sq_rand_gen(Height, rr);
        //        for (int ii = 0; ii < rand_sq.Length; ii++)
        //        {
        //            int index2 = rand_sq[ii];
        //            int context = image[index2, kk];
        //            if (count == 8)
        //            {
        //                int temp = Convert_Binary_to_Int(bits);
        //                output.Add(temp);
        //                count = 0;
        //            }
        //            if ((context % 4) < 2)
        //                bits[count++] = 0;
        //            else
        //                bits[count++] = 1;
        //        }

        //        Xnegative1 = X; Ynegative1 = Y; Znegative1 = Z;
        //    }
        //    if (((Width * Height) % 8) != 0)
        //        output.RemoveAt(output.Count - 1);
        //}
        //public void stegno_layer3(int[,] image,ref ArrayList output)
        //{
        //    restrict();
        //    int count = 0;
        //    int[] bits = new int[8];
        //    double Xnegative1 = xzero;
        //    double Ynegative1 = yzero;
        //    double Znegative1 = zzero;
        //    for (int kk = 0; kk < Height; kk++)
        //    {
        //        int index = 0;
        //        double X = (l1 * Xnegative1) - (Math.Pow(Xnegative1, 3)) - (gamma * Math.Pow(Math.Abs(Ynegative1), m) * Xnegative1);
        //        double Y = (l2 * Ynegative1) - (Math.Pow(Ynegative1, 3)) - (gamma * Math.Pow(Math.Abs(Znegative1), m) * Ynegative1);
        //        double Z = (l3 * Znegative1) - (Math.Pow(Znegative1, 3)) - (gamma * Math.Pow(Math.Abs(Xnegative1), m) * Znegative1);
        //        double select = turning(Xnegative1, Ynegative1, Znegative1);
        //        for (int ii = 0; ii < domains.Length - 1; ii++)
        //            if (domains[ii] <= select && domains[ii + 1] > select)
        //            {
        //                index = ii;
        //                break;
        //            }
        //        Random rr = new Random(index);
        //        int[] rand_sq = sq_rand_gen(Width, rr);
        //        for (int ii = 0; ii < rand_sq.Length; ii++)
        //        {
        //            int index2 = rand_sq[ii];
        //            int context = image[index2, kk];
        //            if (count == 8)
        //            {
        //                int temp = Convert_Binary_to_Int(bits);
        //                output.Add(temp);
        //                count = 0;
        //            }
        //            if ((context % 8) < 4)
        //                bits[count++] = 0;
        //            else
        //                bits[count++] = 1;
        //        }

        //        Xnegative1 = X; Ynegative1 = Y; Znegative1 = Z;
        //    }
        //    if (((Width * Height) % 8) != 0)
        //        output.RemoveAt(output.Count - 1);
        //}
        public void save(string filename, string output)
        {
            StreamWriter w = new StreamWriter(filename);
            w.Write(output);
            w.Close();
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
