using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiChannel
{
    /// <summary>
    /// Работа с массивами бит в представлении int[]
    /// </summary>
    public static class Bits
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_bits"></param>
        /// <returns></returns>
        public static int[] FromString(string _bits)
        {
            List<int> bits = new List<int>();
            if (_bits.Length > 2)
            {
                if (_bits[0] + "" + _bits[1] == "0x")
                {
                    for (int i = 2; i < _bits.Length; i++)
                    {
                        if (_bits[i] > 47 && _bits[i] < 58)
                        {
                            bits.AddRange(Int.ToBits(_bits[i] - 48, 4));
                        }
                        if (_bits[i] > 96 && _bits[i] < 103)
                        {
                            bits.AddRange(Int.ToBits(_bits[i] - 97 + 10, 4));
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < _bits.Length; i++)
                {
                    bits[i] = _bits[i] == '0' ? 0 : 1;
                }
            }
            return bits.ToArray<int>();
        }


    }

    /// <summary>
    /// Работа с целыми числами
    /// </summary>
    public static class Int
    {
        /// <summary>
        ///                       Перевод числа в массив бит 
        /// </summary>
        /// <param name="_value"> Число от 0 до 2^N-1 </param>
        /// <param name="_value"> Задает определенный размер массива 
        ///                       Если задать его избыточным, лишние позиции будут заполнены нулями 
        ///                       Если задать его недостаточным, старшие степени будут утеряны </param>
        /// <returns>             Массив бит в представлении int[N]</returns>
        public static int[] ToBits(int _value, int _size = -1)
        {
            int value = _value;
            int[] bits;
            if (_size != -1)
            {
                bits = new int[_size];
            }
            else
            {
                bits = new int[(int)Math.Log(value, 2) + 1];
            }
            for (int i = (int)Math.Log(value, 2); i > -1; i--)
            {
                var minus = (int)Math.Pow(2, i);
                try
                {
                    if (value >= minus)
                    {
                        value -= minus;
                        bits[bits.Length - 1 - i] = 1;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    if (value >= minus)
                    {
                        value -= minus;
                    }
                }
            }
            return bits;
        }

    }
}
