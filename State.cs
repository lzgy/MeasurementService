using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MeasurementService
{
    internal class State
    {
        public int BufferSize = 256;
        public State()
        {
            Buffer = new byte[BufferSize];
        }
        public Socket Client { get; set; }
        public byte[] Buffer { get; set; }

    }
}
