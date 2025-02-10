using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public class MultiplayerUI : MonoBehaviour
    {
        private bool menuState = true;

        void OnGUI()
        {
            if (Event.current.Equals(Event.KeyboardEvent(KeyCode.BackQuote.ToString())))
            {
                menuState = !menuState;
            }

            if (!menuState)
                return;

            GUI.color = Color.cyan;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;

            GUI.Box(new Rect(10f, 10f, 160f, 300f), "<b>SR2MP</b>");

            if (SteamLobby.Instance != null)
            {
                if (SteamLobby.SteamIsAvailable)
                {
                    SteamLobby.Instance.SteamMenu();
                }
                else
                {
                    GUI.Label(new Rect(15f, 150f, 150f, 210f), "Steam was not initialized!");
                }
            }
            else
            {
                if (GUI.Button(new Rect(15f, 35f, 150f, 25f), "Init"))
                {
                    SteamLobby.Create();
                }
            }

            if (GUI.Button(new Rect(15f, 280f, 150f, 25f), "Hide (~, Ё, Ö)"))
            {
                menuState = false;
            }

            DonatorsMenu();
        }

        private Dictionary<string, string> _Donators = new Dictionary<string, string>();
        private int donatorsStartingPoint;
        private void DonatorsMenu()
        {
            GUI.Box(new Rect(175f, 10f, 300f, 300f), "<b>Donators</b>");

            GUI.Label(new Rect(180f, 35f, 150f, 25f), "<b>Name</b>");
            GUI.Label(new Rect(330f, 35f, 150f, 25f), "<b>Amount</b>");

            for (int i = donatorsStartingPoint; i < (donatorsStartingPoint + 8); i++)
            {
                if (int.TryParse(_Donators.Values.ToList()[i].Replace(" Rubles", null), out int amount))
                {
                    if (amount >= 40000)
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

                GUI.Label(new Rect(180f, 65f + 30f * (i - donatorsStartingPoint), 150f, 25f), $"<b>{_Donators.Keys.ToList()[i]}</b>");
                GUI.Label(new Rect(330f, 65f + 30f * (i - donatorsStartingPoint), 150f, 25f), $"<b>{_Donators.Values.ToList()[i]}</b>");
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

            if (donatorsStartingPoint < (_Donators.Count - 8))
            {
                if (GUI.Button(new Rect(445f, 275f, 25f, 25f), "▼", newButton))
                {
                    donatorsStartingPoint += 1;
                }
            }
        }

        void Start()
        {
            DownloadListOfDonators();
        }

        private void DownloadListOfDonators()
        {
            var webClient = new WebClient();
            var rawFile = webClient.DownloadString("https://raw.githubusercontent.com/Egor935/Slime-Rancher-2-Multiplayer/main/Donators.txt");
            var donators = rawFile.Split('\n');
            foreach (var donator in donators)
            {
                var info = donator.Split('/');
                var name = info[0];
                var amount = info[1];
                _Donators.Add(name, amount);
            }
        }
    }
}
