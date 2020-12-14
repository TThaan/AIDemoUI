using AIDemoUI.ViewModels;
using FourPixCam;
using MatrixHelper;
using System.Collections.ObjectModel;
using System.Linq;

namespace AIDemoUI
{
    public class NeuralNetFactory
    {
        internal static NeuralNet GetNeuralNet(Initializer initializer, NetParametersVM netParametersVM, NetParameters netParameters)
        {
            foreach (var item in netParametersVM.LayerVMs)
            {
                if (item.N != item.Layer.N || item.Layer.N != item.Layer.Processed.Input.m)
                {
                    item.Layer.N = item.N;
                    item.Layer.Processed.Reset();
                }
            }

            netParameters.Layers = netParametersVM.LayerVMs
                .Select(x => x.Layer)
                .ToArray();

            return initializer.GetNeuralNet(netParameters);
        }

        internal static NeuralNet GetNeuralNet_Example01(NetParametersVM netParametersVM, NetParameters netParameters)
        {
            netParametersVM.LayerVMs = new ObservableCollection<LayerVM>
            {
                new LayerVM(0, 2, ActivationType.NullActivator),
                new LayerVM(1, 2, ActivationType.Sigmoid),
                new LayerVM(2, 2, ActivationType.Sigmoid)
            };

            netParametersVM.LearningRate = .5f;
            netParametersVM.IsWithBias = true;

            foreach (var item in netParametersVM.LayerVMs)
            {
                if (item.N != item.Layer.N || item.Layer.N != item.Layer.Processed.Input.m)
                {
                    item.Layer.N = item.N;
                    item.Layer.Processed.Reset();
                }
            }

            netParameters.Layers = netParametersVM.LayerVMs
                .Select(x => x.Layer)
                .ToArray();


            SetExampleWeights01(netParameters);
            SetExampleBiases01(netParameters);

            return new NeuralNet(netParameters.Layers, netParameters.CostType);
        }

        /// <summary>
        /// https://mattmazur.com/2015/03/17/a-step-by-step-backpropagation-example/
        /// </summary>
        static void SetExampleWeights01(NetParameters netParameters)
        {
            netParameters.Layers[1].Weights = new Matrix(new float[,]
                {
                    { .15f, .2f },
                    { .25f, .3f }
                });
            netParameters.Layers[2].Weights = new Matrix(new float[,]
                {
                    { .4f, .45f },
                    { .5f, .55f }
                });
        }
        static void SetExampleBiases01(NetParameters netParameters)
        {
            netParameters.Layers[1].Biases = new Matrix(new float[,]
                {
                    { .35f },
                    { .35f }
                });
            netParameters.Layers[2].Biases = new Matrix(new float[,]
                {
                    { .6f },
                    { .6f }
                });
        }

        static void SetExampleWeights03(NetParameters netParameters)
        {
            netParameters.Layers[1].Weights = new Matrix(new float[,]
                {
                    { -.4f, -.2f, .0f, .2f },
                    { .1f, .2f, .3f, .4f },
                    { .3f, .1f, -.2f, .4f },
                    { -.1f, .2f, .3f, -.4f }
                });
            netParameters.Layers[2].Weights = new Matrix(new float[,]
                {
                    { .1f, -.3f, .0f, .3f },
                    { .2f, -.2f, -.1f, .4f },
                    { .2f, -.1f, -.3f, .4f },
                    { -.2f, .1f, .4f, .2f }
                });
            netParameters.Layers[3].Weights = new Matrix(new float[,]
                {
                    { -.4f, -.2f, .3f, .2f },
                    { .1f, .2f, .3f, .4f },
                    { .3f, .1f, .2f, .4f },
                    { .1f, -.2f, .3f, -.4f },
                    { .3f, .1f, .4f, .1f },
                    { .3f, .1f, -.2f, .0f },
                    { -.1f, -.2f, .3f, -.4f },
                    { .4f, -.4f, .1f, .2f }
                });
            netParameters.Layers[4].Weights = new Matrix(new float[,]
                {
                    { .2f, .0f, .4f, -.2f, .3f, .2f, .0f, .1f },
                    { -.1f, .3f, .1f, .0f, .4f, .2f, .0f, -.2f },
                    { .3f, .1f, -.3f, .1f, .3f, .2f, -.4f, .2f },
                    { .4f, .4f, -.2f, .4f, .1f, -.2f, .2f, .2f }
                });
        }
    }
}