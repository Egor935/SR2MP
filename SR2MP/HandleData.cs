using Il2Cpp;
using Il2CppSystem.IO;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public static class HandleData
    {
        public static void HandleMessage(Packet _packet)
        {
            var msg = _packet.ReadString();
            MelonLogger.Msg(msg);
        }

        public static void HandleConnection(Packet _packet)
        {
            var inGame = _packet.ReadBool();

            SteamLobby.Instance.SecondPlayerConnected = true;
            SteamLobby.Instance.TryToConnect = false;
            SteamLobby.Instance.FriendInGame = inGame;
        }

        public static void HandleMovement(Packet _packet)
        {
            Beatrix.Instance.BeatrixMovement.ReceivedPosition = _packet.ReadVector3();
            Beatrix.Instance.BeatrixMovement.ReceivedRotation = _packet.ReadFloat();
        }

        public static void HandleAnimations(Packet _packet)
        {
            Beatrix.Instance.BeatrixAnimations.HM = _packet.ReadFloat();
            Beatrix.Instance.BeatrixAnimations.FM = _packet.ReadFloat();
            Beatrix.Instance.BeatrixAnimations.Yaw = _packet.ReadFloat();
            Beatrix.Instance.BeatrixAnimations.AS = _packet.ReadInt();
            Beatrix.Instance.BeatrixAnimations.Moving = _packet.ReadBool();
            Beatrix.Instance.BeatrixAnimations.HS = _packet.ReadFloat();
            Beatrix.Instance.BeatrixAnimations.FS = _packet.ReadFloat();
        }

        public static void HandleCameraAngle(Packet _packet)
        {
            Beatrix.Instance.BeatrixVacpack.ReceivedCameraAngle = _packet.ReadFloat();
        }

        public static void HandleVacconeState(Packet _packet)
        {
            Beatrix.Instance.BeatrixVacpack.VacMode = _packet.ReadBool();
        }

        public static void HandleGameModeSwitch(Packet _packet)
        {
            SteamLobby.Instance.FriendInGame = _packet.ReadBool();
        }

        public static void TimeRequested(Packet _packet)
        {
            var time = SRSingleton<SceneContext>.Instance.TimeDirector.worldModel.worldTime;
            SendData.SendTime(time);
        }

        public static void HandleTime(Packet _packet)
        {
            SRSingleton<SceneContext>.Instance.TimeDirector.worldModel.worldTime = _packet.ReadDouble();
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

            Main.Instance.FindActors = true;
        }

        public static void HandleSave(Packet _packet)
        {
            var length = _packet.ReadInt();
            var array = _packet.ReadBytes(length);

            MemoryStream save = new MemoryStream(array);
            save.Seek(0L, SeekOrigin.Begin);

            Main.Instance.PublicStream = save;

            GameData.Summary saveToContinue = SRSingleton<GameContext>.Instance.AutoSaveDirector.GetSaveToContinue();
            SRSingleton<GameContext>.Instance.AutoSaveDirector.BeginLoad(saveToContinue.name, saveToContinue.saveName, null);

            Main.Instance.FindActors = true;
        }

        public static void HandleSlimes(Packet _packet)
        {
            if (Main.Instance.SyncActors)
            {
                var count = _packet.ReadInt();
                for (int i = 0; i < count; i++)
                {
                    var pos = _packet.ReadVector3();
                    var rot = _packet.ReadQuaternion();

                    var syncingSlime = Main.Instance.SyncingSlimes[i];
                    if (syncingSlime != null)
                    {
                        syncingSlime.transform.position = pos;
                        syncingSlime.transform.rotation = rot;
                    }
                }
            }
        }
    }
}
