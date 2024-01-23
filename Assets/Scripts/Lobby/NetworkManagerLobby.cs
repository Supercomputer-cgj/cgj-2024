﻿using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;
using System.Linq;
public class NetworkManagerLobby : NetworkManager
{
    [scene] [SerializeField] private string menuScene = string.Empty;
    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;
    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    
    public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");
        foreach (var prefab in spawnablePrefabs)
            ClientScene.RegisterPrefab(prefab);
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        OnClientConnected?.Invoke();
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        OnClientDisconnected?.Invoke();
    }
    
    public override void OnServerConnect(NetworkConnection conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }
        if (SceneManager.GetActiveScene().name != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
       if (SceneManager.GetActiveScene().name == menuScene)
       {
           NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);
           NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
       }
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        OnClientDisconnected?.Invoke();
    }
    public override void OnStopServer()
    {
        base.OnStopServer();
        OnClientDisconnected?.Invoke();
    }
    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            if (!NetworkServer.active) { return; }
            if (NetworkManager.singleton.numPlayers < 2) { return; }
            ServerChangeScene("Scene_Map");
        }
    }
}