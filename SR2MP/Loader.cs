using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR2MP
{
    [BepInPlugin("Egor_ICE", "SR2MP", "0.1.4")]
    public class Loader : BasePlugin
    {
        public override void Load()
        {
            CreateHarmony();
            RegisterTypes();
            AddComponent<MultiplayerCore>();
            AddComponent<MultiplayerUI>();
        }

        private void RegisterTypes()
        {
            ClassInjector.RegisterTypeInIl2Cpp<SteamLobby>();
            ClassInjector.RegisterTypeInIl2Cpp<NetworkPlayer>();
            ClassInjector.RegisterTypeInIl2Cpp<ReadData>();
        }

        private void CreateHarmony()
        {
            var harmony = new Harmony("SR2MP");
            harmony.PatchAll();
        }
    }
}
