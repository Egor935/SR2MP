using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Player;
using Il2CppMonomiPark.SlimeRancher.SceneManagement;
using Il2CppMonomiPark.SlimeRancher.UI;
using Il2CppMonomiPark.SlimeRancher.UI.MainMenu;
using Il2CppSystem.IO;
using MelonLoader;
using SR2MP.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
            Statics.FriendInGame = _packet.ReadBool();
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
            SRSingleton<GameContext>.Instance.AutoSaveDirector.SaveGame();

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
            FileStorageProvider_GetGameData.HandleSave = true;
            SRSingleton<GameContext>.Instance.AutoSaveDirector.BeginLoad(null, null, null);
        }

        public static void HandleLandPlotUpgrade(Packet _packet)
        {
            var id = _packet.ReadString();
            var upgrade = _packet.ReadInt();

            if (SRSingleton<SceneContext>.Instance != null)
            {
                if (SRSingleton<SceneContext>.Instance.GameModel.landPlots.TryGetValue(id, out LandPlotModel model))
                {
                    if (model.gameObj != null)
                    {
                        var landPlot = model.gameObj.GetComponentInChildren<LandPlot>();
                        landPlot.AddUpgrade((LandPlot.Upgrade)upgrade);
                    }
                }
            }
        }

        public static void HandleLandPlotReplace(Packet _packet)
        {
            var id = _packet.ReadString();
            var type = _packet.ReadInt();

            if (SRSingleton<SceneContext>.Instance != null)
            {
                if (SRSingleton<SceneContext>.Instance.GameModel.landPlots.TryGetValue(id, out LandPlotModel model))
                {
                    if (model.gameObj != null)
                    {
                        var landPlotLocation = model.gameObj.GetComponentInChildren<LandPlotLocation>();
                        var oldLandPlot = model.gameObj.GetComponentInChildren<LandPlot>();
                        var replacementPrefab = SRSingleton<GameContext>.Instance.LookupDirector.GetPlotPrefab((LandPlot.Id)type);
                        landPlotLocation.Replace(oldLandPlot, replacementPrefab);
                    }
                }
            }
        }

        public static void HandleSleep(Packet _packet)
        {
            var endTime = _packet.ReadDouble();

            if (SRSingleton<LockOnDeath>.Instance != null)
            {
                SRSingleton<LockOnDeath>.Instance.LockUntil(endTime, 0f);
            }
        }

        public static void HandleCurrency(Packet _packet)
        {
            var value = _packet.ReadInt();

            if (SRSingleton<SceneContext>.Instance != null)
            {
                int difference = value - SRSingleton<SceneContext>.Instance.PlayerState.model.currency;
                SRSingleton<SceneContext>.Instance.PlayerState.model.currency = value;
                SRSingleton<PopupElementsUI>.Instance.CreateCoinsPopup(difference, PlayerState.CoinsType.NORM);
            }
        }
    }
}
