using HarmonyLib;
using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP.Patches
{
    [HarmonyPatch(typeof(LandPlotLocation), nameof(LandPlotLocation.Replace), MethodType.Normal)]
    class LandPlotLocation_Replace
    {
        public static void Postfix(LandPlotLocation __instance, GameObject replacementPrefab)
        {
            if (!GlobalStuff.HandlePacket)
            {
                var id = __instance.Id;
                var type = (int)replacementPrefab.GetComponent<LandPlot>().typeId;
                SendData.SendLandPlotReplace(id, type);
            }
        }
    }
}
