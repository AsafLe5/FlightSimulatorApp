using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Telnet
{
    public interface ITelnetClient
    {
        bool connect();
        void write(string command);
        //string read();
        void disconnect();
    }
}