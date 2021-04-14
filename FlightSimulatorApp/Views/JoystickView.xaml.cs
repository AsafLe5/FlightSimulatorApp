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

namespace FlightSimulatorApp.Views
{
    public partial class JoystickView : UserControl
    {

        #region CTOR

        public JoystickView()
        {
            InitializeComponent();
        }

        private JoystickViewModel joystickVM;

        public JoystickViewModel JoystickVM
        {
            get { return joystickVM; }
            set
            {
                joystickVM = value;
                this.DataContext = value;
            }
        }



        #endregion

        private void rudderSlider_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
