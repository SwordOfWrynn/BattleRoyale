﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkDiscoveryScript : NetworkDiscovery {

    public static bool IsInLAN;

    private NetworkManager networkManager;
    private NetworkDiscovery networkDiscovery;
    
    [HideInInspector]
    public LobbyManager lobbyManager;

	// Use this for initialization
	void Start () {
        networkManager = GetComponent<NetworkManager>();
        Initialize();
    }

    public void JoinLANGame()
    {
        StartAsClient();
        IsInLAN = true;
        StartCoroutine(lobbyManager.WaitForJoinLAN());
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        networkManager.networkAddress = fromAddress;
        //See if it is already connected, or else it will try to start and make a new player multipule times
        if (networkManager.IsClientConnected() == false)
        {
            networkManager.StartClient();
            IsInLAN = true;
        }
    }

    public void CreateLANGameAsHost()
    {
        networkManager.StartHost();
        StartAsServer();
        IsInLAN = true;
        StartCoroutine(lobbyManager.WaitForCreateLAN());
    }
    public void CreateLANGameAsServer()
    {
        networkManager.StartServer();
        StartAsServer();
        IsInLAN = true;
        StartCoroutine(lobbyManager.WaitForCreateLAN());
    }

}
