using GameServer;
using Il2Cpp;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public class UI : MonoBehaviour
    {
        #region Variables
        //Menu
        private bool menuState = true;
        private string menuMode;

        //Donators
        private WebClient webClient = new WebClient();
        private int donatorsCount;
        private List<string> donatorNames = new List<string>();
        private List<string> donatorAmounts = new List<string>();
        private int donatorsStartingPoint;
        #endregion

        void OnGUI()
        {
            //Unity documentation way
            if (Event.current.Equals(Event.KeyboardEvent(KeyCode.BackQuote.ToString())))
            {
                menuState = !menuState;
            }

            GUI.color = Color.cyan;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;

            if (menuState)
            {
                MainMenu();

                switch (menuMode)
                {
                    case "steam":
                        SteamLobby.Instance.SteamMenu();
                        break;
                    case "custom":
                        CustomLobby.Instance.CustomMenu();
                        break;
                    default:
                        WelcomeMenu();
                        break;
                }
            }
        }

        private void MainMenu()
        {
            #region SR2MP
            GUI.Box(new Rect(10f, 10f, 160f, 300f), "<b>SR2MP</b>");
            #endregion

            #region Donators
            GUI.Box(new Rect(175f, 10f, 300f, 300f), "<b>Donators</b>");

            GUI.Label(new Rect(180f, 35f, 150f, 25f), "<b>Name</b>");
            GUI.Label(new Rect(330f, 35f, 150f, 25f), "<b>Amount</b>");

            for (int i = donatorsStartingPoint; i < (donatorsStartingPoint + 8); i++)
            {
                if (int.TryParse(donatorAmounts[i].Replace(" rubles", null), out int amount))
                {
                    if ((amount >= 40000))
                    {
                        GUI.color = new Color(111f / 255f, 126f / 255f, 206f / 255f);
                    }
                    else if (amount >= 20000)
                    {
                        GUI.color = new Color(190f / 255f, 38f / 255f, 76f / 255f);
                    }
                    else if (amount >= 10000)
                    {
                        GUI.color = new Color(181f / 255f, 206f / 255f, 239f / 255f);
                    }
                    else if (amount >= 1000)
                    {
                        GUI.color = new Color(241f / 255f, 166f / 255f, 22f / 255f);
                    }
                    else if (amount >= 500)
                    {
                        GUI.color = new Color(203f / 255f, 205f / 255f, 211f / 255f);
                    }
                    else if (amount >= 300)
                    {
                        GUI.color = new Color(115f / 255f, 251f / 255f, 255f / 255f);
                    }
                    else if (amount >= 200)
                    {
                        GUI.color = new Color(82f / 255f, 71f / 255f, 194f / 255f);
                    }
                    else if (amount >= 100)
                    {
                        GUI.color = new Color(236f / 255f, 46f / 255f, 49f / 255f);
                    }
                    else
                    {
                        GUI.color = new Color(241f / 255f, 196f / 255f, 15f / 255f);
                    }
                }

                GUI.Label(new Rect(180f, 65f + 30f * (i - donatorsStartingPoint), 150f, 25f), $"<b>{donatorNames[i]}</b>");
                GUI.Label(new Rect(330f, 65f + 30f * (i - donatorsStartingPoint), 150f, 25f), $"<b>{donatorAmounts[i]}</b>");
            }
            GUI.color = Color.cyan;

            var newButton = new GUIStyle(GUI.skin.button);
            newButton.padding.left = 8;
            newButton.padding.top = 3;

            if (donatorsStartingPoint > 0)
            {
                if (GUI.Button(new Rect(445f, 245f, 25f, 25f), "▲", newButton))
                {
                    donatorsStartingPoint -= 1;
                }
            }

            if (donatorsStartingPoint < (donatorsCount - 8))
            {
                if (GUI.Button(new Rect(445f, 275f, 25f, 25f), "▼", newButton))
                {
                    donatorsStartingPoint += 1;
                }
            }
            #endregion

            #region Info
            GUI.Box(new Rect(10f, 315f, 465f, 25f), "You can hide this menu by pressing the Tilde button (~, Ё, Ö)");
            #endregion
        }

        private void WelcomeMenu()
        {
            GUI.Label(new Rect(15f, 35f, 150f, 25f), "Choose your platform:");

            if (GUI.Button(new Rect(15f, 65f, 150f, 25f), "Steam"))
            {
                this.gameObject.AddComponent<SteamLobby>();
                menuMode = "steam";
            }

            if (GUI.Button(new Rect(15f, 95f, 150f, 25f), "Other (EGS, MS)"))
            {
                MultiplayerMain.Instance.SteamIsAvailable = false;
                this.gameObject.AddComponent<CustomLobby>();
                menuMode = "custom";
            }
        }

        void Start()
        {
            DownloadListOfDonators();

            if (!MultiplayerMain.Instance.SteamIsAvailable)
            {
                this.gameObject.AddComponent<CustomLobby>();
                menuMode = "custom";
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
    }
}
