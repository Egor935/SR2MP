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
        public static Beatrix Instance;

        public Movement BeatrixMovement;
        public Animations BeatrixAnimations;
        public Vacpack BeatrixVacpack;

        void Start()
        {
            Instance = this;

            BeatrixMovement = GetComponent<Movement>();
            BeatrixAnimations = GetComponent<Animations>();
            BeatrixVacpack = GetComponent<Vacpack>();
        }
    }
}
