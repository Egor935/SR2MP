using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Regions;
using Il2CppMonomiPark.SlimeRancher.SceneManagement;
using Il2CppMonomiPark.SlimeRancher.UI;
using Il2CppSystem.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    [HarmonyPatch(typeof(AutoSaveDirector), nameof(AutoSaveDirector.SaveGame))]
    class AutoSaveDirector_SaveGame
    {
        public static bool Prefix()
        {
            return !SteamLobby.Joined;
        }
    }

    [HarmonyPatch(typeof(LandPlot), nameof(LandPlot.AddUpgrade))]
    class LandPlot_AddUpgrade
    {
        public static void Postfix(LandPlot __instance, LandPlot.Upgrade upgrade)
        {
            if (!Networking.HandlePacket)
            {
                var id = __instance.transform.parent.GetComponent<LandPlotLocation>().Id;
                SendData.SendLandPlotUpgrade(id, (int)upgrade);
            }
        }
    }

    [HarmonyPatch(typeof(LandPlotLocation), nameof(LandPlotLocation.Replace))]
    class LandPlotLocation_Replace
    {
        public static void Postfix(LandPlotLocation __instance, GameObject replacementPrefab)
        {
            if (!Networking.HandlePacket)
            {
                var id = __instance.Id;
                var type = (int)replacementPrefab.GetComponent<LandPlot>().TypeId;
                SendData.SendLandPlotReplace(id, type);
            }
        }
    }

    [HarmonyPatch(typeof(RanchHouseUI), nameof(RanchHouseUI.SleepUntil))]
    class RanchHouseUI_SleepUntil
    {
        public static void Postfix(double endTime)
        {
            SendData.SendSleep(endTime);
        }
    }
}
