using Addovation.Cloud.Apps.AddoResources.Client.Portable;
using Addovation.Common.Models;
using SenseHat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Xamarin;

namespace TestInterface
{
    public static class AppData
    {
        public static readonly int SensorDataCapacity = 20;

        public static CloudClient CloudClient { get; set; }
        public static SensorReader SensorReader { get; set; }
        public static Queue<SensorData> SensorData { get; set; }

        private static void InitInsights()
        {
            var analyticsKey = Insights.DebugModeKey;
            Insights.Initialize(analyticsKey, false);
            InsightsHelper.ResetUser();
        }

        private static void InitCredential()
        {
            var vault = new PasswordVault();
            try
            {
                var credList = vault.FindAllByResource("idun");
            }
            catch (Exception)
            {
                vault.Add(new PasswordCredential("idun", "alain", "alain"));
            }
        }

        public static void InitCloud()
        {
            try
            {
                InitInsights();
                InitCredential();

                var vault = new PasswordVault();
                var cred = vault.FindAllByResource("idun").Where(c => c.UserName == "alain").Single();
                cred.RetrievePassword();

                var cloudUrl = CommonDictionary.CloudUrls["testcloud.addovation.com"];
                var connectionInfo = new ConnectionInfo(cloudUrl, "race8.addovation.com", cred.UserName, cred.Password);

                CloudClient = new CloudClient
                {
                    ConnectionInfo = connectionInfo,
                    SessionManager = new SessionManager()
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void InitSensor()
        {
            SensorData = new Queue<SenseHat.SensorData>(SensorDataCapacity);

#if ARM
            SensorReader = SensorReader.Create<SenseHatReader>(1000, (reader, data) =>
            {
                if (SensorData.Count == SensorDataCapacity)
                    SensorData.Dequeue();
                SensorData.Enqueue(data);
            }).Result;
#else
            //SensorReader = new DummyReader(1000, 20);

            SensorReader = SensorReader.Create<DummyReader>(1000, (reader, data) =>
            {
                if (SensorData.Count == SensorDataCapacity)
                    SensorData.Dequeue();
                SensorData.Enqueue(data);
            }).Result;
#endif
            //SensorReader.Init();
        }
    }
}
