using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public LevelModel[] levels;
    private int currentLevelIndex = 0;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;


    // reference to ui textmeshpro for level name
    public TMPro.TextMeshProUGUI LevelFinishText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        LoadCurrentLevel();
    }

    public void LoadCurrentLevel()
    {
        LevelFinishText.text = "";

        var levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null)
        {
            levelManager.levelModel = levels[currentLevelIndex];
            levelManager.SetupLevel();
        }
        else
        {
            Debug.LogError("LevelManager not found in the scene.");
        }
    }

    public void LoadNextLevel()
    {
        currentLevelIndex = (currentLevelIndex + 1) % levels.Length;
        string nextLevelName = levels[currentLevelIndex].levelName;
        SceneManager.LoadScene(nextLevelName);
    }

    public void FinishLevel()
    {
        LevelFinishText.text = "Level Finished!";
        RestartLevel();
    }


    // to restart the same level after 5 second, call this method
    public void RestartLevel()
    {
        Invoke(nameof(LoadCurrentLevel), 5f);
    }
}
