using HarmonyLib;
using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR2MP.Patches
{
    [HarmonyPatch(typeof(PlayerState), nameof(PlayerState.SpendCurrency))]
    class PlayerState_SpendCurrency
    {
        public static void Postfix(PlayerState __instance)
        {
            var currency = __instance.GetCurrency();
            SendData.SendCurrency(currency);
        }
    }

    [HarmonyPatch(typeof(PlayerState), nameof(PlayerState.AddCurrency))]
    class PlayerState_AddCurrency
    {
        public static void Postfix(PlayerState __instance)
        {
            var currency = __instance.GetCurrency();
            SendData.SendCurrency(currency);
        }
    }
}
