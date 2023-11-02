using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.IO;
using MelonLoader;
using System;
using System.IO.Compression;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public static class SendData
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

        public static void SendCameraAngle(float angle)
        {
            using (Packet _packet = new Packet((int)Packets.CameraAngle))
            {
                _packet.Write(angle);
                Networking.SendUDPData(_packet);
            }
        }

        public static void SendVacconeState(bool vacMode)
        {
            using (Packet _packet = new Packet((int)Packets.VacconeState))
            {
                _packet.Write(vacMode);
                Networking.SendTCPData(_packet);
            }
        }

        public static void SendGameModeSwitch(bool state)
        {
            using (Packet _packet = new Packet((int)Packets.GameMode))
            {
                _packet.Write(state);
                Networking.SendTCPData(_packet);
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

        public static void RequestSave()
        {
            using (Packet _packet = new Packet((int)Packets.SaveRequest))
            {
                Networking.SendTCPData(_packet);
            }
        }

        public static void SendSave(MemoryStream save)
        {
            using (Packet _packet = new Packet((int)Packets.Save))
            {
                var arraySave = save.ToArray();
                _packet.Write(arraySave.Length);
                _packet.Write(arraySave);
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

        public static void SendLandPlotReplace(string name, int type)
        {
            using (Packet _packet = new Packet((int)Packets.LandPlotReplace))
            {
                _packet.Write(name);
                _packet.Write(type);
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

        public static void SendCurrency(int value)
        {
            using (Packet _packet = new Packet((int)Packets.Currency))
            {
                _packet.Write(value);
                Networking.SendTCPData(_packet);
            }
        }
    }
}
