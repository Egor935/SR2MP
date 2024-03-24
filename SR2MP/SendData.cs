using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Persist;
using Il2CppSystem.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
                Networking.SendTCPData(_packet);
            }
        }

        public static void SendMovement(Vector3 pos, float rot)
        {
            using (Packet _packet = new Packet((int)Packets.Movement))
            {
                _packet.Write(pos);
                _packet.Write(rot);
                Networking.SendUDPData(_packet);
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
                Networking.SendUDPData(_packet);
            }
        }

        public static void SendTime(double time)
        {
            using (Packet _packet = new Packet((int)Packets.Time))
            {
                _packet.Write(time);
                Networking.SendUDPData(_packet);
            }
        }

        public static void SendInGame(bool state)
        {
            using (Packet _packet = new Packet((int)Packets.InGame))
            {
                _packet.Write(state);
                Networking.SendTCPData(_packet);
            }
        }

        public static void RequestSaveData()
        {
            using (Packet _packet = new Packet((int)Packets.SaveDataRequest))
            {
                Networking.SendTCPData(_packet);
            }
        }

        public static void SendSaveData(byte[] saveData)
        {
            using (Packet _packet = new Packet((int)Packets.SaveData))
            {
                _packet.Write(saveData.Length);
                _packet.Write(saveData);
                Networking.SendTCPData(_packet);
            }
        }

        public static void SendLandPlotUpgrade(string id, int upgrade)
        {
            using (Packet _packet = new Packet((int)Packets.LandPlotUpgrade))
            {
                _packet.Write(id);
                _packet.Write(upgrade);
                Networking.SendTCPData(_packet);
            }
        }

        public static void SendLandPlotReplace(string id, int type)
        {
            using (Packet _packet = new Packet((int)Packets.LandPlotReplace))
            {
                _packet.Write(id);
                _packet.Write(type);
                Networking.SendTCPData(_packet);
            }
        }

        public static void SendCurrency(int currency)
        {
            using (Packet _packet = new Packet((int)Packets.Currency))
            {
                _packet.Write(currency);
                Networking.SendTCPData(_packet);
            }
        }

        public static void SendSleep(double endTime)
        {
            using (Packet _packet = new Packet((int)Packets.Sleep))
            {
                _packet.Write(endTime);
                Networking.SendTCPData(_packet);
            }
        }
    }
}
