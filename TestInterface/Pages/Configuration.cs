using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDUN3.ConfigurationPages
{
    public class Configuration : INotifyPropertyChanged
    {
        

        public string Measurement { get; set; }

        public bool Enabled { get; set; }

        public int Interval { get; set; }

        public bool Threshold { get; set; }
        public double? ThresholdBottomValue { get; set; }
        public double? ThresholdCeilingValue { get; set; }

        public int Report
        {
            get { return _report; }
            set
            {
                
                _report = value;
                RaisePropertyChanged("ReportValue1");
                RaisePropertyChanged("ReportValue2");
                RaisePropertyChanged("ReportValue3");
            }
        }

        public bool ReportValue1
        {
            get { return Report.Equals(1); }
            set { Report = 1; }
        }

        public bool ReportValue2
        {
            get { return Report.Equals(2); }
            set { Report = 2; }
        }

        public bool ReportValue3
        {
            get { return Report.Equals(3); }
            set { Report = 3; }
        }

        private int _report = int.MinValue;

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
