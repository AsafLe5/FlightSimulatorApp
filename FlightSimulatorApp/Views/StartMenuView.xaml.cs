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
            startMenuVM.connect();
            startMenuVM.start();
        }

        private void onUploadTrainCSVFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            bool? response = openFileDialog.ShowDialog();

            if (response == true)
                startMenuVM.updateCSVPath(openFileDialog.FileName);
        }

    }

    #endregion
}
