using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            // TODO: send player into game
        }

        public static void UDPTestReceived(int _fromClient, Packet _packet)
        {
            string _msg = _packet.ReadString();

            Console.WriteLine($"Received packet via UDP. Contains message: {_msg}");
        }

        public static void TCPDataReceived(int _fromClient, Packet _packet)
        {
            ServerSend.ResendTCPData(_fromClient, _packet);
        }

        public static void UDPDataReceived(int _fromClient, Packet _packet)
        {
            ServerSend.ResendUDPData(_fromClient, _packet);
        }
    }
}
