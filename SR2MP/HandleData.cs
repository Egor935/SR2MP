using Il2CppMonomiPark.SlimeRancher.DataModel;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Object;

namespace SR2MP
{
    public class HandleData
    {
        public static bool _TimeSynced;

        public static void WelcomeReceived(Packet _packet)
        {
            var msg = _packet.ReadString();
            MelonLogger.Msg(msg);
        }

        public static void MovementReceived(Packet _packet)
        {
            var movement = Beatrix.instance._Movement;
            movement._Position = _packet.ReadVector3();
            movement._Rotation = _packet.ReadFloat();
            movement._Speed = _packet.ReadVector3();

            if (!_TimeSynced)
            {
                double time = FindObjectOfType<TimeDirector>().worldModel.worldTime;
                SendData.SendTime(time);
                _TimeSynced = true;
            }
        }

        public static void AnimationsReceived(Packet _packet)
        {
            var animations = Beatrix.instance._Animations;
            animations._HM = _packet.ReadFloat();
            animations._FM = _packet.ReadFloat();
            animations._Yaw = _packet.ReadFloat();
            animations._AS = _packet.ReadInt();
            animations._Moving = _packet.ReadBool();
            animations._HS = _packet.ReadFloat();
            animations._FS = _packet.ReadFloat();
        }

        public static void TimeReceived(Packet _packet)
        {
            var time = _packet.ReadDouble();
            var worldModel = FindObjectOfType<TimeDirector>().worldModel;
            if (time > worldModel.worldTime)
            {
                worldModel.worldTime = time;
            }
            _TimeSynced = true;
        }
    }
}
