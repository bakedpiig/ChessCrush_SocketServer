using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

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
            socketList.Add(listenSocket);
            listenSocket.Listen(ListenSocketBackLog);
            Console.WriteLine($"Server Opened! \nServer IP: {GetHostIP().ToString()}, Port Number: {Port.ToString()}");

            while (true) 
            {
                ArrayList listeningSocket = new ArrayList(socketList);

                try
                {
                    Socket.Select(listeningSocket, null, null, 1000);
                    foreach(var sock in listeningSocket)
                    {
                        if(sock == listenSocket)
                        {
                            var newSock = listenSocket.Accept();
                            Console.WriteLine($"New Client Accepted! " +
                             $"\nClient IP: {(newSock.RemoteEndPoint as IPEndPoint).Address}, Port: {(newSock.RemoteEndPoint as IPEndPoint).Port}");
                            listenSocket.Listen(ListenSocketBackLog);
                            socketList.Add(newSock);
                        }
                        else
                        {
                            var clientSocket = sock as Socket;
                            byte[] buffer = new byte[256];
                            clientSocket.Receive(buffer);
                            InputMemoryStream inputMemoryStream = new InputMemoryStream(buffer);
                            Task.Run(() =>
                            {
                                StreamRead(inputMemoryStream);
                            });
                        }
                    }
                    
                    foreach(var sock in socketList)
                    {
                        if (!(sock == listenSocket || sock.Connected))
                            socketList.Remove(sock);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return;
                }
            }
            listenSocket.Close();
        }

        private static void StreamRead(InputMemoryStream stream)
        {

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
