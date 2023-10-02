using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class ServerSend
    {
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }

        #region Packets
        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)Packets.Welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }

        public static void UDPTest(int _toClient)
        {
            using (Packet _packet = new Packet((int)Packets.UDP))
            {
                _packet.Write("A test packet for UDP.");

                SendUDPData(_toClient, _packet);
            }
        }

        public static void SendMessage(int id, Packet _packet)
        {
            SendTCPDataToAll(id, _packet);
        }

        public static void SendMovement(int id, Packet _packet)
        {
            SendUDPDataToAll(id, _packet);
        }

        public static void SendAnimations(int id, Packet _packet)
        {
            SendUDPDataToAll(id, _packet);
        }

        public static void SendCameraAngle(int id, Packet _packet)
        {
            SendUDPDataToAll(id, _packet);
        }

        public static void SendVacconeState(int id, Packet _packet)
        {
            SendTCPDataToAll(id, _packet);
        }

        public static void SendGameMode(int id, Packet _packet)
        {
            SendTCPDataToAll(id, _packet);
        }

        public static void SendTime(int id, Packet _packet)
        {
            SendUDPDataToAll(id, _packet);
        }

        public static void SendSaveRequest(int id, Packet _packet)
        {
            SendTCPDataToAll(id, _packet);
        }

        public static void SendSave(int id, Packet _packet)
        {
            SendTCPDataToAll(id, _packet);
        }

        public static void SendLandPlotUpgrade(int id, Packet _packet)
        {
            SendTCPDataToAll(id, _packet);
        }

        public static void SendLandPlotReplace(int id, Packet _packet)
        {
            SendTCPDataToAll(id, _packet);
        }
        #endregion
    }
}
