using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    public GameObject startMenu;
    public GameObject createRoomMenu;
    public GameObject joinRoomMenu;
    public string userName;
    public InputField serverEXELocationField;
    public InputField CRusernameField;
    public InputField JRusernameField;
    public InputField CRipAddressField;
    public InputField JRipAddressField;
    public InputField portNumberField;
    public InputField widthInputField;
    public InputField depthInputField;
    public InputField numberOfPlayersInputField;


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

    public void CreateNewServer()
    {
        userName = CRusernameField.text;
        startMenu.SetActive(false);
        createRoomMenu.SetActive(false);
        joinRoomMenu.SetActive(false);
        CRusernameField.interactable = false;
        CRipAddressField.interactable = false;
        widthInputField.interactable = false;
        depthInputField.interactable = false;
        numberOfPlayersInputField.interactable = false;
        portNumberField.interactable = false;
        serverEXELocationField.interactable = false;


        string args = "-PortNumber " + portNumberField.text + " -MaxNumberOfPlayers " + numberOfPlayersInputField.text + " -MazeWidth " + widthInputField.text + " -MazeDepth " + depthInputField.text;

        string appPath = serverEXELocationField.text;

            //System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(appPath, args);

        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;
        System.Diagnostics.Process.Start(psi);
        waitASec(3f);
        
        Client.instance.ConnectToServer(CRipAddressField.text);

    }
    
    public void ConnectToExistingServer()
    {
        userName = JRusernameField.text;
        
        startMenu.SetActive(false);
        createRoomMenu.SetActive(false);
        joinRoomMenu.SetActive(false);
        JRusernameField.interactable = false;
        JRipAddressField.interactable = false;
        widthInputField.interactable = false;
        depthInputField.interactable = false;
        numberOfPlayersInputField.interactable = false;
        serverEXELocationField.interactable = false;
        Client.instance.ConnectToServer(JRipAddressField.text);
        //Client.instance.CreateNewServer(ipAddressField.text, Width, Depth, MaxNumberOfPlayers);

    }
    public void OpenMainMenu()
    {
        try
        {
            createRoomMenu.SetActive(false);
            joinRoomMenu.SetActive(false);
            startMenu.SetActive(true);
            CRusernameField.interactable = false;
            CRipAddressField.interactable = false;
            JRusernameField.interactable = false;
            JRipAddressField.interactable = false;
            widthInputField.interactable = false;
            depthInputField.interactable = false;
            numberOfPlayersInputField.interactable = false;
            serverEXELocationField.interactable = false;
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error while opening main menu: {_ex}");
        }    
    }
    public void OpenCreateRoomMenu()
    {
        try
        {
            startMenu.SetActive(false);
            joinRoomMenu.SetActive(false);
            createRoomMenu.SetActive(true);
            CRusernameField.interactable = true;
            CRipAddressField.interactable = true;
            JRusernameField.interactable = false;
            JRipAddressField.interactable = false;
            widthInputField.interactable = true;
            depthInputField.interactable = true;
            numberOfPlayersInputField.interactable = true;
            serverEXELocationField.interactable = true;

        }
        catch (Exception _ex)
        {
            Debug.Log($"Error while opening create room menu: {_ex}");
        }
    }
    public void OpenJoinRoomMenu()
    {
        try
        {
            startMenu.SetActive(false);
            createRoomMenu.SetActive(false);
            joinRoomMenu.SetActive(true);

            JRusernameField.interactable = true;
            JRipAddressField.interactable = true;
            CRusernameField.interactable = false;
            CRipAddressField.interactable = false;
            widthInputField.interactable = true;
            depthInputField.interactable = true;
            numberOfPlayersInputField.interactable = true;
            serverEXELocationField.interactable = false;
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error while opening main menu: {_ex}");
        }
    }

    public void waitASec(float secondsToWait)
    {
        IEnumerator coco = ie_waitASec(secondsToWait);
        StartCoroutine(coco);
    }
    private IEnumerator ie_waitASec(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}