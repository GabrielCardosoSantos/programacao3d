﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TesteFFT
{
    public partial class frmDrawing : Form
    {
        public codingTrain train;
        public Algorithms alg;

        float time = 0f;
        List<Tuple<float, float, float, float, float>> x_dft;
        List<Tuple<float, float, float, float, float>> y_dft;
        List<PointF> desenho;
        public frmDrawing()
        {
            InitializeComponent();
            train = new codingTrain();
            alg = new Algorithms();
            desenho = new List<PointF>();

            List<double> x = new List<double>();
            List<double> y = new List<double>();
            train.Values.ForEach(n => {
                x.Add(n.X);
                y.Add(n.Y);                
            });

            x_dft = alg.dft_epycicles(x);
            y_dft = alg.dft_epycicles(y);
        }

        public Vector2 epiCycles(Graphics graphics, double x, double y, float rotation, List<Tuple<float, float, float, float, float>> fourier)
        {
            foreach(var item in fourier)
            {
                double prevx = x;
                double prevy = y;
                float freq = item.Item3;
                float amp = item.Item4;
                float phase = item.Item5;
                x += amp * Math.Cos(freq * time + phase + rotation);
                y += amp * Math.Sin(freq * time + phase + rotation);

                Pen pen = new Pen(Color.Black);
                graphics.TranslateTransform(-(float)amp, -(float)amp);
                graphics.DrawEllipse(pen, (float)prevx, (float) prevy, amp * 2, amp * 2);
                graphics.TranslateTransform((float)amp, (float)amp);
                graphics.DrawLine(pen, (float)prevx, (float)prevy, (float)x, (float)y);
            }
            return new Vector2(Convert.ToSingle(x), Convert.ToSingle(y));
        }

        private void frmDrawing_Paint(object sender, PaintEventArgs e)
        {
            
            var graphics = e.Graphics;

            graphics.SmoothingMode = SmoothingMode.HighQuality;

            var vx = epiCycles(graphics, this.Width / 2, 200, 0, x_dft);
            var vy = epiCycles(graphics, 150, this.Height / 2 + 100, (float) Math.PI / 2, y_dft);
            var v = new PointF(vx.X, vy.Y);

            desenho.Insert(0, v);

            Pen pen = new Pen(Color.Black);
            graphics.DrawLine(pen, vx.X, vx.Y, v.X, v.Y);
            graphics.DrawLine(pen, vy.X, vy.Y, v.X, v.Y);

            if (desenho.Count > 1)
            {
                var path = new GraphicsPath();
                var points = this.desenho.ToArray();
                path.AddCurve(points);
                Pen pen1 = new Pen(Color.Red, 2);
                graphics.DrawPath(pen1, path);
            }
        
            time += (float) ((2 * Math.PI) / this.train.Values.Count);
            if (time > 2 * Math.PI)
            {
                time = 0;
                desenho.Clear();
            }
                  
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}
