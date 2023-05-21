using HarmonyLib;
using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppSystem.IO;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.Object;

namespace SR2MP
{
    [HarmonyPatch(typeof(SavedGame), nameof(SavedGame.Load), MethodType.Normal)]
    class SavedGame_Load
    {
        public static void Prefix(ref Stream stream)
        {
            if (SteamLobby.Instance.JoinedTheGame)
            {
                stream = Main.Instance.PublicStream;
            }
        }
    }

    [HarmonyPatch(typeof(AutoSaveDirector), nameof(AutoSaveDirector.SaveGame), MethodType.Normal)]
    class AutoSaveDirector_SaveGame
    {
        public static bool Prefix()
        {
            if (SteamLobby.Instance.JoinedTheGame)
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
