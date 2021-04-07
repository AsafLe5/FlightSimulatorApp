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
using FlightSimulatorApp.Model;
using FlightSimulatorApp.ViewModel;


namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region CTOR
       
        FlightSimulatorViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = new FlightSimulatorViewModel(new MyFlightSimulatorModel(new MyTelnetClient()));
            DataContext = vm;
        }

        #endregion

        #region Start and Open

        private void onStart(object sender, RoutedEventArgs e)
        {
            vm.connect();
            vm.start();
        }

        private void onOpenCSV(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                vm.updateCSVPath(openFileDialog.FileName);
            }

        }

        #endregion
    }
}
