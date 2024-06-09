using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;
    public PlayerModel playerModel;
    public bool spawnEnemies = true;
    public List<EnemyModel> enemyModels = new List<EnemyModel>();
    public List<Transform> enemySpawnTransforms = new List<Transform>();
    public List<Transform> enemyEndTransforms = new List<Transform>();
    public Dictionary<EnemyModel, List<Transform>> enemyData = new Dictionary<EnemyModel, List<Transform>>();
    public GameObject enemyPrefab;

    private void Start()
    {
        // I will make a dictionary for enemyModels and enemyEndTransforms.
        for (int i = 0; i < enemyModels.Count; i++)
        {
            enemyData.Add(enemyModels[i], new List<Transform> { enemySpawnTransforms[i], enemyEndTransforms[i] });
        }
        SpawnPlayer();
        if (spawnEnemies)
            SpawnEnemies();
    }

    private void SpawnPlayer()
    {
        if (playerPrefab != null && playerSpawnPoint != null)
        {
            GameObject player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity, playerSpawnPoint.transform.parent.transform);
            PlayerView playerView = player.GetComponent<PlayerView>();
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.Initialize(playerModel, playerView);
        }
        else
        {
            Debug.LogError("PlayerPrefab or PlayerSpawnPoint is not assigned in GameManager.");
        }
    }

    private void SpawnEnemies()
    {
        foreach (var enemy in enemyData)
        {

            GameObject enemyObject = Instantiate(enemyPrefab, enemy.Value[0].position, Quaternion.identity, enemy.Value[0].transform.parent.transform);
            EnemyView enemyView = enemyObject.GetComponent<EnemyView>();
            EnemyController enemyController = enemyObject.GetComponent<EnemyController>();
            enemyController.Initialize(enemy.Key, enemyView, enemy.Value[0], enemy.Value[1]);
        }
    }
}
