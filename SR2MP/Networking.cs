using Il2CppInterop.Runtime.InteropTypes.Arrays;
using MelonLoader;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR2MP
{
    public static class Networking
    {
        private delegate void PacketHandler(Packet _packet);
        private static Dictionary<int, PacketHandler> packetHandlers;

        public static void ListenData()
        {
            uint size;
            while (SteamNetworking.IsP2PPacketAvailable(out size, 0))
            {
                byte[] _data = new byte[size];
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
            if (MultiplayerMain.Instance.SteamIsAvailable)
            {
                byte[] data = packet.ToArray();
                SteamNetworking.SendP2PPacket(SteamLobby.Instance.Receiver, data, (uint)data.Length, EP2PSend.k_EP2PSendReliable, 0);
            }
            else
            {
                ClientSend.SendTCPData(packet);
            }
        }

        public static void SendUDPData(Packet packet)
        {
            if (MultiplayerMain.Instance.SteamIsAvailable)
            {
                byte[] data = packet.ToArray();
                SteamNetworking.SendP2PPacket(SteamLobby.Instance.Receiver, data, (uint)data.Length, EP2PSend.k_EP2PSendUnreliable, 0);
            }
            else
            {
                ClientSend.SendUDPData(packet);
            }
        }

        private static void HandleReceivedData(byte[] _data)
        {
            Statics.HandlePacket = true;

            using (Packet _packet = new Packet(_data))
            {
                int _packetId = _packet.ReadInt();
                packetHandlers[_packetId].Invoke(_packet);
            }

            Statics.HandlePacket = false;
        }

        public static void InitializePackets()
        {
            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)Packets.Message, HandleData.HandleMessage },
                { (int)Packets.Movement, HandleData.HandleMovement },
                { (int)Packets.Animations, HandleData.HandleAnimations },
                { (int)Packets.CameraAngle, HandleData.HandleCameraAngle },
                { (int)Packets.VacconeState, HandleData.HandleVacconeState },
                { (int)Packets.GameMode, HandleData.HandleGameModeSwitch },
                { (int)Packets.Time, HandleData.HandleTime },
                { (int)Packets.SaveRequest, HandleData.SaveRequested },
                { (int)Packets.Save, HandleData.HandleSave },
                { (int)Packets.LandPlotUpgrade, HandleData.HandleLandPlotUpgrade },
                { (int)Packets.LandPlotReplace, HandleData.HandleLandPlotReplace },
                { (int)Packets.Sleep, HandleData.HandleSleep },
                { (int)Packets.Currency, HandleData.HandleCurrency },
                { (int)Packets.Actors, HandleData.HandleActors }
            };
        }
    }
}
