using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using MO_31_2_Nomogo_Yahaya.NeuroNet;

namespace MO_31_2_Nomogo_Yahaya
{
    public partial class FormMain : Form
    {
        private double[] inputPixels; 
        private Network network;
        private HiddenLayer hiddenlayer;
        //Конструктор
        public FormMain()
        {
            InitializeComponent();

            inputPixels = new double[15];
            network = new Network();
        }

        //Обработчик кнопки
        private void change_btn_onClick(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == Color.White)
            {
                ((Button)sender).BackColor = Color.Black;
                inputPixels[((Button)sender).TabIndex] = 1d;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
                inputPixels[((Button)sender).TabIndex] = 0d;
            }
        }

        private void button_SaveTestSample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "train.txt";
            string tmpStr = numericUpDown.Value.ToString(); //  Assure-toi que ton contrôle s’appelle bien "numericUpDownNecessary"

            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n";

            File.AppendAllText(path, tmpStr); // Nécessite using System.IO;
        }

        private void button_SaveTrainSample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "test.txt";
            string tmpStr = numericUpDown.Value.ToString(); //  Assure-toi que ton contrôle s’appelle bien "numericUpDownNecessary"

            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n";

            File.AppendAllText(path, tmpStr); // Nécessite using System.IO;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            hiddenlayer = new NeuroNet.HiddenLayer(10, 10, NeuroNet.NeuronType.Hidden, nameof(hiddenlayer));
        }

        private void buttonRecognize_Click(object sender, EventArgs e)
        {
            network.ForwardPass(network, inputPixels);
            labelOut.Text = network.Fact.ToList().IndexOf(network.Fact.Max()).ToString();
            labelProbability.Text = (100 * network.Fact.Max()).ToString("0.00") + " %";

        }

        private void buttonTrain_Click(object sender, EventArgs e)
        {
            network.Train(network);

            for (int i = 0; i < network.E_error_avr.Length; i++)
            {
                chart_Eavr.Series[0].Points.AddY(network.E_error_avr[i]);
            }

            MessageBox.Show("Обучение успешно завершено.", "информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
