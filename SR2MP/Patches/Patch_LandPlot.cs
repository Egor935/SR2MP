using HarmonyLib;
using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR2MP.Patches
{
    [HarmonyPatch(typeof(LandPlot), nameof(LandPlot.AddUpgrade), MethodType.Normal)]
    class LandPlot_AddUpgrade
    {
        public static bool HandlePacket;
        public static void Postfix(LandPlot __instance, LandPlot.Upgrade upgrade)
        {
            if (!HandlePacket)
            {
                var id = __instance.transform.parent.name.Replace("landPlot (", null).Replace(")", null);
                SendData.SendLandPlotUpgrade(int.Parse(id), (int)upgrade);
            }
            else
            {
                HandlePacket = false;
            }
        }
    }
}
