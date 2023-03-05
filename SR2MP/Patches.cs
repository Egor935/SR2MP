using HarmonyLib;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppSystem.IO;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR2MP
{
    [HarmonyPatch(typeof(SavedGame), nameof(SavedGame.Load), MethodType.Normal)]
    class SavedGame_Load
    {
        public static void Prefix(ref Stream stream)
        {
            if (Main.Instance.joinedTheGame)
            {
                stream = Main.Instance.publicStream;
                //Console.WriteLine("Patched: " + stream.Length);
            }
        }
    }

    [HarmonyPatch(typeof(AutoSaveDirector), nameof(AutoSaveDirector.SaveGame), MethodType.Normal)]
    class AutoSaveDirector_SaveGame
    {
        public static bool Prefix()
        {
            if (Main.Instance.joinedTheGame)
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
