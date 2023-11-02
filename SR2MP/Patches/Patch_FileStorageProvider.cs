using HarmonyLib;
using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher;
using Il2CppSystem.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR2MP.Patches
{
    [HarmonyPatch(typeof(FileStorageProvider), nameof(FileStorageProvider.GetGameData), MethodType.Normal)]
    class FileStorageProvider_GetGameData
    {
        public static bool HandleSave;
        public static bool Prefix(FileStorageProvider __instance, MemoryStream dataStream)
        {
            if (HandleSave)
            {
                HandleSave = false;
                return false;
            }
            return true;
        }
    }
}
