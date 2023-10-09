﻿using Il2CppInterop.Runtime.Injection;
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
        public const string ModVersion = "0.1.0";

        public override void OnInitializeMelon()
        {
            ClassInjector.RegisterTypeInIl2Cpp<Main>();
            ClassInjector.RegisterTypeInIl2Cpp<UI>();
            ClassInjector.RegisterTypeInIl2Cpp<SteamLobby>();
            ClassInjector.RegisterTypeInIl2Cpp<ReadData>();
            ClassInjector.RegisterTypeInIl2Cpp<Movement>();
            ClassInjector.RegisterTypeInIl2Cpp<Animations>();
            ClassInjector.RegisterTypeInIl2Cpp<Vacpack>();
            ClassInjector.RegisterTypeInIl2Cpp<Beatrix>();
            ClassInjector.RegisterTypeInIl2Cpp<CustomLobby>();
            ClassInjector.RegisterTypeInIl2Cpp<Client>();
            ClassInjector.RegisterTypeInIl2Cpp<ThreadManager>();
        }

        public override void OnLateInitializeMelon()
        {
            CreateSR2MP();
        }

        private void CreateSR2MP()
        {
            var SR2MP = new GameObject("SR2MP");
            SR2MP.AddComponent<Main>();
            SR2MP.AddComponent<UI>();
            DontDestroyOnLoad(SR2MP);
        }
    }
}
