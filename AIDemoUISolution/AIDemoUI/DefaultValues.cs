﻿using AIDemoUI.ViewModels;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace AIDemoUI
{
    public class DefaultValues
    {
        public void Set(IMainWindowVM mainWindowVM, ISessionContext sessionContext)
        {
            LoadAndSetDefaultSerializedParameters(mainWindowVM, sessionContext);

        }

        private void LoadAndSetDefaultSerializedParameters(IMainWindowVM mainWindowVM, ISessionContext sessionContext)
        {
            Stream stream = File.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DefaultParameters.par"), FileMode.Open);
            BinaryFormatter b = new BinaryFormatter();
            SerializedParameters sp = (SerializedParameters)b.Deserialize(stream);

            try
            {
                sessionContext.NetParameters = sp.NetParameters;
                sessionContext.TrainerParameters = sp.TrainerParameters;

                mainWindowVM.NetParametersVM.AreParametersGlobal = sp.UseGlobalParameters;
                mainWindowVM.NetParametersVM.WeightMin_Global = sp.WeightMin_Global;
                mainWindowVM.NetParametersVM.WeightMax_Global = sp.WeightMax_Global;
                mainWindowVM.NetParametersVM.BiasMin_Global = sp.BiasMin_Global;
                mainWindowVM.NetParametersVM.BiasMax_Global = sp.BiasMax_Global;

                mainWindowVM.NetParametersVM.OnAllPropertiesChanged();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Net- and Trainer-Parameters could not be loaded.\n({e.Message})");
                return;
            }
        }


    }
}
