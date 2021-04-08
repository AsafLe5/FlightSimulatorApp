using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulatorApp.Model;

namespace FlightSimulatorApp.ViewModel
{
    public class MediaViewModel : INotifyPropertyChanged
    {
        #region CTOR and INPC

        IFlightSimulatorModel model;

        public MediaViewModel(IFlightSimulatorModel model)
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

        #region Proprties

        public int VM_CurrentLineIndex
        {
            get { return model.CurrentLineIndex; }
            set { this.model.CurrentLineIndex = value; }
        }

        public int VM_CSVLinesNumber
        {
            get { return model.CSVLinesNumber; }
        }

        public int VM_PlaybackSpeed
        {
            get { return model.PlaybackSpeed; }
            set { this.model.PlaybackSpeed = value; }
        }

        #endregion

        #region Functions

        public void onPlay()
        {
            this.model.onPlay();
        }

        public void onPause()
        {
            this.model.onPause();
        }
        #endregion

    }
}
