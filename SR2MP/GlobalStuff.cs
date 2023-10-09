using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SR2MP
{
    public static class GlobalStuff
    {
        public static bool IsMultiplayer = true;
        public static string SecondPlayerName = "None";
        public static bool Host;
        public static bool FriendInGame;
        public static bool JoinedTheGame;
        public static bool HandlePacket;
    }
}
