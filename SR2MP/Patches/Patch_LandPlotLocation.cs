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
        public static bool HandlePacket;
        public static void Postfix(LandPlotLocation __instance, GameObject replacementPrefab)
        {
            if (!HandlePacket)
            {
                //var id = int.Parse(__instance.transform.parent.name.Replace("landPlot (", null).Replace(")", null));
                var id = __instance.id;
                var type = (int)replacementPrefab.GetComponent<LandPlot>().typeId;

                SendData.SendLandPlotReplace(id, type);
            }
            else
            {
                HandlePacket = false;
            }
        }
    }
}
