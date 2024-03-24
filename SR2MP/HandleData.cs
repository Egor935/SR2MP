using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Player;
using Il2CppMonomiPark.SlimeRancher.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR2MP
{
    public class HandleData
    {
        public static void HandleMessage(Packet _packet)
        {
            var msg = _packet.ReadString();
            Console.WriteLine(msg);
        }

        public static void HandleMovement(Packet _packet)
        {
            if (NetworkPlayer.Instance != null)
            {
                NetworkPlayer.Instance.Movement.ReceivedPosition = _packet.ReadVector3();
                NetworkPlayer.Instance.Movement.ReceivedRotation = _packet.ReadFloat();
                NetworkPlayer.Instance.Movement.MovementReceived = true;
            }
        }

        public static void HandleAnimations(Packet _packet)
        {
            if (NetworkPlayer.Instance != null)
            {
                NetworkPlayer.Instance.Animations.HM = _packet.ReadFloat();
                NetworkPlayer.Instance.Animations.FM = _packet.ReadFloat();
                NetworkPlayer.Instance.Animations.Yaw = _packet.ReadFloat();
                NetworkPlayer.Instance.Animations.AS = _packet.ReadInt();
                NetworkPlayer.Instance.Animations.Moving = _packet.ReadBool();
                NetworkPlayer.Instance.Animations.HS = _packet.ReadFloat();
                NetworkPlayer.Instance.Animations.FS = _packet.ReadFloat();
                NetworkPlayer.Instance.Animations.AnimationsReceived = true;
            }
        }

        public static void HandleTime(Packet _packet)
        {
            var time = _packet.ReadDouble();

            if (SRSingleton<SceneContext>.Instance != null)
            {
                if (SRSingleton<SceneContext>.Instance.TimeDirector != null)
                {
                    SRSingleton<SceneContext>.Instance.TimeDirector._worldModel.worldTime = time;
                }
            }
        }

        public static void HandleInGame(Packet _packet)
        {
            var inGame = _packet.ReadBool();
            Main.FriendInGame = inGame;
        }

        public static void HandleSaveDataRequest(Packet _packet)
        {
            var memoryStream = new Il2CppSystem.IO.MemoryStream();
            SRSingleton<GameContext>.Instance.AutoSaveDirector.SaveGame();
            SRSingleton<GameContext>.Instance.AutoSaveDirector.SavedGame.Save(memoryStream);

            var arraySave = memoryStream.ToArray();
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gzipStream.Write(arraySave, 0, arraySave.Length);
                }
                arraySave = outputStream.ToArray();
            }

            SendData.SendSaveData(arraySave);
        }

        public static void HandleSaveData(Packet _packet)
        {
            var length = _packet.ReadInt();
            var array = _packet.ReadBytes(length);

            using (MemoryStream inputStream = new MemoryStream(array))
            {
                using (GZipStream gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                {
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        gzipStream.CopyTo(outputStream);
                        array = outputStream.ToArray();
                    }
                }
            }

            FileStorageProvider_GetGameData.ReceivedSave = new Il2CppSystem.IO.MemoryStream(array);
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

        public static void HandleCurrency(Packet _packet)
        {
            var value = _packet.ReadInt();

            if (SRSingleton<SceneContext>.Instance != null)
            {
                int difference = value - SRSingleton<SceneContext>.Instance.PlayerState._model.currency;
                SRSingleton<SceneContext>.Instance.PlayerState._model.currency = value;
                SRSingleton<PopupElementsUI>.Instance.CreateCoinsPopup(difference, PlayerState.CoinsType.NORM);
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
    }
}
