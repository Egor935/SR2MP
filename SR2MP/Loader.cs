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
    [BepInPlugin("Egor_ICE", "SR2MP", "0.1.3")]
    public class Loader : BasePlugin
    {
        public override void Load()
        {
            InitializeSR2MP();
            RegisterTypesInIl2Cpp();
            CreateHarmony();
        }

        private void InitializeSR2MP()
        {
            AddComponent<Main>();
            AddComponent<UI>();
            AddComponent<LobbyManager>();
        }

        private void RegisterTypesInIl2Cpp()
        {
            ClassInjector.RegisterTypeInIl2Cpp<NetworkPlayer>();
            ClassInjector.RegisterTypeInIl2Cpp<NetworkMovement>();
            ClassInjector.RegisterTypeInIl2Cpp<NetworkAnimations>();
            ClassInjector.RegisterTypeInIl2Cpp<ReadData>();

            ClassInjector.RegisterTypeInIl2Cpp<SteamLobby>();
            ClassInjector.RegisterTypeInIl2Cpp<CustomLobby>();
            ClassInjector.RegisterTypeInIl2Cpp<Client>();
            ClassInjector.RegisterTypeInIl2Cpp<ThreadManager>();
            ClassInjector.RegisterTypeInIl2Cpp<GameServer.ThreadManager>();
        }

        private void CreateHarmony()
        {
            var harmony = new Harmony("SR2MP");
            harmony.PatchAll();
        }
    }
}
