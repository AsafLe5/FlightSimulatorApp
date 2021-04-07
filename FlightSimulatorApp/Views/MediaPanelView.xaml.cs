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

        MediaViewModel vm;

        public MediaPanelView()
        {
            InitializeComponent();
            vm = new MediaViewModel(new MyFlightSimulatorModel(new MyTelnetClient()));
            DataContext = vm;
        }

        #endregion

        #region Functions

        private void Skip_To_Start(object sender, RoutedEventArgs e)
        {
            this.vm.VM_CurrentLineIndex = 0;
        }

        private void Fast_Forward_Left(object sender, RoutedEventArgs e)
        {
            if (this.vm.VM_CurrentLineIndex > 50)
            {
                this.vm.VM_CurrentLineIndex -= 50;
            }
            else
            {
                this.vm.VM_CurrentLineIndex = 0;
            }
        }

        private void Play_Button(object sender, RoutedEventArgs e)
        {
            this.vm.onPlay();
        }

        private void Pause_Button(object sender, RoutedEventArgs e)
        {
            this.vm.onPause();
        }
        private void Stop_Button(object sender, RoutedEventArgs e)
        {
            this.vm.onPause();
            this.vm.VM_CurrentLineIndex = 0;
        }
        private void Fast_Forward_Right(object sender, RoutedEventArgs e)
        {
            if (this.vm.VM_CurrentLineIndex + 50 < this.vm.VM_CSVLinesNumber)
            {
                this.vm.VM_CurrentLineIndex += 50;
            }
            else
            {
                this.vm.VM_CurrentLineIndex = this.vm.VM_CSVLinesNumber;
            }
        }
        private void End_Button(object sender, RoutedEventArgs e)
        {
            this.vm.VM_CurrentLineIndex = this.vm.VM_CSVLinesNumber;
        }

        #endregion
    }
}
