using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static SR2MP.HandleData;

namespace SR2MP
{
    public class Movement : MonoBehaviour
    {
        public Movement(IntPtr ptr) : base(ptr) { }

        CharacterController _CC;

        public Vector3 _Position;
        public float _Rotation;
        public Vector3 _Speed;

        public void Start()
        {
            _CC = GetComponent<CharacterController>();
        }

        public void Update()
        {
            _CC.SimpleMove(_Speed);
        }

        public void LateUpdate()
        {
            if (_Position != this.transform.position)
            {
                this.transform.position = _Position;
            }

            this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, _Rotation, transform.rotation.eulerAngles.z);
        }
    }
}
