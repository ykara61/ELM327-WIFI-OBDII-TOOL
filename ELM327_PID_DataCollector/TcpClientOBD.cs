using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ELM327_PID_DataCollector
{
    public class TcpClientOBD : TcpClient
    {
        private string ip;
        private int port;
        
        //TcpClient client;
        NetworkStream stream;
        public bool connected = false;

        public delegate void EventPIDholder(string message);
        public event EventPIDholder PidMessageArrived;

        public delegate void EventPID();
        public event EventPID OBDdeviceReady;

        public TcpClientOBD(string _ip, int _port)
        {
            ip = _ip;
            port = _port;
        }


        public void StartOBDdev()
        {

            bool Tryconnect = true;

            Task.Run(() =>
            {
                while (Tryconnect)
                {
                    try
                    {
                        Connect(IPAddress.Parse(ip), port);
                        stream = GetStream();
                        Tryconnect = false;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                Console.WriteLine("Connected");
                connected = true;

                Task.Delay(1000).Wait();

                SetElm327Configs();

                Task.Delay(1000).Wait();

                OBDdeviceReady.Invoke();

                string data = "";
                int k = 0;

                while (true)
                {
                    // Buffer to store the response bytes.
                    byte[] buffer = new byte[ReceiveBufferSize];

                    int bytesRead = stream.Read(buffer, 0,buffer.Length);

                    byte[] mesajj = new byte[bytesRead];

                    for (int i = 0; i < bytesRead; i++)
                    {
                        mesajj[i] = buffer[i];
                    }

                    k++;
                    data += Encoding.Default.GetString(mesajj, 0, bytesRead).Replace("\r"," ");

                    if (data.EndsWith('>') || data.Length> 128 || k> 10)
                    {
                        
                        k = 0;
                        if (data.Length > 128)
                        {
                            send("\r");
                        }

                        PidMessageArrived.Invoke(data);

                        data = "";
                    }
                }
            });
        }

        void SetElm327Configs()
        {

            send("\r");

            send("AT SP 0" + "\r");

            send("AT D" + "\r");

            send("AT DPN" + "\r");

            send("AT I" + "\r");

            send("0100" + "\r");

            send("AT H0" + "\r");

            send("AT AT2" + "\r");

            send("AT SH 7FF" + "\r");

            Console.WriteLine("Battery Voltage:");
            send("AT RV" + "\r");

            send("0902" + "\r");
        }

        public void SendSpeedRequest()
        {
            send("010D" + "\r");
        }

        public void SendRpmRequest()
        {
            send("010C" + "\r");
        }

        public void send(byte[] msg)
        {
            if (stream == null)
            {
                Console.WriteLine("No stream yet");
            }
            Console.WriteLine("SENT");
            //var x = Encoding.ASCII.GetBytes(msg);
            stream.Write(msg, 0, msg.Length);
        }

        public void send(string msg)
        {
            if (stream == null)
            {
                Console.WriteLine("No stream yet");
            }
            Console.WriteLine("SENT");
            //var x = Encoding.ASCII.GetBytes(msg);
            var msgByte = Encoding.ASCII.GetBytes(msg);
            stream.Write(msgByte, 0, msgByte.Length);
        }
    }
}
