using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace NeuralNet
{
    public interface INeuralNetwork
    {
        Action<string> NewLogMessage { get; set; }

        void SGD(
            List<Tuple<Matrix<float>, Matrix<float>>> trainingData, 
            int epochs, 
            int batchSize, 
            float learningRate, 
            List<Tuple<Matrix<float>, int>> testData);
    }
}
