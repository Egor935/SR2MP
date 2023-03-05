using Il2CppMonomiPark.SlimeRancher.Platform.Steam;
using Il2CppMonomiPark.SlimeRancher.SceneManagement;
using Il2CppMonomiPark.SlimeRancher.UI;
using Il2CppSystem.Reflection;
using MelonLoader;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace SR2MP
{
    public class SteamLobby : MonoBehaviour
    {
        public SteamLobby(IntPtr ptr) : base(ptr) { }

        Callback<LobbyCreated_t> lobbyCreated;
        Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
        Callback<LobbyEnter_t> lobbyEntered;

        public static CSteamID LobbyId { get; private set; }

        public static CSteamID receiver;

        public static bool tryToConnect = true;

        public static bool secondPlayerConnected;

        public static bool friendInGame;

        public void Start()
        {
            //mySteamID = SteamUser.GetSteamID();

            //lobbyCreated = Callback<LobbyCreated_t>.Create((Callback<LobbyCreated_t>.DispatchDelegate)OnLobbyCreated);
            //gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create((Callback<GameLobbyJoinRequested_t>.DispatchDelegate)OnGameLobbyJoinRequested);
            //lobbyEntered = Callback<LobbyEnter_t>.Create((Callback<LobbyEnter_t>.DispatchDelegate)OnLobbyEntered);

            Networking.InitializeClientData();
        }

        // Update is called once per frame
        public void Update()
        {
            Networking.ListenData();
        }

        public void FixedUpdate()
        {
            ReadGameMode();
            if (_GameMode != _GameModeCached)
            {
                SendData.SendGameModeSwitch(_GameMode);
                _GameModeCached = _GameMode;
            }

            if (tryToConnect)
            {
                if (receiver != CSteamID.Nil)
                {
                    var inGame = SRSingleton<SystemContext>.Instance.SceneLoader.CurrentSceneGroup.isGameplay;
                    SendData.SendConnection(inGame);
                }
            }
        }

        public static void CreateLobby()
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 2);
        }

        public static void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                return;
            }

            MelonLogger.Msg("Lobby created successefully");

            LobbyId = new CSteamID(callback.m_ulSteamIDLobby);
        }

        public void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            MelonLogger.Msg("Request to join lobby");
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        public void OnLobbyEntered(LobbyEnter_t callback)
        {
            MelonLogger.Msg("You have successefully joined the lobby");
            LobbyId = new CSteamID(callback.m_ulSteamIDLobby);
        }

        bool _GameMode;
        bool _GameModeCached;
        void ReadGameMode()
        {
            var _SystemContext = SRSingleton<SystemContext>.Instance;
            if (_SystemContext != null)
            {
                _GameMode = _SystemContext.SceneLoader.CurrentSceneGroup.isGameplay;
            }
        }
    }
}
