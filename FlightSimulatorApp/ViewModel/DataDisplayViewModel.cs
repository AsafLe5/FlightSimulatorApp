using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulatorApp.Model;

namespace FlightSimulatorApp.ViewModel
{
    public class DataDisplayViewModel : INotifyPropertyChanged
    {
        #region CTOR and INPC

        IFlightSimulatorModel model;

        public DataDisplayViewModel(IFlightSimulatorModel model)
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

        #endregion
    }
}
