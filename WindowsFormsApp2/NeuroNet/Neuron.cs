using static System.Math;
using System.Security.Policy;
using WindowsFormsApp2.NeuroNet;


namespace WindowsFormsApp2.NeuroNet
{
    class Neuron
    {
        private NeuronType type;
        private double[] weights;
        private double[] inputs;
        private double output;
        private double derivative;

        private double a = 0.01d; // paramètre pour LeakyReLU

        public double[] Weights { get => weights; set => weights = value; }
        public double[] Inputs { get => inputs; set => inputs = value; }
        public double Output { get => output; }
        public double Derivative { get => derivative; }

        public Neuron(double[] memoryWeights, NeuronType typeNeuron)
        {
            type = typeNeuron;
            weights = memoryWeights;
        }

        public void Activator(double[] i)
        {
            inputs = i;
            double sum = weights[0];
            for (int j = 0; j < inputs.Length; j++)
            {
                sum += inputs[j] * weights[j + 1];
            }

            switch (type)
            {
                case NeuronType.Hidden:
                    // Tu peux choisir entre LeakyReLU 
                    output = LeakyReLU(sum);
                    derivative = LeakyReLU_Derivator(sum);
                    // Sa dérivée
                    break;

                case NeuronType.output:
                    output = Exp(sum);

                    break;
            }
        }

     

        
        //  Leaky ReLU 
        
        private double LeakyReLU(double x)
        {
            return x > 0 ? x : a * x;
        }

        private double LeakyReLU_Derivator(double x)
        {
            return x > 0 ? 1 : a;
        }
    }
}
