using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR2MP
{
    public class Networking
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

        public static void SendReliableData(Packet packet)
        {
            byte[] data = packet.ToArray();
            SteamNetworking.SendP2PPacket(SteamLobby.Receiver, data, (uint)data.Length, EP2PSend.k_EP2PSendReliable, 0);
        }

        public static void SendUnreliableData(Packet packet)
        {
            byte[] data = packet.ToArray();
            SteamNetworking.SendP2PPacket(SteamLobby.Receiver, data, (uint)data.Length, EP2PSend.k_EP2PSendUnreliable, 0);
        }

        public static bool HandlePacket;
        private static void HandleReceivedData(byte[] _data)
        {
            HandlePacket = true;

            using (Packet _packet = new Packet(_data))
            {
                int _packetId = _packet.ReadInt();
                packetHandlers[_packetId].Invoke(_packet);
            }

            HandlePacket = false;
        }

        public static void InitializePackets()
        {
            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)Packets.Message, HandleData.HandleMessage },
                { (int)Packets.Movement, HandleData.HandleMovement },
                { (int)Packets.Animations, HandleData.HandleAnimations },
                { (int)Packets.Time, HandleData.HandleTime },
                { (int)Packets.InGame, HandleData.HandleInGame },
                { (int)Packets.SaveDataRequest, HandleData.HandleSaveDataRequest },
                { (int)Packets.SaveData, HandleData.HandleSaveData },
                { (int)Packets.LandPlotUpgrade, HandleData.HandleLandPlotUpgrade },
                { (int)Packets.LandPlotReplace, HandleData.HandleLandPlotReplace },
                { (int)Packets.Currency, HandleData.HandleCurrency },
                { (int)Packets.Sleep, HandleData.HandleSleep },
                { (int)Packets.Prices, HandleData.HandlePrices },
                { (int)Packets.MapOpen, HandleData.HandleMapOpen },
                { (int)Packets.GordoEat, HandleData.HandleGordoEat }
            };
        }
    }
}
