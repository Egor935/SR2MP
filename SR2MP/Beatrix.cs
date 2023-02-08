using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public class Beatrix : MonoBehaviour
    {
        public Beatrix(IntPtr ptr) : base(ptr) { }

        public static Beatrix instance;

        public Movement _Movement;
        public Animations _Animations;

        public void Start()
        {
            instance = this;

            _Movement = GetComponent<Movement>();
            _Animations = GetComponent<Animations>();
        }

        public void Update()
        {

        }

        public void FixedUpdate()
        {

        }
    }
}
