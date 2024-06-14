using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    private int defaultLevel = 5;
    private GameObject playerPrefab;
    private GameObject enemyPrefab;
    public LevelModel levelModel;
    public TMPro.TextMeshProUGUI levelNameText;
    public Transform playerSpawnPoint;
    public ParticleSystem characterDeathEffect;
    public List<Transform> enemySpawnTransforms = new List<Transform>();
    public List<Transform> enemyEndTransforms = new List<Transform>();
    public Image blackScreen; // Assuming this is a UI Image for the black screen
    public bool spawnEnemies = true;
    public GameObject redKeyUIImage;
    public GameObject blueKeyUIImage;

    private Dictionary<EnemyModel, List<Transform>> enemyData = new Dictionary<EnemyModel, List<Transform>>();

    private Vector2 leftScreenPosition = new Vector2(-1080, 0);
    private Vector2 rightScreenPosition = new Vector2(1080, 0);
    private Vector2 middleScreenPosition = Vector2.zero;

    void Start()
    {
        blackScreen.rectTransform.anchoredPosition = middleScreenPosition;
        FetchDataFromGameManager();
        levelModel.hasKeyRed = false;
        levelModel.hasKeyBlue = false;
        levelModel.enemyCount = levelModel.enemies.Length;
    }

    void FetchDataFromGameManager()
    {
        playerPrefab = GameManager.instance.playerPrefab;
        enemyPrefab = GameManager.instance.enemyPrefab;
    }

    public void SetupLevel()
    {
        blackScreen.rectTransform.anchoredPosition = middleScreenPosition;
        // Move black screen from right to middle
        StartCoroutine(MoveBlackScreen(middleScreenPosition, rightScreenPosition));

        Debug.Log("Setting up level: " + levelModel.levelName);
        // Destroy all enemies and player if they exist
        DestroyGameObjectsWithTag("Enemy");
        DestroyGameObjectWithTag("Player");

        enemyData.Clear();
        Debug.Log("Enemy count: " + levelModel.enemies.Length);
        for (int i = 0; i < levelModel.enemies.Length; i++)
        {
            enemyData.Add(levelModel.enemies[i], new List<Transform> { enemySpawnTransforms[i], enemyEndTransforms[i] });
        }
        SpawnPlayer();
        if (spawnEnemies)
            SpawnEnemies();

        levelNameText.text = levelModel.levelName;
    }

    void SpawnPlayer()
    {
        if (playerPrefab != null && playerSpawnPoint != null)
        {
            GameObject player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity, playerSpawnPoint.parent);
            PlayerView playerView = player.GetComponent<PlayerView>();
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.Initialize(levelModel.player, playerView);
        }
        else
        {
            Debug.LogError("PlayerPrefab or PlayerSpawnPoint is not assigned.");
        }
    }

    void SpawnEnemies()
    {
        foreach (var enemy in enemyData)
        {
            GameObject enemyInstance = Instantiate(enemyPrefab, enemy.Value[0].position, Quaternion.identity, enemy.Value[0].parent);
            EnemyView enemyView = enemyInstance.GetComponent<EnemyView>();
            EnemyController enemyController = enemyInstance.GetComponent<EnemyController>();
            enemyController.Initialize(enemy.Key, enemyView, enemy.Value[0], enemy.Value[1]);
        }
    }

    public void CollectKey(KeyType keyType)
    {
        if (keyType == KeyType.Red)
        {
            levelModel.hasKeyRed = true;
            if (redKeyUIImage != null)
                redKeyUIImage.SetActive(true);
        }
        else if (keyType == KeyType.Blue)
        {
            levelModel.hasKeyBlue = true;
            if (blueKeyUIImage != null)
                blueKeyUIImage.SetActive(true);
        }
    }

    private void DestroyGameObjectsWithTag(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject go in gameObjects)
        {
            Destroy(go);
        }
    }

    private void DestroyGameObjectWithTag(string tag)
    {
        GameObject go = GameObject.FindGameObjectWithTag(tag);
        if (go != null)
        {
            Destroy(go);
        }
    }

    public IEnumerator MoveBlackScreen(Vector2 fromPosition, Vector2 toPosition)
    {
        yield return new WaitForSeconds(1f);
        float duration = 0.5f;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            blackScreen.rectTransform.anchoredPosition = Vector2.Lerp(fromPosition, toPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        blackScreen.rectTransform.anchoredPosition = toPosition;
    }
}
