using GameServer;
using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public class CustomLobby : MonoBehaviour
    {
        public CustomLobby(IntPtr ptr) : base(ptr) { }

        public static CustomLobby Instance;

        //Stuff
        private bool allowToHostServer = true;
        private bool allowToConnectServer = true;

        public void CustomMenu()
        {
            if (allowToHostServer && allowToConnectServer)
            {
                Client.instance.ip = GUI.TextField(new Rect(15f, 35f, 150f, 25f), Client.instance.ip);
            }
            else
            {
                GUI.Label(new Rect(15f, 35f, 150f, 25f), Client.instance.ip);
            }

            if (allowToHostServer)
            {
                if (GUI.Button(new Rect(15f, 65f, 150f, 25f), "Host server"))
                {
                    ServerInit.Start();
                    Client.instance.ConnectToServer();
                    allowToHostServer = false;
                    allowToConnectServer = false;
                    GlobalStuff.Host = true;
                }
            }
            else
            {
                GUI.Label(new Rect(15f, 65f, 150f, 25f), "Host server");
            }

            if (allowToConnectServer)
            {
                if (GUI.Button(new Rect(15f, 95f, 150f, 25f), "Connect to server"))
                {
                    Client.instance.ConnectToServer();
                    allowToHostServer = false;
                    allowToConnectServer = false;
                }
            }
            else
            {
                GUI.Label(new Rect(15f, 95f, 150f, 25f), "Connect to server");
            }

            //GUI.Label(new Rect(15f, 125f, 150f, 25f), "Connected friend:");
            //GUI.Label(new Rect(15f, 155f, 150f, 25f), GlobalStuff.SecondPlayerName);

            //if (GlobalStuff.SecondPlayerName != "None")
            {
                string inGame = GlobalStuff.FriendInGame ? "<color=green>YES</color>" : "<color=red>NO</color>";
                GUI.Label(new Rect(15f, 125f, 150f, 25f), $"Friend in game: {inGame}");

                if (!GlobalStuff.JoinedTheGame)
                {
                    if (GlobalStuff.FriendInGame)
                    {
                        if (!SRSingleton<SystemContext>.Instance.SceneLoader.CurrentSceneGroup.isGameplay)
                        {
                            if (GUI.Button(new Rect(40f, 155f, 100f, 25f), "Join"))
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
            CreateCustomClientManager();
        }

        public static void CreateCustomClientManager()
        {
            var clientManager = new GameObject("ClientManager");
            DontDestroyOnLoad(clientManager);
            clientManager.AddComponent<Client>();
            clientManager.AddComponent<ThreadManager>();
        }
    }
}
