using GameServer;
using Il2CppMonomiPark.SlimeRancher.Player.CharacterController;
using Il2CppMonomiPark.SlimeRancher.UI;
using Il2CppMonomiPark.UnitPropertySystem;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Object;

namespace Slime_Rancher_2_Multiplayer
{
    public class Main : MelonMod
    {
        Rect menuRect = new Rect(10f, 10f, 200f, 600f);
        Rect dragMenu = new Rect(0f, 0f, 200f, 20f);

        bool checkHostServerButton = true;
        bool checkConnectToServerButton = true;
        public override void OnGUI()
        {
            GUI.color = Color.cyan;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;

            GUI.Box(new Rect(10f, 10f, 160f, 95f), string.Empty);

            if (checkHostServerButton)
            {
                if (GUI.Button(new Rect(15f, 15f, 150f, 25f), "Host server"))
                {
                    StartServer.Main();
                    checkHostServerButton = false;

                    Client.instance.ConnectToServer();
                    checkConnectToServerButton = false;
                }
            }
            else 
            {
                GUI.Label(new Rect(15f, 15f, 150f, 25f), "Server is hosted");
            }

            if (checkConnectToServerButton)
            {
                if (GUI.Button(new Rect(15f, 45f, 150f, 25f), "Connect to server"))
                {
                    if (File.Exists("ip.txt"))
                    {
                        ip = File.ReadAllText("ip.txt");
                        Client.instance.ConnectToServer();
                        checkConnectToServerButton = false;
                        checkHostServerButton = false;
                    }
                    else
                    {
                        MelonLogger.Error("ip.txt not found");
                    }
                }
            }
            else
            {
                GUI.Label(new Rect(15f, 45f, 150f, 25f), "You are connected");
            }

            if (GUI.Button(new Rect(15f, 75f, 25f, 25f), "-")) smoothValue -= 5f;

            
            GUI.Label(new Rect(40f, 75f, 100f, 25f), $"Smooth: {smoothValue}");

            if (GUI.Button(new Rect(140f, 75f, 25f, 25f), "+")) smoothValue += 5f;
        }

        public static string ip = "127.0.0.1";

        [Obsolete]
        public override void OnApplicationStart()
        {
            new Client().Awake();
        }

        public static SRCharacterController player;
        public GameObject beatrix;

        public bool GameLoadCheck = true;
        public static bool GameIsLoaded;
        public float smoothValue = 25f;
        public override void OnUpdate()
        {
            ThreadManager.UpdateMain();

            if (beatrix == null) beatrix = GameObject.Find("BeatrixMainMenu");
            if (beatrix != null)
            {
                beatrix.transform.position = Vector3.Lerp(beatrix.transform.position, ClientHandle.position, Time.deltaTime * smoothValue);
                beatrix.transform.rotation = Quaternion.Lerp(beatrix.transform.rotation, ClientHandle.rotation, Time.deltaTime * smoothValue);
            }

            if (GameLoadCheck)
            {
                Scene scene = SceneManager.GetActiveScene();
                if (string.Equals(scene.name, "MainMenuEnvironment"))
                {
                    var beatrix = GameObject.Find("BeatrixMainMenu");
                    beatrix.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                    DontDestroyOnLoad(beatrix);
                    var hair = GameObject.Find("SR2_bea_hair_main_low");
                    DontDestroyOnLoad(hair.GetComponent<SkinnedMeshRenderer>().material);
                    GameIsLoaded = true;
                    GameLoadCheck = false;
                }
            }
        }

        public static bool serverStarted;
        public override void OnFixedUpdate()
        {
            if (serverStarted)
            {
                ServerThreadManager.UpdateMain();
            }

            if (player == null) player = FindObjectOfType<SRCharacterController>();
            if (player != null)
            {
                var pos = player.transform.position;
                var rot = player.transform.rotation;
                ClientSend.PlayerMovement(pos, rot);
            }
        }
    }
}
