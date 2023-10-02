using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppSystem.IO;
using MelonLoader;
using SR2MP.Patches;
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
            GlobalStuff.FriendInGame = _packet.ReadBool();
        }

        public static void HandleTime(Packet _packet)
        {
            var time = _packet.ReadDouble();

            if (SRSingleton<SceneContext>.Instance != null)
            {
                if (SRSingleton<SceneContext>.Instance.TimeDirector != null)
                {
                    SRSingleton<SceneContext>.Instance.TimeDirector.worldModel.worldTime = time;
                }
            }
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

            SavedGame_Load.SaveStream = save;

            GameData.Summary saveToContinue = SRSingleton<GameContext>.Instance.AutoSaveDirector.GetSaveToContinue();
            SRSingleton<GameContext>.Instance.AutoSaveDirector.BeginLoad(saveToContinue.name, saveToContinue.saveName, null);
        }

        public static void HandleLandPlotUpgrade(Packet _packet)
        {
            var id = _packet.ReadInt();
            var upgrade = _packet.ReadInt();

            LandPlot_AddUpgrade.HandlePacket = true;
            var landplot = GameObject.Find($"landPlot ({id})");
            landplot.GetComponentInChildren<LandPlot>().AddUpgrade((LandPlot.Upgrade)upgrade);
        }

        //public static void HandleLandPlotReplace(Packet _packet)
        //{
        //    var id = _packet.ReadInt();
        //    var type = _packet.ReadInt();
        //
        //    LandPlotLocation_Replace.HandlePacket = true;
        //    var landplot = GameObject.Find($"landPlot ({id})");
        //    var landplotLocation = landplot.GetComponent<LandPlotLocation>();
        //    var replacementPrefab = SRSingleton<GameContext>.Instance.LookupDirector.GetPlotPrefab((LandPlot.Id)type);
        //    landplotLocation.Replace(landplot.GetComponentInChildren<LandPlot>(), replacementPrefab);
        //}

        public static void HandleLandPlotReplace(Packet _packet)
        {
            var id = _packet.ReadString();
            var type = _packet.ReadInt();

            if (SRSingleton<SceneContext>.Instance.GameModel.AllLandPlots().TryGetValue(id, out LandPlotModel model))
            {
                if (model.gameObj != null)
                {
                    model.InstantiatePlot(SRSingleton<GameContext>.Instance.LookupDirector.GetPlotPrefab((LandPlot.Id)type), false);
                    model.Init();

                    //model.gameObj.GetComponentInChildren<LandPlot>(true)?.Awake();
                    //model.gameObj.GetComponentInChildren<GardenCatcher>(true)?.Awake();
                    //model.gameObj.GetComponentInChildren<SiloStorage>(true)?.Awake();

                    //model.NotifyParticipants();
                }
            }
        }
    }
}
