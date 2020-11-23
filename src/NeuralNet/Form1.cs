using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuralNet
{
    public partial class Form1 : Form
    {
        MnistLoader mMnistLoader;
        INeuralNetwork mNetwork;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            rbNetwork_CheckedChanged(this, EventArgs.Empty);
        }

        private void btnCreateNetwork_Click(object sender, EventArgs e)
        {
            btnCreateNetwork.Enabled = false;
            UpdateControlStatus(btnCreateNetwork);

            if (mMnistLoader == null)
            {
                lblDataLoaded.Text = "Loading data...";
                UpdateControlStatus(lblDataLoaded);

                LoadData();
            }

            lblDataLoaded.Text = "Data loaded, training network...";
            UpdateControlStatus(lblDataLoaded);

            CleanUpOldNetwork();
            mNetwork = CreateNetwork();

            Task.Factory.StartNew(() => 
                mNetwork.SGD(
                    mMnistLoader.TrainingData,
                    (int)nudEpochs.Value,
                    (int)nudBatchSize.Value,
                    (float)nudLearningRate.Value,
                    mMnistLoader.TestData)
                )
                .ContinueWith( t => {
                    btnCreateNetwork.Enabled = true;
                    lblDataLoaded.Text = "Training Finished!";
                }
                , TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void CleanUpOldNetwork()
        {
            if (mNetwork != null)
            {
                mNetwork.NewLogMessage = null;
                mNetwork = null;
                lbOutput.Items.Clear();
            }
        }

        private INeuralNetwork CreateNetwork()
        {
            INeuralNetwork neuralNet = null;

            var sizes = txtSizes.Text.Split(',').Select(s => Convert.ToInt32(s)).ToArray();

            if (rbNetwork1.Checked)
            {
                neuralNet = new Network(sizes);
            }
            else if (rbNetwork2.Checked)
            {
                var network2 = new Network2(sizes, 5.0f, 0.3f);

                neuralNet = network2;
            }

            neuralNet.NewLogMessage = 
                m => this.Invoke((MethodInvoker)delegate
                {
                    lbOutput.Items.Add(m);
                });

            return neuralNet;
        }

        private void LoadData()
        {
            mMnistLoader = new MnistLoader(
                @"..\..\..\mnistdata\train-images.idx3-ubyte",
                @"..\..\..\mnistdata\train-labels.idx1-ubyte",
                @"..\..\..\mnistdata\t10k-images.idx3-ubyte",
                @"..\..\..\mnistdata\t10k-labels.idx1-ubyte"
                );

            mMnistLoader.Load();
        }

        private void UpdateControlStatus(Control control)
        {
            control.Refresh();
            Application.DoEvents();
        }
        
        private void rbNetwork_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNetwork1.Checked)
                nudLearningRate.Value = 3.0M;
            else if (rbNetwork2.Checked)
                nudLearningRate.Value = 0.4M;
        }
    }
}
