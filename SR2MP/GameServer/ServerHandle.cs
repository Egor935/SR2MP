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

        public static void MessageReceived(int _fromClient, Packet _packet)
        {
            ServerSend.SendMessage(_fromClient, _packet);
        }

        public static void MovementReceived(int _fromClient, Packet _packet)
        {
            ServerSend.SendMovement(_fromClient, _packet);
        }

        public static void AnimationsReceived(int _fromClient, Packet _packet)
        {
            ServerSend.SendAnimations(_fromClient, _packet);
        }

        public static void CameraAngleReceived(int _fromClient, Packet _packet)
        {
            ServerSend.SendCameraAngle(_fromClient, _packet);
        }

        public static void VacconeStateReceived(int _fromClient, Packet _packet)
        {
            ServerSend.SendVacconeState(_fromClient, _packet);
        }

        public static void GameModeReceived(int _fromClient, Packet _packet)
        {
            ServerSend.SendGameMode(_fromClient, _packet);
        }

        public static void TimeReceived(int _fromClient, Packet _packet)
        {
            ServerSend.SendTime(_fromClient, _packet);
        }

        public static void SaveRequestReceived(int _fromClient, Packet _packet)
        {
            ServerSend.SendSaveRequest(_fromClient, _packet);
        }

        public static void SaveReceived(int _fromClient, Packet _packet)
        {
            ServerSend.SendSave(_fromClient, _packet);
        }

        public static void LandPlotUpgradeReceived(int _fromClient, Packet _packet)
        {
            ServerSend.SendLandPlotUpgrade(_fromClient, _packet);
        }

        public static void LandPlotReplaceReceived(int _fromClient, Packet _packet)
        {
            ServerSend.SendLandPlotReplace(_fromClient, _packet);
        }

        public static void SleepReceived(int _fromClient, Packet _packet)
        {
            ServerSend.SendSleep(_fromClient, _packet);
        }

        public static void CurrencyReceived(int _fromClient, Packet _packet)
        {
            ServerSend.SendCurrency(_fromClient, _packet);
        }
    }
}
