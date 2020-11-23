using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using NeuralNet.CostFunctions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NeuralNet
{
    public class Network2 : INeuralNetwork
    {
        private int mNumberOfLayers;
        private int[] mNetworkLayerSizes;
        private Matrix<float>[] mBiases;   // bias matrix has only one column
        private Matrix<float>[] mWeights;  // mxn matrix for each layer, m-number of neurons in current layer, n-number of neurons in previous layer
        private ICostFunction mCostFunction = new CrossEntrophyCost();
        private float mRegularizationParam;
        private float mMomentumDamping;

        public Network2(
            int[] netwowkLayerSizes,
            float regularizationParam = 0.0f,
            float momentumDamping = 0.0f)
        {
            mNumberOfLayers = netwowkLayerSizes.Length;
            mNetworkLayerSizes = netwowkLayerSizes;
            mRegularizationParam = regularizationParam;
            mMomentumDamping = momentumDamping;

            InitBiases();
            InitWeights();
        }

        private void InitBiases()
        {
            var biases = new List<Matrix<float>>();

            // add one list for each layer except first one (input layer)
            for (int i = 1; i < mNumberOfLayers; i++)
            {
                var layerBiases = Matrix<float>.Build.Random(mNetworkLayerSizes[i], 1, new Normal(0, 1));
                biases.Add(layerBiases);
            }

            mBiases = biases.ToArray();
        }

        // init using standard deviation 1/sqrt(numberOfInputConnectionsToTheNeuron)
        private void InitWeights()
        {
            var weights = new List<Matrix<float>>();

            for (int i = 1; i < mNumberOfLayers; i++)
            {
                var layerWeights = Matrix<float>.Build.Random(mNetworkLayerSizes[i], mNetworkLayerSizes[i - 1],
                    new Normal(0, 1 / Math.Sqrt(mNetworkLayerSizes[i - 1])));
                weights.Add(layerWeights);
            }

            mWeights = weights.ToArray();
        }

        public void SGD(
            List<Tuple<Matrix<float>, Matrix<float>>> trainingData,
            int epochs,
            int miniBatchSize,
            float eta,
            List<Tuple<Matrix<float>, int>> testData)
        {
            var velocity_w = CloneWithZeroes(mWeights);

            for (int epoch = 0; epoch < epochs; epoch++)
            {
                var trainEpochTime = Stopwatch.StartNew();
                var miniBatches = trainingData.PartitionToRandomSubsets(miniBatchSize);
                foreach (var miniBatch in miniBatches)
                {
                    //UpdateMiniBatch(miniBatch, eta, trainingData.Count);
                    UpdateMiniBatchMomentum(miniBatch, eta, trainingData.Count, velocity_w);
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

        public Action<string> NewLogMessage
        {
            get; set;
        }

        private void LogMessage(string message)
        {
            if (NewLogMessage != null)
            {
                NewLogMessage(message);
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
                a = z.Sigmoid();
            }

            return a;
        }

        private int GetRowIndexWithMaxValueInFirsColumn(Matrix<float> resultVector)
        {
            int maxIndex = 0;
            float maxValue = 0.0f;
            for (int i = 0; i < resultVector.RowCount; i++)
            {
                if (resultVector[i, 0] > maxValue)
                {
                    maxValue = resultVector[i, 0];
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        private void UpdateMiniBatch(List<Tuple<Matrix<float>, Matrix<float>>> miniBatch, float eta, int totalSizeOfTrainingDS)
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
            var L2_regularizationTerm = 1 - eta * (mRegularizationParam / totalSizeOfTrainingDS);

            for (int i = 0; i < mBiases.Length; i++)
                mBiases[i] = mBiases[i] - etaDividedByM * nabla_b[i];

            for (int i = 0; i < mWeights.Length; i++)
                mWeights[i] = L2_regularizationTerm * mWeights[i] - etaDividedByM * nabla_w[i];
        }

        private void UpdateMiniBatchMomentum(List<Tuple<Matrix<float>, Matrix<float>>> miniBatch, float eta, int totalSizeOfTrainingDS, Matrix<float>[] velocity_w)
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
            var L2_regularizationTerm = 1 - eta * (mRegularizationParam / totalSizeOfTrainingDS);

            for (int i = 0; i < mBiases.Length; i++)
                mBiases[i] = mBiases[i] - etaDividedByM * nabla_b[i];

            for (int i = 0; i < mWeights.Length; i++)
                velocity_w[i] = mMomentumDamping * velocity_w[i] - etaDividedByM * nabla_w[i];

            for (int i = 0; i < mWeights.Length; i++)
                mWeights[i] = L2_regularizationTerm * mWeights[i] + velocity_w[i];
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

                activation = z.Sigmoid();
                activations.Add(activation);
            }

            // backward pass
            var delta = mCostFunction.Delta(zs[zs.Count - 1], activations[activations.Count - 1], inputOutput.Item2);
            nabla_b[nabla_b.Length - 1] = delta;
            nabla_w[nabla_w.Length - 1] = delta * activations[activations.Count - 2].Transpose();

            for (int i = 2; i < mNumberOfLayers; i++)
            {
                var z = zs[zs.Count - i];
                var sp = z.SigmoidPrime();
                delta = (mWeights[mWeights.Length - i + 1].Transpose() * delta).HadamardProduct(sp);
                nabla_b[nabla_b.Length - i] = delta;
                nabla_w[nabla_w.Length - i] = delta * activations[activations.Count - i - 1].Transpose();
            }
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
