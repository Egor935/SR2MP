using Il2CppMonomiPark.SlimeRancher.Player.CharacterController;
using MelonLoader;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Object;

namespace SR2MP
{
    public class Main : MelonMod
    {
        #region Variables
        public const string version = "0.0.5";

        public static Main Instance;

        public SRCharacterController _Player;
        GameObject _Beatrix;
        public Animator _PlayerAnimator;
        public Animator _BeatrixAnimator;

        bool GameLoadCheck = true;
        bool GameIsLoaded;

        public static bool menuState = true;
        #endregion

        [Obsolete]
        public override void OnApplicationStart()
        {
            SteamAPI.Init();
            
            ClassInjector.RegisterTypeInIl2Cpp<SteamLobby>();
            ClassInjector.RegisterTypeInIl2Cpp<ReadData>();
            ClassInjector.RegisterTypeInIl2Cpp<Movement>();
            ClassInjector.RegisterTypeInIl2Cpp<Animations>();
            ClassInjector.RegisterTypeInIl2Cpp<Beatrix>();
        }

        [Obsolete]
        public override void OnApplicationLateStart()
        {
            Instance = this;
            var _NM = new GameObject("NetworkManager");
            DontDestroyOnLoad(_NM);
            _NM.AddComponent<SteamLobby>();
        }

        int _FriendsCount;
        List<CSteamID> _FriendsIDs = new List<CSteamID>();
        List<string> _FriendsNames = new List<string>();

        int _StartingPoint = 0;

        bool _FriendSelected;
        int _SelectedFriend;
        public override void OnGUI()
        {
            if (menuState)
            {
                GUI.color = Color.cyan;
                GUI.skin.box.normal.textColor = Color.white;

                #region Main

                GUI.Box(new Rect(10f, 10f, 160f, 300f), "<b>SR2MP</b>");

                if ((_FriendsCount == 0) || _FriendSelected)
                {
                    if (GUI.Button(new Rect(15f, 35f, 150f, 25f), "Select friend"))
                    {
                        _FriendsCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
                        //MelonLogger.Msg($"Friends count: {_FriendsCount}");
                        for (int i = 0; i < _FriendsCount; i++)
                        {
                            var id = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
                            _FriendsIDs.Add(id);
                            var name = SteamFriends.GetFriendPersonaName(id);
                            _FriendsNames.Add(name);

                            //MelonLogger.Msg($"{name}: {id.m_SteamID}");
                            _FriendSelected = false;
                        }
                    }
                }

                else
                {
                    if (GUI.Button(new Rect(15f, 35f, 150f, 25f), "Up ▲"))
                    {
                        if (_StartingPoint > 0)
                        {
                            _StartingPoint -= 1;
                        }
                    }

                    for (int i = _StartingPoint; i < _StartingPoint + 7; i++)
                    {
                        if (GUI.Button(new Rect(15f, 65f + 30f * (i - _StartingPoint), 150f, 25f), _FriendsNames[i]))
                        {
                            var id = _FriendsIDs[i];
                            SteamLobby.receiver = id;
                            MelonLogger.Msg(id.m_SteamID);

                            _FriendSelected = true;
                            _SelectedFriend = i;
                        }
                    }

                    if (GUI.Button(new Rect(15f, 275f, 150f, 25f), "Down ▼"))
                    {
                        if (_FriendsCount > 0)
                        {
                            if (_StartingPoint < (_FriendsCount - 7))
                            {
                                _StartingPoint += 1;
                            }
                        }
                    }
                }

                if (_FriendSelected)
                {
                    GUI.Label(new Rect(15f, 65f, 150f, 25f), "Selected friend:");
                    GUI.Label(new Rect(15f, 95f, 150f, 25f), _FriendsNames[_SelectedFriend]);
                }


                #endregion

                #region Donators

                GUI.Box(new Rect(180f, 10f, 300f, 300f), "<b>Donators</b>");

                GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                GUI.Label(new Rect(180f, 35f, 100f, 25f), "Name");
                GUI.Label(new Rect(280f, 35f, 100f, 25f), "Amount");
                GUI.Label(new Rect(380f, 35f, 100f, 25f), "Date");

                //1 donator
                GUI.Label(new Rect(180f, 65f, 100f, 25f), "DragonCoreGS");
                GUI.Label(new Rect(280f, 65f, 100f, 25f), "1500 rubles");
                GUI.Label(new Rect(380f, 65f, 100f, 25f), "01 FEB 23");

                //2 donator
                GUI.Label(new Rect(180f, 95f, 100f, 25f), "Evan Ranger");
                GUI.Label(new Rect(280f, 95f, 100f, 25f), "500 rubles");
                GUI.Label(new Rect(380f, 95f, 100f, 25f), "01 DEC 22");

                //3 donator
                GUI.Label(new Rect(180f, 125f, 100f, 25f), "PinkTarr");
                GUI.Label(new Rect(280f, 125f, 100f, 25f), "200 rubles");
                GUI.Label(new Rect(380f, 125f, 100f, 25f), "08 FEB 23");

                //4 donator
                GUI.Label(new Rect(180f, 155f, 100f, 25f), "Ikra Game");
                GUI.Label(new Rect(280f, 155f, 100f, 25f), "31 rubles");
                GUI.Label(new Rect(380f, 155f, 100f, 25f), "02 JAN 23");

                #endregion

                #region Info
                GUI.Box(new Rect(10f, 320f, 470f, 25f), "You can hide this menu by pressing the Tilde button (~)");
                #endregion
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
                    _Beatrix.AddComponent<CharacterController>();
                    _Beatrix.AddComponent<Movement>();
                    _Beatrix.AddComponent<Animations>();
                    _Beatrix.AddComponent<Beatrix>();
                    GameIsLoaded = true;
                    GameLoadCheck = false;
                }
            }
        }

        public override void OnLateUpdate()
        {

        }

        public override void OnFixedUpdate()
        {
            if (_Player == null)
            {
                _Player = FindObjectOfType<SRCharacterController>();
                if (_Player != null)
                {
                    _PlayerAnimator = _Player.GetComponent<Animator>();
                    _BeatrixAnimator.avatar = _PlayerAnimator.avatar;
                    _BeatrixAnimator.runtimeAnimatorController = _PlayerAnimator.runtimeAnimatorController;
                    _Player.gameObject.AddComponent<ReadData>();
                }
            }
        }
    }
}
