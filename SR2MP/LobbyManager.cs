using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public class LobbyManager : MonoBehaviour
    {
        public static bool SteamIsAvailable;
        public static LobbyType CurrentLobbyType;

        void Start()
        {
            SteamIsAvailable = SteamAPI.Init();
            if (!SteamIsAvailable)
            {
                CreateCustomLobby();
            }
        }

        public static void CreateSteamLobby()
        {
            var steamLobby = new GameObject("SteamLobby");
            steamLobby.AddComponent<SteamLobby>();
            DontDestroyOnLoad(steamLobby);

            CurrentLobbyType = LobbyType.Steam;
        }

        public static void CreateCustomLobby()
        {
            GameObject customLobby = new GameObject("CustomLobby");
            customLobby.AddComponent<Client>();
            customLobby.AddComponent<ThreadManager>();
            DontDestroyOnLoad(customLobby);

            CurrentLobbyType = LobbyType.Custom;
        }

        public enum LobbyType
        {
            None,
            Steam,
            Custom
        }
    }
}
