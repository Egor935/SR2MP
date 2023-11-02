using HarmonyLib;
using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR2MP.Patches
{
    [HarmonyPatch(typeof(AutoSaveDirector), nameof(AutoSaveDirector.SaveGame), MethodType.Normal)]
    class AutoSaveDirector_SaveGame
    {
        public static bool Prefix()
        {
            if (Statics.JoinedTheGame)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
