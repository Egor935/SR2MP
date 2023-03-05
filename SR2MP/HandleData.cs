using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Persist;
using Il2CppSystem.IO;
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
        public static void ConnectionReceived(Packet _packet)
        {
            var inGame = _packet.ReadBool();

            SteamLobby.secondPlayerConnected = true;
            SteamLobby.tryToConnect = false;
            SteamLobby.friendInGame = inGame;
        }

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

        public static void HandleTime(Packet _packet)
        {
            SRSingleton<SceneContext>.Instance.TimeDirector.worldModel.worldTime = _packet.ReadDouble();
        }

        public static void HandleCameraAngle(Packet _packet)
        {
            Beatrix.instance._Vacpack._CameraAngle = _packet.ReadFloat();
        }

        public static void HandleVacconeState(Packet _packet)
        {
            Beatrix.instance._Vacpack.vacMode = _packet.ReadBool();
        }

        public static void SaveRequested(Packet _packet)
        {
            MemoryStream memoryStream = new MemoryStream();
            {
                var _ASD = SRSingleton<GameContext>.Instance.AutoSaveDirector;
                _ASD.SavedGame.Save(memoryStream);
                memoryStream.Seek(0L, SeekOrigin.Begin);
            }

            SendData.SendSave(memoryStream);
        }

        public static void HandleSave(Packet _packet)
        {
            var length = _packet.ReadInt();
            var array = _packet.ReadBytes(length);

            MemoryStream save = new MemoryStream(array);
            save.Seek(0L, SeekOrigin.Begin);

            Main.Instance.publicStream = save;

            GameData.Summary saveToContinue = SRSingleton<GameContext>.Instance.AutoSaveDirector.GetSaveToContinue();
            SRSingleton<GameContext>.Instance.AutoSaveDirector.BeginLoad(saveToContinue.name, saveToContinue.saveName, null);
        }

        public static void HandleGameModeSwitch(Packet _packet)
        {
            SteamLobby.friendInGame = _packet.ReadBool();
        }

        public static void TimeRequested(Packet _packet)
        {
            var time = SRSingleton<SceneContext>.Instance.TimeDirector.worldModel.worldTime;
            SendData.SendTime(time);
        }
    }
}
