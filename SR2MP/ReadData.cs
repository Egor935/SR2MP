using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Player.CharacterController;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using System;
//using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Il2Cpp.ActorVortexer;

namespace SR2MP
{
    public class ReadData : MonoBehaviour
    {
        private SRCharacterController _Player;
        private Animator _PlayerAnimator;
        private Animator _VacconeAnimator;

        //Movement
        private Vector3 _Position;
        private float _Rotation;

        //Animations
        private float _HM;
        private float _FM;
        private float _Yaw;
        private int _AS;
        private bool _Moving;
        private float _HS;
        private float _FS;

        //Vacpack
        private float _CameraAngle;
        private bool _VacMode;
        private bool _CachedVacMode;

        //Time
        private double _Time;

        //GameMode
        private bool _GameMode;
        private bool _GameModeCached;

        //Actors
        private Dictionary<long, IdentifiableModel> _Actors;

        void Start()
        {
            _Player = FindObjectOfType<SRCharacterController>();
            _PlayerAnimator = _Player.GetComponent<Animator>();
            _VacconeAnimator = GameObject.Find("PlayerCameraKCC/First Person Objects/vac shape/Vaccone Prefab").GetComponent<Animator>();
        }

        void Update()
        {
            ReadVacconeState();
            if (_CachedVacMode != _VacMode)
            {
                SendData.SendVacconeState(_VacMode);
                _CachedVacMode = _VacMode;
            }

            ReadGameMode();
            if (_GameMode != _GameModeCached)
            {
                SendData.SendGameModeSwitch(_GameMode);
                _GameModeCached = _GameMode;
            }
        }

        void FixedUpdate()
        {
            ReadMovement();
            SendData.SendMovement(_Position, _Rotation);

            ReadAnimations();
            SendData.SendAnimations(_HM, _FM, _Yaw, _AS, _Moving, _HS, _FS);

            ReadCameraAngle();
            SendData.SendCameraAngle(_CameraAngle);

            if (!Statics.JoinedTheGame)
            {
                ReadTime();
                SendData.SendTime(_Time);

                ReadActors();
                SendData.SendActors(_Actors);
            }
        }

        private void ReadMovement()
        {
            _Position = _Player.transform.position;
            _Rotation = _Player.transform.rotation.eulerAngles.y;
        }

        private void ReadAnimations()
        {
            _HM = _PlayerAnimator.GetFloat("HorizontalMovement");
            _FM = _PlayerAnimator.GetFloat("ForwardMovement");
            _Yaw = _PlayerAnimator.GetFloat("Yaw");
            _AS = _PlayerAnimator.GetInteger("AirborneState");
            _Moving = _PlayerAnimator.GetBool("Moving");
            _HS = _PlayerAnimator.GetFloat("HorizontalSpeed");
            _FS = _PlayerAnimator.GetFloat("ForwardSpeed");
        }

        private void ReadCameraAngle()
        {
            _CameraAngle = _Player._cameraController._targetVerticalAngle;
        }

        private void ReadVacconeState()
        {
            _VacMode = _VacconeAnimator.GetBool("vacMode");
        }

        private void ReadTime()
        {
            _Time = SRSingleton<SceneContext>.Instance.TimeDirector._worldModel.worldTime;
        }

        private void ReadGameMode()
        {
            var _SystemContext = SRSingleton<SystemContext>.Instance;
            if (_SystemContext != null)
            {
                _GameMode = _SystemContext.SceneLoader.CurrentSceneGroup.IsGameplay;
            }
        }

        private int identifiablesCountCached;
        private bool slimesAfterLoadingFound;
        private void ReadActors()
        {
            if (!slimesAfterLoadingFound)
            {
                var identifiables = SRSingleton<SceneContext>.Instance.GameModel.identifiables;
                if (identifiables.Count != identifiablesCountCached)
                {
                    var syncingSlimes = new Dictionary<long, IdentifiableModel>();
                    var keys = identifiables.Keys;
                    foreach (var key in keys)
                    {
                        var identifiable = identifiables[key];
                        if (identifiable != null)
                        {
                            if (identifiable.GetIl2CppType().Name.Equals(nameof(SlimeModel)))
                            {
                                if (identifiable.Transform != null)
                                {
                                    syncingSlimes.Add(key, identifiable);
                                }
                            }
                        }
                    }
                    _Actors = syncingSlimes;
                    identifiablesCountCached = identifiables.Count;
                }
                else
                {
                    slimesAfterLoadingFound = true;
                }
            }

            var newDictionary = new Dictionary<long, IdentifiableModel>();
            foreach (var key in _Actors.Keys)
            {
                if (_Actors[key].Transform != null)
                {
                    newDictionary.Add(key, _Actors[key]);
                }
            }
            _Actors = newDictionary;
        }
    }
}
