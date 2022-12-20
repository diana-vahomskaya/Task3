using System;
using System.Drawing;
using System.Windows.Forms;

namespace _3D
{
    public class Drawer
    {
        public PictureBox pic_box;
        public Bitmap bmp;
        public Graphics g;

        public Color
            ColorBackGround = Color.Black,
            ColorPen = Color.Green,
            ColorBrush = Color.Black;

        public int width, height;
        public double minX, maxX, minY, maxY, minZ, maxZ;

        public Drawer() { }
        public Drawer(PictureBox pb)
        {
            pic_box = pb;
            width = pic_box.Width;
            height = pic_box.Height;

            minX = 0;
            minY = 0;
            minZ = 0;
            maxX = 1;
            maxY = 1;
            maxZ = 1;

            InitGraphics();
        }
        public Drawer(PictureBox pb, Array _sizes)
        {
            pic_box = pb;
            width = pic_box.Width;
            height = pic_box.Height;

            InitGraphics();
            if (_sizes == null || _sizes.Length == 0)
            {
                minX = 0;
                minY = 0;
                minZ = 0;
                maxX = 1;
                maxY = 1;
                maxZ = 1;
            }
            else
            {
                Resize(_sizes);
            }
        }
        public void InitGraphics()
        {
            bmp = new Bitmap(width, height);
            g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }
        public void Resize(Array _sizes)
        {
            double[] sizes = new double[6];
            for (int i = 0; i < 6; i++)
            {
                sizes[i] = (int)(i * 2) / 6; ;
            }
            for (int i = 0; i < _sizes.Length; i++)
            {
                sizes[i] = ((double[])_sizes)[i];
            }
            minX = sizes[0];
            maxX = sizes[1];
            minY = sizes[2];
            maxY = sizes[3];
            minZ = sizes[4];
            maxZ = sizes[5];
        }
        public void Resize(double[] x, double[] y, double[][] z)
        {
            minX = double.MaxValue;
            maxX = double.MinValue;
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] < minX) minX = x[i];
                if (x[i] > maxX) maxX = x[i];
            }

            minY = double.MaxValue;
            maxY = double.MinValue;
            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] < minY) minY = y[i];
                if (y[i] > maxY) maxY = y[i];
            }

            minZ = double.MaxValue;
            maxZ = double.MinValue;
            for (int i = 0; i < x.Length; i++)
            {
                for (int j = 0; j < y.Length; j++)
                {
                    if (z[i][j] < minZ) minZ = z[i][j];
                    if (z[i][j] > maxZ) maxZ = z[i][j];
                }
            }
        }
        public void GraphicsSetTransform()
        {
            g.ResetTransform();
            g.ScaleTransform((float)((double)width / (maxX - minX)), -(float)((double)height / (maxY - minY)));
            g.TranslateTransform(-(float)minX, (float)minY);
        }

       
    }
    public class Drawer3D : Drawer
    {
        public Drawer3D() { }
        public Drawer3D(PictureBox pb)
        {
            pic_box = pb;
            width = pic_box.Width;
            height = pic_box.Height;

            minX = 0;
            minY = 0;
            minZ = 0;
            maxX = 1;
            maxY = 1;
            maxZ = 1;

            InitGraphics();
        }
        public Drawer3D(PictureBox pb, Array _sizes)
        {
            pic_box = pb;
            width = pic_box.Width;
            height = pic_box.Height;

            if (_sizes == null || _sizes.Length < 1)
            {
                minX = 0;
                minY = 0;
                minZ = 0;
                maxX = 1;
                maxY = 1;
                maxZ = 1;
                InitGraphics();
            }
            else
            {
                Resize(_sizes);
                InitGraphics();
            }
        }

        private void DrawAxis(mat4 m4, double mult = 1.5, double offset = 1.1)
        {
            double x1 = 0, y1 = 0, z1 = 0;
            double x2 = 0, y2 = 0, z2 = 0;
            double x3 = 0, y3 = 0, z3 = 0;
            double x4 = 0, y4 = 0, z4 = 0;

            vec4 v1;
            vec4 v2;
            vec4 v3;
            vec4 v4;

            Pen axis_pen = new Pen(Color.Pink, 0.005f);

            x1 = minX * offset;
            x2 = maxX * offset;
            y1 = minY * offset;
            y2 = minY * offset;
            z1 = minZ;
            z2 = minZ;

            v1 = new vec4(x1, y1, z1);
            v2 = new vec4(x2, y2, z2);
            v1 = v1 * m4;
            v2 = v2 * m4;
            PointF[] matr1 = new PointF[]
                    {
                        new PointF(v1.y(), v1.z()),
                        new PointF(v2.y(), v2.z())
                    };
            g.DrawLine(axis_pen, matr1[0], matr1[1]);

            x1 = minX * offset;
            x2 = minX * offset;
            y1 = minY * offset;
            y2 = maxY * offset;
            z1 = minZ;
            z2 = minZ;

            v1 = new vec4(x1, y1, z1);
            v2 = new vec4(x2, y2, z2);
            v1 = v1 * m4;
            v2 = v2 * m4;
            matr1 = new PointF[]
                    {
                        new PointF(v1.y(), v1.z()),
                        new PointF(v2.y(), v2.z())
                    };
            g.DrawLine(axis_pen, matr1[0], matr1[1]);

            x1 = minX * offset;
            x2 = minX * offset;
            y1 = minY * offset;
            y2 = minY * offset;
            z1 = minZ;
            z2 = maxZ * mult;

            v1 = new vec4(x1, y1, z1);
            v2 = new vec4(x2, y2, z2);
            v1 = v1 * m4;
            v2 = v2 * m4;
            matr1 = new PointF[]
                    {
                        new PointF(v1.y(), v1.z()),
                        new PointF(v2.y(), v2.z())
                    };
            g.DrawLine(axis_pen, matr1[0], matr1[1]);
        }


        /// <summary>
        /// Отрисовка 3D фигуры, вид которой определяется уравнением z = f(x, y);
        /// </summary>
        /// <param name="x"></param> массив х координат
        /// <param name="y"></param> массив у координат
        /// <param name="z"></param> массив значений функции f (координаты z)
        /// <param name="angleX"></param> угол по оси ох
        /// <param name="angleY"></param> угол по оси оy
        /// <param name="angleZ"></param> угол по оси оz
        /// <param name="mult"></param> величина зумирования
        public void Draw(double[] x, double[] y, double[][] _z, double angleX, double angleY, double angleZ, double mult, bool _normalize = false)
        {
            Resize(x, y, _z);

            double[][] z;
            /*if (_normalize)
            {
                z = new double[_z.Length][];
                for (int i = 0; i < z.Length; i++)
                {
                    z[i] = new double[_z[i].Length];
                    for (int j = 0; j < z[i].Length; j++)
                    {
                        z[i][j] = _z[i][j] / maxZ;
                    }
                }
                maxZ = 1;
            }
            else
            {
                z = _z;
            }*/
            z = _z;

            GraphicsSetTransform();
            g.Clear(ColorBackGround);

            int l1 = x.Length;
            int l2 = y.Length;

            double x1 = 0, y1 = 0, z1 = 0;
            double x2 = 0, y2 = 0, z2 = 0;
            double x3 = 0, y3 = 0, z3 = 0;
            double x4 = 0, y4 = 0, z4 = 0;

            vec4 v1;
            vec4 v2;
            vec4 v3;
            vec4 v4;

            SolidBrush brush = new SolidBrush(ColorBrush);
            Pen pen = new Pen(ColorPen, (float)0);

            mat4 m4 = new mat4();
            m4.translate(0, 0, -maxZ / 2);
            m4.scale(mult, mult, mult);
            m4.perspective(1000);
            m4.rotateZ(angleX);
            m4.rotateY(angleY);
            m4.rotateZ(angleZ);

            DrawAxis(m4);

            for (int i = 1; i < l1; i++)
            {
                x1 = x[i - 1];
                x2 = x[i];
                x3 = x[i];
                x4 = x[i - 1];
                for (int j = 1; j < l2; j++)
                {
                    y1 = y[j - 1];
                    y2 = y[j - 1];
                    y3 = y[j];
                    y4 = y[j];

                    z1 = z[i - 1][j - 1];
                    z2 = z[i][j - 1];
                    z3 = z[i][j];
                    z4 = z[i - 1][j];

                    v1 = new vec4(x1, y1, z1);
                    v2 = new vec4(x2, y2, z2);
                    v3 = new vec4(x3, y3, z3);
                    v4 = new vec4(x4, y4, z4);

                    v1 = v1 * m4;
                    v2 = v2 * m4;
                    v3 = v3 * m4;
                    v4 = v4 * m4;

                    PointF[] matr = new PointF[]
                    {
                        new PointF(v1.y(), v1.z()),
                        new PointF(v2.y(), v2.z()),
                        new PointF(v3.y(), v3.z()),
                        new PointF(v4.y(), v4.z())
                    };

                    if (_normalize)
                    {
                        int avg = (int)((0.25 * (z1 + z2 + z3 + z4)) * 255);
                        float avg_f = avg / 255.0f;

                        int red = (int)(Math.Pow(avg_f, 0.2) * (255 - 0));
                        int blue = (int)(255 - Math.Pow(avg_f, 0.2) * 255);
                        int green = (int)(255 - Math.Pow(Math.Pow(2 * avg_f - 1, 2), 0.2) * 255);

                        Color clr = Color.FromArgb(
                            red,
                            green,
                            blue);

                        Pen pn = new Pen(clr, 0.0f);
                        SolidBrush brsh = new SolidBrush(clr);
                        g.DrawPolygon(pn, matr);
                        g.FillPolygon(brsh, matr);
                    }
                    else
                    {
                        g.DrawPolygon(pen, matr);
                        g.FillPolygon(brush, matr);
                    }
                }
            }

            pic_box.Image = bmp;
        }

    }
}
