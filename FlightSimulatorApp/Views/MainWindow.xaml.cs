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
using System.Runtime.InteropServices;


namespace FlightSimulatorApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region CTOR

        MyFlightSimulatorModel flightSimulatorModel;

        DataDisplayViewModel dataDisplayViewModel;
        GraphViewModel graphViewModel;
        JoystickViewModel joystickViewModel;
        MediaViewModel mediaViewModel;
        StartMenuViewModel startMenuViewModel;

        public MainWindow()
        {
            InitializeComponent();
            flightSimulatorModel = new MyFlightSimulatorModel(new MyTelnetClient());

            dataDisplayViewModel = new DataDisplayViewModel(flightSimulatorModel);
            dataDisplayControl.DataDisplayVM = dataDisplayViewModel;

            graphViewModel = new GraphViewModel(flightSimulatorModel);
            graphControl.GraphVM = graphViewModel;

            joystickViewModel = new JoystickViewModel(flightSimulatorModel);
            joystickControl.JoystickVM = joystickViewModel;

            mediaViewModel = new MediaViewModel(flightSimulatorModel);
            mediaPanelControl.MediaVM = mediaViewModel;

            startMenuViewModel = new StartMenuViewModel(flightSimulatorModel);
            startMenuControl.StartMenuVM = startMenuViewModel;

        }

        #endregion
    }
}
