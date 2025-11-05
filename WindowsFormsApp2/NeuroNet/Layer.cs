using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using WindowsFormsApp2.NeuroNet;

namespace WindowsFormsApp2.NeuroNet
{
    abstract class Layer
    {
        
        protected string name_Layer;
        string pathDirWeights;
        string pathFileWeights;
        protected int numofneurons;
        protected int numofprevneurons;
        protected const double learningrate = 0.0060;
        protected const double momentum = 0.00d;
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
            int i, j;
            numofneurons = non;
            numofprevneurons = nopn;
            Neurons = new Neuron[non];
            name_Layer = nm_Layer;
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";

            lastdeltaweights = new double[non, nopn + 1];
            double[,] Weights;

            if (File.Exists(pathFileWeights))
                Weights = WeightInitialize(MemoryMode.GET, pathFileWeights);
            else
            {
                Directory.CreateDirectory(pathDirWeights);
                Weights = WeightInitialize(MemoryMode.INIT, pathFileWeights);
            }

            for (i = 0; i < non; i++)
            {
                double[] tmp_weights = new double[nopn + 1];
                for (j = 0; j < nopn; j++)
                {
                    tmp_weights[j] = Weights[i, j];
                }
                Neurons[i] = new Neuron(tmp_weights, nt);
            }
        }

        public double[,] WeightInitialize(MemoryMode mm, string path)
        {
            int i, j;
            char[] delim = new char[] { ',', ' ' };
            string[] tmpStrWeights;
            double[,] weights = new double[numofneurons, numofprevneurons + 1];

            switch (mm)
            {
                case MemoryMode.GET:
                    tmpStrWeights = File.ReadAllLines(path);
                    string[] memory_element;
                    for (i = 0; i < numofneurons; i++)
                    {
                        memory_element = tmpStrWeights[i].Split(delim, StringSplitOptions.RemoveEmptyEntries);
                        for (j = 0; j < numofprevneurons + 1; j++)
                        {
                            // Correction ici : Replace + double.Parse avec CultureInfo
                            weights[i, j] = double.Parse(memory_element[j].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }
                    break;

                case MemoryMode.INIT:
                    Random rnd = new Random();
                    for (i = 0; i < numofneurons; i++)
                    {
                        for (j = 0; j < numofprevneurons + 1; j++)
                        {
                            // Initialisation aléatoire entre -1 et 1
                            weights[i, j] = rnd.NextDouble() * 2 - 1;
                        }
                    }

                    // Sauvegarde dans le fichier
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        for (i = 0; i < numofneurons; i++)
                        {
                            string line = "";
                            for (j = 0; j < numofprevneurons + 1; j++)
                            {
                                line += weights[i, j].ToString(System.Globalization.CultureInfo.InvariantCulture);
                                if (j < numofprevneurons) line += ",";
                            }
                            sw.WriteLine(line);
                        }
                    }
                    break;

                case MemoryMode.SET:
                    // Sauvegarde des poids modifiés (ex: après apprentissage)
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        for (i = 0; i < numofneurons; i++)
                        {
                            string line = "";
                            for (j = 0; j < numofprevneurons + 1; j++)
                            {
                                line += weights[i, j].ToString(System.Globalization.CultureInfo.InvariantCulture);
                                if (j < numofprevneurons) line += ",";
                            }
                            sw.WriteLine(line);
                        }
                    }
                    break;
            }

            return weights;
        }

        abstract public void Recognize(Network net, Layer nextlayer);
        abstract public double[] BackwardPass(double[] stuff);

    }
    
 }    

            
        
    

