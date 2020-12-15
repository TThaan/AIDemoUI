using AIDemoUI.ViewModels;
using MatrixHelper;
using NNet_InputProvider;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace AIDemoUI
{
    //public class SampleSetFactory
    //{
    //    public static async Task<SampleSet> GetSampleSet(NetParametersVM netParametersVM, NetParameters netParameters, SampleSetParameters sampleSetParameters)
    //    {
    //        foreach (var item in netParametersVM.LayerVMs)
    //        {
    //            if (item.N != item.Layer.N || item.Layer.N != item.Layer.Processed.Input.m)
    //            {
    //                item.Layer.N = item.N;
    //                item.Layer.Processed.Reset();
    //            }
    //        }

    //        netParameters.Layers = netParametersVM.LayerVMs
    //            .Select(x => x.Layer)
    //            .ToArray();

    //        SampleSet result = Creator.GetSampleSet(sampleSetParameters);
    //        await result.SetSamples();
    //        return result;
    //    }
    //    /// <summary>
    //    /// MattMazur
    //    /// </summary>
    //    public static SampleSet GetSampleSet01()
    //    {
    //        SampleSet result = Creator.GetSampleSet(SetName.Custom);
    //        result.TrainingSamples = new Sample[1000];

    //        Sample.Tolerance = 0.2f;

    //        for (int i = 0; i < result.TrainingSamples.Length; i++)
    //        {
    //            result.TrainingSamples[i] = new Sample()
    //            {
    //                Input = new Matrix(new float[] { .05f, .1f }),
    //                ExpectedOutput = new Matrix(new float[] { .01f, .99f })
    //            };
    //        }

    //        result.TestingSamples = new Sample[1];

    //        result.TestingSamples[0] = new Sample()
    //        {
    //            Input = new Matrix(new float[] { .05f, .1f }),
    //            ExpectedOutput = new Matrix(new float[] { .01f, .99f })
    //        };

    //        return result;
    //    }
    //    /// <summary>
    //    /// FourPixSingleSample
    //    /// </summary>
    //    public static SampleSet GetSampleSet02()
    //    {
    //        SampleSet result = Creator.GetSampleSet(SetName.Custom);
            
    //        using (Stream stream = File.Open(@"C:\Users\Jan_PC\Documents\_NeuralNetApp\" + "TrainingData.dat", FileMode.Open))
    //        {
    //            BinaryFormatter bf = new BinaryFormatter();
    //            result.TrainingSamples = (Sample[])bf.Deserialize(stream);
    //        }
    //        using (Stream stream = File.Open(@"C:\Users\Jan_PC\Documents\_NeuralNetApp\" + "TestingData.dat", FileMode.Open))
    //        {
    //            BinaryFormatter bf = new BinaryFormatter();
    //            result.TestingSamples = (Sample[])bf.Deserialize(stream);
    //        }

    //        Sample.Tolerance = .25f;

    //        return result;
    //    }
    //}
}
