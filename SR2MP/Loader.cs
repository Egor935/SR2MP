using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Object;

namespace SR2MP
{
    public class Loader : MelonMod
    {
        public const string ModVersion = "0.0.8";

        public override void OnInitializeMelon()
        {
            ClassInjector.RegisterTypeInIl2Cpp<Main>();
            ClassInjector.RegisterTypeInIl2Cpp<SteamLobby>();
            ClassInjector.RegisterTypeInIl2Cpp<ReadData>();
            ClassInjector.RegisterTypeInIl2Cpp<Movement>();
            ClassInjector.RegisterTypeInIl2Cpp<Animations>();
            ClassInjector.RegisterTypeInIl2Cpp<Vacpack>();
            ClassInjector.RegisterTypeInIl2Cpp<Beatrix>();
        }

        public override void OnLateInitializeMelon()
        {
            var SR2MP = new GameObject("SR2MP");
            SR2MP.AddComponent<Main>();
            SR2MP.AddComponent<SteamLobby>();
            DontDestroyOnLoad(SR2MP);
        }
    }
}
