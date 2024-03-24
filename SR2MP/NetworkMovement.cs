using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public class NetworkMovement : MonoBehaviour
    {
        public Vector3 ReceivedPosition = new Vector3(529.1114f, 17.11f, 338.0007f);
        public float ReceivedRotation;

        public bool MovementReceived;

        void Update()
        {
            if (MovementReceived)
            {
                this.transform.position = ReceivedPosition;
                this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, ReceivedRotation, transform.rotation.eulerAngles.z);

                MovementReceived = false;
            }
        }
    }
}
