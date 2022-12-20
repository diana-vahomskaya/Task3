using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _3D;

namespace MultiChannel
{
    public partial class Form3D : Form
    {
        _3D.Drawer3D drw;
        double[][][] Z;
        double[][] X, Y;
        double angleX, angleY;
        int mode;
        int[] graph_count;
        bool first_save = true;

        public Form3D(double[][] _x, double[][] _y, double[][][] _z)
        {
            InitializeComponent();

            drw = new _3D.Drawer3D(MainPicture);
            mode = 0;

            X = _x;
            Y = _y;
            Z = _z;

            graph_count = new int[3];

            UpdateValues();
            PM2_rb.Checked = true;
        }

        private void TrackChange(object sender, EventArgs e)
        {
            UpdateValues();
            Draw(mode);
        }

        private void RB_changed(object sender, EventArgs e)
        {
            if (PM2_rb.Checked)
                mode = 0;
            if (FT_rb.Checked)
                mode = 1;
            if (AM_rb.Checked)
                mode = 2;
            Draw(mode);
        }

        void UpdateValues()
        {
            angleX = HorizontalTR_action.Value;
            angleY = VerticalTR_action.Value;
        }

        void Draw(int _mode)
        {
            drw.Draw(X[_mode], Y[_mode], Z[_mode], angleX - 180, angleY, 0, 0.6, true);

            double
                angleX1 = 35,
                angleY1 = 35,
                angleZ1 = 35,
                mult = 0.6,
                minX = -2,
                maxX = 2,
                minY = -2,
                maxY = 2,
                minZ = 0,
                maxZ = 4;

            double dx = (maxX - minX) / (double)100;
            double dy = (maxY - minY) / (double)100;

            double[] x = new double[100];
            double[] y = new double[100];
            double[][] z = new double[100][];
            for (int i = 0; i < 100; i++)
            {
                z[i] = new double[100];
                x[i] = minX + (i) * dx;
                for (int j = 0; j < 100; j++)
                {
                    y[j] = minY + (j) * dy;
                    if (x[i] * x[i] + y[j] * y[j] < 4)
                        z[i][j] = Math.Sqrt(4 - x[i] * x[i] - y[j] * y[j]);
                    else
                        z[i][j] = 0;
                }
            }
        }
    }
}
