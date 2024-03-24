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
        public static SteamLobby Instance;

        //Callbacks
        Callback<LobbyCreated_t> lobbyCreated;
        Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
        Callback<LobbyEnter_t> lobbyEntered;

        //Steam
        public CSteamID Lobby;
        public CSteamID Receiver = CSteamID.Nil;

        //Stuff
        private bool allowCreateLobby = true;
        private bool getSecondPlayer;
        private string secondPlayerName = "None";

        public static bool Host = true;

        public void SteamMenu()
        {
            if (allowCreateLobby)
            {
                if (GUI.Button(new Rect(15f, 35f, 150f, 25f), "Create lobby"))
                {
                    CreateLobby();
                    Host = true;
                    allowCreateLobby = false;
                }

                GUI.Label(new Rect(15f, 65f, 150f, 25f), "Invite friend");
            }
            else
            {
                GUI.Label(new Rect(15f, 35f, 150f, 25f), "Create lobby");

                if (getSecondPlayer)
                {
                    if (GUI.Button(new Rect(15f, 65f, 150f, 25f), "Invite friend"))
                    {
                        SteamFriends.ActivateGameOverlayInviteDialog(Lobby);
                    }
                }
                else
                {
                    GUI.Label(new Rect(15f, 65f, 150f, 25f), "Invite friend");
                }
            }

            GUI.Label(new Rect(15f, 95f, 150f, 25f), "Connected friend:");
            GUI.Label(new Rect(15f, 125f, 150f, 25f), secondPlayerName);

            if (Receiver != CSteamID.Nil)
            {
                string _FriendInGame = Main.FriendInGame ? "<color=green>YES</color>" : "<color=red>NO</color>";
                GUI.Label(new Rect(15f, 155f, 150f, 25f), $"In game: {_FriendInGame}");

                if (!Main.Joined)
                {
                    if (Main.FriendInGame)
                    {
                        if (!SRSingleton<SystemContext>.Instance.SceneLoader.CurrentSceneGroup.IsGameplay)
                        {
                            if (GUI.Button(new Rect(40f, 185f, 100f, 25f), "Join"))
                            {
                                Main.Joined = true;
                                SendData.RequestSaveData();
                            }
                        }
                    }
                }
            }
        }

        void Start()
        {
            Instance = this;

            //Create callbacks
            lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

            Networking.InitializePackets();
        }


        void Update()
        {
            Networking.ListenData();
            SteamAPI.RunCallbacks();

            if (getSecondPlayer)
            {
                var secondPlayer = SteamMatchmaking.GetLobbyMemberByIndex(Lobby, 1);
                if (secondPlayer != CSteamID.Nil)
                {
                    Receiver = secondPlayer;
                    secondPlayerName = SteamFriends.GetFriendPersonaName(secondPlayer);
                    SendData.SendMessage("Welcome to the lobby");
                    getSecondPlayer = false;
                }
            }
        }


        public void CreateLobby()
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 2);
            getSecondPlayer = true;
        }

        public void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                return;
            }

            Lobby = new CSteamID(callback.m_ulSteamIDLobby);

            Debug.Log("Lobby created successfully");
        }

        public void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            Debug.Log("Request to join lobby");
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        public void OnLobbyEntered(LobbyEnter_t callback)
        {
            Debug.Log("You have successfully joined the lobby");

            var lobbyId = new CSteamID(callback.m_ulSteamIDLobby);

            int members = SteamMatchmaking.GetNumLobbyMembers(lobbyId);
            for (int i = 0; i < members; i++)
            {
                CSteamID member = SteamMatchmaking.GetLobbyMemberByIndex(lobbyId, i);
                if (member != SteamUser.GetSteamID())
                {
                    Receiver = member;
                    secondPlayerName = SteamFriends.GetFriendPersonaName(member);
                    SendData.SendMessage($"Player {SteamFriends.GetPersonaName()} successefully connected");
                    allowCreateLobby = false;
                    Host = false;
                }
            }
        }
    }
}
