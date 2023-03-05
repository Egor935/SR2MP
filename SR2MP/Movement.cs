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
        public Movement(IntPtr ptr) : base(ptr) { }

        CharacterController _CC;

        public Vector3 _Position = new Vector3(529.1114f, 17.11f, 338.0007f);
        public float _Rotation;
        public Vector3 _Speed;

        public void Start()
        {
            _CC = GetComponent<CharacterController>();
        }

        public void Update()
        {
            if (_Position != this.transform.position)
            {
                this.transform.position = _Position;
            }

            this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, _Rotation, transform.rotation.eulerAngles.z);

            _CC.SimpleMove(_Speed);
        }
    }
}
