using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using FlightSimulatorApp.ViewModel;
using FlightSimulatorApp.Model;

namespace FlightSimulatorApp.Views
{
    /// <summary>
    /// Interaction logic for MediaPanelView.xaml
    /// </summary>
    public partial class MediaPanelView : UserControl
    {
        #region CTOR

        public MediaPanelView()
        {
            InitializeComponent();
        }

        private MediaViewModel mediaVM;

        public MediaViewModel MediaVM
        {
            get { return mediaVM; }
            set
            {
                mediaVM = value;
                this.DataContext = value;
            }
        }

        #endregion

        #region Functions

        private void Skip_To_Start(object sender, RoutedEventArgs e)
        {
            this.MediaVM.VM_CurrentLineIndex = 0;
            this.MediaVM.VM_Time = "00:00:00";
        }

        private void Fast_Forward_Left(object sender, RoutedEventArgs e)
        {
            if (this.MediaVM.VM_CurrentLineIndex > 50)
            {
                this.MediaVM.VM_CurrentLineIndex -= 50;
            }
            else
            {
                this.MediaVM.VM_CurrentLineIndex = 0;
            }
        }

        private void Play_Button(object sender, RoutedEventArgs e)
        {
            this.MediaVM.onPlay();
        }

        private void Pause_Button(object sender, RoutedEventArgs e)
        {
            this.MediaVM.onPause();
        }
        private void Stop_Button(object sender, RoutedEventArgs e)
        {
            this.MediaVM.onPause();
            this.MediaVM.VM_CurrentLineIndex = 0;
            this.MediaVM.VM_Time = "00:00:00";

        }
        private void Fast_Forward_Right(object sender, RoutedEventArgs e)
        {
            if (this.MediaVM.VM_CurrentLineIndex + 50 < this.MediaVM.VM_CSVLinesNumber)
            {
                this.MediaVM.VM_CurrentLineIndex += 50;
            }
            else
            {
                this.MediaVM.VM_CurrentLineIndex = this.MediaVM.VM_CSVLinesNumber;
            }
        }
        private void End_Button(object sender, RoutedEventArgs e)
        {
            this.MediaVM.VM_CurrentLineIndex = this.MediaVM.VM_CSVLinesNumber;
        }

        #endregion
    }
}
