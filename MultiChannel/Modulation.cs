using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiChannel
{
    public class Modulation
    {
        public static double A, deviateA;
        public static double DFreq, MainFreq, ModulationFreq, T;
        public int CountsPerBit;

        private double[] output;

        /// <summary>
        /// Инициализация всех полей, необходимых для модуляции
        /// </summary>
        /// <param name="_DFreq"> Частота дискретизации </param>
        /// <param name="_MainFreq"> Несущая частота </param>
        /// <param name="_ModulationFreq"> Модулирующая частота </param>
        /// <param name="_T"> Время длительности сигнала </param>
        /// <param name="_A"> Амплитуда сигнала </param>
        /// <param name="_deviateA"> Изменение амплитуды сигнала </param>
        public Modulation(double _DFreq, double _MainFreq, double _ModulationFreq, int _CountsPerBit = 64, double _A = 1, double _deviateA = 0.75)
        {
            DFreq = _DFreq;
            MainFreq = _MainFreq;
            ModulationFreq = _ModulationFreq;
            CountsPerBit = _CountsPerBit;
            A = _A;
            deviateA = _deviateA;
        }

        /// <summary>
        /// Формирование отчетов синусоиды. Можно вызвать один раз, а потом вызывать только GetTime для каждого следующего пакета (если надо будет отрисовывать)
        /// </summary>
        /// <param name="_bits"> Массив битов, которые надо преобразовать в ФМ2 промодулированный сигнал </param>
        /// <returns> Массив отчетов output[T * DFreq * ChannelsPerMessage] промодулированного сигнала </returns>
        public double[] PM2(int[] _bits)
        {
            int[] bits = _bits;
            output = new double[(int)(CountsPerBit * bits.Length)];

            double phase = 0;

            for (int i = 0; i < bits.Length; i++)
            {
                for (int j = 0; j < CountsPerBit; j++)
                {
                    output[i * CountsPerBit + j] = A * Math.Sin(phase);
                    phase += MainFreq / DFreq * Math.PI * 2;
                }
                if (i != bits.Length - 1)
                    if (bits[i] != bits[i + 1]) phase += Math.PI;
                if (phase > Math.PI * 2) phase -= Math.PI * 2;
            }
            return output;
        }

        /// <summary>
        /// Формирование отчетов синусоиды. Можно вызвать один раз, а потом вызывать только GetTime для каждого следующего пакета (если надо будет отрисовывать)
        /// </summary>
        /// <param name="_bits"> Массив битов, которые надо преобразовать в ФМ2 промодулированный сигнал </param>
        /// <returns> Массив отчетов output[T * DFreq * ChannelsPerMessage] промодулированного сигнала </returns>
        public double[] FT(int[] _bits)
        {
            int[] bits = _bits;
            output = new double[(int)(CountsPerBit * bits.Length)];

            double phase = 0;

            for (int i = 0; i < bits.Length; i++)
            {
                double CurrentFreq = MainFreq - (1 - bits[i]) * ModulationFreq;
                for (int j = 0; j < CountsPerBit; j++)
                {
                    output[i * CountsPerBit + j] = A * Math.Sin(phase);
                    phase += CurrentFreq / DFreq * Math.PI * 2;
                }
                if (phase > Math.PI * 2) phase -= Math.PI * 2;
            }
            return output;
        }

        /// <summary>
        /// Формирование отчетов синусоиды. Можно вызвать один раз, а потом вызывать только GetTime для каждого следующего пакета (если надо будет отрисовывать)
        /// </summary>
        /// <param name="_bits"> Массив битов, которые надо преобразовать в ФМ2 промодулированный сигнал </param>
        /// <returns> Массив отчетов output[T * DFreq * ChannelsPerMessage] промодулированного сигнала </returns>
        public double[] AM(int[] _bits)
        {
            int[] bits = _bits;
            output = new double[(int)(CountsPerBit * bits.Length)];

            double phase = 0;

            for (int i = 0; i < bits.Length; i++)
            {
                double CurrentA = A - (1 - bits[i]) * deviateA;
                for (int j = 0; j < CountsPerBit; j++)
                {
                    output[i * CountsPerBit + j] = CurrentA * Math.Sin(phase);
                    phase += MainFreq / DFreq * Math.PI * 2;
                }
                if (phase > Math.PI * 2) phase -= Math.PI * 2;
            }
            return output;
        }



        /// <summary>
        /// Формирование временной оси для отрисовки очетов (если надо будет)
        /// </summary>
        /// <param name="_CurrentTime"> Время, которое было перед вызовом метода (можно получить из предыдущего канала) </param>
        /// <returns> Возвращает time[T * DFreq * ChannelsPerMessage] отчетов времени, каждый из которых соответствует своему отчету сигнала </returns>
        public double[] GetTime(double _CurrentTime = 0)
        {
            if (output != null)
            {
                double[] time = new double[output.Length];
                time[0] = 0;
                for (int i = 1; i < time.Length; i++)
                {
                    time[i] = time[i - 1] + 1.0 / DFreq;
                }
                return time;
            }
            else
            {
                return new double[] { -1 };
            }
        }
    }
}
