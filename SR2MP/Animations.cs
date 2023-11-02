﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public class Animations : MonoBehaviour
    {
        private Animator _Animator;

        public float HM;
        public float FM;
        public float Yaw;
        public int AS;
        public bool Moving;
        public float HS;
        public float FS;

        void Start()
        {
            _Animator = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
            _Animator.SetFloat("HorizontalMovement", HM);
            _Animator.SetFloat("ForwardMovement", FM);
            _Animator.SetFloat("Yaw", Yaw);
            _Animator.SetInteger("AirborneState", AS);
            _Animator.SetBool("Moving", Moving);
            _Animator.SetFloat("HorizontalSpeed", HS);
            _Animator.SetFloat("ForwardSpeed", FS);
        }
    }
}
