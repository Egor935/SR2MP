using Il2CppSystem.Collections.Generic;
using System;
using System.Diagnostics;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public class SendData
    {
        public static void SendMessage(string msg)
        {
            using (Packet _packet = new Packet((int)Packets.Message))
            {
                _packet.Write(msg);
                Networking.SendReliableData(_packet);
            }
        }

        public static void SendMovement(Vector3 pos, float rot)
        {
            using (Packet _packet = new Packet((int)Packets.Movement))
            {
                _packet.Write(pos);
                _packet.Write(rot);
                Networking.SendUnreliableData(_packet);
            }
        }

        public static void SendAnimations(float f1, float f2, float f3, int i1, bool b1, float f4, float f5)
        {
            using (Packet _packet = new Packet((int)Packets.Animations))
            {
                _packet.Write(f1);
                _packet.Write(f2);
                _packet.Write(f3);
                _packet.Write(i1);
                _packet.Write(b1);
                _packet.Write(f4);
                _packet.Write(f5);
                Networking.SendUnreliableData(_packet);
            }
        }

        public static void SendTime(double time)
        {
            using (Packet _packet = new Packet((int)Packets.Time))
            {
                _packet.Write(time);
                Networking.SendUnreliableData(_packet);
            }
        }

        public static void SendInGame(bool state)
        {
            using (Packet _packet = new Packet((int)Packets.InGame))
            {
                _packet.Write(state);
                Networking.SendReliableData(_packet);
            }
        }

        public static void RequestSaveData()
        {
            using (Packet _packet = new Packet((int)Packets.SaveDataRequest))
            {
                Networking.SendReliableData(_packet);
            }
        }

        public static void SendSaveData(byte[] saveData)
        {
            using (Packet _packet = new Packet((int)Packets.SaveData))
            {
                _packet.Write(saveData.Length);
                _packet.Write(saveData);
                Networking.SendReliableData(_packet);
            }
        }

        public static void SendLandPlotUpgrade(string name, int upgrade)
        {
            using (Packet _packet = new Packet((int)Packets.LandPlotUpgrade))
            {
                _packet.Write(name);
                _packet.Write(upgrade);
                Networking.SendReliableData(_packet);
            }
        }

        public static void SendLandPlotReplace(string name, int type)
        {
            using (Packet _packet = new Packet((int)Packets.LandPlotReplace))
            {
                _packet.Write(name);
                _packet.Write(type);
                Networking.SendReliableData(_packet);
            }
        }

        public static void SendCurrency(int currency, bool reliable)
        {
            using (Packet _packet = new Packet((int)Packets.Currency))
            {
                _packet.Write(currency);
                if (reliable)
                {
                    Networking.SendReliableData(_packet);
                }
                else
                {
                    Networking.SendUnreliableData(_packet);
                }
            }
        }

        public static void SendSleep(double endTime)
        {
            using (Packet _packet = new Packet((int)Packets.Sleep))
            {
                _packet.Write(endTime);
                Networking.SendReliableData(_packet);
            }
        }

        public static void SendPrices(Dictionary<IdentifiableType, EconomyDirector.CurrValueEntry> prices)
        {
            using (Packet _packet = new Packet((int)Packets.Prices))
            {
                _packet.Write(prices.Count);
                foreach (var price in prices)
                {
                    _packet.Write(price.Value.CurrValue);
                }
                Networking.SendReliableData(_packet);
            }
        }

        public static void SendMapOpen(string name)
        {
            using (Packet _packet = new Packet((int)Packets.MapOpen))
            {
                _packet.Write(name);
                Networking.SendReliableData(_packet);
            }
        }

        public static void SendGordoEat(string name, int eatenCount)
        {
            using (Packet _packet = new Packet((int)Packets.GordoEat))
            {
                _packet.Write(name);
                _packet.Write(eatenCount);
                Networking.SendReliableData(_packet);
            }
        }
    }
}
