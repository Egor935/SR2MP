using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public class Movement : MonoBehaviour
    {
        public Vector3 ReceivedPosition = new Vector3(529.1114f, 17.11f, 338.0007f);
        public float ReceivedRotation;

        void FixedUpdate()
        {
            //Update position only when we receive it
            if (ReceivedPosition != this.transform.position)
            {
                this.transform.position = ReceivedPosition;
            }

            //Update rotation only when we receive it
            if (ReceivedRotation != this.transform.rotation.eulerAngles.y)
            {
                this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, ReceivedRotation, transform.rotation.eulerAngles.z);
            }
        }
    }
}
