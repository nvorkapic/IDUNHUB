using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI.Xaml;

namespace SenseHat
{
    public abstract class SensorReader
    {
        public delegate void SensorReaderTick(SensorReader reader, SensorData data);

        private ThreadPoolTimer timer;
        //protected DispatcherTimer timer;
        //protected int updateIntervalMs;
        //protected int capacity;

        public SensorReaderTick Tick;
        //public Queue<SensorData> Data { get; set; }

        public static Task<SensorReader> Create<T>(int intervalMs, SensorReaderTick tick)
            where T : SensorReader, new()
        {
            var sr = new T();

            return sr.InitAsync().ContinueWith<SensorReader>((task) =>
            {
                sr.Tick = tick;
                sr.timer = ThreadPoolTimer.CreatePeriodicTimer((t) => sr.Tick(sr, sr.Read()), TimeSpan.FromMilliseconds((double)intervalMs));
                return sr;
            });
        }

        protected abstract Task InitAsync();

        public abstract SensorData Read();

        //public SensorReader(int updateIntervalMs, int capacity)
        //{
        //    this.updateIntervalMs = updateIntervalMs;
        //    this.capacity = capacity;
        //    Data = new Queue<SensorData>(capacity);
        //}

        //public virtual void Init()
        //{
        //    timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(updateIntervalMs) };
        //    timer.Tick += Timer_Tick;
        //    timer.Start();
        //}

        //protected void AddData(SensorData data)
        //{
        //    if (Data.Count == capacity)
        //        Data.Dequeue();
        //    Data.Enqueue(data);
        //    Tick?.Invoke(this, data);
        //}

        //protected virtual void Timer_Tick(object sender, object e)
        //{
        //}
    }
}
