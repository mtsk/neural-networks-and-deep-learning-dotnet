using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNet.CostFunctions
{
    public interface ICostFunction
    {
        Matrix<float> Fn(Matrix<float> a, Matrix<float> y);

        Matrix<float> Delta(Matrix<float> z, Matrix<float> a, Matrix<float> y);
    }
}
