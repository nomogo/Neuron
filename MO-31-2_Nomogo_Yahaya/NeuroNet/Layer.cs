using MO_31_2_Nomogo_Yahaya.NeuroNet;
using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace MO_31_2_Nomogo_Yahaya.NeuroNet
{
    abstract class Layer
    {
        protected string name_Layer; 
        string pathDirWeights; 
        string pathFileWeights; 
        protected int numofneurons; 
        protected int numofprevneurons; 
        protected const double learningrate = 0.538;
        protected const double momentum = 0.090d; 
        protected double[,] lastdeltaweights; 
        protected Neuron[] neurons; 

        public Neuron[] Neurons { get => neurons; set => neurons = value; }
        public double[] Data 
        {
            set
            {
                for (int i = 0; i < numofneurons; i++)
                {
                    Neurons[i].Activator(value);
                }
            }
        }

        protected Layer(int non, int nopn, NeuronType nt, string nm_Layer)
        {
            numofneurons = non; 
            numofprevneurons = nopn; 
            Neurons = new Neuron[non]; 
            name_Layer = nm_Layer; 
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";

            double[,] Weights; 
            lastdeltaweights = new double[non, nopn + 1];

            if (File.Exists(pathFileWeights)) 
                Weights = WeightInitialize(MemoryMode.GET, pathFileWeights); 
            else
            {
                Directory.CreateDirectory(pathDirWeights);
                Weights = WeightInitialize(MemoryMode.INIT, pathFileWeights);
            }

            for (int i = 0; i < non; i++) 
            {
                double[] tmp_weights = new double[nopn + 1];
                for (int j = 0; j < nopn; j++)
                {
                    tmp_weights[j] = Weights[i, j];
                }
                Neurons[i] = new Neuron(tmp_weights, nt); 
            }
        }

        public double[,] WeightInitialize(MemoryMode mm, string path)
        {
            char[] delim = new char[] { ';', ' ' };
            string[] tmpStrWeights;
            double[,] weights = new double[numofneurons, numofprevneurons + 1];

            switch (mm)
            {
                case MemoryMode.GET:
                    tmpStrWeights = File.ReadAllLines(path);        
                    string[] memory_elemnt; 

                    
                    for (int i = 0; i < numofneurons; i++)
                    {
                        memory_elemnt = tmpStrWeights[i].Split(delim);  
           
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            weights[i, j] = double.Parse(memory_elemnt[j].Replace(',', '.'),
                                System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }
                    break;

                case MemoryMode.SET:
                    tmpStrWeights = new string[numofneurons]; 
                    for (int i = 0; i < numofneurons; i++)
                    {
                        string[] memory_elemnt2 = new string[numofprevneurons + 1];
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            memory_elemnt2[j] = neurons[i].Weights[j]
                                .ToString(System.Globalization.CultureInfo.InvariantCulture)
                                .Replace('.', ',');
                        }
                        tmpStrWeights[i] = string.Join(";", memory_elemnt2);
                    }
                    File.WriteAllLines(path, tmpStrWeights);
                    break;

                case MemoryMode.INIT:
                    Random random = new Random();
                    for (int i = 0; i < numofneurons; i++)
                    {
                        double sum = 0.0;
                        double squaredSum = 0.0;

                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            weights[i, j] = random.NextDouble() * 2.0 - 1.0;

                            sum += weights[i, j];
                            squaredSum += weights[i, j] * weights[i, j];
                        }

                        double mean = sum / (numofprevneurons + 1);
                        double variance = (squaredSum / (numofprevneurons + 1)) - (mean * mean);
                        double root = Math.Sqrt(variance);

                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            weights[i, j] = (weights[i, j] - mean) / root;
                        }
                    }

                  
                    string[] lines = new string[numofneurons];
                    for (int i = 0; i < numofneurons; i++)
                    {
                        string[] row = new string[numofprevneurons + 1];
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            row[j] = weights[i, j]
                                .ToString(System.Globalization.CultureInfo.InvariantCulture)
                                .Replace('.', ',');
                        }
                        lines[i] = string.Join(";", row);
                    }
                    File.WriteAllLines(path, lines);
                    break;
            }
            return weights;
        }
        abstract public void Recognize(Network net, Layer nextLayer);
        abstract public double[] BackwardPass(double[] staff);
    }
}
