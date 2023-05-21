using Il2Cpp;
using Il2CppSteamworks;
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

        public static SteamLobby Instance;

        //Steam
        public bool SteamIsAvailable;
        public CSteamID Receiver;

        //Stuff
        public bool TryToConnect = true;
        public bool SecondPlayerConnected;
        public bool FriendInGame;
        public bool JoinedTheGame;

        //Friends
        private int friendsCount;
        private List<CSteamID> friendsIDs = new List<CSteamID>();
        private List<string> friendsNames = new List<string>();
        private int startingPoint;
        private bool friendSelected;
        private int selectedFriend;
        private bool noFriends;

        void OnGUI()
        {
            if (Main.Instance.MenuState)
            {
                GUI.color = Color.cyan;
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;

                GUI.Box(new Rect(10f, 10f, 160f, 300f), "<b>SR2MP</b>");

                if (SteamIsAvailable)
                {
                    if (noFriends)
                    {
                        GUI.Label(new Rect(15f, 35f, 150f, 250f), "No friends found");
                        return;
                    }

                    if ((friendsCount == 0) || friendSelected)
                    {
                        if (GUI.Button(new Rect(15f, 35f, 150f, 25f), "Select friend"))
                        {
                            friendsCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);

                            if (friendsCount > 0)
                            {
                                for (int i = 0; i < friendsCount; i++)
                                {
                                    var id = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
                                    friendsIDs.Add(id);
                                    var name = SteamFriends.GetFriendPersonaName(id);
                                    friendsNames.Add(name);

                                    friendSelected = false;
                                }
                            }
                            else
                            {
                                noFriends = true;
                            }
                        }
                    }
                    else
                    {
                        if (friendsCount > 9)
                        {
                            //Up
                            if (startingPoint > 0)
                            {
                                if (GUI.Button(new Rect(15f, 35f, 150f, 25f), "Up ▲"))
                                {
                                    startingPoint -= 1;
                                }
                            }
                            else
                            {
                                GUI.Label(new Rect(15f, 35f, 150f, 25f), "Up ▲");
                            }

                            //Friends list
                            for (int i = startingPoint; i < (startingPoint + 7); i++)
                            {
                                if (GUI.Button(new Rect(15f, 65f + 30f * (i - startingPoint), 150f, 25f), friendsNames[i]))
                                {
                                    Receiver = friendsIDs[i];
                                    friendSelected = true;
                                    selectedFriend = i;
                                }
                            }

                            //Down
                            if (startingPoint < (friendsCount - 7))
                            {
                                if (GUI.Button(new Rect(15f, 275f, 150f, 25f), "Down ▼"))
                                {
                                    startingPoint += 1;
                                }
                            }
                            else
                            {
                                GUI.Label(new Rect(15f, 275f, 150f, 25f), "Down ▼");
                            }
                        }
                        else
                        {
                            for (int i = 0; i < friendsCount; i++)
                            {
                                if (GUI.Button(new Rect(15f, 35f + 30f * i, 150f, 25f), friendsNames[i]))
                                {
                                    Receiver = friendsIDs[i];
                                    friendSelected = true;
                                    selectedFriend = i;
                                }
                            }
                        }
                    }

                    if (friendSelected)
                    {
                        string connected = SecondPlayerConnected ? "<color=green>YES</color>" : "<color=red>NO</color>";
                        string inGame = FriendInGame ? "<color=green>YES</color>" : "<color=red>NO</color>";
                        GUI.Label(new Rect(15f, 65f, 150f, 25f), "Selected friend:");
                        GUI.Label(new Rect(15f, 95f, 150f, 25f), friendsNames[selectedFriend]);
                        GUI.Label(new Rect(15f, 125f, 150f, 25f), $"Connected: {connected}");
                        GUI.Label(new Rect(15f, 155f, 150f, 25f), $"In game: {inGame}");

                        if (!JoinedTheGame)
                        {
                            if (FriendInGame)
                            {
                                if (!SRSingleton<SystemContext>.Instance.SceneLoader.CurrentSceneGroup.isGameplay)
                                {
                                    if (GUI.Button(new Rect(40f, 185f, 100f, 25f), "Join"))
                                    {
                                        JoinedTheGame = true;
                                        SendData.RequestSave();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    GUI.Label(new Rect(15f, 35f, 150f, 250f), "This mod is currently unavailable for other versions except Steam");
                }
            }
        }

        void Start()
        {
            Instance = this;
            SteamIsAvailable = SteamAPI.Init();
            Networking.InitializePackets();
        }

        void Update()
        {
            Networking.ListenData();
        }

        void FixedUpdate()
        {
            ReadGameMode();
            if (_GameMode != _GameModeCached)
            {
                SendData.SendGameModeSwitch(_GameMode);
                _GameModeCached = _GameMode;
            }

            if (TryToConnect)
            {
                if (Receiver != CSteamID.Nil)
                {
                    var inGame = SRSingleton<SystemContext>.Instance.SceneLoader.CurrentSceneGroup.isGameplay;
                    SendData.SendConnection(inGame);
                }
            }
        }

        private bool _GameMode;
        private bool _GameModeCached;
        private void ReadGameMode()
        {
            var _SystemContext = SRSingleton<SystemContext>.Instance;
            if (_SystemContext != null)
            {
                _GameMode = _SystemContext.SceneLoader.CurrentSceneGroup.isGameplay;
            }
        }
    }
}
