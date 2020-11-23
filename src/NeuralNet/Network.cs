using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NeuralNet
{
    public class Network : INeuralNetwork
    {        
        private void LogMessage(string message)
        {
            if (NewLogMessage != null)
            {
                NewLogMessage(message);
            }
        }

        private int mNumberOfLayers;
        private int[] mNetworkLayerSizes;
        private Matrix<float>[] mBiases;   // bias matrix has only one column
        private Matrix<float>[] mWeights;  // mxn matrix for each layer, m-number of neurons in current layer, n-number of neurons in previous layer
        
        public Network(int[] netwowkLayerSizes)
        {
            mNumberOfLayers = netwowkLayerSizes.Length;
            mNetworkLayerSizes = netwowkLayerSizes;

            InitBiases();
            InitWeights();
        }

        private void InitBiases()
        {
            var biases = new List<Matrix<float>>();

            // add one list for each layer except first one (input layer)
            for (int i = 1; i < mNumberOfLayers; i++)
            {               
                var layerBiases = Matrix<float>.Build.Random(mNetworkLayerSizes[i], 1, new Normal(0,1));
                biases.Add(layerBiases);
            }

            mBiases = biases.ToArray();
        }

        private void InitWeights()
        {
            var weights = new List<Matrix<float>>();

            for (int i = 1; i < mNumberOfLayers; i++)
            {
                var layerWeights = Matrix<float>.Build.Random(mNetworkLayerSizes[i], mNetworkLayerSizes[i-1], new Normal(0, 1));
                weights.Add(layerWeights);
            }

            mWeights = weights.ToArray();
        }

        public Action<string> NewLogMessage
        {
            get; set;
        }

        public void SGD(
            List<Tuple<Matrix<float>, Matrix<float>>> trainingData, 
            int epochs, 
            int miniBatchSize,
            float eta, 
            List<Tuple<Matrix<float>, int>> testData)
        {
            for (int epoch = 0; epoch < epochs; epoch++)
            {
                var trainEpochTime = Stopwatch.StartNew();
                var miniBatches = trainingData.PartitionToRandomSubsets(miniBatchSize);
                foreach (var miniBatch in miniBatches)
                {
                    UpdateMiniBatch(miniBatch, eta);
                }
                trainEpochTime.Stop();

                var evaluationTime = Stopwatch.StartNew();
                var noOfCorrectResults = Evaluate(testData);
                evaluationTime.Stop();

                LogMessage(string.Format("Epoch {0} => Accuracy on evaluation data: {1} / {2}, Epoch training time: {3}, Evaluation time: {4}", 
                    epoch, noOfCorrectResults, testData.Count, 
                    trainEpochTime.Elapsed.ToString("d\\.hh\\:mm\\:ss"),
                    evaluationTime.Elapsed.ToString("d\\.hh\\:mm\\:ss")));
            }
        }
        
        private int Evaluate(List<Tuple<Matrix<float>, int>> testData)
        {
            int noOfCorrectResults = 0;

            foreach (var inputOutput in testData)
            {
                var neuralNetResultVector = Feedforward(inputOutput.Item1);
                int neuralNetResult = GetRowIndexWithMaxValueInFirsColumn(neuralNetResultVector);

                if (neuralNetResult == inputOutput.Item2)
                {
                    noOfCorrectResults++;
                }
            }

            return noOfCorrectResults;
        }

        private Matrix<float> Feedforward(Matrix<float> input)
        {
            var a = input;

            for (int i = 0; i < mNumberOfLayers - 1; i++)
            {
                var z = mWeights[i] * a + mBiases[i];
                a = Sigmoid(z);
            }

            return a;
        }

        private int GetRowIndexWithMaxValueInFirsColumn(Matrix<float> resultVector)
        {
            int maxIndex = 0;
            float maxValue = 0.0f;
            for (int i = 0; i < resultVector.RowCount; i++)
            {
                if (resultVector[i,0] > maxValue)
                {
                    maxValue = resultVector[i,0];
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        private void UpdateMiniBatch(List<Tuple<Matrix<float>, Matrix<float>>> miniBatch, float eta)
        {
            var nabla_b = CloneWithZeroes(mBiases);
            var nabla_w = CloneWithZeroes(mWeights);

            foreach (var inputOutput in miniBatch)
            {
                Matrix<float>[] delta_nabla_b;
                Matrix<float>[] delta_nabla_w;
                Backprop(inputOutput, out delta_nabla_b, out delta_nabla_w);

                for (int i = 0; i < nabla_b.Length; i++)
                    nabla_b[i] = nabla_b[i] + delta_nabla_b[i];

                for (int i = 0; i < nabla_w.Length; i++)
                    nabla_w[i] = nabla_w[i] + delta_nabla_w[i];
            }

            var etaDividedByM = eta / (float)miniBatch.Count;

            for (int i = 0; i < mBiases.Length; i++)
                mBiases[i] = mBiases[i] - etaDividedByM * nabla_b[i];

            for (int i = 0; i < mWeights.Length; i++)
                mWeights[i] = mWeights[i] - etaDividedByM * nabla_w[i];
        }

        private void Backprop(
            Tuple<Matrix<float>, Matrix<float>> inputOutput,
            out Matrix<float>[] nabla_b,
            out Matrix<float>[] nabla_w)
        {
            nabla_b = CloneWithZeroes(mBiases);
            nabla_w = CloneWithZeroes(mWeights);

            // feedforward
            var activation = inputOutput.Item1;
            var activations = new List<Matrix<float>>() { activation };     // layer-by-layer activations
            var zs = new List<Matrix<float>>();                             // layer-by-layer z vectors 

            for (int i = 0; i < mNumberOfLayers - 1; i++)
            {
                var z = mWeights[i] * activation + mBiases[i];
                zs.Add(z);

                activation = Sigmoid(z);
                activations.Add(activation);
            }

            // backward pass
            var delta = CostDerivative(activations[activations.Count - 1], inputOutput.Item2).HadamardProduct(SigmoidPrime(zs[zs.Count - 1]));
            nabla_b[nabla_b.Length - 1] = delta;
            nabla_w[nabla_w.Length - 1] = delta * activations[activations.Count - 2].Transpose();

            for (int i = 2; i < mNumberOfLayers; i++)
            {
                var z = zs[zs.Count - i];
                var sp = SigmoidPrime(z);
                delta = (mWeights[mWeights.Length - i + 1].Transpose() * delta).HadamardProduct(sp);
                nabla_b[nabla_b.Length - i] = delta;
                nabla_w[nabla_w.Length - i] = delta * activations[activations.Count - i - 1].Transpose();
            }
        }

        private Matrix<float> Sigmoid(Matrix<float> z)
        {
            var z_Calc = Matrix<float>.Build.Dense(z.RowCount, z.ColumnCount);

            for (int i = 0; i < z_Calc.RowCount; i++)
                for (int j = 0; j < z_Calc.ColumnCount; j++)
                    z_Calc[i, j] = (float)Math.Exp(-z[i, j]);

            z_Calc = 1 / (1 + z_Calc);

            return z_Calc;
        }

        private Matrix<float> SigmoidPrime(Matrix<float> z)
        {
            return Sigmoid(z).HadamardProduct(1 - Sigmoid(z));
        }

        private Matrix<float> CostDerivative(Matrix<float> output_activations, Matrix<float> y)
        {
            return output_activations - y;
        }

        private Matrix<float>[] CloneWithZeroes(Matrix<float>[] source)
        {
            var result = new Matrix<float>[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                var sourceValue = source[i];
                result[i] = Matrix<float>.Build.Dense(sourceValue.RowCount, sourceValue.ColumnCount);
            }

            return result;
        }
    }
}
