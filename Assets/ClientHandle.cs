using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

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

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        
            int _id = _packet.ReadInt();
            Vector3 _position = _packet.ReadVector3();
            Vector3 _velocity = _packet.ReadVector3();
            GameManager.players[_id].transform.position = _position;
            GameManager.players[_id].transform.Find("Mouse").GetComponent<MouseMovement>().rotateMouse(_velocity);
        
        
        
        
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }

    public static void PlayerVelocity(Packet _packet)
    {
        
        int _id = _packet.ReadInt();
        Vector3 _velocity = _packet.ReadVector3();
        Debug.Log(_velocity.ToString());
        if (GameManager.players.Count > 0)
        {
            GameManager.players[_id].transform.Find("Mouse").GetComponent<MouseMovement>().rotateMouse(_velocity);
        }
        
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    public static void PlayerHealth(Packet _packet)
    {
        int _id = _packet.ReadInt();
        float _health = _packet.ReadFloat();

        GameManager.players[_id].SetHealth(_health);
    }

    public static void PlayerRespawned(Packet _packet)
    {
        int _id = _packet.ReadInt();
        GameManager.players[_id].Respawn();
    }

    public static void UpdateMazeWidth(Packet _packet)
    {
        int _width = _packet.ReadInt();
        GameManager.Width = _width;
        MazeBuilder.width = _width;
    }

    public static void UpdateMazeDepth(Packet _packet)
    {
        int _depth = _packet.ReadInt();
        GameManager.Depth = _depth;
        MazeBuilder.depth = _depth;
    }

    public static void UpdateWallStateArray(Packet _packet)
    {
        byte[] flatArray = _packet.ToArray();
        byte[] targetArray = new byte[flatArray.Length - 4];

        for (int i = 4; i < flatArray.Length; i++)
        {
            targetArray[i - 4] = flatArray[i];
        }

        bool[,,] _wallStateArray = new bool[GameManager.Width, GameManager.Depth, 4];
        ConversionUtility.FromBytes<bool>(_wallStateArray, targetArray);
        GameObject.Find("MazeManager").gameObject.GetComponent<MazeBuilder>().updateWallsUsingNewState(_wallStateArray);
    }
    public static void CreateItemSpawner(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        Vector3 _spawnerPosition = _packet.ReadVector3();
        bool _hasItem = _packet.ReadBool();
        int _itemType = _packet.ReadInt();

        GameManager.instance.CreateItemSpawner(_spawnerId, _spawnerPosition, _hasItem, _itemType);
    }

    public static void ItemSpawned(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        if (GameManager.itemSpawners.Count > 0)
        {
            GameManager.itemSpawners[_spawnerId].ItemSpawned();
        }
        
    }

    public static void ItemPickedUp(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        int _byPlayer = _packet.ReadInt();

        GameManager.itemSpawners[_spawnerId].ItemPickedUp(GameManager.players[_byPlayer]);
        GameManager.players[_byPlayer].itemCount++;
    }
    public static void moveItemSpawner(Packet _packet)
    {
        Debug.Log("Moving Item!");
        int _spawnerId = _packet.ReadInt();
        Vector3 newPosition = _packet.ReadVector3();
        GameManager.itemSpawners[_spawnerId].basePosition = newPosition;
        GameManager.itemSpawners[_spawnerId].transform.position = newPosition;
    }
    
}