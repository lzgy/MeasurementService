using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Text;
using System.Timers;

namespace MeasurementService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private Measure measure;

        private SocketClient client;

        private System.Timers.Timer timer;

        private int Period { get; set; }

        public Worker(ILogger<Worker> logger)
        {
            Period = Convert.ToInt32( ConfigurationManager.AppSettings["Period"]);

            timer = new System.Timers.Timer(Period);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;

            measure = new MeasureFromOpenHardware();
            try
            {
                client = new SocketClient(ConfigurationManager.AppSettings["ServerIP"].ToString(), Convert.ToUInt16(ConfigurationManager.AppSettings["Port"]));

                #region események
                try
                {
                    client.OnConnect += new SocketClient.OnConnectEventHandler(client_OnConnect);
               
                    client.OnDisconnect += new SocketClient.OnDisconnectEventHandler(client_OnDisconnected);
                    //client.OnSend += new SocketClient.onSendEventHandler(client_OnSend);
                    client.Received += new SocketClient.onReceiveEventHandler(client_OnReceive);
                }
                catch { }
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogError("Nem tudott kapcsolódni! " + ex.Message, null);
            }
            _logger = logger;
        }
        /// <summary>
        /// Beállítja menetközben a periódus idõt
        /// és menti az appConfig-ba
        /// </summary>
        /// <param name="period"></param>
        private void setPeriodFromServer(int period) 
        {
            Period = period;
            //timer.Enabled = false;
            timer.Stop();
            timer.Interval = Period;
            //timer.Enabled = true;
            timer.Start();

            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["Periodus"].Value = period.ToString();
            config.Save(ConfigurationSaveMode.Modified);
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (client != null)
            {
                if (!client.Connected)
                {
                    try
                    {
                        client.Connect();
                    }
                    catch (Exception ex) 
                    {
                        _logger.LogError("Nem tudott kapcsolódni", null);
                    }
                }
                else
                {
                    try
                    {
                        byte[] buf = new byte[256];
                        string m = client.Name + ";" + measure.refreshToString() + "\n";
                        buf = Encoding.ASCII.GetBytes(m);
                        client.Send(buf, 0, buf.Length);
                        _logger.LogInformation("{time} Eküldött üzenet: " + m, DateTimeOffset.Now);
                    }
                    catch (Exception ex) 
                    {
                        _logger.LogError("{time} Hiba történt az üzenetküldés során" + ex.Message , DateTime.Now);
                    }
                }
            }
            else
            {
                
            }
        }

        private void client_OnReceive(SocketClient sender, String data)
        {
            if (client.Connected)
            {
                _logger.LogInformation("Szerver üzenete: " + data, DateTimeOffset.Now);
            }
        }
        //private void client_OnSend(SocketClient sender, StringState data)
        //{
        //    throw new NotImplementedException();
        //}

        private void client_OnDisconnected(SocketClient sender)
        {
            _logger.LogInformation("{time} Szerverrõl lecsatlakozott!", DateTime.Now);
        }

        private void client_OnConnect(SocketClient sender, bool connected)
        {
            client.state.Buffer = Encoding.Default.GetBytes(client.Name);
            client.Send(client.state.Buffer, 0, client.state.Buffer.Length);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
