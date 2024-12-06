using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SR2MP
{
    public class MultiplayerCore : MonoBehaviour
    {
        bool getBeatrix = true;
        bool setUpBeatrix = false;
        GameObject localPlayer;

        void Start()
        {

        }

        void Update()
        {
            if (localPlayer == null)
            {
                if (SceneContext.Instance != null)
                {
                    if (SceneContext.Instance.Player != null)
                    {
                        SceneContext.Instance.Player.AddComponent<ReadData>();
                        localPlayer = SceneContext.Instance.Player;
                    }
                }
            }

            if (setUpBeatrix)
            {
                if (SceneContext.Instance != null)
                {
                    if (SceneContext.Instance.Player != null)
                    {
                        SetNetworkPlayerAnimator();
                        setUpBeatrix = false;
                    }
                }
            }

            if (getBeatrix)
            {
                if (SceneManager.GetActiveScene().name == "MainMenuEnvironment")
                {
                    CreateNetworkPlayer();
                    getBeatrix = false;
                    setUpBeatrix = true;
                }
            }
        }

        private void CreateNetworkPlayer()
        {
            var networkPlayer = GameObject.Find("BeatrixMainMenu");
            Instantiate(networkPlayer);

            networkPlayer.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            networkPlayer.AddComponent<NetworkPlayer>();
            DontDestroyOnLoad(networkPlayer);
        }

        private void SetNetworkPlayerAnimator()
        {
            var localPlayerAnimator = SceneContext.Instance.Player.GetComponent<Animator>();
            NetworkPlayer.Instance.PlayerAnimator.avatar = localPlayerAnimator.avatar;
            NetworkPlayer.Instance.PlayerAnimator.runtimeAnimatorController = localPlayerAnimator.runtimeAnimatorController;
        }
    }
}
