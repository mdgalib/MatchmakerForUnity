using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using fredfishgames.networking;
using System;
using UnityEngine.Networking;
using System.Threading;
using UnityEngine.UI;

public class Matchmaking : MonoBehaviour {
    Matchmaker mmkr;
    [Header("Connection info")]
    public string Address = "servers.fredfishgames.co.uk";
    public int Port = 24713;
    Action<string> getServerCallback;
    Action<bool> getStatusCallback;
    Thread GettingMatchThread;
    public static Matchmaking singleton;

    #region EDIT

    void OnGettingStatus()
    {
        MenuManager.singleton.FindMenuElementWithTag("status").popup.gameObject.GetComponentInChildren<Button>().interactable = false;
        MenuManager.singleton.Open("status");
        MenuManager.singleton.Close("login");
    }

    void OnStartClient() {
        MenuManager.singleton.Open("getting");
        MenuManager.singleton.FindMenuElementWithTag("main").popup.gameObject.GetComponent<CanvasGroup>().interactable = false;
    }


    void OnStartServer()
    {
        MenuManager.singleton.Open("sending");
        MenuManager.singleton.FindMenuElementWithTag("main").popup.gameObject.GetComponent<CanvasGroup>().interactable = false;
    }

    void OnClearServer() {

    }
#endregion

    #region getServer
    //Get a server to join
    public void StartClient()
    {
        getServerCallback = GetServerCallback;
        GettingMatchThread = mmkr.GetServer(getServerCallback);
        OnStartClient();
    }

    public static void GetServerCallback(string address) {
        MenuManager.singleton.FindMenuElementWithTag("main").popup.gameObject.GetComponent<CanvasGroup>().interactable = true;
        MenuManager.singleton.Close("getting");
        if (address == "0") {
            Debug.Log("No server found...");
            MenuManager.singleton.Open("gettingfailed");
            return;
        }
        NetworkManager networkManager = NetworkManager.singleton;
        networkManager.networkAddress = address;
        networkManager.StartClient();
        MenuManager.singleton.Open("lobby");
    }

    #endregion
    #region sendServer
    //Get a server to join
    public void StartServer()
    {
        Action<string> sendServerCallback = SendServerCallback;
        GettingMatchThread = mmkr.SendServer(sendServerCallback);
        OnStartServer();
    }


    public static void SendServerCallback(string args)
    {
        MenuManager.singleton.FindMenuElementWithTag("main").popup.gameObject.GetComponent<CanvasGroup>().interactable = true;
        MenuManager.singleton.Close("sending");
        if (args == "0")
        {
            Debug.Log("Couldn't send server...");
            MenuManager.singleton.Open("gettingfailed");
            return;
        }
        NetworkManager networkManager = NetworkManager.singleton;
        networkManager.StartHost();
        MenuManager.singleton.Open("lobby");
    }

    #endregion
    #region getStatus
    //Get a server to join
    public void GetStatus()
    {
        Action<string> getStatusCallback = GetStatusCallback;
        GettingMatchThread = mmkr.GetStatus(getStatusCallback);
        OnGettingStatus();
    }


    public static void GetStatusCallback(string args)
    {
        if (args == "0")
        {
            MenuManager.singleton.FindMenuElementWithTag("status").popup.gameObject.GetComponentInChildren<Button>().interactable = true;
        }
        if (args == "1") {
            MenuManager.singleton.Close("status");
            MenuManager.singleton.Close("start");
        }
    }
    #endregion
    #region clearServer
    //Get a server to join
    public void ClearServer()
    {
        Action<string> clearServerCallback = ClearServerCallback;
        GettingMatchThread = mmkr.ClearServer(clearServerCallback);
        OnClearServer();
    }


    public static void ClearServerCallback(string args)
    {
        if (args == "0")
        {
            MenuManager.singleton.FindMenuElementWithTag("status").popup.gameObject.GetComponentInChildren<Button>().interactable = true;
        }
        if (args == "1")
        {
            MenuManager.singleton.Close("status");
            MenuManager.singleton.Close("start");
        }
    }
    // Use this for initialization
    void Start()
    {
        mmkr = new Matchmaker(Address, Port);
        singleton = this;
    }
    void Update()
    {
        mmkr.Update();

    }
    #endregion
}
