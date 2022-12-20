using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiChannel
{
    public class Arrays
    {
        // <summary>
        /// Наложение шума с заданной энергией на передаваемый массив
        /// </summary>
        /// <param name="array"> Массив, который будет зашумлен </param>
        /// <param name="SNR"> Значение SNR </param>
        /// <returns> Зашумленный массив </returns>
        public static double[] AddNoise(double[] array, double SNR)
        {
            Random rand = new Random();
            /// Начнем накладывать шум
            double[] noise = new double[array.Length];
            double[] to_return = new double[array.Length];
            /*for (int k = 0; k < array.Length; k++)
            {
                noise[k] = (rand.NextDouble() - 0.5) / 12;
            }*/
            for (int i = 0; i < 12; i++)
            {
                for (int k = 0; k < array.Length; k++)
                {
                    noise[k] += (rand.NextDouble() - 0.5) / 12;
                }
            }
            /// Отнормировали шум
            double s_energy = GetEnergy(array);
            double n_energy = GetEnergy(noise);
            double multiplier = Math.Sqrt(s_energy / n_energy * Math.Pow(10, -SNR / 10));
            for (int k = 0; k < array.Length; k++)
            {
                to_return[k] = array[k] + noise[k] * multiplier;
            }
            return to_return;
        }

        /// <summary>
        /// Получение энергии массива
        /// </summary>
        /// <param name="array"> Массив </param>
        /// <returns> Энергия массива array </returns>
        private static double GetEnergy(double[] array)
        {
            double energy = 0;
            for (int i = 0; i < array.Length; i++)
            {
                energy += array[i] * array[i];
            }
            return energy;
        }

        public static double[] Rxx(double[] _arr1, double[] _arr2)
        {
            double[] arr1, arr2, to_return;

            if (_arr1.Length >= _arr2.Length)
            {
                arr1 = _arr1;
                arr2 = _arr2;
                to_return = new double[arr1.Length];
            }
            else
            {
                arr1 = _arr2;
                arr2 = _arr1;
                to_return = new double[arr1.Length];
            }

            for (int i = 0; i < arr1.Length; i++)
            {
                for (int j = 0; j < arr2.Length; j++)
                {
                    to_return[i] += arr1[(arr1.Length + i + j) % arr1.Length] * arr2[j];
                }
            }

            return to_return;
        }

        public static double[] Normalize(double[] arr, double min = 0, double max = 1)
        {
            double[] to_return = new double[arr.Length];

            double maxVal = Arrays.GetMax(arr);
            double minVal = Arrays.GetMin(arr);

            for (int i = 0; i < arr.Length; i++)
            {
                to_return[i] = min + (max - min) * (arr[i] - minVal) / (maxVal - minVal);
            }

            return to_return;
        }
        public static double[] Zip(double[] arr, int k = 2)
        {
            double[] to_return = new double[(int)(arr.Length / k)];
            double avg = 0;
            for (int i = 0; i < to_return.Length; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    if (i * k + j < arr.Length)
                        avg += arr[i * k + j];
                    else
                        avg += 0;
                }
                to_return[i] = avg / k;
                avg = 0;
            }
            return to_return;
        }

        public static double GetSKO(double[] arr, double value)
        {
            double to_return = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                to_return += (arr[i] - value) * (arr[i] - value);
            }
            return Math.Sqrt(to_return / arr.Length);
        }
        public static double GetMax(double[] arr)
        {
            double max = Double.MinValue;
            for (int i = 0; i < arr.Length; i++)
            {
                if (max < arr[i]) max = arr[i];
            }
            return max;
        }
        public static double GetAvg(double[] arr)
        {
            double to_return = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                to_return += arr[i] / arr.Length;
            }
            return to_return;
        }
        public static int GetMaxIdx(double[] arr)
        {
            int max = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[max] < arr[i]) max = i;
            }
            return max;
        }
        public static double GetMin(double[] arr)
        {
            double min = Double.MaxValue;
            for (int i = 0; i < arr.Length; i++)
            {
                if (min > arr[i]) min = arr[i];
            }
            return min;
        }


        public static int[] Merge(int[] arr1, int[] arr2)
        {
            int[] to_return = new int[arr1.Length + arr2.Length];
            for (int i = 0; i < arr1.Length; i++)
            {
                to_return[i] = arr1[i];
            }
            for (int i = arr1.Length; i < to_return.Length; i++)
            {
                to_return[i] = arr2[i - arr1.Length];
            }
            return to_return;
        }
    }
}
