

namespace WindowsFormsApp2.NeuroNet
{
    class Network
    {
        private InputLayer input_Layer = null;
        private HiddenLayer hidden_Layer1 = new HiddenLayer(70, 15, NeuronType.Hidden, nameof(hidden_Layer1));
        private HiddenLayer hidden_Layer2 = new HiddenLayer(32, 70, NeuronType.Hidden, nameof(hidden_Layer2));
        private OutputLayer output_Layer = new OutputLayer(10, 32, NeuronType.output, nameof(output_Layer));

        private double[] fact = new double[10];
        private double[] e_error_avr;

        public double[] Fact { get => fact; }  // correction ici
        public double[] E_error_avr { get => e_error_avr; set => e_error_avr = value; }

        public Network() { }

        public void FordWardPass(Network net, double[] netInput)
        {
            net.hidden_Layer1.Data = netInput;
            net.hidden_Layer1.Recognize(null, net.hidden_Layer2);
            net.hidden_Layer2.Recognize(null, net.output_Layer);
            net.output_Layer.Recognize(net, null);
        }
        public void Train(Network net)
        {
            net.input_Layer = new InputLayer(NetworkMode.Train);
            int epoches = 10;
            double tmpSumError;
            double[] errors;
            double[] temp_gsums1;
            double[] temp_gsums2;

            e_error_avr = new double[epoches];
            for (int k = 0; k < epoches; k++)
            {
                e_error_avr[k] = 0;
                net.input_Layer.Shuffling_Array_Rows(net.input_Layer.Trainset);
                for (int i = 0; i < net.input_Layer.Trainset.GetLength(0); i++)
                {
                    double[] tmpTrain = new double[15];
                    for (int j = 0; j < tmpTrain.Length; j++)
                        tmpTrain[j] = net.input_Layer.Trainset[i, j + 1];

                    FordWardPass(net, tmpTrain);

                    tmpSumError = 0;
                    errors = new double[net.fact.Length];
                    for (int x = 0; x < errors.Length; x++)
                    {
                        if (x == net.input_Layer.Trainset[i, 0])
                            errors[x] = 1.0 - net.fact[x];
                        else
                            errors[x] = -net.fact[x];

                        tmpSumError += errors[x] * errors[x] / 2;
                    }
                    e_error_avr[k] = tmpSumError / errors.Length;

                    temp_gsums2 = net.output_Layer.BackwardPass(errors);
                    temp_gsums1 = net.hidden_Layer2.BackwardPass(temp_gsums2);
                    net.hidden_Layer1.BackwardPass(temp_gsums1);
                }
                e_error_avr[k] /= net.input_Layer.Trainset.GetLength(0);
            }

            net.input_Layer = null;

            net.hidden_Layer1.WeightInitialize(MemoryMode.SET, nameof(hidden_Layer1) + "_memory.csv");
            net.hidden_Layer2.WeightInitialize(MemoryMode.SET, nameof(hidden_Layer2) + "_memory.csv");
            net.output_Layer.WeightInitialize(MemoryMode.SET, nameof(output_Layer) + "_memory.csv");
        }
    }
}

