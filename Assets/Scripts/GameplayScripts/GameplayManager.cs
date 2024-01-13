using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameplayManager : MonoBehaviourPunCallbacks 
{
    public JoystickController JoystickController;
    public List<Transform> spawnPlayerPosList=new List<Transform>();

    public GameObject playerPrefab;
    public static GameplayManager gameplayManagerInstance;

    public PlayerController playerController;
    public MainCameraFollow mainCamera;
    private void Awake()
    {
        gameplayManagerInstance = this;
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        GameObject player;
        player= PhotonNetwork.Instantiate(playerPrefab.name, spawnPlayerPosList[GameConstants.GetGlobalIntValue(GameConstants.SpawnPlayerNoGlobalValueConst)].position, Quaternion.identity);
        playerController = player.GetComponent<PlayerController>();
        mainCamera.player = player.transform;
    }


    public void JumpPlayer()
    {
        playerController.JumpPlayerActions();
    }

    public void FirePlayer()
    {
        playerController.Fire();
    }
    
    
    
    
    
}
