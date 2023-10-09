using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR2MP.Patches
{
    [HarmonyPatch(typeof(RanchHouseUI), nameof(RanchHouseUI.SleepUntil), MethodType.Normal)]
    class RanchHouseUI_SleepUntil
    {
        public static void Postfix(double endTime)
        {
            if (GlobalStuff.JoinedTheGame && !GlobalStuff.HandlePacket)
            {
                SendData.SendSleep(endTime);
            }
        }
    }
}
