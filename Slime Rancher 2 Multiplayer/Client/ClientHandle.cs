using GameServer;
using Il2CppMonomiPark.SlimeRancher.Player.CharacterController;
using Il2CppMonomiPark.SlimeRancher.UI;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Slime_Rancher_2_Multiplayer
{
    public class ClientHandle : MonoBehaviour
    {
        public static void Welcome(Packets _packet)
        {
            string _msg = _packet.ReadString();
            int _myId = _packet.ReadInt();

            Debug.Log($"Message from server: {_msg}");
            Client.instance.myId = _myId;
            ClientSend.WelcomeReceived();

            Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        }

        public static void UDPTest(Packets _packet)
        {
            string _msg = _packet.ReadString();

            Debug.Log($"Received packet via UDP. Contains message: {_msg}");
            ClientSend.UDPTestReceived();
        }


        public static void SpawnPlayer(Packets _packet)
        {

        }


        public static GameObject beatrix;

        public static Vector3 position;
        public static Quaternion rotation;
        public static void PlayerMovement(Packets _packet)
        {
            position = _packet.ReadVector3();
            rotation = _packet.ReadQuaternion();
        }
    }
}
