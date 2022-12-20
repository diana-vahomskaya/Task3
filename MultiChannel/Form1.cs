using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiChannel
{
    public partial class Form1 : Form
    {
        Random rnd;
        Thread SNR_exp_thread, dopler_exp_thread;

        double A, devA, DF, modF, devF;
        Modulation pure_md, dopler_md;

        int mod_type;
        bool[] maximize = new bool[3] { false, false, false };

        int accuracy = 5;

        int CountsPerBit;

        int[] bits, changed_bits;

        double[][] x, wx;

        double[][] s, ws;
        double[][] ns, nws;

        double[][] rxx;
        double[][] SNR_Exp_results, dopler_Exp_results;

        Complex[][] cs, wcs;
        Complex[][] crxx;

        double SNR, minSNR, maxSNR;


        double[][] SNR_values;

        double doplerF, min_doplerF = 0, max_doplerF = 1000;


        double[][] doplerF_values;

        double[][][] Z;


        int m_type = 0,
            N,  // Число усреднений
            M,  // Число значений SNR
            L;  // Число значений добавки Доплера

        public Form1()
        {
            InitializeComponent();

            StartInit();

            UpdateValues();
        }
        public void UpdateValues()
        {
            A = ToDouble(Amp_param);
            devA = ToDouble(devAmp_param);
            DF = ToDouble(DFreq_param);
            modF = ToDouble(modFreq_param);
            devF = ToDouble(devFreq_param);
            doplerF = ToDouble(doplerF_param);
            CountsPerBit = ToInt(CountsPerBit_param);
            pure_md = new Modulation(DF, modF, devF, CountsPerBit, A, devA);
            dopler_md = new Modulation(DF, modF + doplerF, devF, CountsPerBit, A, devA);

            dTau_param.Text = (CountsPerBit / DF).ToString($"F{accuracy}");

            bits = Bits.FromString(Data_param.Text);
            changed_bits = ExpandBits(bits);

            N = ToInt(N_param);

            M = ToInt(M_param);
            SNR = ToDouble(SNR_param);
            minSNR = ToDouble(minSNR_param);
            maxSNR = ToDouble(maxSNR_param);

            L = M;

            SNR_values = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                SNR_values[i] = new double[M + 1];
            }
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < M + 1; i++)
                {
                    SNR_values[j][i] = minSNR + i * (maxSNR - minSNR) / M;
                }
            }

            doplerF_values = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                doplerF_values[i] = new double[L + 1];
            }
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < L + 1; i++)
                {
                    doplerF_values[j][i] = min_doplerF + i * (max_doplerF - min_doplerF) / L;
                }
            }

            x = new double[3][];
            s = new double[3][];
            ns = new double[3][];
            cs = new Complex[3][];
            Z = new double[3][][];

            wx = new double[3][];
            ws = new double[3][];
            nws = new double[3][];
            wcs = new Complex[3][];

            rxx = new double[3][];

            SNR_Exp_results = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                SNR_Exp_results[i] = new double[M + 1];
            }
            dopler_Exp_results = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                dopler_Exp_results[i] = new double[L + 1];
            }

            for (int i = 0; i < 3; i++)
            {
                GetSignal(i, s, ns, x, bits, SNR, pure_md);
            }
        }
        public void StartInit()
        {
            rnd = new Random();

            mod_type = 0;

            Bitrate_param.Text = (ToDouble(DFreq_param) / ToInt(CountsPerBit_param)).ToString();
        }

        public void GetSignal(int idx, double[][] signal, double[][] noised_signal, double[][] time_arr, int[] data_bits, double _SNR, Modulation _md)
        {
            switch (idx)
            {
                case 0:
                    signal[idx] = _md.PM2(data_bits);
                    time_arr[idx] = _md.GetTime();
                    noised_signal[idx] = Arrays.AddNoise(signal[idx], _SNR);
                    break;
                case 1:
                    signal[idx] = _md.FT(data_bits);
                    time_arr[idx] = _md.GetTime();
                    noised_signal[idx] = Arrays.AddNoise(signal[idx], _SNR);
                    break;
                case 2:
                    signal[idx] = _md.AM(data_bits);
                    time_arr[idx] = _md.GetTime();
                    noised_signal[idx] = Arrays.AddNoise(signal[idx], _SNR);
                    break;
            }
        }
        private int[] ExpandBits(int[] data_bits)
        {
            int[] to_return;
            List<int> additional_arr = new List<int>();
            int add;
            add = rnd.Next(-2, 2) * 2;
            add = 0;
            double tau = (bits.Length + add) * CountsPerBit / DF;
            Tau_param.Text = tau.ToString($"F{accuracy}");
            for (int i = 0; i < data_bits.Length + add; i++)
            {
                additional_arr.Add(rnd.Next(0, 2));
            }
            to_return = Arrays.Merge(additional_arr.ToArray<int>(), data_bits);
            additional_arr.Clear();
            add = rnd.Next(-2, 2) * 2;
            add = 0;
            for (int i = 0; i < data_bits.Length + add; i++)
            {
                additional_arr.Add(rnd.Next(0, 2));
            }
            to_return = Arrays.Merge(to_return, additional_arr.ToArray<int>());
            return to_return;
        }
        private void Exp_action_Click(object sender, EventArgs e)
        {
            if (SNR_exp_rb.Checked)
            {
                SNR_ExpInit();
            }
            if (dopler_exp_rb.Checked)
            {
                dopler_ExpInit();
            }
        }
        private void SNR_ExpInit()
        {
            UpdateValues();
            textBox19.Text = "τ0 оценка::";
            textBox24.Text = "Δτ0:";
            GetSignal(mod_type, ws, nws, wx, changed_bits, SNR, pure_md);

            SNR_Exp(mod_type, SNR, ToDouble(Tau_param));

            DrawWS(mod_type);
            DrawRxx(mod_type);
        }
        private void dopler_ExpInit(bool _draw = true)
        {
            UpdateValues();

            textBox19.Text = "SKO:";
            textBox24.Text = "max/SKO:";

            GetSignal(mod_type, ws, nws, wx, changed_bits, SNR, dopler_md);

            dopler_Exp(mod_type, 10, ToDouble(Tau_param), modF + doplerF);

            if (_draw)
            {
                DrawWS(mod_type);
                DrawRxx(mod_type);
            }
        }

        private bool SNR_Exp(int _mode, double _SNR, double _tau, bool _threaded = false)
        {
            ns[_mode] = Arrays.AddNoise(s[_mode], _SNR);
            nws[_mode] = Arrays.AddNoise(ws[_mode], _SNR);

            for (int i = 0; i < wx.Length; i++)
            {
                rxx[_mode] = Arrays.Rxx(nws[_mode], ns[_mode]);
            }
            int max_idx = Arrays.GetMaxIdx(rxx[_mode]);
            double exp_tau = (double)(max_idx) * 1.0 / DF;
            if (!_threaded)
            {
                ExpTau_param.Text = exp_tau.ToString($"F{accuracy}");
                dTau_param.Text = "0";
            }
            if (Math.Abs(_tau - exp_tau) <= CountsPerBit / DF)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private double dopler_Exp(int _mode, double _SNR, double _tau, double _modF, bool _threaded = false)
        {
            ns[_mode] = Arrays.AddNoise(s[_mode], _SNR);
            dopler_md = new Modulation(DF, _modF, devF, CountsPerBit, A, devA);
            GetSignal(_mode, ws, nws, wx, changed_bits, 10, dopler_md);
            //nws[_mode] = Arrays.AddNoise(ws[_mode], _SNR);

            for (int i = 0; i < wx.Length; i++)
            {
                rxx[_mode] = Arrays.Rxx(nws[_mode], ns[_mode]);
            }
            double sko = Arrays.GetSKO(rxx[_mode], Arrays.GetAvg(rxx[_mode]));
            double max = Arrays.GetMax(rxx[_mode]);
            if (!_threaded)
            {
                ExpTau_param.Text = sko.ToString($"F{accuracy}");
                dTau_param.Text = (max / sko).ToString($"F{accuracy}");
            }
            return max / sko;
        }

        public void DrawWS(int idx)
        {
            chart_Signal.Series[0].Points.Clear();
            for(int i = 0; i < nws[idx].Length; i++)
            {
                chart_Signal.Series[0].Points.AddXY(wx[idx][i], nws[idx][i]);
            }
        }
        public void DrawRxx(int idx)
        {
            chart_Rxx.Series[0].Points.Clear();
            for(int i = 0; i < rxx[idx].Length; i++)
            {
                chart_Rxx.Series[0].Points.AddXY(wx[idx][i], rxx[idx][i]);
            }
        }
        public void DrawSNRStat()
        {
            chart_Exp.Series[0].Points.Clear();
            chart_Exp.Series[1].Points.Clear();
            chart_Exp.Series[2].Points.Clear();
            for(int i = 0; i < SNR_Exp_results[0].Length; i++)
            {
                chart_Exp.Series[0].Points.AddXY(SNR_values[0][i], SNR_Exp_results[0][i]);
                chart_Exp.Series[1].Points.AddXY(SNR_values[1][i], SNR_Exp_results[1][i]);
                chart_Exp.Series[2].Points.AddXY(SNR_values[2][i], SNR_Exp_results[2][i]);
            }
        }
        public void DrawDoplerStat()
        {
            chart_Exp.Series[0].Points.Clear();
            chart_Exp.Series[1].Points.Clear();
            chart_Exp.Series[2].Points.Clear();
            for (int i = 0; i < SNR_Exp_results[0].Length; i++)
            {
                chart_Exp.Series[0].Points.AddXY(doplerF_values[0][i], dopler_Exp_results[0][i]);
                chart_Exp.Series[1].Points.AddXY(doplerF_values[1][i], dopler_Exp_results[1][i]);
                chart_Exp.Series[2].Points.AddXY(doplerF_values[2][i], dopler_Exp_results[2][i]);
            }
        }

        private void manyExps_action_Click(object sender, EventArgs e)
        {
            if (SNR_exp_rb.Checked)
            {
                UpdateValues();

                for (int i = 0; i < 3; i++)
                {
                    GetSignal(i, s, ns, x, bits, SNR, pure_md);
                    GetSignal(i, ws, nws, wx, changed_bits, SNR, dopler_md);
                }
                SNR_ExpThread();
            }
            if (dopler_exp_rb.Checked)
            {
                UpdateValues();
                for (int i = 0; i < 3; i++)
                {
                    GetSignal(i, s, ns, x, bits, 10, pure_md);
                    GetSignal(i, ws, nws, wx, changed_bits, 10, dopler_md);
                }
                dopler_ExpThread();
            }
        }
        private void SNR_ExpThread()
        {
            double tau = ToDouble(Tau_param);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M + 1; j++)
                {
                    Parallel.For(0, 3, k =>
                    {
                        if (i == 0)
                        {
                            SNR_Exp_results[k][j] += (SNR_Exp(k, SNR_values[k][j], tau, true) == true ? 1.0 : 0.0);
                        }
                        else
                        {
                            double res = (SNR_Exp(k, SNR_values[k][j], tau, true) == true ? 1.0 : 0.0);
                            SNR_Exp_results[k][j] = (SNR_Exp_results[k][j] * (i) + res) / (i + 1);
                        }
                    });
                }
            }
            DrawSNRStat();
        }
        private void dopler_ExpThread()
        {
            double tau = ToDouble(Tau_param);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < L + 1; j++)
                {
                    Parallel.For(0, 3, k =>
                    {
                        if (i == 0)
                        {
                            dopler_Exp_results[k][j] += (dopler_Exp(k, 10, tau, modF + doplerF_values[k][j], true));
                        }
                        else
                        {
                            double res = (dopler_Exp(k, 10, tau, modF + doplerF_values[k][j], true));
                            dopler_Exp_results[k][j] = (dopler_Exp_results[k][j] * (i) + res) / (i + 1);
                        }
                    });
                }
            }
            DrawDoplerStat();
        }
        private void Draw3D_param_Click(object sender, EventArgs e)
        {
            UpdateValues();
            double tau = ToDouble(Tau_param);
            List<double> maxes = new List<double>();

            dopler_md = new Modulation(DF, modF + doplerF, devF, CountsPerBit, A, devA);
            for (int m = 0; m < 3; m++)
                GetSignal(m, ws, nws, wx, changed_bits, 10, dopler_md);

            for (int m = 0; m < 3; m++)
            {
                Z[m] = new double[M + 1][];
                for (int j = 0; j < M + 1; j++)
                {
                    dopler_md = new Modulation(DF, modF + doplerF_values[m][j], devF, CountsPerBit, A, devA);
                    GetSignal(m, s, ns, x, bits, 10, dopler_md);
                    rxx[m] = Arrays.Rxx(nws[m], ns[m]);

                    Z[m][j] = new double[rxx[0].Length];
                    for (int i = 0; i < rxx[m].Length; i++)
                    {
                        if (rxx[m][i] > 0)
                            Z[m][j][i] = rxx[m][i];
                        else
                            Z[m][j][i] = 0;
                    }
                    Z[m][j] = Arrays.Zip(Z[m][j], 8);
                    maxes.Add(Arrays.GetMax(Z[m][j]));
                }
            }
            double AbsoluteMax = Arrays.GetMax(maxes.ToArray<double>());
            double[][] X = new double[3][];
            double[][] Y = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                X[i] = Arrays.Normalize(doplerF_values[i], -4, 4);
                Y[i] = Arrays.Normalize(wx[i], -4, 4);
                Y[i] = Arrays.Zip(Y[i], 10);
                Y[i] = Arrays.Normalize(Y[i], -4, 4);
                for (int j = 0; j < M + 1; j++)
                {
                    Z[i][j] = Arrays.Normalize(Z[i][j], 0, maxes[j] / AbsoluteMax);
                    for (int k = 0; k < Z[i][j].Length; k++)
                    {
                        Z[i][j][k] = Math.Pow(Z[i][j][k], 1.5);
                    }

                }
            }

            Form3D f3d = new Form3D(X, Y, Z);
            f3d.Show();
        }


        private void PM2_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (PM2_rb.Checked)
            {
                mod_type = 0;
            }
        }

        private void FT_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (FT_rb.Checked)
            {
                mod_type = 1;
            }
        }

        private void AM_rb_CheckedChanged(object sender, EventArgs e)
        {
            if (AM_rb.Checked)
            {
                mod_type = 2;
            }
        }

        private void CountsPerBit_param_TextChanged(object sender, EventArgs e)
        {
            if (CountsPerBit_param.Text != "")
                if (ToInt(CountsPerBit_param) > 0)
                    Bitrate_param.Text = (ToDouble(DFreq_param) / ToInt(CountsPerBit_param)).ToString();
        }

        private void DFreq_param_TextChanged(object sender, EventArgs e)
        {
            if (DFreq_param.Text != "")
                if (ToInt(DFreq_param) > 0)
                    Bitrate_param.Text = (ToDouble(DFreq_param) / ToInt(CountsPerBit_param)).ToString();
        }
        public double ToDouble(TextBox TB)
        {
            return Convert.ToDouble(TB.Text);
        }
        public int ToInt(TextBox TB)
        {
            return Convert.ToInt32(TB.Text);
        }
    }
}
