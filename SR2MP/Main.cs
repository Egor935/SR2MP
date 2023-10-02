using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher.Player.CharacterController;
using Il2CppSystem.IO;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SR2MP
{
    public class Main : MonoBehaviour
    {
        public Main(IntPtr ptr) : base(ptr) { }

        #region Variables
        public static Main Instance;

        //Main objects
        public SRCharacterController Player;
        private GameObject _Beatrix;
        public Animator PlayerAnimator;
        private Animator _BeatrixAnimator;

        //Game loading
        public bool GameLoadCheck = true;
        public bool GameIsLoaded;

        //Stuff
        public bool SteamIsAvailable;
        #endregion

        void Start()
        {
            Instance = this;
            SteamIsAvailable = SteamAPI.Init();
        }

        void Update()
        {
            //Game loading
            if (GameLoadCheck)
            {
                Scene activeScene = SceneManager.GetActiveScene();
                if (activeScene.name.Equals("MainMenuEnvironment"))
                {
                    GetBeatrixModel();
                    GameIsLoaded = true;
                    GameLoadCheck = false;
                }
            }
        }

        void FixedUpdate()
        {
            if (Player == null)
            {
                Player = FindObjectOfType<SRCharacterController>();
                if (Player != null)
                {
                    HandlePlayer();
                }
            }
        }

        private void GetBeatrixModel()
        {
            _Beatrix = GameObject.Find("BeatrixMainMenu");
            Instantiate(_Beatrix);

            var hair = GameObject.Find("SR2_bea_hair_main_low");
            var material = hair.GetComponent<SkinnedMeshRenderer>().material;
            DontDestroyOnLoad(material);

            _Beatrix.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            _BeatrixAnimator = _Beatrix.GetComponent<Animator>();
            _Beatrix.AddComponent<Movement>();
            _Beatrix.AddComponent<Animations>();
            _Beatrix.AddComponent<Vacpack>();
            _Beatrix.AddComponent<Beatrix>();

            DontDestroyOnLoad(_Beatrix);
        }

        private void HandlePlayer()
        {
            PlayerAnimator = Player.GetComponent<Animator>();
            _BeatrixAnimator.avatar = PlayerAnimator.avatar;
            _BeatrixAnimator.runtimeAnimatorController = PlayerAnimator.runtimeAnimatorController;
            _BeatrixAnimator.updateMode = AnimatorUpdateMode.AnimatePhysics;
            Player.gameObject.AddComponent<ReadData>();
        }
    }
}
