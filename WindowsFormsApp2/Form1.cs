using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WindowsFormsApp2.NeuroNet;


namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private double[] inputPixels;
        private Network network;
        public Form1()
        {
            InitializeComponent();
            inputPixels = new double[15];
            network = new Network();
        }
        private void changing_State_Pixel_Button_click(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == Color.Black)
            {
                ((Button)sender).BackColor = Color.White;
                inputPixels[((Button)sender).TabIndex] = 1d;
            }
            else
            {
                ((Button)sender).BackColor = Color.Black;
                inputPixels[((Button)sender).TabIndex] = 0d;
            }
        }
        private void button_SaveTestSample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "train.txt";
            string tmpStr = numericUpDownNecessary.Value.ToString(); //  Assure-toi que ton contrôle s’appelle bien "numericUpDownNecessary"

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
            string tmpStr = numericUpDownNecessary.Value.ToString(); //  Assure-toi que ton contrôle s’appelle bien "numericUpDownNecessary"

            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n";

            File.AppendAllText(path, tmpStr); // Nécessite using System.IO;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            HiddenLayer hiddenLayer = new HiddenLayer(10,10,NeuronType.Hidden,nameof(hiddenLayer));
        }

        private void Label_Output_Click(object sender, EventArgs e)
        {

        }

        private void probability_Click(object sender, EventArgs e)
        {
           
        }
        private void recognizebutton_Click(object sender, EventArgs e) 
        {
            network.FordWardPass(network, inputPixels);
            Label_Output.Text = network.Fact.ToList().IndexOf(network.Fact.Max()).ToString();
            
            probability.Text = (100 * network.Fact.Max()).ToString("0.00") + " %";
        }

        private void ButtonTrain_Click(object sender, EventArgs e)
        {
            network.Train(network);
            for (int i = 0; i < network.E_error_avr.Length; i++) {

                chart_Eavr.Series[0].Points.AddY(network.E_error_avr[i]);
            }

            MessageBox.Show("Обучение успешно завершено", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
