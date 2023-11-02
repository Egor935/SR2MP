using MelonLoader;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            MelonLogger.Msg($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                MelonLogger.Msg($"Server: Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            // TODO: send player into game
        }

        public static void UDPTestReceived(int _fromClient, Packet _packet)
        {
            string _msg = _packet.ReadString();

            MelonLogger.Msg($"Server: Received packet via UDP. Contains message: {_msg}");
        }

        public static void TCPDataReceived(int _fromClient, Packet _packet)
        {
            ServerSend.SendData(_fromClient, _packet, 0);
        }

        public static void UDPDataReceived(int _fromClient, Packet _packet)
        {
            ServerSend.SendData(_fromClient, _packet, 1);
        }
    }
}
