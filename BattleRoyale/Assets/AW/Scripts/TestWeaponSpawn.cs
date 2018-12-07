﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestWeaponSpawn : NetworkBehaviour {

    NetworkManager networkManager;
    NetworkDiscoveryScript networkDiscoveryScript;
    public GameObject weaponItem;

	// Use this for initialization
	void Start () {
        networkManager = NetworkManager.singleton;
        networkDiscoveryScript = networkManager.GetComponent<NetworkDiscoveryScript>();
        if (networkDiscoveryScript.isServer)
        {
            Utility.InstantiateOverNetwork(weaponItem, this.transform.position, Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
