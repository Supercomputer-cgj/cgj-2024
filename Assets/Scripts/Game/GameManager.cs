using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string playerIdPrefix = "Player";
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    private static Dictionary<string, Enemy> enemies = new Dictionary<string, Enemy>();
    public MatchSettings matchSettings;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
        
        Debug.LogError("Plus d'une instance de game Manager dans la scène");
    }

    //permet de collecter tous les Players et de leur assigné un id
    public static void RegisterPlayer(string netID, Player player)
    {
        string playerId = playerIdPrefix + netID;
        players.Add(playerId, player);
        player.transform.name = playerId;
    }
    //desinregistre un joueur du dictionnaire
    public static void UnRegisterPlayer(string playerId)
    {
        players.Remove(playerId);
    }
    public static Player getPlayer(string playerid)
    {
        return players[playerid];
    }

    public static void RegisterEnemy(string netID, Enemy enemy)
    {
        string enemyId = enemy.transform.name + netID;
        enemies.Add(enemyId,enemy);
    }

    public static void UnRegisterEnemy(string enemyId)
    {
        enemies.Remove(enemyId);
    }

    public static Enemy getEnemy(string enemyId)
    {
        return enemies[enemyId];
    }



    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 200));
        GUILayout.BeginVertical();
        foreach (string playerId in players.Keys)
        {
            GUILayout.Label(playerId + " - " + players[playerId].transform.name);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
        
        
        GUILayout.BeginArea(new Rect(50, 50, 200, 200));
        GUILayout.BeginVertical();
        foreach (string enemiesId in enemies.Keys)
        {
            GUILayout.Label(enemiesId + " - " + enemies[enemiesId].transform.name);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
