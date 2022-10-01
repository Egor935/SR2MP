using Slime_Rancher_2_Multiplayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packets _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            //Server.clients[_fromClient].SendIntoGame(_username);
            // TODO: send player into game
        }

        public static void UDPTestReceived(int _fromClient, Packets _packet)
        {
            string _msg = _packet.ReadString();

            Console.WriteLine($"Received packet via UDP. Contains message: {_msg}");
        }

        public static void SpawnBodyReceived(int _fromClient, Packets _packet)
        {
            ServerSend.SpawnPlayer();
        }

        public static void PlayerMovementReceived(int _fromClient, Packets _packet)
        {
            var pos = _packet.ReadVector3();
            var rot = _packet.ReadQuaternion();
            ServerSend.PlayerMovement(_fromClient, pos, rot);
        }
    }
}
