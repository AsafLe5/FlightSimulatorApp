﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulatorApp.Model;

using System.ComponentModel;

namespace FlightSimulatorApp.ViewModel
{
    class FlightSimulatorViewModel : INotifyPropertyChanged
    {
        #region CTOR and INPC

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

        #endregion

        public int VM_CurrentLineIndex
        {
            get { return model.CurrentLineIndex; }
            set { this.model.CurrentLineIndex = value; }
        }

        public int VM_CSVLinesNumber
        {
            get { return model.CSVLinesNumber; }
        }
    }
}
