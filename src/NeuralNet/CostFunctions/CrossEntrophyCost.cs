using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace NeuralNet.CostFunctions
{
    public class CrossEntrophyCost : ICostFunction
    {
        public Matrix<float> Delta(Matrix<float> z, Matrix<float> a, Matrix<float> y)
        {
            return a - y;
        }

        public Matrix<float> Fn(Matrix<float> a, Matrix<float> y)
        {
            throw new NotImplementedException();
        }
    }
}
