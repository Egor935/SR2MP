using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public class ClientSend : MonoBehaviour
    {
        private static void SendTCPData(Packet _packet)
        {
            _packet.WriteLength();
            Client.instance.tcp.SendData(_packet);
        }

        private static void SendUDPData(Packet _packet)
        {
            _packet.WriteLength();
            Client.instance.udp.SendData(_packet);
        }

        #region Packets
        public static void WelcomeReceived()
        {
            using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
            {
                _packet.Write(Client.instance.myId);
                _packet.Write(Environment.UserName);

                SendTCPData(_packet);
            }
        }

        public static void UDPTestReceived()
        {
            using (Packet _packet = new Packet((int)ClientPackets.updTestReceived))
            {
                _packet.Write("Received a UDP packet.");

                SendUDPData(_packet);
            }
        }

        public static void SendMovement(Vector3 pos, float rot)
        {
            using (Packet _packet = new Packet((int)ClientPackets.movementReceived))
            {
                _packet.Write(pos);
                _packet.Write(rot);

                SendUDPData(_packet);
            }
        }

        public static void SendAnimations(float f1, float f2, float f3, int i1, bool b1, float f4, float f5)
        {
            using (Packet _packet = new Packet((int)ClientPackets.animationsReceived))
            {
                _packet.Write(f1);
                _packet.Write(f2);
                _packet.Write(f3);
                _packet.Write(i1);
                _packet.Write(b1);
                _packet.Write(f4);
                _packet.Write(f5);

                SendUDPData(_packet);
            }
        }
        #endregion
    }
}
