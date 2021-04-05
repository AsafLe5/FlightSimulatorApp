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
            this.model.PropertyChanged +=
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

        public int VM_PlaybackSpeed
        {
            get { return model.PlaybackSpeed; }
            set { this.model.PlaybackSpeed = value; }
        }
        public float VM_AileronCurrentValue
        {
            get { return model.AileronCurrentValue; }
        }

        public float VM_AileronMaximunValue
        {
            get { return model.AileronMaximunValue; }

        }

        public float VM_AileronMinimumValue
        {
            get { return model.AileronMinimumValue; }

        }

        public float VM_ElevatorCurrentValue
        {
            get { return model.ElevatorCurrentValue; }
        }


        public float VM_ElevatorMaximunValue
        {
            get { return model.ElevatorMaximunValue; }

        }

        public float VM_ElevatorMinimumValue
        {
            get { return model.ElevatorMinimumValue; }

        }

        public float VM_ThrottleCurrentValue
        {
            get { return model.ThrottleCurrentValue; }
        }

        public float VM_ThrottleMaximunValue
        {
            get { return model.ThrottleMaximunValue; }

        }

        public float VM_ThrottleMinimumValue
        {
            get { return model.ThrottleMinimumValue; }

        }

        public float VM_RudderCurrentValue
        {
            get { return model.RudderCurrentValue; }
        }

        public float VM_RudderMaximunValue
        {
            get { return model.RudderMaximunValue; }

        }

        public float VM_RudderMinimumValue
        {
            get { return model.RudderMinimumValue; }

        }

        public float VM_CurrentAltimeter
        {
            get { return model.CurrentAltimeter; }
        }

        public float VM_CurrentAirSpeed
        {
            get { return model.CurrentAirSpeed; }
        }

        public float VM_CurrentHeading
        {
            get { return model.CurrentHeading; }
        }

        public float VM_CurrentPitch
        {
            get { return model.CurrentPitch; }
        }

        public float VM_CurrentRoll
        {
            get { return model.CurrentRoll; }
        }

        public float VM_CurrentYaw
        {
            get { return model.CurrentYaw; }
        }
    }
}
