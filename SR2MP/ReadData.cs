using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Player.CharacterController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public class ReadData : MonoBehaviour
    {
        private SRCharacterController _Player;
        private Animator _PlayerAnimator;

        //Movement
        private Vector3 _PlayerPosition;
        private float _PlayerRotation;

        //Animations
        private float _HM;
        private float _FM;
        private float _Yaw;
        private int _AS;
        private bool _Moving;
        private float _HS;
        private float _FS;

        //Time
        private double _Time;

        //InGame
        private bool _InGame;
        private bool _InGameCached;

        void Start()
        {
            _Player = FindObjectOfType<SRCharacterController>();
            _PlayerAnimator = _Player.GetComponent<Animator>();
        }

        void Update()
        {
            ReadInGame();
            if (_InGame != _InGameCached)
            {
                SendData.SendInGame(_InGame);
                _InGameCached = _InGame;
            }
        }

        void FixedUpdate()
        {
            ReadMovement();
            SendData.SendMovement(_PlayerPosition, _PlayerRotation);

            ReadAnimations();
            SendData.SendAnimations(_HM, _FM, _Yaw, _AS, _Moving, _HS, _FS);

            if (!Main.Joined)
            {
                ReadTime();
                SendData.SendTime(_Time);
            }
        }

        private void ReadMovement()
        {
            _PlayerPosition = _Player.transform.position;
            _PlayerRotation = _Player.transform.rotation.eulerAngles.y;
        }

        private void ReadAnimations()
        {
            _HM = _PlayerAnimator.GetFloat("HorizontalMovement");
            _FM = _PlayerAnimator.GetFloat("ForwardMovement");
            _Yaw = _PlayerAnimator.GetFloat("Yaw");
            _AS = _PlayerAnimator.GetInteger("AirborneState");
            _Moving = _PlayerAnimator.GetBool("Moving");
            _HS = _PlayerAnimator.GetFloat("HorizontalSpeed");
            _FS = _PlayerAnimator.GetFloat("ForwardSpeed");
        }

        private void ReadTime()
        {
            _Time = SRSingleton<SceneContext>.Instance.TimeDirector._worldModel.worldTime;
        }

        private void ReadInGame()
        {
            var _SystemContext = SRSingleton<SystemContext>.Instance;
            if (_SystemContext != null)
            {
                _InGame = _SystemContext.SceneLoader.CurrentSceneGroup.IsGameplay;
            }
        }
    }
}
