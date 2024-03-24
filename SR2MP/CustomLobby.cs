using GameServer;
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
        //Stuff
        private static bool allowToHostServer = true;
        private static bool allowToConnectServer = true;
        public static bool Host;

        public static string ip = "127.0.0.1";

        public static void CustomMenu()
        {
            if (allowToHostServer && allowToConnectServer)
            {
                ip = GUI.TextField(new Rect(15f, 35f, 150f, 25f), ip);
            }
            else
            {
                GUI.Label(new Rect(15f, 35f, 150f, 25f), ip);
            }

            if (allowToHostServer)
            {
                if (GUI.Button(new Rect(15f, 65f, 150f, 25f), "Host server"))
                {
                    CreateServer();

                    Server.Start(2, 26950);
                    Client.instance.ConnectToServer();
                    allowToHostServer = false;
                    allowToConnectServer = false;
                    Host = true;
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

            string _FirendInGame = Main.FriendInGame ? "<color=green>YES</color>" : "<color=red>NO</color>";
            GUI.Label(new Rect(15f, 125f, 150f, 25f), $"Friend in game: {_FirendInGame}");

            if (!Main.Joined)
            {
                if (Main.FriendInGame)
                {
                    if (!SRSingleton<SystemContext>.Instance.SceneLoader.CurrentSceneGroup.IsGameplay)
                    {
                        if (GUI.Button(new Rect(40f, 155f, 100f, 25f), "Join"))
                        {
                            Main.Joined = true;
                            SendData.RequestSaveData();
                        }
                    }
                }
            }
        }

        private static void CreateServer()
        {
            var server = new GameObject("Server");
            server.AddComponent<GameServer.ThreadManager>();
            DontDestroyOnLoad(server);
        }
    }
}
