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
        //Player
        private Animator _Animator;

        //Movement
        private Vector3 _Position;
        private float _Rotation;

        //Animations
        private float _HorizontalMovement;
        private float _ForwardMovement;
        private float _Yaw;
        private int _AirborneState;
        private bool _Moving;
        private float _HorizontalSpeed;
        private float _ForwardSpeed;

        //Time
        private double _Time;

        //InGame
        private bool _InGame;
        private bool _InGameCached;

        //Currency
        private int _Currency;
        private int _CurrencyCached;
        private bool currencyChanged;

        void Start()
        {
            _Animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (HandleData.CurrencyReceived)
            {
                _CurrencyCached = HandleData.ReceivedCurrency;
                HandleData.CurrencyReceived = false;
            }

            ReadCurrency();
            if (_Currency != _CurrencyCached)
            {
                SendData.SendCurrency(_Currency, false);
                _CurrencyCached = _Currency;
                //currencyChanged = true;
            }
            //else
            //{
            //    if (currencyChanged)
            //    {
            //        SendData.SendCurrency(_Currency, true);
            //        currencyChanged = false;
            //    }
            //}

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
            SendData.SendMovement(_Position, _Rotation);

            ReadAnimations();
            SendData.SendAnimations(_HorizontalMovement, _ForwardMovement, _Yaw, _AirborneState, _Moving, _HorizontalSpeed, _ForwardSpeed);

            if (SteamLobby.Host)
            {
                ReadTime();
                SendData.SendTime(_Time);
            }
        }

        private void ReadMovement()
        {
            _Position = this.transform.position;
            _Rotation = this.transform.rotation.eulerAngles.y;
        }

        private void ReadAnimations()
        {
            _HorizontalMovement = _Animator.GetFloat("HorizontalMovement");
            _ForwardMovement = _Animator.GetFloat("ForwardMovement");
            _Yaw = _Animator.GetFloat("Yaw");
            _AirborneState = _Animator.GetInteger("AirborneState");
            _Moving = _Animator.GetBool("Moving");
            _HorizontalSpeed = _Animator.GetFloat("HorizontalSpeed");
            _ForwardSpeed = _Animator.GetFloat("ForwardSpeed");
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

        private void ReadCurrency()
        {
            _Currency = SRSingleton<SceneContext>.Instance.PlayerState._model.currency;
        }
    }
}
