using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NeuralNet
{
    public class MnistLoader
    {
        private string mTrainPixelFilePath;
        private string mTrainLabelFilePath;
        private string mTestPixelFilePath;
        private string mTestLabelFilePath;
        
        private const float cNormalizationRange = 255;     // pixel values are from 0 to 255

        // list of tuples with <image data; image label as output vector (matrix)>
        private List<Tuple<Matrix<float>, Matrix<float>>> mTrainingData;
        public List<Tuple<Matrix<float>, Matrix<float>>> TrainingData
        {
            get
            {
                return mTrainingData;
            }
        }

        private List<Tuple<Matrix<float>, int>> mTestData;
        public List<Tuple<Matrix<float>, int>> TestData
        {
            get
            {
                return mTestData;
            }
        }

        private List<Tuple<Matrix<float>, int>> mValidationData;
        public List<Tuple<Matrix<float>, int>> ValidationData
        {
            get
            {
                return mTestData;
            }
        }

        public MnistLoader(
            string train_pixelFilePath, 
            string train_labelFilePath, 
            string test_pixelFilePath, 
            string test_labelFilePath)
        {
            mTrainPixelFilePath = train_pixelFilePath;
            mTrainLabelFilePath = train_labelFilePath;
            mTestPixelFilePath = test_pixelFilePath;
            mTestLabelFilePath = test_labelFilePath;
        }

        public void Load()
        {
            var allTrainingData = LoadData(mTrainPixelFilePath, mTrainLabelFilePath, 60000);
            var trainingData = allTrainingData.Take(50000).ToList();

            mTrainingData = TransformTrainingData(trainingData);
            mValidationData = allTrainingData.Skip(50000).Take(10000).ToList();
            mTestData = LoadData(mTestPixelFilePath, mTestLabelFilePath, 10000);
        }

        private List<Tuple<Matrix<float>, int>> LoadData(string pixelFile, string labelFile, int numImages)
        {
            var result = new List<Tuple<Matrix<float>, int>>(numImages);

            FileStream ifsPixels = new FileStream(pixelFile, FileMode.Open);
            FileStream ifsLabels = new FileStream(labelFile, FileMode.Open);

            BinaryReader brImages = new BinaryReader(ifsPixels);
            BinaryReader brLabels = new BinaryReader(ifsLabels);

            int magic1 = brImages.ReadInt32(); // stored as Big Endian
            magic1 = ReverseBytes(magic1); // convert to Intel format

            int imageCount = brImages.ReadInt32();
            imageCount = ReverseBytes(imageCount);

            int numRows = brImages.ReadInt32();
            numRows = ReverseBytes(numRows);
            int numCols = brImages.ReadInt32();
            numCols = ReverseBytes(numCols);

            int magic2 = brLabels.ReadInt32();
            magic2 = ReverseBytes(magic2);

            int numLabels = brLabels.ReadInt32();
            numLabels = ReverseBytes(numLabels);

            float[] imageData = new float[28 * 28];
            for (int di = 0; di < numImages; ++di)
            {
                for (int i = 0; i < 28 * 28; ++i)
                    imageData[i] = GetNormalizedValue(brImages.ReadByte());

                byte label = brLabels.ReadByte();

                var mnistImageMatrix = Matrix<float>.Build.Dense(imageData.Length, 1, (float[])imageData.Clone());

                result.Add(
                    new Tuple<Matrix<float>, int>(
                    mnistImageMatrix, (int)label
                    ));               
            }

            ifsPixels.Close(); brImages.Close();
            ifsLabels.Close(); brLabels.Close();

            return result;
        }

        private float GetNormalizedValue(byte value)
        {
            float result = 0;

            result = value / cNormalizationRange;

            return result;
        }

        private int ReverseBytes(int v)
        {
            byte[] intAsBytes = BitConverter.GetBytes(v);
            Array.Reverse(intAsBytes);
            return BitConverter.ToInt32(intAsBytes, 0);
        }

        private List<Tuple<Matrix<float>, Matrix<float>>> TransformTrainingData(List<Tuple<Matrix<float>, int>> images)
        {
            var result = new List<Tuple<Matrix<float>, Matrix<float>>>(images.Count);

            foreach (var image in images)
            {                
                result.Add(
                    new Tuple<Matrix<float>, Matrix<float>>(
                    image.Item1, VectorizeNumber(image.Item2)
                    ));
            }

            return result;
        }

        private Matrix<float> VectorizeNumber(int number)
        {
            var result = Matrix<float>.Build.Dense(10, 1);

            result[(int)number, 0] = 1.0f;

            return result;
        }
    }
}
