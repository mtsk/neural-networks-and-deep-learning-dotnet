using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

namespace NeuralNet
{
    public static class Extensions
    {       
        public static List<List<T>> PartitionToRandomSubsets<T>(this List<T> list, int subsetSize)
        {
            // shuffle
            Random rnd = new Random();
            var shuffled = list.OrderBy(x => rnd.Next()).ToArray();

            int numberOfSubsets = list.Count / subsetSize;

            List<List<T>> result = new List<List<T>>();
            for (int subsetIndex = 0; subsetIndex < numberOfSubsets; subsetIndex++)
            {
                var subset = shuffled.Skip(subsetIndex * subsetSize).Take(subsetSize);
                result.Add(subset.ToList());
            }

            return result;
        }

        public static Matrix<float> HadamardProduct(this Matrix<float> matrix1, Matrix<float> matrix2)
        {
            if (matrix1.ColumnCount != matrix2.ColumnCount || matrix1.RowCount != matrix2.RowCount)
                throw new ArgumentException("Marixes do not have same dimensions!");

            var result = Matrix<float>.Build.Dense(matrix1.RowCount, matrix1.ColumnCount);

            for (int i = 0; i < result.RowCount; i++)
                for (int j = 0; j < result.ColumnCount; j++)
                    result[i, j] = matrix1[i, j] * matrix2[i, j];

            return result;
        }

        public static Matrix<float> Sigmoid(this Matrix<float> source)
        {
            var z_Calc = Matrix<float>.Build.Dense(source.RowCount, source.ColumnCount);

            for (int i = 0; i < z_Calc.RowCount; i++)
                for (int j = 0; j < z_Calc.ColumnCount; j++)
                    z_Calc[i, j] = (float)Math.Exp(-source[i, j]);

            z_Calc = 1 / (1 + z_Calc);

            return z_Calc;
        }

        public static Matrix<float> SigmoidPrime(this Matrix<float> source)
        {
            return source.Sigmoid().HadamardProduct(1 - source.Sigmoid());
        }
    }
}
