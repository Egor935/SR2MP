using Il2CppMonomiPark.SlimeRancher.Platform.Steam;
using Il2CppMonomiPark.SlimeRancher.SceneManagement;
using MelonLoader;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static CSteamID mySteamID;

        public static int myId;
        public static int ping;
        public static bool getSecondPlayer = true;

        public static bool tryToRequestData = true;
        public static bool requestedDataSent;

        public void Start()
        {
            mySteamID = SteamUser.GetSteamID();
            //lobbyCreated = Callback<LobbyCreated_t>.Create(new Callback<LobbyCreated_t>.DispatchDelegate(OnLobbyCreated));
            //gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(new Callback<GameLobbyJoinRequested_t>.DispatchDelegate(OnGameLobbyJoinRequested));
            //lobbyEntered = Callback<LobbyEnter_t>.Create(new Callback<LobbyEnter_t>.DispatchDelegate(OnLobbyEntered));

            Networking.InitializeClientData();
        }

        // Update is called once per frame
        public void Update()
        {
            Networking.ListenData();

            /*
            if (getSecondPlayer)
            {
                if (myId == 0)
                {
                    var secondPlayer = SteamMatchmaking.GetLobbyMemberByIndex(LobbyId, 1);
                    if (secondPlayer != CSteamID.Nil)
                    {
                        receiver = secondPlayer;
                        SendData.SendWelcome("Welcome to the server");
                        getSecondPlayer = false;
                    }
                }
                else
                {
                    getSecondPlayer = false;
                }
            }
            */
        }

        public void FixedUpdate()
        {
            if (tryToRequestData)
            {
                if (receiver != CSteamID.Nil)
                {
                    if (receiver.m_SteamID > mySteamID.m_SteamID)
                    {
                        if (SRSingleton<SystemContext>.Instance.SceneLoader.currentSceneGroup.isGameplay)
                        {
                            SendData.RequestData();
                        }
                    }
                    else
                    {
                        tryToRequestData = false;
                    }
                }
            }
        }

        public static void CreateLobby()
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 2);
        }

        public void OnLobbyCreated(LobbyCreated_t callback)
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

            if (SteamMatchmaking.GetLobbyMemberByIndex(LobbyId, 0) == SteamUser.GetSteamID())
            {
                myId = 0;
            }
            else
            {
                myId = 1;
                receiver = SteamMatchmaking.GetLobbyMemberByIndex(LobbyId, 0);
                //SendData.SendWelcome($"Player {SteamFriends.GetPersonaName()} successefully connected");
            }
        }
    }
}
