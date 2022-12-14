using GameServer;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using static UnityEngine.Object;
using static SR2MP.Main.Variables;
using static SR2MP.ClientHandle.ReceivedValues;
using Ping = System.Net.NetworkInformation.Ping;
using System.Threading;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using Il2CppMonomiPark.SlimeRancher.Player.CharacterController;

namespace SR2MP
{
    public class Main : MelonMod
    {
        public class Variables
        {
            public const string version = "0.0.4";
            public static string IP
            {
                get
                {
                    if (File.Exists("ip.txt"))
                    {
                        var ip = File.ReadAllText("ip.txt");
                        if (ip != "")
                        {
                            return ip;
                        }
                        else
                        {
                            MelonLogger.Error("ip.txt is empty");
                            return null;
                        }
                    }
                    else
                    {
                        MelonLogger.Error("ip.txt not found");
                        File.Create("ip.txt");
                        var path = Path.GetFullPath("ip.txt");
                        MelonLogger.Warning($"ip.txt was created in this path: {path} \nPlease open it and insert your IP if you are hosting the server, or your friend's IP if you are connecting to the server and if it has already been hosted.");
                        return null;
                    }
                }
            }
            public static int port = 22222;
            public static int playersCount = 2;

            public static bool checkHostServerButton = true;
            public static bool checkConnectToServerButton = true;
            public static float smoothness = 3f;

            public static bool IPavailable
            {
                get
                {
                    Ping ping = new Ping();
                    PingReply pingReply = ping.Send(IP);
                    return (pingReply.Status == IPStatus.Success) && (!IP.Contains("127.0.0"));
                }
            }

            public static bool menuState = true;
            public static Thread newThread;

            public static bool GameLoadCheck = true;
            public static bool GameIsLoaded;

            public static SRCharacterController player;
            public static GameObject _Beatrix;

            public static Animator _PlayerAnimator;
            public static Animator _BeatrixAnimator;
        }

        public static void CreateClientManager()
        {
            var clientManager = new GameObject("ClientManager");
            DontDestroyOnLoad(clientManager);
            clientManager.AddComponent<Client>();
            clientManager.AddComponent<ThreadManager>();
        }

        public override void OnInitializeMelon()
        {
            ClassInjector.RegisterTypeInIl2Cpp<Client>();
            ClassInjector.RegisterTypeInIl2Cpp<ThreadManager>();
        }

        public override void OnGUI()
        {
            if (menuState)
            {
                GUI.color = Color.cyan;
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;

                GUI.Box(new Rect(10f, 10f, 160f, 95f), string.Empty);

                if (checkHostServerButton)
                {
                    if (GUI.Button(new Rect(15f, 15f, 150f, 25f), "Host server"))
                    {
                        if (IP != null)
                        {
                            MelonLogger.Msg("Trying to host...");
                            newThread = new Thread(() =>
                            {
                                if (IPavailable)
                                {
                                    ServerStart.Start();
                                    checkHostServerButton = false;
                                    CreateClientManager();
                                    checkConnectToServerButton = false;
                                }
                                else
                                {
                                    MelonLogger.Error("Please paste your IP from the Hamachi or RadminVPN into the ip.txt and try again");
                                }
                                newThread.Abort();
                            });
                            newThread.Start();
                        }
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
                        if (IP != null)
                        {
                            MelonLogger.Msg("Trying to connect...");
                            newThread = new Thread(() =>
                            {
                                if (IPavailable)
                                {
                                    CreateClientManager();
                                    checkConnectToServerButton = false;
                                    checkHostServerButton = false;
                                }
                                else
                                {
                                    MelonLogger.Error("IP is not available, please paste your friend's IP from Hamachi or RadminVPN into the ip.txt and try again");
                                }
                                newThread.Abort();
                            });
                            newThread.Start();
                        }
                    }
                }
                else
                {
                    GUI.Label(new Rect(15f, 45f, 150f, 25f), "You are connected");
                }

                if (GUI.Button(new Rect(15f, 75f, 25f, 25f), "-"))
                {
                    if (smoothness > 1)
                    {
                        smoothness -= 1f;
                    }
                }

                GUI.Label(new Rect(40f, 75f, 100f, 25f), $"Smoothness: {smoothness}");

                if (GUI.Button(new Rect(140f, 75f, 25f, 25f), "+"))
                {
                    if (smoothness < 9)
                    {
                        smoothness += 1f;
                    }
                }
            }
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(192))
            {
                menuState = !menuState;
            }

