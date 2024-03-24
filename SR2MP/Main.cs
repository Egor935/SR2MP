using Il2CppMonomiPark.SlimeRancher.Player.CharacterController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SR2MP
{
    public class Main : MonoBehaviour
    {
        private SRCharacterController _LocalPlayer;
        private Animator _BeatrixAnimator;

        private bool gameLoadCheck = true;
        private bool gameIsLoaded;

        public static bool Joined;
        public static bool HandlePacket;
        public static bool FriendInGame;

        void Start()
        {

        }

        void Update()
        {
            if (gameLoadCheck)
            {
                Scene activeScene = SceneManager.GetActiveScene();
                if (activeScene.name.Equals("MainMenuEnvironment"))
                {
                    GetBeatrixModel();
                    gameIsLoaded = true;
                    gameLoadCheck = false;
                }
            }
        }

        void FixedUpdate()
        {
            if (_LocalPlayer == null)
            {
                _LocalPlayer = FindObjectOfType<SRCharacterController>();
                if (_LocalPlayer != null)
                {
                    _LocalPlayer.gameObject.AddComponent<ReadData>();
                    HandleBeatrix();
                }
            }
        }

        private void GetBeatrixModel()
        {
            var networkPlayer = GameObject.Find("BeatrixMainMenu");
            Instantiate(networkPlayer);

            var hair = GameObject.Find("SR2_bea_hair_main_low");
            var material = hair.GetComponent<SkinnedMeshRenderer>().material;
            DontDestroyOnLoad(material);

            networkPlayer.transform.localScale = 0.9f * Vector3.one;
            networkPlayer.AddComponent<NetworkPlayer>();
            DontDestroyOnLoad(networkPlayer);

            _BeatrixAnimator = networkPlayer.GetComponent<Animator>();
        }

        private void HandleBeatrix()
        {
            var playerAnimator = _LocalPlayer.GetComponent<Animator>();
            _BeatrixAnimator.avatar = playerAnimator.avatar;
            _BeatrixAnimator.runtimeAnimatorController = playerAnimator.runtimeAnimatorController;
            //_BeatrixAnimator.updateMode = AnimatorUpdateMode.AnimatePhysics;
        }
    }
}
