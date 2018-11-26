﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserStats : MonoBehaviour {

    public TextMeshProUGUI killCount;
    public TextMeshProUGUI deathCount;

	// Use this for initialization
	void Start () {
        if(UserAccountManager.IsLoggedIn)
            UserAccountManager.instance.RetrieveData(OnReceivedData);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnReceivedData(string _data)
    {
        if (killCount == null || deathCount == null)
            return;

        killCount.text = "Kills: " + Utility.DataToIntValue(_data, UserAccountManager.KillCountDataSymbol);
        deathCount.text = "Deaths: " + Utility.DataToIntValue(_data, UserAccountManager.DeathCountDataSymbol);
    }

}
