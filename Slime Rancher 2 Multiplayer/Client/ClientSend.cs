using GameServer;
using KinematicCharacterController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Slime_Rancher_2_Multiplayer
{
    public class ClientSend : MonoBehaviour
    {
        private static void SendTCPData(Packets _packet)
        {
            _packet.WriteLength();
            Client.instance.tcp.SendData(_packet);
        }

        private static void SendUDPData(Packets _packet)
        {
            _packet.WriteLength();
            Client.instance.udp.SendData(_packet);
        }

        #region Packets
        public static void WelcomeReceived()
        {
            using (Packets _packet = new Packets((int)ClientPackets.welcomeReceived))
            {
                _packet.Write(Client.instance.myId);
                _packet.Write(Environment.UserName);

                SendTCPData(_packet);
            }
        }

        public static void UDPTestReceived()
        {
            using (Packets _packet = new Packets((int)ClientPackets.updTestReceived))
            {
                _packet.Write("Received a UDP packet.");

                SendUDPData(_packet);
            }
        }

        public static void SpawnPlayer()
        {
            using (Packets _packet = new Packets((int)ServerPackets.spawnPlayer))
            {
                SendTCPData(_packet);
            }
        }

        public static void PlayerMovement(Vector3 pos, Quaternion rot)
        {
            using (Packets _packet = new Packets((int)ServerPackets.SendMovement))
            {
                _packet.Write(pos);
                _packet.Write(rot);
                SendTCPData(_packet);
            }
        }
        #endregion
    }
}
