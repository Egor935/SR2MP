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
        public Vacpack _Vacpack;

        public void Start()
        {
            instance = this;

            _Movement = GetComponent<Movement>();
            _Animations = GetComponent<Animations>();
            _Vacpack = GetComponent<Vacpack>();
        }

        public void Update()
        {

        }

        public void FixedUpdate()
        {

        }
    }
}
