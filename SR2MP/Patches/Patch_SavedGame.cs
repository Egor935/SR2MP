using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppSystem.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR2MP.Patches
{
    [HarmonyPatch(typeof(SavedGame), nameof(SavedGame.Load), MethodType.Normal)]
    class SavedGame_Load
    {
        public static MemoryStream SaveStream;
        public static void Prefix(ref Stream stream)
        {
            if (Statics.JoinedTheGame)
            {
                stream = SaveStream;
            }
        }
    }
}
