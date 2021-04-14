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
            startButton.IsEnabled = false;
            startMenuVM.connect();
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
                uploadTrainCSVButton.IsEnabled = false;
                startButton.IsEnabled = true;
            }
        }

        private void ShowInstructionsPopUp(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Instructions:\n" +
                "If you want to start the program:\n" +
                "Click on the 'Upload a Train CSV' button and choose the CSV file of\n" +
                "the desired flight.\n " +
                "Please make sure you did the following:\n" + "" +
                "1) Insert the following instructions under the 'Additional Settings' in the Flight Gear's Setting:\n" +
                "--generic=socket,in,10,127.0.0.1,6400,tcp,playback_small\n" +
                "--fdm = null\n" +
                "2) Insert the 'playback_small.xml' under the Flight Gear's \\data\\Protocol folder.\n\n" +
                "For Your Information: The threshold is 0.5"
                , "Instuctions", MessageBoxButton.OK);
        }
    }

    #endregion
}
