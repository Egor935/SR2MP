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

        public NetworkMovement Movement;
        public NetworkAnimations Animations;

        void Start()
        {
            Movement = this.gameObject.AddComponent<NetworkMovement>();
            Animations = this.gameObject.AddComponent<NetworkAnimations>();

            Instance = this;
        }
    }
}
