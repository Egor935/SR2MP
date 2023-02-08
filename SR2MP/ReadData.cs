﻿using Il2CppMonomiPark.SlimeRancher.Player.CharacterController;
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
        public ReadData(IntPtr ptr) : base(ptr) { }

        SRCharacterController _Player;
        Animator _PlayerAnimator;

        Vector3 _Position;
        float _Rotation;
        Vector3 _Speed;

        float _HM;
        float _FM;
        float _Yaw;
        int _AS;
        bool _Moving;
        float _HS;
        float _FS;

        public void Start()
        {
            _Player = Main.Instance._Player;
            _PlayerAnimator = Main.Instance._PlayerAnimator;
        }

        public void Update()
        {

        }

        public void FixedUpdate()
        {
            ReadMovement();
            SendData.SendMovement(_Position, _Rotation, _Speed);

            ReadAnimations();
            SendData.SendAnimations(_HM, _FM, _Yaw, _AS, _Moving, _HS, _FS);
        }

        void ReadMovement()
        {
            _Position = _Player.transform.position;
            _Rotation = _Player.transform.rotation.eulerAngles.y;
            _Speed = _Player.MovementVector;
        }

        void ReadAnimations()
        {
            _HM = _PlayerAnimator.GetFloat("HorizontalMovement");
            _FM = _PlayerAnimator.GetFloat("ForwardMovement");
            _Yaw = _PlayerAnimator.GetFloat("Yaw");
            _AS = _PlayerAnimator.GetInteger("AirborneState");
            _Moving = _PlayerAnimator.GetBool("Moving");
            _HS = _PlayerAnimator.GetFloat("HorizontalSpeed");
            _FS = _PlayerAnimator.GetFloat("ForwardSpeed");
        }
    }
}
