using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

namespace MeasurementService
{    /// <summary>
     /// CSatlakozó kliens osztály, Csatlakozik egy socket serverhez 
     /// </summary>
    internal class SocketClient
    {
        //<TODO> Asszinkron kivételkezelést megoldani 
        public delegate void OnConnectEventHandler(SocketClient sender, bool connected);
        public event OnConnectEventHandler OnConnect;

        public delegate void onSendEventHandler(SocketClient sender, StringState data);
        public event onSendEventHandler OnSend;

        public delegate void onReceiveEventHandler(SocketClient sender, String data);
        public event onReceiveEventHandler Received;

        public delegate void OnDisconnectEventHandler(SocketClient sender);
        public event OnDisconnectEventHandler OnDisconnect;

        private bool connected;
        public bool Connected
        {
            get
            {
                if (state != null && state.Client != null)
                {
                    return state.Client.Connected;
                }
                return false;
            }
        }

        public String Name { get; private set; }
        
        public StringState state;
        IPEndPoint endPoint = null;

        public string getIP_PORT() 
        {
            return endPoint.ToString();
        }

        public SocketClient(String ipAddress, UInt16 portName) : this()
        {
            this.Name = ConfigurationManager.AppSettings["Name"].ToString();
            endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), portName);
            try
            {
                Connect();
            }
            catch { }
        }

        public SocketClient() 
        {
            if (endPoint == null) { endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000); }
            state = new StringState();
        }

        Socket getNewSocket() 
        { 
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect() 
        {
            if (endPoint == null) return;
            try
            {
                state.Client = getNewSocket();
                state.Client.BeginConnect(endPoint, ConnectCallback, null);
            }
            catch { throw; }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                state.Client.EndConnect(ar);
                if (OnConnect != null)
                {
                    OnConnect(this, Connected);
                }
                state.Buffer = new byte[state.Client.ReceiveBufferSize];
                state.Client.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, ReceiveCallback, state);
            }
            catch { }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try 
            {
                String receivedTemp = String.Empty;
                state = (StringState)ar.AsyncState;
                Socket handler = state.Client;

                int readed = state.Client.EndReceive(ar);
                

                if (readed == 0)
                {
                    return;
                }

                if (readed > 0)
                {
                    state.Data.Append(Encoding.Default.GetString(state.Buffer, 0, readed));
                    receivedTemp = state.Data.ToString();
                    if (receivedTemp.IndexOf("\n") > -1)
                    {
                        if (Received != null)
                        {
                            Received(this, receivedTemp);
                        }
                        state.Buffer = new byte[state.BufferSize];
                        state.Data = new StringBuilder();
                        handler.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0, new AsyncCallback(ReceiveCallback), state);
                    }
                    else
                    {
                        handler.BeginReceive(state.Buffer, 0, state.Buffer.Length, 0, new AsyncCallback(ReceiveCallback), state);
                    }
                }
                else
                {
                    handler.BeginReceive(new byte[] { 0 }, 0, 0, 0, ReceiveCallback, null);
                }

                //if (Received != null) 
                //{
                //    Received(this, state);
                //}
                
                //újra indítjuk a beolvasást!
                //state.Client.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, ReceiveCallback, null);
            } catch { }
        }

        public void Disconnect()
        {
            try
            {
                if (state.Client.Connected)
                {
                    state.Client.Close();
                    state.Client = null;
                    if (OnDisconnect != null)
                    {
                        OnDisconnect(this);
                    }
                }
            }
            catch { }
        }

        public void Send(byte[] data, int index, int length)
        {
            try
            {
                String ellenor = Encoding.Default.GetString(data);
                if (OnSend != null)
                {
                    OnSend(this, state);
                }
                state.Client.BeginSend(data, index, length, SocketFlags.None, sendCallback, null);
            }
            catch { }
        }

        void sendCallback(IAsyncResult ar)
        {
            try
            {
                int sended = state.Client.EndSend(ar);
                if (sended == 0)
                {
                    return;
                }
                int send = state.Client.Send(state.Buffer, state.Buffer.Length, 0);
            }
            catch { }
        }
    }
}
