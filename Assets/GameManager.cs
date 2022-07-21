using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, ItemSpawner> itemSpawners = new Dictionary<int, ItemSpawner>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject itemSpawnerPrefab;

    public GameObject pillObjectPrefab;
    public GameObject rewardObjectPrefab;
    public GameObject buttonObjectPrefab;

    public static int Width;
    public static int Depth;
    public bool isInsulated;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }
        Debug.Log($"Spawning player {_username}");
        _player.GetComponent<PlayerManager>().Initialize(_id, _username);
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void legacyCreateItemSpawner(int _spawnerId, Vector3 _position, bool _hasItem)
    {
        GameObject _spawner = Instantiate(itemSpawnerPrefab, _position, itemSpawnerPrefab.transform.rotation);
        _spawner.GetComponent<ItemSpawner>().Initialize(_spawnerId, _hasItem);
        itemSpawners.Add(_spawnerId, _spawner.GetComponent<ItemSpawner>());
    }
    public void CreateItemSpawner(int _spawnerId, Vector3 _position, bool _hasItem, int _itemType)
    {
        GameObject _spawner;
        switch (_itemType)
        {
            case 1:
                _spawner = Instantiate(rewardObjectPrefab, _position, rewardObjectPrefab.transform.rotation);                
                break;
            case 2:
                _spawner = Instantiate(pillObjectPrefab, _position, itemSpawnerPrefab.transform.rotation);                
                break;
            case 3:
                _spawner = Instantiate(buttonObjectPrefab, _position, itemSpawnerPrefab.transform.rotation);
                break;
            default:
                _spawner = Instantiate(itemSpawnerPrefab, _position, itemSpawnerPrefab.transform.rotation);
                break;
        }
        _spawner.GetComponent<ItemSpawner>().Initialize(_spawnerId, _hasItem, _itemType);
        itemSpawners.Add(_spawnerId, _spawner.GetComponent<ItemSpawner>());
    }
    


}