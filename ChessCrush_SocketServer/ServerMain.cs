using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChessCrush_SocketServer
{
    class ServerMain
    {
        private static Socket listenSocket;
        private static readonly int ListenSocketBackLog = 32;
        private static readonly int Port = 48000;
        private static List<Socket> socketList = new List<Socket>();

        public static void Main(string[] args)
        {
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(new IPEndPoint(GetHostIP(), Port));
            listenSocket.Listen(ListenSocketBackLog);
            Console.WriteLine($"Server IP: {GetHostIP().ToString()}, Port Number: {Port.ToString()}");
            listenSocket.BeginAccept(ar => EndAccept(ar), null);

            while (true) 
            {
                Thread.Sleep(3000);
            }
            listenSocket.Close();
        }

        private static void EndAccept(IAsyncResult ar)
        {
            var newSock = listenSocket.EndAccept(ar);
            listenSocket.BeginAccept(res => EndAccept(res), null);
            Console.WriteLine($"New Client Accepted! " +
                $"\nClient IP: {(newSock.RemoteEndPoint as IPEndPoint).Address}, Port: {(newSock.RemoteEndPoint as IPEndPoint).Port}");
            socketList.Add(newSock);
        }

        private static IPAddress GetHostIP()
        {
            string strHostName;
            strHostName = Dns.GetHostName();

            var ipEntry = Dns.GetHostEntry(strHostName);
            var addr = ipEntry.AddressList;
            return IPAddress.Parse(addr[addr.Length - 1].ToString());
        }
    }
}
