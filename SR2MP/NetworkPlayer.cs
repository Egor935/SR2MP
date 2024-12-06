using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public class NetworkPlayer : MonoBehaviour
    {
        public static NetworkPlayer Instance;
        public Animator PlayerAnimator;

        public Vector3 ReceivedPosition = new Vector3(530f, 17.11f, 335f);
        public float ReceivedRotation = 40f;
        public bool MovementReceived = true;

        public float HorizontalMovement;
        public float ForwardMovement;
        public float Yaw;
        public int AirborneState;
        public bool Moving;
        public float HorizontalSpeed;
        public float ForwardSpeed;
        public bool AnimationsReceived;

        void Start()
        {
            Instance = this;
            PlayerAnimator = GetComponent<Animator>();
        }

        void Update()
        {

        }

        void FixedUpdate()
        {
            if (MovementReceived)
            {
                this.transform.position = ReceivedPosition;
                this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, ReceivedRotation, this.transform.rotation.z);
                MovementReceived = false;
            }

            if (AnimationsReceived)
            {
                PlayerAnimator.SetFloat("HorizontalMovement", HorizontalMovement);
                PlayerAnimator.SetFloat("ForwardMovement", ForwardMovement);
                PlayerAnimator.SetFloat("Yaw", Yaw);
                PlayerAnimator.SetInteger("AirborneState", AirborneState);
                PlayerAnimator.SetBool("Moving", Moving);
                PlayerAnimator.SetFloat("HorizontalSpeed", HorizontalSpeed);
                PlayerAnimator.SetFloat("ForwardSpeed", ForwardSpeed);
                AnimationsReceived = false;
            }
        }
    }
}
