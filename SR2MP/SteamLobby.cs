using Il2Cpp;
using Il2CppSony.NP;
using MelonLoader;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SR2MP
{
    public class SteamLobby : MonoBehaviour
    {
        public SteamLobby(IntPtr ptr) : base(ptr) { }

        #region Variables
        public static SteamLobby Instance;

        //Callbacks
        Callback<LobbyCreated_t> lobbyCreated;
        Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
        Callback<LobbyEnter_t> lobbyEntered;

        //Steam
        public CSteamID Lobby;
        public CSteamID Receiver;

        //Stuff
        private bool allowCreateLobby = true;
        private bool getSecondPlayer;
        #endregion

        public void SteamMenu()
        {
            if (allowCreateLobby)
            {
                if (GUI.Button(new Rect(15f, 35f, 150f, 25f), "Create lobby"))
                {
                    CreateLobby();
                    GlobalStuff.Host = true;
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
            GUI.Label(new Rect(15f, 125f, 150f, 25f), GlobalStuff.SecondPlayerName);

            if (Receiver != CSteamID.Nil)
            {
                string inGame = GlobalStuff.FriendInGame ? "<color=green>YES</color>" : "<color=red>NO</color>";
                GUI.Label(new Rect(15f, 155f, 150f, 25f), $"In game: {inGame}");

                if (!GlobalStuff.JoinedTheGame)
                {
                    if (GlobalStuff.FriendInGame)
                    {
                        if (!SRSingleton<SystemContext>.Instance.SceneLoader.CurrentSceneGroup.isGameplay)
                        {
                            if (GUI.Button(new Rect(40f, 185f, 100f, 25f), "Join"))
                            {
                                GlobalStuff.JoinedTheGame = true;
                                SendData.RequestSave();
                            }
                        }
                    }
                }
            }
        }

        void Start()
        {
            Instance = this;

            lobbyCreated = Callback<LobbyCreated_t>.Create(new Callback<LobbyCreated_t>.DispatchDelegate(OnLobbyCreated));
            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(new Callback<GameLobbyJoinRequested_t>.DispatchDelegate(OnGameLobbyJoinRequested));
            lobbyEntered = Callback<LobbyEnter_t>.Create(new Callback<LobbyEnter_t>.DispatchDelegate(OnLobbyEntered));

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
                    GlobalStuff.SecondPlayerName = SteamFriends.GetFriendPersonaName(secondPlayer);
                    SendData.SendMessage("Welcome to the lobby!");
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

            MelonLogger.Msg("Lobby created successefully");

            Lobby = new CSteamID(callback.m_ulSteamIDLobby);
        }

        public void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            MelonLogger.Msg("Request to join lobby");
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        public void OnLobbyEntered(LobbyEnter_t callback)
        {
            MelonLogger.Msg("You have successfully joined the lobby");

            var lobbyId = new CSteamID(callback.m_ulSteamIDLobby);

            int members = SteamMatchmaking.GetNumLobbyMembers(lobbyId);
            for (int i = 0; i < members; i++)
            {
                CSteamID member = SteamMatchmaking.GetLobbyMemberByIndex(lobbyId, i);
                if (member != SteamUser.GetSteamID())
                {
                    Receiver = member;
                    GlobalStuff.SecondPlayerName = SteamFriends.GetFriendPersonaName(member);
                    SendData.SendMessage($"Player {SteamFriends.GetPersonaName()} successefully connected!");
                    GlobalStuff.Host = false;
                    allowCreateLobby = false;
                }
            }
        }
    }
}
