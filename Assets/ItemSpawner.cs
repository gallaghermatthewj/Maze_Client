using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpawner : MonoBehaviour
{
    public int spawnerId;
    public bool hasItem;
    public GameObject objectModel;
    public MeshRenderer itemModel;

    public float itemRotationSpeed = 50f;
    public float itemBobSpeed = 2f;
    public Vector3 basePosition;
    public enum itemIndex { itemCheese = 1, itemPill, itemButton, itemLever };

    public itemIndex itemType=0;

    private void Update()
    {
        if (hasItem)
        {
            if ((int)itemType <= 2)
            {
                transform.Rotate(Vector3.up, itemRotationSpeed * Time.deltaTime, Space.World);
                transform.position = basePosition + new Vector3(0f, 0.25f * Mathf.Sin(Time.time * itemBobSpeed), 0f);
            }
        }
        else
        {
            itemModel.enabled = false;
        }
    }

    public void Initialize(int _spawnerId, bool _hasItem)
    {
        spawnerId = _spawnerId;
        hasItem = _hasItem;
        itemModel.enabled = _hasItem;
        
        basePosition = transform.position;
    }
    public void Initialize(int _spawnerId, bool _hasItem, int _itemType)
    {
        spawnerId = _spawnerId;
        hasItem = _hasItem;
        itemType = (itemIndex)_itemType;
        setModel();
        
        

        basePosition = transform.position;
    }
    private void setModel()
    {
        GameObject _objectModel;
        Debug.Log(itemType.ToString());
        switch (itemType)
        {
            case itemIndex.itemCheese:
                _objectModel = Resources.Load("Prefabs/cheese") as GameObject;
                break;
            case itemIndex.itemPill:
                _objectModel = Resources.Load("Prefabs/Pill") as GameObject;
                break;
            case itemIndex.itemButton:
                _objectModel = Resources.Load("Prefabs/PiezoButton") as GameObject;
                break;
            case itemIndex.itemLever:
                _objectModel = Resources.Load("Prefabs/PiezoButton") as GameObject;
                break;
            default:
                _objectModel = Resources.Load("Prefabs/cheese") as GameObject;
                break;
        }
        if (_objectModel != null)
        {
            objectModel = Instantiate(_objectModel, transform.position, transform.rotation);
            itemModel = objectModel.GetComponent<MeshRenderer>();
        }
    }
    public void ItemSpawned()
    {
        hasItem = true;
        itemModel.enabled = true;
        setModel();
        
    }
    
    public void ItemPickedUp(PlayerManager _player)
    {
        
        hasItem = false;
        itemModel.enabled = false;
        setModel();


        switch (itemType)
        {
            case itemIndex.itemCheese:
                eatCheese(_player);
                break;
            case itemIndex.itemPill:
                //Add some points
                //Trigger random effect!
                takePill(_player);
                break;
            case itemIndex.itemButton:
                //Trigger Random maze State effect
                stepOnButton(_player);
                break;
            case itemIndex.itemLever:
                //Redraw Maze!!!
                break;
            default:
                //Placebo. Because this should not happen. Ever.
                break;
        }
    }
    private void eatCheese(PlayerManager _player)
    {
        if (_player.transform.tag != "OtherPlayer")
        {
            _player.transform.Find("Camera").GetComponent<CameraBehaviorManager>().eatCheese(_player);
        }
        
        //Add 1000 points!
        //GameObject _player = GameObject.Find("LocalPlayer");
        //_player.GetComponent<PlayerManager>().score+=1000f;
        //Happy Mouse noise!
        //Vibrate Controller?
    }
    private void takePill(PlayerManager _player)
    {
        if (_player.transform.tag != "OtherPlayer")
        {
            _player.transform.Find("Camera").GetComponent<CameraBehaviorManager>().takePill(_player);
        }
    }
    private void stepOnButton(PlayerManager _player)
    {
        if (_player.transform.tag != "OtherPlayer")
        {
            _player.transform.Find("Camera").GetComponent<CameraBehaviorManager>().stepOnButton();
        }
    }
    private void pullLever()
    {

    }
}