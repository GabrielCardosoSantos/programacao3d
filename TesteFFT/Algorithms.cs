﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteFFT
{
    public class Algorithms
    {
        //
        List<double> cosList = new List<double>();
        List<double> sinList = new List<double>();

        public List<double> IDFT(List<double> values)
        {
            double[] valores = new double[values.Count];
            
            for (int n = 0; n < values.Count; n++)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    valores[i] += Math.Cos(2 * Math.PI * n / values.Count * i) * cosList[n] - Math.Sin(2 * Math.PI * n / values.Count * i) * sinList[n];
                }
            }
            
            return (new List<double>(valores));
        }


        public List<Epicycle> dft_epycicles(List<double> x)
        {
            List<Epicycle> X = new List<Epicycle>();
            int N = x.Count;
            for (int k = 0; k < N; k++)
            {

                //double re = 0;
                //double im = 0;
                //for (int n = 0; n < N; n++)
                //{
                //    double phi = (Math.PI * 2 * k * n) / N;
                //    re += x[n] * Math.Cos(phi);
                //    im -= x[n] * Math.Sin(phi);
                //}
                //re = re / N;
                //im = im / N;

                //double freq = k;
                //double amp = Math.Sqrt(re * re + im * im);
                //double phase = Math.Atan2(im, re);

                //X.Add(new Epicycle(re, im, freq, amp, phase));
                
                double cos = 0.0;
                double sin = 0.0;

                for (int i = 0; i < x.Count; i++)
                {
                    cos += x[i] * Math.Cos(2 * Math.PI * k / x.Count * i);
                    sin -= x[i] * Math.Sin(2 * Math.PI * k / x.Count * i);
                }

                cos = cos / x.Count;
                sin = sin / x.Count;

                double freq = k;
                double amp = Math.Sqrt(cos * cos + sin * sin);
                double phase = Math.Atan2(sin, cos);



                X.Add(new Epicycle(1f / x.Count * cos, 1f / x.Count * sin, freq, amp, phase));
            }
            
            return X.OrderByDescending(n => n.amp).ToList();
        }

        public List<double> DFT(List<double> values)
        {

            if (cosList.Count > 0)
            {
                cosList.Clear();
                sinList.Clear();
            }
            
            for (int n = 0; n < values.Count; n++)
            {
                double cos = 0.0;
                double sin = 0.0;

                for (int i = 0; i < values.Count; i++)
                {
                    cos += values[i] * Math.Cos(2 * Math.PI * n / values.Count * i);
                    sin += values[i] * Math.Sin(2 * Math.PI * n / values.Count * i);
                }

                //cosList.Add(cos);
                //sinList.Add(sin);

                cosList.Add(1f / values.Count * cos);
                sinList.Add(-1f / values.Count * sin);
            }

            return cosList;
        }

        public List<double> IDCT(List<double> values)
        {
            List<double> valores = new List<double>();
            int n = values.Count;
            for (int i = 0; i < n; ++i)
            {
                double soma = 0.0;
                double s;
                for (int k = 0; k < n; ++k)
                {
                    if (k == 0)
                        s = Math.Sqrt(0.5);
                    else
                        s = 1.0;
                    soma += s * values[k] * Math.Cos(Math.PI * (i + 0.5) * k / n);
                }
                var v = soma * Math.Sqrt(2f / values.Count);
                valores.Add(v);
            }

            return valores;
        }

        public List<double> DCT(List<double> values)
        {
            List<double> valores = new List<double>();

            for (int i = 0; i < values.Count; ++i)
            {
                double s, n, sum = 0f;

                if (i == 0)
                    s = Math.Sqrt(0.5);
                else
                    s = 1.0;

                for (int j = 0; j < values.Count; ++j)
                    sum += s * values[j] * Math.Cos(Math.PI * (j + 0.5) * i / values.Count);

                n = sum * Math.Sqrt(2f / values.Count);

                valores.Add(n);
            }

            return valores;
        }


    }
}
