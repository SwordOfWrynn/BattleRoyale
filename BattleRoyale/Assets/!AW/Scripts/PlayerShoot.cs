﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour {

    private const string PLAYER_TAG = "Player";
    
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;

	// Use this for initialization
	void Start () {
		if(cam == null)
        {
            if (Debug.isDebugBuild)
                Debug.LogError("PlayerShoot -- Start: No camera referenced");
            this.enabled = false;
        }

        weaponManager = GetComponent<WeaponManager>();
	}
	
	// Update is called once per frame
	void Update () {
        currentWeapon = weaponManager.GetCurrentWeapon();

        if (PauseMenu.isOn)
            return;

        if (currentWeapon.fireRate <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }
	}

    //called on server when a player shoots
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    //called on all clients when we need to display a shoot effect
    [ClientRpc]
    void RpcDoShootEffect()
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
    }

    //called when we hit somerhing
    [Command]
    void CmdOnHit(Vector3 _pos, Vector3 _normal)
    {
        RpcDoHitEffect(_pos, _normal);
    }

    //called on all clients to spawn in a hit effect
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
    {
        GameObject hitEffect = SimplePool.Spawn(weaponManager.GetCurrentGraphics().impactEffectPrefab, _pos, Quaternion.LookRotation(_normal));
        //Utility.DespawnAfterSeconds(hitEffect, 2f);
        //SimplePool.Despawn(hitEffect);
        StartCoroutine(Utility.DespawnAfterSeconds(hitEffect, 2f));
    }

    [Client] //called on the local client
    void Shoot()
    {
        if (isLocalPlayer == false)
            return;
        //we are shooting call on shoot method on server
        CmdOnShoot();

        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask))
        {
            //We hit something
            if(hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damage);
            }
            //we hit something, call OnHit method on server
            CmdOnHit(hit.point, hit.normal);

        }

    }

    [Command] //called on the server
    void CmdPlayerShot(string _playerID, int _damage)
    {
        if (Debug.isDebugBuild)
            Debug.Log(_playerID + " has been shot");

        Player player = GameManager.GetPlayer(_playerID);

        player.RpcTakeDamage(_damage);
    }

}
