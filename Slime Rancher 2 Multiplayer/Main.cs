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
        public override void OnGUI()
        {
            GUI.color = Color.cyan;

            GUI.Box(new Rect(10f, 10f, 160f, 95f), string.Empty);

            if (GUI.Button(new Rect(15f, 15f, 150f, 25f), "Host server")) StartServer.Main();
            //GUI.TextField(new Rect(10f, 40f, 150f, 25f), ip);
            if (GUI.Button(new Rect(15f, 45f, 150f, 25f), "Connect to server")) Client.instance.ConnectToServer();

            if (GUI.Button(new Rect(15f, 75f, 25f, 25f), "-")) smoothValue -= 5f;

            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(40f, 75f, 100f, 25f), $"Smooth: {smoothValue}");

            if (GUI.Button(new Rect(140f, 75f, 25f, 25f), "+")) smoothValue += 5f;

            //menuRect = GUI.Window(0, menuRect, (GUI.WindowFunction)Menu, "Menu");
        }

        public static string ip = File.ReadAllText("ip.txt");
        /*void Menu(int ID0)
        {
            GUILayout.BeginArea(new Rect(0f, 0f, 200f, 600f));
            GUILayout.EndArea();
            //GUI.color = Color.cyan;

            //GUILayout.BeginVertical(null);

            //if (GUILayout.Button("Host server", GUI.skin.button, null)) StartServer.Main();
            ip = GUILayout.TextField(ip, null);
            if (GUILayout.Button("Connect to server", null)) Client.instance.ConnectToServer();

            GUILayout.BeginHorizontal(default);
            if (GUILayout.Button("-", null)) smoothValue -= 5f;
            GUILayout.Label(smoothValue.ToString(), null);
            if (GUILayout.Button("+", null)) smoothValue += 5f;
            GUILayout.EndHorizontal();

            //GUILayout.EndVertical();
            GUI.DragWindow(dragMenu);
        }*/

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
