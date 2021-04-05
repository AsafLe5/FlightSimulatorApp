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

namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FlightSimulatorViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = new FlightSimulatorViewModel(new MyFlightSimulatorModel(new MyTelnetClient()));
            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.connect();
            vm.start();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                vm.updateCSVPath(openFileDialog.FileName);
            }

        }


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

        private void rudderSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
