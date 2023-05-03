using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    // -----------------------------------------
    #region Private Serializable Fields

    [Tooltip("The maximum number of players per room. New room will be created when a room is full.")]
    [SerializeField] private byte maxPlayersPerRoom = 4;

    #endregion
    // -----------------------------------------

    // -----------------------------------------
    #region Private Fields

    bool isConnecting;
    string gameVersion = "0.0.0";

    #endregion
    // -----------------------------------------

    // -----------------------------------------
    #region Public Fields

    [Tooltip("The UI Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;

    #endregion
    // -----------------------------------------

    // -----------------------------------------
    #region MonoBehaviour CallBacks

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    #endregion
    // -----------------------------------------

    // -----------------------------------------
    #region Public Methods

    public void Connect()
    {
        isConnecting = true;
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Not connected to Photon Cloud. Now connect ...");
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.Log("Already connected. Let's join in random room!");
            PhotonNetwork.JoinRandomRoom();
        }
    }

    #endregion
    // -----------------------------------------

    // -----------------------------------------

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        if(isConnecting)
        {
            Debug.Log("OnConnectedToMaster() was called by PUN !");
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string msg)
    {
        Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available, so let's create one.\nCalling >> PhotonNetwork.CreateRoom");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() was called by PUN. Now this client is in a room.");
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the [ Room for 1 ]");
            PhotonNetwork.LoadLevel("Room for 1");
        }
    }

    #endregion
    // -----------------------------------------
}
