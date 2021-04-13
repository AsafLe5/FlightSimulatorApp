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
    /// Interaction logic for StartMenuView.xaml
    /// </summary>
    public partial class StartMenuView : UserControl
    {

        #region CTOR

        public StartMenuView()
        {
            InitializeComponent();
        }

        private StartMenuViewModel startMenuVM;

        public StartMenuViewModel StartMenuVM
        {
            get { return startMenuVM; }
            set
            {
                startMenuVM = value;
                this.DataContext = value;
            }
        }

        #endregion

        #region Start and Open

        private void onStartOnlineMode(object sender, RoutedEventArgs e)
        {
            this.startMenuVM.VM_IsOnline = true;
            startMenuVM.connect();
            startMenuVM.start();
        }

        private void onStartOfflineMode(object sender, RoutedEventArgs e)
        {
            this.startMenuVM.VM_IsOnline = false;
            startMenuVM.start();
        }

        private void onUploadTrainCSVFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                trainCSVPathTextbox.Text = openFileDialog.FileName;
                startMenuVM.updateTrainCSVPath(openFileDialog.FileName);
            }
        }

        private void onUploadTestCSVFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                testCSVPathTextbox.Text = openFileDialog.FileName;
                startMenuVM.updateTestCSVPath(openFileDialog.FileName);
            }
        }

        private void ShowInstructionsPopUp(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Welcome to our instructions\n" +
                "1) If you want to start the program:\n\t" +
                "Click on 'upload train CSV button' and choose the correct file\n\t" +
                "then you can choose Offline/Online mode.\n" +
                "2) Offline mode: run the program without the FlightGear.\n" +
                "3) Online mode: run the program with the FlightGear.\n" +
                "4) If you want to speedup or slowup the program you can slide the " +
                "'Play Speed' slider to change the current speed.\n" +
                "5) If you want to see the values of the 'Linear Regression Graph' you need to roll the mouse pulley " +
                "beside the left area of the graphs and choose your own attribute.\n" +
                "♡ you from Ilan, Noam, Hanna and Asaf", "Instuctions", MessageBoxButton.OK);
        }
    }

    #endregion
}
