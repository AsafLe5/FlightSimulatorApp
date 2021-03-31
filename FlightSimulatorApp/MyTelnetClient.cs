using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

namespace FlightSimulatorApp
{
    class MyTelnetClient : ITelnetClient
    {
        private string ip;
        private int port;
        private Socket socket;
        //TODO for read?
        private byte[] buffer;

        public MyTelnetClient()
        {
            this.ip = "127.0.0.1";
            this.port = 6400;
        }

        public void connect()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(this.ip, this.port);
        }

        public void disconnect()
        {
            socket.Disconnect(true);
        }

        //TODO Do we need this?
        public string read()
        {
            //TODO size of buffer
            socket.Receive(buffer, 0, 1024, 0);
            return buffer.ToString();
        }

        public void write(string command)
        {
            var commandInBytes = Encoding.ASCII.GetBytes(command + "\r\n");
            socket.Send(commandInBytes);
        }
    }
}
