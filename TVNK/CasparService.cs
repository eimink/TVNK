using System;
using System.IO;
using System.Net.Sockets;
namespace TVNK
{
    public class CasparService
    {
        public TcpClient casparClient;

        private string ip = "127.0.0.1";
        private int port = 5250;

        public bool isConnected { get { if (casparClient != null) return false; else return casparClient.Connected; }}

        public CasparService()
        {
            casparClient = new TcpClient();
            Connect();
        }

        public CasparService(CasparConfigModel config)
        {
            if (config != null)
            {
                ip = config.host;
                port = config.port;
            }
            casparClient = new TcpClient();
            Connect();
        }

        public void Connect()
        {
            try
            {
                casparClient.Connect(ip, port);
                if(casparClient.Connected)
                {
                    Console.WriteLine("Connected to CasparCG Server at "+ip+":"+port);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error connecting to CasparCG Server: " + ex.Message);
            }
        }

        public void Disconnect()
        {
            casparClient.Close();
        }

        public void SendCommand(string command)
        {
            try
            {
                var reader = new StreamReader(casparClient.GetStream());
                var writer = new StreamWriter(casparClient.GetStream());
                writer.WriteLine(command);
                var reply = reader.ReadLine();
                Console.WriteLine(reply);
                if (reply.Contains("201"))
                {
                    reply = reader.ReadLine();
                    Console.WriteLine(reply);
                }
                else if (reply.Contains("200"))
                {
                    while (reply.Length > 0)
                    {
                        reply = reader.ReadLine();
                        Console.WriteLine(reply);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CasparCG Communication exception: " + ex.Message);
            }
        }
    }
}
