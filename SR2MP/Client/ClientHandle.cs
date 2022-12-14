using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static SR2MP.ClientHandle.ReceivedValues;

namespace SR2MP
{
    public class ClientHandle : MonoBehaviour
    {
        public static void Welcome(Packet _packet)
        {
            string _msg = _packet.ReadString();
            int _myId = _packet.ReadInt();

            Debug.Log($"Message from server: {_msg}");
            Client.instance.myId = _myId;
            ClientSend.WelcomeReceived();

            Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        }

        public static void UDPTest(Packet _packet)
        {
            string _msg = _packet.ReadString();

            Debug.Log($"Received packet via UDP. Contains message: {_msg}");
            ClientSend.UDPTestReceived();
        }

        public class ReceivedValues
        {
            public static Vector3 position;
            public static float rotation;

            public static float f1;
            public static float f2;
            public static float f3;
            public static int i1;
            public static bool b1;
            public static float f4;
            public static float f5;
        }
        
        public static void MovementReceived(Packet _packet)
        {
            position = _packet.ReadVector3();
            rotation = _packet.ReadFloat();
        }
        
        public static void AnimationsReceived(Packet _packet)
        {
            f1 = _packet.ReadFloat();
            f2 = _packet.ReadFloat();
            f3 = _packet.ReadFloat();
            i1 = _packet.ReadInt();
            b1 = _packet.ReadBool();
            f4 = _packet.ReadFloat();
            f5 = _packet.ReadFloat();
        }
    }
}
