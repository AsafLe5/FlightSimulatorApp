using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulatorApp.Model;

namespace FlightSimulatorApp.ViewModel
{
    class JoystickViewModel : INotifyPropertyChanged
    {
        #region CTOR and INPC

        IFlightSimulatorModel model;

        public JoystickViewModel(IFlightSimulatorModel model)
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

        #endregion

        #region Data Region

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

        #endregion
    }
}
