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
    [HarmonyPatch(typeof(FileStorageProvider), nameof(FileStorageProvider.GetGameData), MethodType.Normal)]
    class FileStorageProvider_GetGameData
    {
        public static MemoryStream ReceivedSave;
        public static bool Prefix(FileStorageProvider __instance, MemoryStream dataStream)
        {
            if (ReceivedSave != null)
            {
                __instance.CopyStream(ReceivedSave, dataStream);
                ReceivedSave = null;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(AutoSaveDirector), nameof(AutoSaveDirector.SaveGame), MethodType.Normal)]
    class AutoSaveDirector_SaveGame
    {
        public static bool Prefix()
        {
            return !Main.Joined;
        }
    }

    [HarmonyPatch(typeof(LandPlot), nameof(LandPlot.AddUpgrade), MethodType.Normal)]
    class LandPlot_AddUpgrade
    {
        public static void Postfix(LandPlot __instance, LandPlot.Upgrade upgrade)
        {
            if (!Main.HandlePacket)
            {
                var id = __instance.transform.parent.GetComponent<LandPlotLocation>().Id;
                SendData.SendLandPlotUpgrade(id, (int)upgrade);
            }
        }
    }

    [HarmonyPatch(typeof(LandPlotLocation), nameof(LandPlotLocation.Replace), MethodType.Normal)]
    class LandPlotLocation_Replace
    {
        public static void Postfix(LandPlotLocation __instance, GameObject replacementPrefab)
        {
            if (!Main.HandlePacket)
            {
                var id = __instance.Id;
                var type = (int)replacementPrefab.GetComponent<LandPlot>().TypeId;
                SendData.SendLandPlotReplace(id, type);
            }
        }
    }

    [HarmonyPatch(typeof(PlayerState), nameof(PlayerState.AddCurrency), MethodType.Normal)]
    class PlayerState_AddCurrency
    {
        public static void Postfix(PlayerState __instance)
        {
            var currency = __instance.GetCurrency();
            SendData.SendCurrency(currency);
        }
    }

    [HarmonyPatch(typeof(PlayerState), nameof(PlayerState.SpendCurrency), MethodType.Normal)]
    class PlayerState_SpendCurrency
    {
        public static void Postfix(PlayerState __instance)
        {
            var currency = __instance.GetCurrency();
            SendData.SendCurrency(currency);
        }
    }

    [HarmonyPatch(typeof(RanchHouseUI), nameof(RanchHouseUI.SleepUntil), MethodType.Normal)]
    class RanchHouseUI_SleepUntil
    {
        public static void Postfix(double endTime)
        {
            SendData.SendSleep(endTime);
        }
    }
}

//Patch by KomiksPL
namespace UnstrippedClasses
{
    [HarmonyPatch(typeof(GUIStateObjects))]
    internal static class Patch_GUIStateObjects
    {
        private static Dictionary<int, Il2CppSystem.Object> s_StateCache = new Dictionary<int, Il2CppSystem.Object>();

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GUIStateObjects.QueryStateObject))]
        private static bool QueryStateObject(Il2CppSystem.Type t, int controlID, ref Il2CppSystem.Object __result)
        {
            Il2CppSystem.Object o = s_StateCache[controlID];
            __result = t.IsInstanceOfType(o) ? o : null;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GUIStateObjects.GetStateObject))]
        private static bool GetStateObject(Il2CppSystem.Type t, int controlID, ref Il2CppSystem.Object __result)
        {
            if (!s_StateCache.TryGetValue(controlID, out var instance) || instance.GetIl2CppType() != t)
            {
                instance = Il2CppSystem.Activator.CreateInstance(t);
                s_StateCache[controlID] = instance;
            }
            __result = instance;
            return false;
        }
    }
}
