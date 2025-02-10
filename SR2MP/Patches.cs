using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    //[HarmonyPatch(typeof(AutoSaveDirector), nameof(AutoSaveDirector.SaveGame))]
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
                var name = __instance.GetComponentInParent<LandPlotLocation>().name;
                SendData.SendLandPlotUpgrade(name, (int)upgrade);
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
                var type = (int)replacementPrefab.GetComponent<LandPlot>().TypeId;
                SendData.SendLandPlotReplace(__instance.name, type);
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

    [HarmonyPatch(typeof(EconomyDirector), nameof(EconomyDirector.ResetPrices))]
    class EconomyDirector_ResetPrices
    {
        public static float[] ReceivedPrices;
        public static void Postfix(EconomyDirector __instance)
        {
            if (SteamLobby.Host)
            {
                SendData.SendPrices(__instance._currValueMap);
            }
            else
            {
                int index = 0;
                foreach (var price in __instance._currValueMap)
                {
                    price.value.CurrValue = ReceivedPrices[index];
                    index++;
                }
            }
        }
    }

    [HarmonyPatch(typeof(TechUIInteractable), nameof(TechUIInteractable.OnInteract))]
    class TechUIInteractable_OnInteract
    {
        public static void Postfix(TechUIInteractable __instance)
        {
            if (!Networking.HandlePacket)
            {
                SendData.SendMapOpen(__instance.name);
            }
        }
    }

    [HarmonyPatch(typeof(GordoEat), nameof(GordoEat.SetEatenCount))]
    class GordoEat_SetEatenCount
    {
        public static void Postfix(GordoEat __instance, int eatenCount)
        {
            if (!Networking.HandlePacket)
            {
                SendData.SendGordoEat(__instance.name, eatenCount);
            }
        }
    }
}
