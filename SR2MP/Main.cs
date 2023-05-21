using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher.Player.CharacterController;
using Il2CppSystem.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SR2MP
{
    public class Main : MonoBehaviour
    {
        public Main(IntPtr ptr) : base(ptr) { }

        #region Variables
        public static Main Instance;

        //Main objects
        public SRCharacterController Player;
        public GameObject Beatrix;
        public Animator PlayerAnimator;
        public Animator BeatrixAnimator;

        //Game loading
        public bool GameLoadCheck = true;
        public bool GameIsLoaded;

        //Menu
        public bool MenuState = true;

        //Donators
        private WebClient webClient = new WebClient();
        private int donatorsCount;
        private List<string> donatorNames = new List<string>();
        private List<string> donatorAmounts = new List<string>();
        private int donatorsStartingPoint;

        //Stuff
        public MemoryStream PublicStream;

        //Slimes
        public bool SyncActors;
        public bool SendActors;
        public bool FindActors;
        private int cachedLength = 0;
        public List<GameObject> SyncingSlimes = new List<GameObject>();
        #endregion

        void Start()
        {
            Instance = this;
            DownloadListOfDonators();
        }

        void OnGUI()
        {
            if (MenuState)
            {
                GUI.color = Color.cyan;
                GUI.skin.label.alignment = TextAnchor.MiddleCenter;

                #region Donators
                GUI.Box(new Rect(180f, 10f, 300f, 300f), "<b>Donators</b>");

                GUI.Label(new Rect(180f, 35f, 150f, 25f), "<b>Name</b>");
                GUI.Label(new Rect(330f, 35f, 150f, 25f), "<b>Amount</b>");

                for (int i = donatorsStartingPoint; i < (donatorsStartingPoint + 8); i++)
                {
                    GUI.Label(new Rect(180f, 65f + 30f * (i - donatorsStartingPoint), 150f, 25f), donatorNames[i]);
                    GUI.Label(new Rect(330f, 65f + 30f * (i - donatorsStartingPoint), 150f, 25f), donatorAmounts[i]);
                }

                var newButton = new GUIStyle(GUI.skin.button);
                newButton.padding.left = 8;
                newButton.padding.top = 3;

                if (donatorsStartingPoint > 0)
                {
                    if (GUI.Button(new Rect(450f, 245f, 25f, 25f), "▲", newButton))
                    {
                        donatorsStartingPoint -= 1;
                    }
                }

                if (donatorsStartingPoint < (donatorsCount - 8))
                {
                    if (GUI.Button(new Rect(450f, 275f, 25f, 25f), "▼", newButton))
                    {
                        donatorsStartingPoint += 1;
                    }
                }
                #endregion

                #region Info
                GUI.Box(new Rect(10f, 320f, 470f, 25f), "You can hide this menu by pressing the Tilde button (~) or Ö");
                #endregion
            }
        }

        void Update()
        {
            //Menu
            if (Input.GetKeyDown(192))
            {
                MenuState = !MenuState;
            }

            //Game loading
            if (GameLoadCheck)
            {
                Scene activeScene = SceneManager.GetActiveScene();
                if (Equals(activeScene.name, "MainMenuEnvironment"))
                {
                    GetBeatrixModel();
                    GameIsLoaded = true;
                    GameLoadCheck = false;
                }
            }

            //Slimes
            if (FindActors)
            {
                var actors = FindObjectsOfType<IdentifiableActor>();
                if (actors.Length > 1)
                {
                    if (cachedLength != actors.Length)
                    {
                        cachedLength = actors.Length;
                    }
                    else
                    {
                        foreach (var actor in actors)
                        {
                            string actorType = actor.identType.GetIl2CppType().FullName;
                            if (actorType.Equals(nameof(SlimeDefinition)))
                            {
                                SyncingSlimes.Add(actor.gameObject);
                            }
                        }
                        if (SteamLobby.Instance.JoinedTheGame)
                        {
                            SyncActors = true;
                        }
                        else
                        {
                            SendActors = true;
                        }
                        FindActors = false;
                    }
                }
            }
        }

        void FixedUpdate()
        {
            if (Player == null)
            {
                Player = FindObjectOfType<SRCharacterController>();
                if (Player != null)
                {
                    HandlePlayer();
                }
            }
        }
        
        private void DownloadListOfDonators()
        {
            var rawFile = webClient.DownloadString("https://raw.githubusercontent.com/Egor935/Slime-Rancher-2-Multiplayer/main/Donators.txt");
            var donators = rawFile.Split('\n');
            donatorsCount = donators.Length;
            foreach (var donator in donators)
            {
                var info = donator.Split('/');
                donatorNames.Add(info[0]);
                donatorAmounts.Add(info[1]);
            }
        }

        private void GetBeatrixModel()
        {
            Beatrix = GameObject.Find("BeatrixMainMenu");
            Instantiate(Beatrix);

            var hair = GameObject.Find("SR2_bea_hair_main_low");
            var material = hair.GetComponent<SkinnedMeshRenderer>().material;
            DontDestroyOnLoad(material);
            
            Beatrix.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            BeatrixAnimator = Beatrix.GetComponent<Animator>();
            Beatrix.AddComponent<CharacterController>();
            Beatrix.AddComponent<Movement>();
            Beatrix.AddComponent<Animations>();
            Beatrix.AddComponent<Vacpack>();
            Beatrix.AddComponent<Beatrix>();

            DontDestroyOnLoad(Beatrix);
        }

        private void HandlePlayer()
        {
            PlayerAnimator = Player.GetComponent<Animator>();
            BeatrixAnimator.avatar = PlayerAnimator.avatar;
            BeatrixAnimator.runtimeAnimatorController = PlayerAnimator.runtimeAnimatorController;
            BeatrixAnimator.updateMode = AnimatorUpdateMode.AnimatePhysics;
            Player.gameObject.AddComponent<ReadData>();

            if (SteamLobby.Instance.JoinedTheGame)
            {
                SendData.RequestTime();
            }
        }
    }
}
