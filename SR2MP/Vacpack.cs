using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public class Vacpack : MonoBehaviour
    {
        public Vacpack(IntPtr ptr) : base(ptr) { }

        public float _CameraAngle;
        bool setVaccone = true;
        public GameObject _Bone;
        public GameObject _BoneBarrel;
        public GameObject _Vaccone;
        public Animator _VacconeAnimator;
        public bool vacMode;
        Quaternion startRot = Quaternion.Euler(344.0474f, 317.0452f, 63.0084f);
        Quaternion finishRot = Quaternion.Euler(400.018f, 457.137f, 248.535f);

        public void Start()
        {
            _Bone = GameObject.Find("BeatrixMainMenu/rig_new/MainC/ROOTJ/Spine01J/Spine02J/Spine03J/Spine04J/rClavicleJ/rShoulderJ");
            _BoneBarrel = GameObject.Find("BeatrixMainMenu/rig_new/MainC/ROOTJ/Spine01J/Spine02J/Spine03J/Spine04J/rClavicleJ/rShoulderJ/rElbowJ/rWristJ/vacAttach/vacStandard/mesh_Vac/vac_export_mesh_vac_barrel");
        }

        public void Update()
        {
            float convert = 0;
            if (_CameraAngle >= 0)
            {
                convert = (0.5f / 90f) * (90f - _CameraAngle);
            }
            else
            {
                convert = 0.5f + (0.5f / 90f) * (_CameraAngle * -1f);
            }

            _Bone.transform.localRotation = Quaternion.Lerp(startRot, finishRot, convert);

            if (setVaccone)
            {
                if (SystemContext.Instance.SceneLoader.currentSceneGroup.isGameplay)
                {
                    var vaccone = GameObject.Find("PlayerCameraKCC/First Person Objects/vac shape/Vaccone Prefab");
                    if (vaccone != null)
                    {
                        var parent = Beatrix.instance._Vacpack._BoneBarrel.transform;
                        _Vaccone = Instantiate(vaccone, parent);
                        _Vaccone.transform.localPosition = new Vector3(0f, 0f, 0.5f);
                        _Vaccone.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                        _VacconeAnimator = _Vaccone.GetComponent<Animator>();
                        _Vaccone.active = true;
                        setVaccone = false;
                    }
                }
            }
        }

        public void FixedUpdate()
        {
            if (_VacconeAnimator != null)
            {
                _VacconeAnimator.SetBool("vacMode", vacMode);
            }
        }
    }
}
