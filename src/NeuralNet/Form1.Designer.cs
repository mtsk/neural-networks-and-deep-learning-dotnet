namespace NeuralNet
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblDataLoaded = new System.Windows.Forms.Label();
            this.btnCreateNetwork = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nudBatchSize = new System.Windows.Forms.NumericUpDown();
            this.nudEpochs = new System.Windows.Forms.NumericUpDown();
            this.nudLearningRate = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSizes = new System.Windows.Forms.TextBox();
            this.lbOutput = new System.Windows.Forms.ListBox();
            this.rbNetwork1 = new System.Windows.Forms.RadioButton();
            this.rbNetwork2 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nudBatchSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEpochs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLearningRate)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDataLoaded
            // 
            this.lblDataLoaded.AutoSize = true;
            this.lblDataLoaded.BackColor = System.Drawing.Color.Green;
            this.lblDataLoaded.ForeColor = System.Drawing.Color.White;
            this.lblDataLoaded.Location = new System.Drawing.Point(10, 9);
            this.lblDataLoaded.Name = "lblDataLoaded";
            this.lblDataLoaded.Size = new System.Drawing.Size(89, 13);
            this.lblDataLoaded.TabIndex = 1;
            this.lblDataLoaded.Text = "Data Not Loaded";
            // 
            // btnCreateNetwork
            // 
            this.btnCreateNetwork.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateNetwork.Location = new System.Drawing.Point(491, 428);
            this.btnCreateNetwork.Name = "btnCreateNetwork";
            this.btnCreateNetwork.Size = new System.Drawing.Size(103, 23);
            this.btnCreateNetwork.TabIndex = 2;
            this.btnCreateNetwork.Text = "Train Network";
            this.btnCreateNetwork.UseVisualStyleBackColor = true;
            this.btnCreateNetwork.Click += new System.EventHandler(this.btnCreateNetwork_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Mini-Batch Size:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Epochs:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Learning Rate:";
            // 
            // nudBatchSize
            // 
            this.nudBatchSize.Location = new System.Drawing.Point(98, 125);
            this.nudBatchSize.Name = "nudBatchSize";
            this.nudBatchSize.Size = new System.Drawing.Size(72, 20);
            this.nudBatchSize.TabIndex = 11;
            this.nudBatchSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // nudEpochs
            // 
            this.nudEpochs.Location = new System.Drawing.Point(98, 99);
            this.nudEpochs.Name = "nudEpochs";
            this.nudEpochs.Size = new System.Drawing.Size(72, 20);
            this.nudEpochs.TabIndex = 12;
            this.nudEpochs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudLearningRate
            // 
            this.nudLearningRate.DecimalPlaces = 2;
            this.nudLearningRate.Location = new System.Drawing.Point(98, 151);
            this.nudLearningRate.Name = "nudLearningRate";
            this.nudLearningRate.Size = new System.Drawing.Size(72, 20);
            this.nudLearningRate.TabIndex = 13;
            this.nudLearningRate.Value = new decimal(new int[] {
            30,
            0,
            0,
            65536});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Network:";
            // 
            // txtSizes
            // 
            this.txtSizes.Location = new System.Drawing.Point(65, 54);
            this.txtSizes.Name = "txtSizes";
            this.txtSizes.Size = new System.Drawing.Size(207, 20);
            this.txtSizes.TabIndex = 15;
            this.txtSizes.Text = "784,30,10";
            // 
            // lbOutput
            // 
            this.lbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbOutput.FormattingEnabled = true;
            this.lbOutput.Location = new System.Drawing.Point(13, 180);
            this.lbOutput.Name = "lbOutput";
            this.lbOutput.Size = new System.Drawing.Size(581, 238);
            this.lbOutput.TabIndex = 16;
            // 
            // rbNetwork1
            // 
            this.rbNetwork1.AutoSize = true;
            this.rbNetwork1.Location = new System.Drawing.Point(294, 43);
            this.rbNetwork1.Name = "rbNetwork1";
            this.rbNetwork1.Size = new System.Drawing.Size(71, 17);
            this.rbNetwork1.TabIndex = 17;
            this.rbNetwork1.Text = "Network1";
            this.rbNetwork1.UseVisualStyleBackColor = true;
            this.rbNetwork1.CheckedChanged += new System.EventHandler(this.rbNetwork_CheckedChanged);
            // 
            // rbNetwork2
            // 
            this.rbNetwork2.AutoSize = true;
            this.rbNetwork2.Checked = true;
            this.rbNetwork2.Location = new System.Drawing.Point(294, 66);
            this.rbNetwork2.Name = "rbNetwork2";
            this.rbNetwork2.Size = new System.Drawing.Size(71, 17);
            this.rbNetwork2.TabIndex = 18;
            this.rbNetwork2.TabStop = true;
            this.rbNetwork2.Text = "Network2";
            this.rbNetwork2.UseVisualStyleBackColor = true;
            this.rbNetwork2.CheckedChanged += new System.EventHandler(this.rbNetwork_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 463);
            this.Controls.Add(this.rbNetwork2);
            this.Controls.Add(this.rbNetwork1);
            this.Controls.Add(this.lbOutput);
            this.Controls.Add(this.txtSizes);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudLearningRate);
            this.Controls.Add(this.nudEpochs);
            this.Controls.Add(this.nudBatchSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCreateNetwork);
            this.Controls.Add(this.lblDataLoaded);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.nudBatchSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEpochs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLearningRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblDataLoaded;
        private System.Windows.Forms.Button btnCreateNetwork;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudBatchSize;
        private System.Windows.Forms.NumericUpDown nudEpochs;
        private System.Windows.Forms.NumericUpDown nudLearningRate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSizes;
        private System.Windows.Forms.ListBox lbOutput;
        private System.Windows.Forms.RadioButton rbNetwork1;
        private System.Windows.Forms.RadioButton rbNetwork2;
    }
}

