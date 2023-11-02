using Il2Cpp;
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
        public float ReceivedCameraAngle;
        private GameObject _Bone;
        private GameObject _BoneBarrel;
        private GameObject _Vaccone;
        private Animator _VacconeAnimator;
        public bool VacMode;
        private Quaternion startRot = Quaternion.Euler(344.0474f, 317.0452f, 63.0084f);
        private Quaternion finishRot = Quaternion.Euler(400.018f, 457.137f, 248.535f);

        void Start()
        {
            _Bone = GameObject.Find("BeatrixMainMenu/rig_new/MainC/ROOTJ/Spine01J/Spine02J/Spine03J/Spine04J/rClavicleJ/rShoulderJ");
            _BoneBarrel = GameObject.Find("BeatrixMainMenu/rig_new/MainC/ROOTJ/Spine01J/Spine02J/Spine03J/Spine04J/rClavicleJ/rShoulderJ/rElbowJ/rWristJ/vacAttach/vacStandard/mesh_Vac/vac_export_mesh_vac_barrel");
        }

        void Update()
        {
            float convert = 0;
            if (ReceivedCameraAngle >= 0)
            {
                convert = (0.5f / 90f) * (90f - ReceivedCameraAngle);
            }
            else
            {
                convert = 0.5f + (0.5f / 90f) * (ReceivedCameraAngle * -1f);
            }

            _Bone.transform.localRotation = Quaternion.Lerp(startRot, finishRot, convert);

            if (_Vaccone == null)
            {
                if (SRSingleton<SystemContext>.Instance.SceneLoader.currentSceneGroup.isGameplay)
                {
                    var vaccone = GameObject.Find("PlayerCameraKCC/First Person Objects/vac shape/Vaccone Prefab");
                    if (vaccone != null)
                    {
                        var parent = _BoneBarrel.transform;
                        _Vaccone = Instantiate(vaccone, parent);
                        _Vaccone.transform.localPosition = new Vector3(0f, 0f, 0.5f);
                        _Vaccone.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                        _VacconeAnimator = _Vaccone.GetComponent<Animator>();
                        _Vaccone.active = true;
                    }
                }
            }
        }

        void FixedUpdate()
        {
            if (_VacconeAnimator != null)
            {
                _VacconeAnimator.SetBool("vacMode", VacMode);
            }
        }
    }
}
