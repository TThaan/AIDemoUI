using AIDemoUI.ViewModels;
using MatrixHelper;
using NeuralNetBuilder;
using NeuralNetBuilder.FactoriesAndParameters;
using System;
using static AIDemoUI.SampleData.SampleDataInitializer;

namespace AIDemoUI.SampleData
{
    public class LayerParametersVMSampleData : LayerParametersVM
    {
        Random rnd;

        public LayerParametersVMSampleData()
            : base(SampleSessionContext, SampleMediator)//
        {
            // Parameters Data
            LayerParameters = new LayerParameters
            { 
                Id = 0 
            };
            NeuronsPerLayer = 4;
            ActivationType = ActivationType.SoftMaxWithCrossEntropyLoss;
            WeightMin = -.8f;
            WeightMax = .8f;

            // Layer Data

            //rnd = new Random();

            //Layer.Input = new Matrix(new[]
            //{
            //    .23f,-.41f,-.52f,.68f
            //});

            //Layer.Output = Input.GetCopy();
            //Layer.Biases = null;
            //Layer.Weights = new Matrix(
            //    new[,] 
            //    {
            //        { .23f,-.41f,-.52f,.68f},
            //        { .23f,-.41f,-.52f,.68f}, 
            //        { .23f,-.41f,-.52f,.68f}, 
            //        { .23f,-.41f,-.52f,.68f},
            //        { .23f,-.41f,-.52f,.68f},
            //        { .23f,-.41f,-.52f,.68f},
            //        { .23f,-.41f,-.52f,.68f},
            //        { .23f,-.41f,-.52f,.68f}
            //    });

            //throw new System.ArgumentException($"" +
            //    $"SampleSessionContext.Net.Layers[0].Weights == null: " +
            //    $"{SampleSessionContext.Net.Layers[0].Weights == null}");
        }

        private float GetRndFloat()
        {
            // result: between -1 and 1 and rounded to two decimal places
            return (float)Math.Round(2 * rnd.NextDouble(), 2) - 1; 
        }
    }
}
