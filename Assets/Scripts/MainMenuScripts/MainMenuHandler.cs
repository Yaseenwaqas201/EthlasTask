using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviourPunCallbacks
{
    [Header(" Reference of Panel ")] 
    public GameObject panelsParrent ;
    public GameObject connectionPanel ;
    public GameObject playerNamePanel;

    [Header("Reference of connection Panel")]
    public Text connectionWarning;
    public GameObject reconnectBtn;
    
    [Header(" PLayer Name Panel References")]
    public InputField playerName;


    

    public void SubmitPlayerName()
    {
        if (playerName.text == "")
        {
            print(" No thing enter"); // here We assign Random Name
        }
        playerNamePanel.SetActive(false);
        panelsParrent.SetActive(false);
    }


    public void PlayGame()
    {
        reconnectBtn.SetActive(false);
        if(PhotonNetwork.IsConnected)
            PhotonNetwork.JoinRandomRoom();
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        connectionPanel.SetActive(true);
        panelsParrent.SetActive(true);
    }


    public void PlayOffline()
    {
        SceneManager.LoadSceneAsync("OfflineMode");
    }
    
    public override void OnConnectedToMaster()
    {
        PlayGame();
    }

    private void CreateNewRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        connectionWarning.text = " Failed to connect with server try later";
        reconnectBtn.SetActive(true);

    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateNewRoom();
    }

    
    // On Join Room We will call for load Gameplay Scene
    public override void OnJoinedRoom()
    {
        connectionPanel.SetActive(true);
        panelsParrent.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("OnlineMode");
        }
        
    }

}
