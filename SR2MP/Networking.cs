using MelonLoader;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnityEngine;

namespace SR2MP
{
    public class Networking : MonoBehaviour
    {
        private delegate void PacketHandler(Packet _packet);
        private static Dictionary<int, PacketHandler> packetHandlers;

        void Start()
        {
            InitializeClientData();
        }

        public static void ListenData()
        {
            uint size;
            while (SteamNetworking.IsP2PPacketAvailable(out size, 0))
            {
                Il2CppStructArray<byte> _data = new Il2CppStructArray<byte>(size);
                uint bytesRead;

                CSteamID remoteId;

                if (SteamNetworking.ReadP2PPacket(_data, size, out bytesRead, out remoteId, 0))
                {
                    HandleReceivedData(_data);
                }
            }
        }

        public static void SendTCPData(Packet packet)
        {
            byte[] data = packet.ToArray();
            SteamNetworking.SendP2PPacket(SteamLobby.receiver, data, (uint)data.Length, EP2PSend.k_EP2PSendReliable, 0);
        }

        public static void SendUDPData(Packet packet)
        {
            byte[] data = packet.ToArray();
            SteamNetworking.SendP2PPacket(SteamLobby.receiver, data, (uint)data.Length, EP2PSend.k_EP2PSendUnreliable, 0);
        }

        public static void HandleReceivedData(byte[] _data)
        {
            //MelonLogger.Msg("Packet size: " + _data.Length);
            using (Packet _packet = new Packet(_data))
            {
                int _packetId = _packet.ReadInt();
                packetHandlers[_packetId].Invoke(_packet);
            }
        }

        public static void InitializeClientData()
        {
            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)Packets.Welcome, HandleData.WelcomeReceived },
                { (int)Packets.Movement, HandleData.MovementReceived },
                { (int)Packets.Animations, HandleData.AnimationsReceived },
                { (int)Packets.Time, HandleData.TimeReceived },
                { (int)Packets.RequestData, HandleData.HandleRequestedData },
                { (int)Packets.DataRequested, HandleData.HandleDataRequested },
                { (int)Packets.CameraAngle, HandleData.HandleCameraAngle },
                { (int)Packets.VacconeState, HandleData.HandleVacconeState },
            };
            MelonLogger.Msg("Initialized packets.");
        }
    }
}
