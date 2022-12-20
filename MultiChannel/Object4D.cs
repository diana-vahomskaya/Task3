using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D
{
    class mat4
    {
        // Ранг матрицы
        int _rank = 4;
        double M_PI = Math.PI;

        // Элементы матрицы
        public double[][] _matrix;

        // Конструктор по умолчанию
        public mat4()
        {
            _rank = 4;
            _matrix = new double[_rank][];
            for (int i = 0; i < _rank; i++)
                _matrix[i] = new double[_rank];

            reset();
        }

        // Конструктор
        public mat4(mat4 matr)
        {
            _matrix = matr._matrix;
        }

        // Конструктор
        public mat4(double m11, double m12, double m13,
            double m21, double m22, double m23,
            double m31, double m32, double m33,
            double dx = 0, double dy = 0, double dz = 0)
        {
            reset();
            _matrix[0][0] = m11;
            _matrix[0][1] = m12;
            _matrix[0][2] = m13;
            _matrix[0][3] = 0;

            _matrix[1][0] = m21;
            _matrix[1][1] = m22;
            _matrix[1][2] = m23;
            _matrix[1][3] = 0;

            _matrix[2][0] = m31;
            _matrix[2][1] = m32;
            _matrix[2][2] = m33;
            _matrix[2][3] = 0;

            _matrix[3][0] = dx;
            _matrix[3][1] = dy;
            _matrix[3][2] = dz;
            _matrix[3][3] = 1;
        }

        // Операция умножения на матрицу
        public static mat4 operator *(mat4 left, mat4 right)
        {
            mat4 _newmatrix = new mat4();
            for (int i = 0; i < _newmatrix._rank; i++)//строка 
            {
                for (int j = 0; j < _newmatrix._rank; j++)//столбцы
                {
                    _newmatrix._matrix[i][j] = 0;//создаём новую матрицу
                    for (int k = 0; k < _newmatrix._rank; k++)//для умножения 2-ух матриц
                    {
                        _newmatrix._matrix[i][j] += (left._matrix[i][k]) * (right._matrix[k][j]);
                    }
                }
            }
            return _newmatrix;
        }

        // Сброс матрицы в единичную
        public void reset()
        {
            this._matrix = new double[4][];
            this._matrix[0] = new double[4];
            this._matrix[1] = new double[4];
            this._matrix[2] = new double[4];
            this._matrix[3] = new double[4];
            for (int i = 0; i < 4; i++)
            {
                _matrix[i][i] = 1;
            }
        }

        // Поворот относительно оси X. Угол в градусах!
        public void rotateX(double angle)
        {
            double fi = angle * M_PI / 180;

            mat4 rmatr = new mat4(
                1.0, 0.0, 0.0,
                0.0, Math.Cos(fi), Math.Sin(fi),
                0.0, -Math.Sin(fi), Math.Cos(fi));

            mat4 matr = new mat4(this * rmatr);
            this._matrix = matr._matrix;
        }

        // Поворот относительно оси Y. Угол в градусах!
        public void rotateY(double angle)
        {
            double fi = angle * M_PI / 180;

            mat4 rmatr = new mat4(
                Math.Cos(fi), 0.0, -Math.Sin(fi),
                0.0, 1.0, 0.0,
                Math.Sin(fi), 0.0, Math.Cos(fi));

            mat4 matr = new mat4(this * rmatr);
            this._matrix = matr._matrix;
        }

        // Поворот относительно оси Z. Угол в градусах!
        public void rotateZ(double angle)
        {
            double fi = angle * M_PI / 180;

            mat4 rmatr = new mat4(
                Math.Cos(fi), Math.Sin(fi), 0.0,
                -Math.Sin(fi), Math.Cos(fi), 0.0,
                0.0, 0.0, 1.0);

            mat4 matr = new mat4(this * rmatr);
            this._matrix = matr._matrix;
        }

        // Перенос (сдвиг)
        public void translate(double x, double y, double z)
        {
            mat4 tmatr = new mat4(
                1.0, 0.0, 0.0,
                0.0, 1.0, 0.0,
                0.0, 0.0, 1.0,
                x, y, z);

            mat4 matr = new mat4(this * tmatr);
            this._matrix = matr._matrix;
        }

        // Масштабирование
        public void scale(double x, double y, double z)
        {
            mat4 smatr = new mat4(
                x, 0.0, 0.0,
                0.0, y, 0.0,
                0.0, 0.0, z);

            mat4 matr = new mat4(this * smatr);
            this._matrix = matr._matrix;
        }

        // Перспективное преобразование вдоль оси X
        public void perspective(double center)
        {
            mat4 pmatr = new mat4(
                1.0, 0.0, 0.0,
                0.0, 1.0, 0.0,
                0.0, 0.0, 1.0);
            pmatr._matrix[0][3] = -1 / center;

            mat4 matr = new mat4(this * pmatr);
            this._matrix = matr._matrix;
        }
    };

    class vec4
    {
        // Размерность вектора
        int _size = 4;

        // Элементы вектора
        public double[] _vector;

        // Конструктор
        public vec4(double x = 0, double y = 0, double z = 0)
        {
            _vector = new double[_size];
            _vector[0] = x;
            _vector[1] = y;
            _vector[2] = z;
            _vector[3] = 1;
        }

        public vec4(vec4 vect)
        {
            this._vector = vect._vector;
        }

        // Операция умножения на матрицу
        public static vec4 operator *(vec4 left, mat4 right)
        {
            vec4 _newvector = new vec4();
            for (int i = 0; i < left._size; i++)
            {
                _newvector._vector[i] = 0;
                for (int k = 0; k < left._size; k++)
                {
                    _newvector._vector[i] += (left._vector[k]) * (right._matrix[k][i]);
                }
            }
            for (int j = 0; j < left._size; j++)
            {
                _newvector._vector[j] /= _newvector._vector[left._size - 1];
            }
            return _newvector;
        }

        public float x()
        {
            return (float)_vector[0];
        }
        public float y()
        {
            return (float)_vector[1];
        }
        public float z()
        {
            return (float)_vector[2];
        }
    };
}
