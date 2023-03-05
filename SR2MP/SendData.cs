using Il2CppSystem.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public class SendData
    {
        public static void SendConnection(bool inGame)
        {
            using (Packet _packet = new Packet((int)Packets.Connect))
            {
                _packet.Write(inGame);
                Networking.SendUDPData(_packet);
            }
        }

        public static void SendWelcome(string msg)
        {
            using (Packet _packet = new Packet((int)Packets.Welcome))
            {
                _packet.Write(msg);
                Networking.SendTCPData(_packet);
            }
        }

        public static void SendMovement(Vector3 pos, float rot, Vector3 speed)
        {
            using (Packet _packet = new Packet((int)Packets.Movement))
            {
                _packet.Write(pos);
                _packet.Write(rot);
                _packet.Write(speed);
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
                Networking.SendTCPData(_packet);
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

        public static void SendGameModeSwitch(bool state)
        {
            using (Packet _packet = new Packet((int)Packets.GameMode))
            {
                _packet.Write(state);
                Networking.SendTCPData(_packet);
            }
        }

        public static void RequestTime()
        {
            using (Packet _packet = new Packet((int)Packets.TimeRequest))
            {
                Networking.SendTCPData(_packet);
            }
        }
    }
}
