using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulatorApp.Model;

namespace FlightSimulatorApp.ViewModel
{
    public class StartMenuViewModel : INotifyPropertyChanged
    {
        #region CTOR and INPC

        IFlightSimulatorModel model;

        public StartMenuViewModel(IFlightSimulatorModel model)
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

        #region Functions

        public void updateTrainCSVPath(string csvPath)
        {
            model.updateTrainCSVPath(csvPath);
        }

        public void connect()
        {
            model.connect();
        }

        public void start()
        {
            model.start();
        }

        #endregion



    }
}
