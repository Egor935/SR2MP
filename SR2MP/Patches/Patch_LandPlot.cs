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
        public static void Postfix(LandPlot __instance, LandPlot.Upgrade upgrade)
        {
            if (!Statics.HandlePacket)
            {
                var id = __instance.transform.parent.GetComponent<LandPlotLocation>().Id;
                SendData.SendLandPlotUpgrade(id, (int)upgrade);
            }
        }
    }
}