            if (GameLoadCheck)
            {
                Scene activeScene = SceneManager.GetActiveScene();
                if (Equals(activeScene.name, "MainMenuEnvironment"))
                {
                    var beatrix = GameObject.Find("BeatrixMainMenu");
                    beatrix.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                    DontDestroyOnLoad(beatrix);
                    var hair = GameObject.Find("SR2_bea_hair_main_low");
                    var material = hair.GetComponent<SkinnedMeshRenderer>().material;
                    DontDestroyOnLoad(material);
                    _Beatrix = beatrix;
                    _BeatrixAnimator = _Beatrix.GetComponent<Animator>();
                    GameIsLoaded = true;
                    GameLoadCheck = false;
                }
            }
        }

        public override void OnFixedUpdate()
        {
            if (player == null)
            {
                player = FindObjectOfType<SRCharacterController>();
                if (player != null)
                {
                    _PlayerAnimator = player.GetComponent<Animator>();
                    _BeatrixAnimator.avatar = _PlayerAnimator.avatar;
                    _BeatrixAnimator.runtimeAnimatorController = _PlayerAnimator.runtimeAnimatorController;
                }
            }
            else
            {
                var pos = player.transform.position;
                var rot = player.transform.rotation.eulerAngles.y;
                ClientSend.SendMovement(pos, rot);

                var HM = _PlayerAnimator.GetFloat("HorizontalMovement");
                var FM = _PlayerAnimator.GetFloat("ForwardMovement");
                var Yaw = _PlayerAnimator.GetFloat("Yaw");
                var AS = _PlayerAnimator.GetInteger("AirborneState");
                var Moving = _PlayerAnimator.GetBool("Moving");
                var HS = _PlayerAnimator.GetFloat("HorizontalSpeed");
                var FS = _PlayerAnimator.GetFloat("ForwardSpeed");
                ClientSend.SendAnimations(HM, FM, Yaw, AS, Moving, HS, FS);
            }

            if (_Beatrix != null)
            {
                _Beatrix.transform.position = Vector3.Lerp(_Beatrix.transform.position, position, 4f * smoothness * Time.fixedDeltaTime);

                var newRotation = Quaternion.Euler(_Beatrix.transform.rotation.eulerAngles.x, rotation, _Beatrix.transform.rotation.eulerAngles.z);
                _Beatrix.transform.rotation = Quaternion.Lerp(_Beatrix.transform.rotation, newRotation, 4f * smoothness * Time.fixedDeltaTime);

                _BeatrixAnimator.SetFloat("HorizontalMovement", f1);
                _BeatrixAnimator.SetFloat("ForwardMovement", f2);
                _BeatrixAnimator.SetFloat("Yaw", f3);
                _BeatrixAnimator.SetInteger("AirborneState", i1);
                _BeatrixAnimator.SetBool("Moving", b1);
                _BeatrixAnimator.SetFloat("HorizontalSpeed", f4);
                _BeatrixAnimator.SetFloat("ForwardSpeed", f5);
            }
        }
    }

    public class Input
    {
        [DllImport("user32.dll")]
        static extern int GetAsyncKeyState(int vKey);

        public static bool GetKeyDown(int key)
        {
            if (0 != (GetAsyncKeyState(key) & 0x8000))
            {
                if (!input)
                {
                    input = true;
                    return true;
                }
            }
            else
            {
                input = false;
            }
            return false;
        }

        static bool input;
    }
}
