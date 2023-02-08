using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public class Animations : MonoBehaviour
    {
        public Animations(IntPtr ptr) : base(ptr) { }

        public Animator _Animator;

        public float _HM;
        public float _FM;
        public float _Yaw;
        public int _AS;
        public bool _Moving;
        public float _HS;
        public float _FS;

        public void Start()
        {
            _Animator = GetComponent<Animator>();
        }

        public void Update()
        {

        }

        public void FixedUpdate()
        {
            _Animator.SetFloat("HorizontalMovement", _HM);
            _Animator.SetFloat("ForwardMovement", _FM);
            _Animator.SetFloat("Yaw", _Yaw);
            _Animator.SetInteger("AirborneState", _AS);
            _Animator.SetBool("Moving", _Moving);
            _Animator.SetFloat("HorizontalSpeed", _HS);
            _Animator.SetFloat("ForwardSpeed", _FS);
        }
    }
}
