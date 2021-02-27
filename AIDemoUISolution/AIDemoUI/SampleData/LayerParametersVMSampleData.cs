using AIDemoUI.ViewModels;
using System;
using static AIDemoUI.SampleData.MockData;

namespace AIDemoUI.SampleData
{
    public class LayerParametersVMSampleData : LayerParametersVM
    {
        Random rnd;

        public LayerParametersVMSampleData()
            : base(MockSessionContext, MockMediator)//
        {
            // Parameters Data
            LayerParameters = MockLayerParameters01;

            // Layer Data

            rnd = new Random();

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
        }

        private float GetRndFloat()
        {
            // result: between -1 and 1 and rounded to two decimal places
            return (float)Math.Round(2 * rnd.NextDouble(), 2) - 1; 
        }
    }
}
