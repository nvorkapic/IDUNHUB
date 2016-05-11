using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseHat
{
    public class DummyReader : SensorReader
    {
        private Random rnd = new Random();

        //public DummyReader(int updateIntervalMs, int capacity) : base(updateIntervalMs, capacity)
        //{
        //    rnd = new Random();
        //}

        //protected override void Timer_Tick(object sender, object e)
        //{
        //    var data = new SensorData
        //    {
        //        Date = DateTime.Now,
        //        Temperature = 30.0f + (float)(rnd.NextDouble() * 0.5),
        //        Humidity = 40.0f + (float)(rnd.NextDouble() * 0.5),
        //        Pressure = 1010.0f + (float)(rnd.NextDouble() * 2.0),
        //    };
        //    AddData(data);
        //}

        protected override Task InitAsync()
        {
            return Task.CompletedTask;
        }

        public override SensorData Read()
        {
            return new SensorData
            {
                Date = DateTime.Now,
                Temperature = 30.0f + (float)(rnd.NextDouble() * 0.5),
                Humidity = 40.0f + (float)(rnd.NextDouble() * 0.5),
                Pressure = 1010.0f + (float)(rnd.NextDouble() * 2.0),
            };
        }
    }
}
