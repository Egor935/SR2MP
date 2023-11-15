using Il2Cpp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public class HandleSlimes : MonoBehaviour
    {
        public static HandleSlimes Instance;

        public Dictionary<long, Vector3> Positions = new Dictionary<long, Vector3>();
        public Dictionary<long, Vector3> Rotations = new Dictionary<long, Vector3>();

        void Start()
        {
            Instance = this;
        }

        void Update()
        {
            if (SRSingleton<SceneContext>.Instance != null)
            {
                var identifiables = SRSingleton<SceneContext>.Instance.GameModel.identifiables;
                foreach (var key in Positions.Keys)
                {
                    if (identifiables.ContainsKey(key))
                    {
                        var identifiableTransform = identifiables[key].Transform;
                        identifiableTransform.position = Positions[key];
                        identifiableTransform.rotation = Quaternion.Euler(Rotations[key]);
                    }
                }
            }

            foreach (var key in Positions.Keys.ToList())
            {
                if (Positions[key] == null)
                {
                    Positions.Remove(key);
                    Rotations.Remove(key);
                }
            }
        }
    }
}
