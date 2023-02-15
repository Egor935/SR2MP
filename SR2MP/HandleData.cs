using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Persist;
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
            FindObjectOfType<TimeDirector>().worldModel.worldTime = _packet.ReadDouble();
        }

        public static void HandleRequestedData(Packet _packet)
        {
            if (!SteamLobby.requestedDataSent)
            {
                SendData.DataRequested();

                double time = FindObjectOfType<TimeDirector>().worldModel.worldTime;
                SendData.SendTime(time);
            }
        }

        public static void HandleDataRequested(Packet _packet)
        {
            SteamLobby.tryToRequestData = false;
            MelonLogger.Msg("Data requested");
        }

        public static void HandleCameraAngle(Packet _packet)
        {
            Beatrix.instance._Vacpack._CameraAngle = _packet.ReadFloat();
        }

        public static void HandleVacconeState(Packet _packet)
        {
            Beatrix.instance._Vacpack.vacMode = _packet.ReadBool();
        }
    }
}
