using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace FlightSimulatorApp
{
    class FlightSimulatorViewModel : INotifyPropertyChanged
    {
        IFlightSimulatorModel model;

        public FlightSimulatorViewModel(IFlightSimulatorModel model)
        {
            this.model = model;
            model.PropertyChanged +=
                delegate (Object sender, PropertyChangedEventArgs e)
                {
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }



        public void updateCSVPath(string csvPath)
        {
            model.updateCSVPath(csvPath);
        }
        public void start()
        {
            model.start();
        }

        public int VM_CurrentLineIndex
        {
            get { return model.CurrentLineIndex; }
            set { this.model.CurrentLineIndex = value; }
        }

        public int VM_CSVLinesNumber
        {
            get { return model.CSVLinesNumber; }
        }

        public void connect()
        {
            model.connect();
        }

        public double VM_AileronCurrentValue
        {
            get { return model.AileronCurrentValue; }
        }

        public double VM_AileronMaximunValue
        {
            get { return model.AileronMaximunValue; }

        }

        public double VM_AileronMinimumValue
        {
            get { return model.AileronMinimumValue; }

        }

        public double VM_ElevatorCurrentValue
        {
            get { return model.ElevatorCurrentValue; }
        }


        public double VM_ElevatorMaximunValue
        {
            get { return model.ElevatorMaximunValue; }

        }

        public double VM_ElevatorMinimumValue
        {
            get { return model.ElevatorMinimumValue; }

        }

        public double VM_ThrottleCurrentValue
        {
            get { return model.ThrottleCurrentValue; }
        }

        public double VM_ThrottleMaximunValue
        {
            get { return model.ThrottleMaximunValue; }

        }

        public double VM_ThrottleMinimumValue
        {
            get { return model.ThrottleMinimumValue; }

        }

        public double VM_RudderCurrentValue
        {
            get { return model.RudderCurrentValue; }
        }

        public double VM_RudderMaximunValue
        {
            get { return model.RudderMaximunValue; }

        }

        public double VM_RudderMinimumValue
        {
            get { return model.RudderMinimumValue; }

        }
    }
}
