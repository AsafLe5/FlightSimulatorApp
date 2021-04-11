using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulatorApp.Model;
using System.ComponentModel;
using OxyPlot;

namespace FlightSimulatorApp.ViewModel
{
    public class GraphViewModel : INotifyPropertyChanged
    {
        #region CTOR and INPC

        IFlightSimulatorModel model;

        public GraphViewModel(IFlightSimulatorModel model)
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

        public List<string> VM_AttributesList
        {
            get { return this.model.AttributesList; }
        }

        #region Original Graph Properties

        public List<DataPoint> VM_DataPointsList
        {
            get { return this.model.DataPointsList; }
        }

        public string VM_CurrentAttribute
        {
            get { return this.model.CurrentAttribute; }
            set
            {
                this.model.CurrentAttribute = value;
            }
        }

        #endregion

        #region Correlative Graph Properties

        public List<DataPoint> VM_DataPointsListToCorrelative
        {
            get { return this.model.DataPointsListToCorrelative; }
        }

        public string VM_CurrentCorrelativeAttribute
        {
            get { return this.model.CurrentCorrelativeAttribute; }
        }

        #endregion
    }
}
