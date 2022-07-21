using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.userName);

            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(Vector2 _inputVector)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputVector);

            //SendUDPData(_packet);
            SendTCPData(_packet);
        }
    }

    public static void LegacyPlayerMovement(bool[] _inputs)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            //SendUDPData(_packet);
            SendTCPData(_packet);
        }
    }

    public static void PlayerShoot(Vector3 _facing)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
        using (Packet _packet = new Packet((int)ClientPackets.playerShoot))
        {
            _packet.Write(_facing);
            SendTCPData(_packet);
        }
    }

    public static void TriggerMazeRedraw(int _mechanismIndex)
    {
        Debug.Log(System.Reflection.MethodBase.GetCurrentMethod());
        using (Packet _packet = new Packet((int)ClientPackets.triggerMazeRedraw))
        {
            _packet.Write(_mechanismIndex);
            SendTCPData(_packet);
        }
    }

    #endregion
}